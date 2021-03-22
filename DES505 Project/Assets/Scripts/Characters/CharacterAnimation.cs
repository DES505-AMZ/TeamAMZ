using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public AudioClip[] footStepClips;
    Animator m_animator;
    CharacterNavBase m_characterNav;
    AudioSource m_audioSource;

    int footstepIndex = 0;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        
        m_audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        m_characterNav = GetComponent<CharacterNavBase>();
    }

    private void FixedUpdate()
    {
        if (m_animator)
        {
            m_animator.SetFloat(GameConstants.k_AnimParamNameMoveSpeed, m_characterNav.currentSpeed);
            m_animator.speed = m_characterNav.currentSpeed > 1f ? m_characterNav.currentSpeed : 1f;
        }
    }

    public void PlayFootStep()
    {
        if (footStepClips.Length > 0)
        {
            m_audioSource.PlayOneShot(footStepClips[footstepIndex]);
            footstepIndex = (footstepIndex + 1) / footStepClips.Length;
        }
    }
}
