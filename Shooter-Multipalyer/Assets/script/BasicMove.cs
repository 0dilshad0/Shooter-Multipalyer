using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMove : MonoBehaviour
{
    public PlayerInput playerInput;
    public Animator animator;
    private Vector2 smoothInput = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;
    public float smoothTime = 0.1f; // Adjust this for smoother or faster transitions

    private void FixedUpdate()
    {
        OnMove();
    }

    private void OnMove()
    {
        Vector2 movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Smooth transition using SmoothDamp
        smoothInput.x = Mathf.SmoothDamp(smoothInput.x, movementInput.x, ref currentVelocity.x, smoothTime);
        smoothInput.y = Mathf.SmoothDamp(smoothInput.y, movementInput.y, ref currentVelocity.y, smoothTime);

        // Apply smoothed values to animator
        animator.SetFloat("x", smoothInput.x);
        animator.SetFloat("z", smoothInput.y);
    }
}
