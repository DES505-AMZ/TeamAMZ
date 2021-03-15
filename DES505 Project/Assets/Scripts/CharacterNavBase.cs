using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    protected int m_patrolNodeIndex;
    protected RespawnController m_respawnController;

    protected virtual void Start()
    {        
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetPathDestinationToClosestNode();

        m_respawnController = GetComponent<RespawnController>();
        if(m_respawnController != null)
            m_respawnController.onRespawn += OnRespawn;
    }

    public virtual void OnRespawn()
    {
        SetPathDestinationToClosestNode();
    }

    protected void SetNavDestination(Vector3 destination)
    {
        if (navMeshAgent)
        {
            navMeshAgent.SetDestination(destination);
        }
    }

    protected void SetNavAgentMaxSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    protected bool DestinationArrived()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    protected bool IsPathValid()
    {
        return patrolPath && patrolPath.pathNodes.Count > 0;
    }

    protected void UpdatePathDestination(bool inverseOrder = false)
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

    protected void SetPathDestinationToClosestNode()
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
}
