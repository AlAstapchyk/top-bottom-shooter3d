using UnityEngine.Animations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator animator;
    private PlayerMovement movement;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();
        movement.OnPlayerMove.AddListener(SetRunningTrue);
        movement.OnPlayerStop.AddListener(SetRunningFalse);
        movement.OnPlayerStrafe.AddListener(SetStrafeTrue);
    }

    void SetRunningTrue()
    {
        animator.SetBool("Run", true);
    }

    void SetRunningFalse()
    {
        animator.SetBool("Run", false);
    }

    void SetStrafeFalse()
    {
        animator.SetBool("Strafe", false);
    }

    void SetStrafeTrue()
    {
        Debug.Log("Strafing now");
        animator.SetBool("Strafe", true);
    }

    void Update()
    {
        
        
    }
}
