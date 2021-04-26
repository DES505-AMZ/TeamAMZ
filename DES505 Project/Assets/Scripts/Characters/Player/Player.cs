using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public TannoySystem tannoy;

    [Header("Camera Rotation")]
    public float rotationSpeed = 200f;
    [Range(0.1f, 1f)] public float photoingRotationMultiplier = 0.4f;

    [Header("Movement")]
    public float maxWalkSpeed = 10f;
    public float movementSharpness = 15;
    [Range(0, 1)] public float crouchedSpeedRatio = 0.5f;
    [Range(1, 5)] public float runSpeedRatio = 2f;

    [Header("Character Height")]
    [Range(0.1f, 1f)] public float cameraHeightRatio = 0.9f;
    public float capsuleHeightStanding = 2f;
    public float capsuleHeightCrouching = 1f;
    public float crouchingSharpness = 10f;
 
    [Header("Sandy's Camera FX")]
    public AudioClip cameraFX;
    public Animator flashAnim;
    AudioSource audioS;

    [Header("Interaction")]
    public float maxInteractionDistance = 5f;
    public LayerMask interactableLayers;

    [Header("Make Noise")]
    public float noiseRange = 3f;
    public AnimationCurve noiseRangeCurve;
    [Range(0, 1)] public float crouchedNoiseRatio = 0.2f;
    [Range(1, 3)] public float runNoiseRatio = 1.2f;

    public NormalState normalState;
    public PhotoState photoState;
    public CaughtState caughtState;
    public PlayerStateBase actionState;

    public WalkState walkState;
    public CrouchState crouchState;
    public RunState runState;
    public PlayerStateBase movementState;

    public PlayerInputHandler inputHandler { get; set; }
    Rigidbody m_rigidbody;
    CapsuleCollider m_collider;

    float m_playerHorizontalAngle;
    float m_cameraVerticalAngle;

    Vector3 m_characterVelocity;
    Interactable m_interactableObject;
    Interactable m_preInteractableObject;

    public float currentRotationSpeed { get; set; }
    public float targetMoveSpeed { get; set; }
    public float characterHeight { get; set; }
    public float maxNoiseRange { get; set; }
    public float currentNoiseRange { get; set; }

    public UnityAction<Ray> onPhoto;
    public UnityAction onCaught;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_collider = GetComponent<CapsuleCollider>();
        inputHandler = GetComponent<PlayerInputHandler>();
        audioS = GetComponent<AudioSource>();
        tannoy = FindObjectOfType<TannoySystem>();

        normalState = new NormalState();
        photoState = new PhotoState();
        caughtState = new CaughtState();
        walkState = new WalkState();
        crouchState = new CrouchState();
        runState = new RunState();
    }

    private void Start()
    {
        UpdateCharacterHeight(false);

        actionState = normalState;
        movementState = walkState;
        movementState.Enter(this);
        actionState.Enter(this);
    }

    public void Initialize(Transform location = null)
    {
        if (location)
        {
            transform.position = new Vector3(location.position.x, location.position.y, location.position.z);
            transform.rotation = location.rotation;
        }

        ChangeStateMovement(walkState);
        ChangeStateAction(normalState);
    }

    private void Update()
    {
        movementState.UpdateLogic(this);
        actionState.UpdateLogic(this);        
    }

    private void FixedUpdate()
    {
        movementState.UpdatePhysics(this);
        actionState.UpdatePhysics(this);
        SetCurrentNoiseRange();
    }

    public void ChangeStateAction(PlayerStateBase newState)
    {
        actionState.Exit(this);
        actionState = newState;
        actionState.Enter(this);
    }

    public void ChangeStateMovement(PlayerStateBase newState)
    {
        movementState.Exit(this);
        movementState = newState;
        movementState.Enter(this);
    }

    public void HandleLookRotation()
    {
        // horizontal character rotation
        m_playerHorizontalAngle += inputHandler.GetLookInputsHorizontal() * currentRotationSpeed;
        transform.rotation = Quaternion.Euler(0f, m_playerHorizontalAngle, 0f);

        // vertical camera rotation
        m_cameraVerticalAngle -= inputHandler.GetLookInputsVertical() * currentRotationSpeed;
        m_cameraVerticalAngle = Mathf.Clamp(m_cameraVerticalAngle, -70f, 70f);
        playerCamera.transform.localRotation = Quaternion.Euler(m_cameraVerticalAngle, 0f, 0f);
    }

    public void HandleMovement()
    {
        Vector3 moveInput = inputHandler.GetMoveInput();
        Vector3 targetVelocity = transform.forward * moveInput.z * targetMoveSpeed + transform.right * moveInput.x * targetMoveSpeed ;

        m_characterVelocity = Vector3.Lerp(m_characterVelocity, targetVelocity, movementSharpness * Time.deltaTime);
        m_characterVelocity.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = m_characterVelocity;
    }

    public void UpdateCharacterHeight(bool smooth)
    {
        if (!smooth)
        {
            m_collider.height = characterHeight;
            m_collider.center = Vector3.up * m_collider.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.up * characterHeight * cameraHeightRatio;
        }
        else if (m_collider.height != characterHeight)
        {
            // resize the capsule and adjust camera position
            m_collider.height = Mathf.Lerp(m_collider.height, characterHeight, crouchingSharpness * Time.deltaTime);
            m_collider.center = Vector3.up * m_collider.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, Vector3.up * characterHeight * cameraHeightRatio, crouchingSharpness * Time.deltaTime);
        }
    }

    void SetCurrentNoiseRange()
    {
        currentNoiseRange = maxNoiseRange * m_characterVelocity.magnitude * 0.5f * noiseRangeCurve.Evaluate(m_characterVelocity.magnitude / targetMoveSpeed);
    }

    public bool GetInteractableObject()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, maxInteractionDistance))
        {
            m_interactableObject = hit.collider.GetComponent<Interactable>();
            if (m_preInteractableObject != null && m_interactableObject != m_preInteractableObject)
            {
                m_preInteractableObject.OnLookExit();
            }
            if (m_interactableObject)
            {
                m_interactableObject.OnLookAt();
            }
            m_preInteractableObject = m_interactableObject;
            return true;
        }
        else
        {
            if (m_interactableObject)
            {
                m_interactableObject.OnLookExit();
                m_interactableObject = null;
            }
        }
        return false;
    }

    public void InteractWithObject()
    {
        if (m_interactableObject && inputHandler.GetInteractInputDown())
        {
            m_interactableObject.OnInteraction(new Ray(playerCamera.transform.position, playerCamera.transform.forward));
            m_interactableObject = null;
        }
    }

    public void TakePhoto()
    {
        if (inputHandler.GetInteractInputDown())
        {
            audioS.PlayOneShot(cameraFX);
            flashAnim.SetTrigger("Flash");
            // check whether the photo is evidence
            if (onPhoto != null)
            {
                onPhoto(new Ray(playerCamera.transform.position, playerCamera.transform.forward));
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            caughtState.caughtPosition = collision.transform.position;
            ChangeStateAction(caughtState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, currentNoiseRange);
    }
}
