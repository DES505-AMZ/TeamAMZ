using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public Animator animator;

    [Header("detection")]
    public Transform eyePoint;
    public float sightRange = 5f;
    [Range(1f, 90f)]
    public float sightIncludedAngle = 60f;
    public float lostTargetTimeout = 4f;
    public float orientationSpeed = 10f;
    public GameObject target;

    Transform nearbyTarget;
    bool isSeeingTarget;

    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();

        aiState = AIState.Patrol;
    }

    void Update()
    {
        DetectTarget();
        UpdateAIStateTransitions();
        UpdateCurrentAIState();
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
                        // move to the latest target position this enemy saw
                        SetNavDestination(nearbyTarget.position);
                    }
                    else
                        aiState = AIState.Patrol;
                }
                break;
        }
    }

    void UpdateCurrentAIState()
    {
        switch (aiState)
        {
            case AIState.Patrol:
                // TODO patrol movement

                break;
            case AIState.Seek:
                if(DestinationArrived())
                {
                    // TODO seek behaviour
                    LookOrientTowards(target.transform.position);
                    StartCoroutine(SeekTarget());
                }
                break;
            case AIState.Chase:
                SetNavDestination(target.transform.position);
                break;
        }
    }

    IEnumerator SeekTarget()
    {
        yield return new WaitForSeconds(lostTargetTimeout);
        if(!isSeeingTarget)
            aiState = AIState.Patrol;
    }

    void DetectTarget()
    {
        isSeeingTarget = false;
        float dist = Vector3.Distance(target.transform.position, transform.position);
        if (dist < sightRange)
        {
            Vector3 targetDir = target.transform.position - eyePoint.position;
            float degree = Vector3.Angle(targetDir, eyePoint.forward);

            if(degree < sightIncludedAngle && degree > -sightIncludedAngle)
            {
                RaycastHit hit;
                if(Physics.Raycast(eyePoint.position, targetDir, out hit, sightRange))
                {
                    if(hit.collider.gameObject.tag == "Player")
                    {
                        isSeeingTarget = true;
                        nearbyTarget = target.transform;
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
            eyePoint.rotation = Quaternion.Slerp(eyePoint.rotation, targetRotation, Time.deltaTime * orientationSpeed);
        }
    }

    void SetNavDestination(Vector3 destination)
    {
        if (m_navMeshAgent)
        {
            m_navMeshAgent.SetDestination(destination);
        }
    }

    bool DestinationArrived()
    {
        return !m_navMeshAgent.pathPending && m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance;
    }
}
