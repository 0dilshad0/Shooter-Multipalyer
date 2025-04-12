using UnityEngine;
using UnityEngine.InputSystem;

public class BasicMove : MonoBehaviour
{
    public PlayerInput playerInput;
    public Animator animator;
    private Vector2 smoothInput = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;
    public float smoothTime = 0.1f;
    public float Gravity = 3f;
    private ReloadAll reloadAll;
    private PlayerRotation playerRotation;
    private Vector3 position;
    private Inventory inventory;
    private CharacterController controller;
    private void Awake()
    {
        playerRotation = GetComponent<PlayerRotation>();
        inventory = GetComponent<Inventory>();
        controller = GetComponent<CharacterController>();
        reloadAll = reloadAll = GetComponent<ReloadAll>();
    }
    private void FixedUpdate()
    {
        position.y = Gravity * Time.deltaTime;
        controller.Move(position);
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

    public void Reload(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            reloadAll.Reloading();
        }
    }    

    public void Heal(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            reloadAll.Healing();
        }
    }
    public void Grenade(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            reloadAll.GrenadeThrow();
        }
    }
    public void Smoke(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            reloadAll.Smoke_Throw();
        }
    }
    public void Inventory(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inventory.InventoryONOFF();
        }
    }

    public void CamaraSwitch(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            playerRotation.SwitchCamara();
        }
    }
}
