using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterPhysics : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField]
    [Tooltip("Movement speed, in units per frame")]
    private float movementSpeed;
    private float decelerationRate = 1.0f;
    [SerializeField]
    private float dashMultiplier = 3.0f;
    [SerializeField]
    private float dashCooldownTime = 1.0f;

    private float currentJumpTime;
    private float currentDashCooldown;
    private bool isDashing = false;
    private bool addDash = false;

    private RaycastHit hit;

    private Rigidbody rb;

    public bool IsInterrupted = false;

    private bool isGrounded;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            isGrounded = value;
        }
    }

    private CharacterModel characterModel;
    private CharacterModel _CharacterModel
    {
        get
        {
            if (characterModel == null && GetComponent<CharacterModel>() != null)
                characterModel = GetComponent<CharacterModel>();
            return characterModel;
        }
    }

    float velocityX;
    float velocityY;
    float velocityZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 movement)
    {
        if (!IsInterrupted)
        {
            var deceleration = isDashing ? 0f : (1 - Mathf.Max(Mathf.Abs(movement.x), Mathf.Abs(movement.y)));

            // Changes the orientation
            transform.LookAt(transform.position + movement);

            Vector2 movInput = new Vector2(movement.x, movement.z);
            Vector2 movInputPerFrame = (isDashing) ? 
                Mathf.Max((currentDashCooldown/dashCooldownTime), 1 / dashMultiplier) * movInput.normalized * movementSpeed * dashMultiplier * Time.deltaTime : 
                movInput.normalized * movementSpeed * Time.deltaTime;

            velocityX = movInputPerFrame.x;
            velocityZ = movInputPerFrame.y;

            //velocityX = (1 - deceleration) * movInputPerFrame.x + Mathf.Lerp(rb.velocity.x, 0f, deceleration * decelerationRate); //Mathf.Lerp(movInputPerFrame.x, 0f, deceleration);
            //velocityZ = (1 - deceleration) * movInputPerFrame.z + Mathf.Lerp(rb.velocity.z, 0f, deceleration * decelerationRate); //Mathf.Lerp(movInputPerFrame.y, 0f, deceleration);

            if (addDash)
                addDash = false;
        }
    }

    public void Dash()
    {
        if (!IsInterrupted)
        {
            if (!isDashing)
            {
                isDashing = true;
                addDash = true;
                currentDashCooldown = dashCooldownTime;

                if (_CharacterModel != null)
                {
                    _CharacterModel.SetDashTrigger();
                }
            }
        }
    }

    private void Update()
    {
        if (!IsInterrupted)
        {
            // Sends parameters to the model
            if (_CharacterModel != null)
            {
                _CharacterModel.SetGroundedState(IsGrounded);
                _CharacterModel.SetMovementSpeed(Mathf.Abs(velocityX) + Mathf.Abs(velocityZ));
                _CharacterModel.SetVerticalVelocity(velocityY);
            }

            if (isDashing)
            {
                currentDashCooldown -= Time.deltaTime;

                if (currentDashCooldown < 0)
                    isDashing = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsInterrupted)
        {
            rb.velocity = new Vector3(velocityX, velocityY, velocityZ);
        } else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
