using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void PhotoAction(Ray ray);
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("Player Camera")]
    public Camera playerCamera;
    public float freezeSeconds = 0.3f;
    
    [Header("Camera Rotation")]
    public float rotationSpeed = 200f;
    [Range(0.1f, 1f)]
    public float aimingRotationMultiplier = 0.4f;
    
    [Header("Camera Shake")]
    public float MaxShakeRange = 0.5f;
    public float shakeSpeed = 1f;
    [Range(0.1f, 1f)]
    public float shakeInterval = 0.3f;
    public float shakeSpeedMultiplier { get; set; }

    [Header("Movement")]
    public float maxWalkSpeed = 10f;
    public float movementSharpness = 15;
    [Range(0, 1)]
    public float crouchedSpeedRatio = 0.5f;

    [Header("Character Height")]
    [Range(0.1f, 1f)]
    public float cameraHeightRatio = 0.9f;
    public float capsuleHeightStanding = 2f;
    public float capsuleHeightCrouching = 1f;
    public float crouchingSharpness = 10f;
    public Vector3 headPosition
    {
        get
        {
            return playerCamera.transform.position;
        }
    }

    [Header("Interaction")]
    public float maxInteractionDistance = 5f;
    public LayerMask interactableLayers;

    public bool isCrouching { get; private set; }

    public float RotationMultiplier
    {
        get
        {
            if (m_isAiming)
            {
                return aimingRotationMultiplier;
            }

            return 1f;
        }
    }
  
    PlayerInputHandler m_inputHandler;
    Rigidbody m_rigidbody;
    CapsuleCollider m_collider;
    float m_playerHorizontalAngle;
    float m_cameraVerticalAngle;
    bool m_isAiming;
    float m_targetCharacterHeight;
    Vector3 m_characterVelocity;
    Interactable m_interactableObject;
    float m_shakeRange = 0.5f;

    public event PhotoAction onPhoto;
    public UnityAction onCaught;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_collider = GetComponent<CapsuleCollider>();
        m_inputHandler = GetComponent<PlayerInputHandler>();

        m_isAiming = false;

        SetCrouchingState(false);
        UpdateCharacterHeight(false);

        StartCoroutine(SetCameraShakingState());
    }

    public void Restart(Transform location)
    {
        transform.position = new Vector3(location.position.x, transform.position.y, location.position.z);
        transform.rotation = location.rotation;

        m_isAiming = false;
        SetCrouchingState(false);
        UpdateCharacterHeight(false);
    }

    void Update()
    {
        if(m_inputHandler.GetCrouchInputDown())
        {
            SetCrouchingState(!isCrouching);
        }

        UpdateCharacterHeight(true);
        ShakeCamera();
        HandleLookRotation();
        CameraMode();
        TakePhoto();

        if(GetInteractableObject())
        {
            InteractWithObject();
        } 
    }

    void FixedUpdate()
    {
        

        HandleMovement();
    }

    void HandleLookRotation()
    {
        // horizontal character rotation
        transform.Rotate(new Vector3(0f, m_inputHandler.GetLookInputsHorizontal() * rotationSpeed * RotationMultiplier, 0f), Space.Self);

        // vertical camera rotation
        m_cameraVerticalAngle -= m_inputHandler.GetLookInputsVertical() * rotationSpeed * RotationMultiplier;
        m_cameraVerticalAngle = Mathf.Clamp(m_cameraVerticalAngle, -70f, 70f);
        playerCamera.transform.localEulerAngles = new Vector3(m_cameraVerticalAngle, 0f, 0f);
    }

    void HandleMovement()
    {
        Vector3 moveInput = m_inputHandler.GetMoveInput();

        Vector3 targetVelocity = transform.forward * moveInput.z * maxWalkSpeed + transform.right * moveInput.x * maxWalkSpeed;
        if (isCrouching)
            targetVelocity *= crouchedSpeedRatio;

        m_characterVelocity = Vector3.Lerp(m_characterVelocity, targetVelocity, movementSharpness * Time.deltaTime);
        m_characterVelocity.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = m_characterVelocity;
    }

    void SetCrouchingState(bool crouched)
    {
        if(crouched)
        {
            m_targetCharacterHeight = capsuleHeightCrouching;
        }
        else
        {
            m_targetCharacterHeight = capsuleHeightStanding;
        }
        isCrouching = crouched;
    }

    void UpdateCharacterHeight(bool smooth)
    {
        if(!smooth)
        {
            m_collider.height = m_targetCharacterHeight;
            m_collider.center = Vector3.up * m_collider.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.up * m_targetCharacterHeight * cameraHeightRatio;
        }
        else if (m_collider.height != m_targetCharacterHeight)
        {
            // resize the capsule and adjust camera position
            m_collider.height = Mathf.Lerp(m_collider.height, m_targetCharacterHeight, crouchingSharpness * Time.deltaTime);
            m_collider.center = Vector3.up * m_collider.height * 0.5f;
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, Vector3.up * m_targetCharacterHeight * cameraHeightRatio, crouchingSharpness * Time.deltaTime);            
        }
    }

    bool GetInteractableObject()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, maxInteractionDistance))
        {
            m_interactableObject = hit.collider.GetComponent<Interactable>();
            if (m_interactableObject)
                m_interactableObject.OnLookAt();
            return true;
        }
        else
            m_interactableObject = null;
        return false;
    }

    void InteractWithObject()
    {
        if (!m_isAiming && m_interactableObject && m_inputHandler.GetInteractInputDown())
        {
            m_interactableObject.OnInteraction(new Ray(playerCamera.transform.position, playerCamera.transform.forward));
            m_interactableObject = null;
        }
    }

    void CameraMode()
    {
        if(m_inputHandler.GetAimInputDown())
        if (!m_isAiming)
        {
            m_isAiming = true;
                UIManager.Instance.SetCameraCanvasVisible(true);

        }
        else if (m_isAiming)
        {
            m_isAiming = false;
                UIManager.Instance.SetCameraCanvasVisible(false);
            }
    }

    void TakePhoto()
    {
        if(m_isAiming && m_inputHandler.GetInteractInputDown())
        {
            StartCoroutine(FreezeCamera(freezeSeconds));
            // check whether the photo is evidence
            if (onPhoto != null)
            {
                onPhoto(new Ray(playerCamera.transform.position, playerCamera.transform.forward));
            }
        }
    }

    IEnumerator FreezeCamera(float time)
    {
        Time.timeScale = 0f;
        m_inputHandler.canProcessInput = false;

        float waitCounter = 0;
        while(waitCounter < time)
        {
            waitCounter += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        m_inputHandler.canProcessInput = true;
    }

    IEnumerator SetCameraShakingState()
    {
        m_shakeRange = MaxShakeRange;
        while(true)
        {
            m_shakeRange = -m_shakeRange;
            yield return new WaitForSeconds(shakeInterval);
        }
    }

    void ShakeCamera()
    {
        playerCamera.transform.localPosition = new Vector3(
                Mathf.MoveTowards(playerCamera.transform.localPosition.x, m_shakeRange * shakeSpeedMultiplier, shakeSpeed * Time.deltaTime),
                playerCamera.transform.localPosition.y,
                playerCamera.transform.localPosition.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (onCaught != null)
                onCaught();
        }
    }
}
