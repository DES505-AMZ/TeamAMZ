using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Patrol,
    }
    public AIState aiState { get; private set; }

    public float patrolSpeed = 1f;

    CharacterNavBase navAgent;
    Animator animator;

    private void Awake()
    {
        navAgent = GetComponent<CharacterNavBase>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        SetStartAIState();
        navAgent.onReset += SetStartAIState;
    }

    void FixedUpdate()
    {
        UpdateCurrentAIState();

        if (animator)
        {
            animator.SetFloat(GameConstants.k_AnimParamNameMoveSpeed, navAgent.currentSpeed);
            animator.speed = navAgent.currentSpeed > 1f ? navAgent.currentSpeed : 1f;
        }
    }

    void UpdateCurrentAIState()
    {
        switch (aiState)
        {
            case AIState.Idle:
                break;
            case AIState.Patrol:
                navAgent.UpdatePathDestination();
                navAgent.SetNavDestination(navAgent.GetPatrolPathDestination());
                break;
        }
    }

    void SetStartAIState()
    {
        if (navAgent.IsPathValid())
        {
            navAgent.SetNavAgentMaxSpeed(patrolSpeed);
            aiState = AIState.Patrol;
        }
        else
        {
            //Debug.Log("idle");
            navAgent.SetNavAgentMaxSpeed(0f);
            aiState = AIState.Idle;
        }
    }
}
