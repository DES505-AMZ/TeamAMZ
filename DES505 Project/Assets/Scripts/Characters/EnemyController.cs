using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterNavBase
{
    public enum AIState
    {
        Patrol,
        Seek,
        Chase,
    }
    public AIState aiState { get; private set; }

    [Header("Movement")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;

    public float maxMoveSpeed
    {
        get
        {
            if (aiState == AIState.Chase)
                return chaseSpeed;
            else
                return patrolSpeed;
        }
    }

    [Header("detection")]
    public Transform eyePoint;
    public float sightRange = 5f;
    [Range(1f, 90f)]
    public float sightIncludedAngle = 20f;
    public float lostTargetTimeout = 4f;
    public float orientationSpeed = 10f;
    public bool shakeCamera = true;

    [Header("Audio clips")]
    public AudioClip clipCloseTargetUnnoticed;
    public AudioClip clipLostTarget;
    public AudioClip[] clipsDetectTarget;
    AudioSource m_audioSource;

    PlayerController m_targetPlayer;
    Transform m_nearbyTarget;
    bool m_isSeeingTarget;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();

        eyePoint = transform.GetChild(0);
        if (eyePoint == null)
            eyePoint = transform;

        m_targetPlayer = PlayerController.Instance;
        aiState = AIState.Patrol;
    }

    public override void OnRespawn()
    {
        base.OnRespawn();

        m_targetPlayer = PlayerController.Instance;
        aiState = AIState.Patrol;
    }

    void FixedUpdate()
    {
        DetectTarget();
        UpdateAIStateTransitions();
        UpdateCurrentAIState();
        SetNavAgentMaxSpeed(maxMoveSpeed);
    }

    void UpdateAIStateTransitions()
    {
        switch (aiState)
        {
            case AIState.Patrol:
                if(m_isSeeingTarget)
                {
                    aiState = AIState.Chase;
                }
                break;
            case AIState.Seek:
                if(m_isSeeingTarget)
                {
                    aiState = AIState.Chase;
                }
                break;
            case AIState.Chase:
                if (!m_isSeeingTarget)
                {
                    if (m_nearbyTarget != null)
                    {
                        aiState = AIState.Seek;
                        // move to the latest m_targetPlayer position this enemy saw
                        SetNavDestination(m_nearbyTarget.position);
                    }
                    else
                    {
                        aiState = AIState.Patrol;
                        SetPathDestinationToClosestNode();
                    }
                }
                break;
        }
    }

    void UpdateCurrentAIState()
    {
        switch (aiState)
        {
            case AIState.Patrol:
                UpdatePathDestination();
                SetNavDestination(GetPatrolDestination());
                break;
            case AIState.Seek:
                if(DestinationArrived())
                {
                    LookOrientTowards(m_targetPlayer.transform.position);
                    StartCoroutine(SeekTarget());
                }
                break;
            case AIState.Chase:
                SetNavDestination(m_targetPlayer.transform.position);
                LookOrientTowards(m_targetPlayer.transform.position);
                break;
        }
    }

    IEnumerator SeekTarget()
    {
        yield return new WaitForSeconds(lostTargetTimeout);
        if (!m_isSeeingTarget)
        {
            m_audioSource.PlayOneShot(clipLostTarget);
            aiState = AIState.Patrol;
            LookOrientTowards(eyePoint.position + transform.forward);
            SetPathDestinationToClosestNode();           
        }
    }

    void DetectTarget()
    {
        m_isSeeingTarget = false;
        float dist = Vector3.Distance(m_targetPlayer.headPosition, eyePoint.position);
        if (dist < sightRange)
        {
            Vector3 targetDir = m_targetPlayer.headPosition - eyePoint.position;
            float degree = Vector3.Angle(targetDir, eyePoint.forward);
            if(degree < sightIncludedAngle && degree > -sightIncludedAngle)
            {
                RaycastHit hit;
                if(Physics.Raycast(eyePoint.position, targetDir, out hit, sightRange))
                {                    
                    if(hit.collider.gameObject == m_targetPlayer.gameObject)
                    {
                        m_isSeeingTarget = true;
                        m_nearbyTarget = m_targetPlayer.transform;
                    }
                }
            }
        }
        if(shakeCamera)
            m_targetPlayer.shakeSpeedMultiplier = Mathf.Clamp(1f - dist / sightRange, 0f, 1f);
    }

    void LookOrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - eyePoint.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(eyePoint.rotation, targetRotation, orientationSpeed);
            //eyePoint.rotation = Quaternion.Slerp(eyePoint.rotation, targetRotation, orientationSpeed);
        }
    }

    public Vector3 GetPatrolDestination()
    {
        if (IsPathValid())
        {
            return patrolPath.GetPositionOfPathNode(m_patrolNodeIndex);
        }
        else
        {
            return m_targetPlayer.transform.position;
        }
    }

    private void OnDrawGizmos()
    {        
        Vector3 endPoint = eyePoint.position + eyePoint.forward * sightRange;
        float radius = sightRange * Mathf.Tan(Mathf.Deg2Rad * sightIncludedAngle);
        Vector3 upVec = eyePoint.up * radius;
        Vector3 rightVec = eyePoint.right * radius;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(eyePoint.position, endPoint);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(eyePoint.position, endPoint + upVec);
        Gizmos.DrawLine(eyePoint.position, endPoint - upVec);
        Gizmos.DrawLine(eyePoint.position, endPoint + rightVec);
        Gizmos.DrawLine(eyePoint.position, endPoint - rightVec);       
        Gizmos.DrawWireSphere(endPoint, radius);
    }
}
