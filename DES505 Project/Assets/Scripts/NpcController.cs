using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : CharacterNavBase
{
    public enum AIState
    {
        Idle,
        Patrol,
    }
    public AIState aiState { get; private set; }

    public float patrolSpeed = 1f;

    Animator m_animator;

    protected override void Start()
    {
        base.Start();

        m_animator = GetComponent<Animator>();

        SetStartAIState();
    }

    public override void OnRespawn()
    {
        base.OnRespawn();

        SetStartAIState();
    }

    void FixedUpdate()
    {
        UpdateCurrentAIState();

        if (m_animator)
        {
            m_animator.SetFloat("MoveSpeed", currentSpeed);
            m_animator.speed = currentSpeed > 1f ? currentSpeed : 1f;
        }
    }

    void UpdateCurrentAIState()
    {
        switch (aiState)
        {
            case AIState.Idle:
                break;
            case AIState.Patrol:
                UpdatePathDestination();
                SetNavDestination(patrolPath.GetPositionOfPathNode(m_patrolNodeIndex));
                break;
        }
    }

    void SetStartAIState()
    {
        if (IsPathValid())
        {
            SetNavAgentMaxSpeed(patrolSpeed);
            aiState = AIState.Patrol;
        }
        else
        {
            SetNavAgentMaxSpeed(0f);
            aiState = AIState.Idle;
        }
    }
}
