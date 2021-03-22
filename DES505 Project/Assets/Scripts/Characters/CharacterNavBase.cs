using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

//[RequireComponent(typeof(NavMeshAgent), typeof(RespawnController))]
public class CharacterNavBase : MonoBehaviour
{
    public float pathReachingRadius = 3f;

    public NavMeshAgent navMeshAgent { get; private set; }

    public PatrolPath patrolPath { get; set; }

    public float currentSpeed
    {
        get
        {
            return navMeshAgent.velocity.magnitude;
        }
    }

    public UnityAction onReset;

    protected int m_patrolNodeIndex;
    protected RespawnController m_respawnController;

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {        
        SetPathDestinationToClosestNode();

        m_respawnController = GetComponent<RespawnController>();
        if(m_respawnController != null)
            m_respawnController.onRespawn += OnRespawn;
    }

    public virtual void OnRespawn()
    {
        SetPathDestinationToClosestNode();
        if (onReset != null)
            onReset();
    }

    public void SetNavDestination(Vector3 destination)
    {
        if (navMeshAgent)
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    public void SetNavAgentMaxSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    public bool DestinationArrived()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    public bool IsPathValid()
    {
        return patrolPath && patrolPath.pathNodes.Count > 0;
    }

    public void UpdatePathDestination(bool inverseOrder = false)
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

    public void SetPathDestinationToClosestNode()
    {
        if (IsPathValid())
        {
            int closestNodeIndex = 0;
            for (int i = 0; i < patrolPath.pathNodes.Count; ++i)
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

    public Vector3 GetPatrolPathDestination()
    {
        if (IsPathValid())
        {
            return patrolPath.GetPositionOfPathNode(m_patrolNodeIndex);
        }
        return Vector3.zero;
    }
}
