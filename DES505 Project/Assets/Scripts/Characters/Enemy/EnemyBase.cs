using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 1f;
    public float seekSpeed = 1.5f;
    public float chaseSpeed = 2f;
    public float maxMoveSpeed { get; set; }

    [Header("detection")]
    public Transform eyePoint;
    public float sightRange = 10f;
    public float orientationSpeed = 10f;
    [Range(1f, 90f)]
    public float sightIncludedAngle = 45f;

    [Header("Stun")]
    public float stunTimeInterval = 1f;

    [Header("Audio clips")]
    public AudioClip clipCloseTargetUnnoticed;
    public AudioClip clipLostTarget;
    public AudioClip[] clipsDetectTarget;

    public CharacterNavBase navAgent { get; set; }
    public AudioSource audioSource { get; set; }
    public Player target;
    public Vector3 targetLocationLastSeen { get; set; }

    public EnemyStateBase aiState;
    public EnemyPatrolState patrolState;
    public EnemySeekState seekState;
    public EnemyChaseState chaseState;

    protected void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        navAgent = GetComponent<CharacterNavBase>();      
    }

    protected void Start()
    {
        target = FindObjectOfType<Player>();
        InitializeState();
        SetInitialAiState();

        navAgent.onReset += SetInitialAiState;
    }

    protected virtual void InitializeState()
    {
        patrolState = new EnemyPatrolState();
        seekState = new EnemySeekState();
        chaseState = new EnemyChaseState();
    }

    protected void SetInitialAiState()
    {
        if (aiState != null) aiState.Exit(this);
        aiState = patrolState;
        aiState.Enter(this);
    }

    protected void Update()
    {
        aiState.UpdateLogic(this);
    }

    protected void FixedUpdate()
    {
        aiState.UpdatePhysics(this);
    }

    public void ChangeState(EnemyStateBase newState)
    {
        aiState.Exit(this);
        aiState = newState;
        aiState.Enter(this);
    }

    public void UpdateMaxSpeed(float speed)
    {
        maxMoveSpeed = speed;
        navAgent.SetNavAgentMaxSpeed(maxMoveSpeed);
    }

    public void LookOrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - eyePoint.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(eyePoint.rotation, targetRotation, orientationSpeed);
        }
    }

    public void UpdateTargetLocation(Vector3 position)
    {
        navAgent.SetNavDestination(position);
    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public bool SightDetection()
    {
        float dist = Vector3.Distance(target.transform.position, eyePoint.position);
        if (dist < sightRange)
        {
            Vector3 targetDir = target.transform.position - eyePoint.position;
            float degree = Vector3.Angle(targetDir, eyePoint.forward);
            if (degree < sightIncludedAngle && degree > -sightIncludedAngle)
            {
                RaycastHit hit;
                if (Physics.Raycast(eyePoint.position, targetDir, out hit, sightRange))
                {
                    if (hit.collider.tag == GameConstants.k_TagNamePlayer)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool SoundDetection()
    {
        if (GetDistanceToTarget() < target.currentNoiseRange)
        {
            targetLocationLastSeen = target.transform.position;
            return true;
        }
        return false;
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
        //Gizmos.DrawWireSphere(endPoint, radius);
    }
}
