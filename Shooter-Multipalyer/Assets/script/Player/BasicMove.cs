using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class BasicMove : MonoBehaviour
{
    public PlayerInput playerInput;
    public Animator animator;
    private Vector2 smoothInput = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;
    public float smoothTime = 0.1f;
    public float Gravity = 3f;
    public GameObject Ui;
    public GameObject Camera;
    public CinemachineCamera TPPCamera;
    public CinemachineCamera TPPFireCamera;
    public MultiAimConstraint aimConstraint;
    public SpriteRenderer Marker;
    public Camera miniMapCamara;

    private ReloadAll reloadAll;
    private PlayerRotation playerRotation;
    private Vector3 position;
    private Inventory inventory;
    private CharacterController controller;
    private PlayAudio playAudio;
    private PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponentInParent<PhotonView>();
      
       

        playerRotation = GetComponent<PlayerRotation>();
        inventory = GetComponent<Inventory>();
        controller = GetComponent<CharacterController>();
        reloadAll =  GetComponent<ReloadAll>();
        playAudio = GetComponent<PlayAudio>();

        if (!photonView.IsMine)
        {
            Ui.SetActive(false);
            playerInput.enabled = false;
            TPPCamera.enabled = false;
            TPPCamera.Priority = 0;
            TPPFireCamera.Priority = 0;
            Camera.SetActive(false);
            aimConstraint.weight = 0;
            Marker.color = Color.red;
            miniMapCamara.enabled = false;
        }
        else
        {
            TPPCamera.enabled = true;
            TPPCamera.Priority = 1;
            TPPFireCamera.Priority = 2;
            Camera.SetActive(true);
            aimConstraint.weight = 1;
            miniMapCamara.enabled = true;
        }



    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        position.y = Gravity * Time.deltaTime;
        controller.Move(position);
        OnMove();

        
    }

    

    private void OnMove()
    {
        Vector2 movementInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Smooth transition
        smoothInput.x = Mathf.SmoothDamp(smoothInput.x, movementInput.x, ref currentVelocity.x, smoothTime);
        smoothInput.y = Mathf.SmoothDamp(smoothInput.y, movementInput.y, ref currentVelocity.y, smoothTime);


        // Animator values
        animator.SetFloat("x", smoothInput.x);
        animator.SetFloat("z", smoothInput.y);

        // Footstep sounds
        if (Mathf.RoundToInt(movementInput.x) != 0 || Mathf.RoundToInt(movementInput.y) != 0)
        {
            playAudio.FootStepSoundPlay();
        }
        else
        {
            playAudio.FootStepSoundPause();
        }
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
