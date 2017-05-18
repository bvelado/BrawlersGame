using UnityEngine;

public class CharacterModel : MonoBehaviour {

    Animator animator;
    [SerializeField]
    private ParticleSystem dashTrail;

    private float movementSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMovementSpeed(float movementVelocity)
    {
        //animator.SetFloat("MovementVelocity", movementVelocity);
        movementSpeed = movementVelocity;
    }

    public void SetVerticalVelocity(float verticalVelocity)
    {
        //animator.SetFloat("VerticalVelocity", verticalVelocity);
    }

    public void SetGroundedState(bool isGrounded)
    {
        //animator.SetBool("IsGrounded", isGrounded);
    }

    public void SetDashTrigger()
    {
        //animator.SetTrigger("Dash");

        if (!dashTrail.isPlaying)
            dashTrail.Stop();

        dashTrail.Play();
    }
}
