using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        Seek,
        Chase,
    }
    public AIState aiState { get; private set; }
    public NavMeshAgent m_navMeshAgent { get; private set; }

    public PatrolPath patrolPath { get; set; }

    Animator animator;

    [Header("Movement")]
    public float patrolSpeed = 1f;
    public float chaseSpeed = 2f;
    public float currentSpeed
    {
        get
        {
            return m_navMeshAgent.velocity.magnitude;
        }
    }

    [Header("Patrol")]
    public float pathReachingRadius = 3f;
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
    PlayerController targetPlayer;

    Transform nearbyTarget;
    bool isSeeingTarget;
    int m_patrolNodeIndex;

    float distance;

    void Start()
    {
        targetPlayer = FindObjectOfType<PlayerController>();
        eyePoint = transform.GetChild(0);
        if (eyePoint == null)
            eyePoint = transform;

        m_navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        aiState = AIState.Patrol;
        SetPathDestinationToClosestNode();
    }

    void FixedUpdate()
    {
        DetectTarget();
        UpdateAIStateTransitions();
        UpdateCurrentAIState();
        SetNavAgentMaxSpeed(maxMoveSpeed);

        if (animator)
        {
            animator.SetFloat("MoveSpeed", currentSpeed);
            animator.speed = currentSpeed > 1f ? currentSpeed : 1f;
        }
    }

    void UpdateAIStateTransitions()
    {
        switch (aiState)
        {
            case AIState.Patrol:
                if(isSeeingTarget)
                {
                    aiState = AIState.Chase;
                }
                break;
            case AIState.Seek:
                if(isSeeingTarget)
                {
                    aiState = AIState.Chase;
                }
                break;
            case AIState.Chase:
                if (!isSeeingTarget)
                {
                    if (nearbyTarget != null)
                    {
                        aiState = AIState.Seek;
                        // move to the latest targetPlayer position this enemy saw
                        SetNavDestination(nearbyTarget.position);
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
                    LookOrientTowards(targetPlayer.transform.position);
                    StartCoroutine(SeekTarget());
                }
                break;
            case AIState.Chase:
                SetNavDestination(targetPlayer.transform.position);
                LookOrientTowards(targetPlayer.transform.position);
                break;
        }
    }

    IEnumerator SeekTarget()
    {
        yield return new WaitForSeconds(lostTargetTimeout);
        if (!isSeeingTarget)
        {
            aiState = AIState.Patrol;
            LookOrientTowards(eyePoint.position + transform.forward);
            SetPathDestinationToClosestNode();           
        }
    }

    void DetectTarget()
    {
        isSeeingTarget = false;
        float dist = Vector3.Distance(targetPlayer.headPosition, eyePoint.position);
        if (dist < sightRange)
        {
            Vector3 targetDir = targetPlayer.headPosition - eyePoint.position;
            float degree = Vector3.Angle(targetDir, eyePoint.forward);
            distance = degree;
            if(degree < sightIncludedAngle && degree > -sightIncludedAngle)
            {
                RaycastHit hit;
                if(Physics.Raycast(eyePoint.position, targetDir, out hit, sightRange))
                {                   
                    if(hit.collider.gameObject == targetPlayer.gameObject)
                    {
                        isSeeingTarget = true;
                        nearbyTarget = targetPlayer.transform;
                    }
                }
            }
        }
    }

    void LookOrientTowards(Vector3 lookPosition)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - eyePoint.position, Vector3.up).normalized;
        if (lookDirection.sqrMagnitude != 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            eyePoint.rotation = Quaternion.Slerp(eyePoint.rotation, targetRotation, orientationSpeed);
        }
    }

    void SetNavDestination(Vector3 destination)
    {
        if (m_navMeshAgent)
        {
            m_navMeshAgent.SetDestination(destination);
        }
    }

    void SetNavAgentMaxSpeed(float speed)
    {
        m_navMeshAgent.speed = speed;
    }

    bool DestinationArrived()
    {
        return !m_navMeshAgent.pathPending && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance;
    }

    bool IsPathValid()
    {
        return patrolPath && patrolPath.pathNodes.Count > 0;
    }

    void UpdatePathDestination(bool inverseOrder = false)
    {
        if (IsPathValid())
        {
            float dist = (transform.position - patrolPath.GetPositionOfPathNode(m_patrolNodeIndex)).magnitude;
            if (dist <= pathReachingRadius)
            {
                m_patrolNodeIndex = inverseOrder ? (m_patrolNodeIndex - 1) : (m_patrolNodeIndex + 1);
                if (m_patrolNodeIndex < 0)
                {
                    m_patrolNodeIndex += patrolPath.pathNodes.Count;
                }
                if (m_patrolNodeIndex >= patrolPath.pathNodes.Count)
                {
                    m_patrolNodeIndex -= patrolPath.pathNodes.Count;
                }

                
            }
        }
    }

    void SetPathDestinationToClosestNode()
    {
        if(IsPathValid())
        {
            int closestNodeIndex = 0;
            for(int i = 0; i < patrolPath.pathNodes.Count; ++i)
            {
                float dist = patrolPath.GetDistanceToNode(transform.position, i);
                if (dist < patrolPath.GetDistanceToNode(transform.position, closestNodeIndex))
                    closestNodeIndex = i;
            }

            m_patrolNodeIndex = closestNodeIndex;
        }
        else
        {
            m_patrolNodeIndex = 0;
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
            return targetPlayer.transform.position;
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
