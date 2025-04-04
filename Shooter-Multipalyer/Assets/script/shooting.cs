using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
public class shooting : MonoBehaviour
{
    public float aimDuration=0.3f;
    public Rig RigLayer;
    public Transform FireOriginPoint;
    public Transform CrossTargetPoint;
    public ParticleSystem muzzlEffenct;
    public ParticleSystem HittEffect;

    Ray ray;
    RaycastHit hitinfo;
    private InputAction FireAction;
    private PlayerInput playerInput;
    

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        FireAction = playerInput.actions["Fire"];
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        ray.origin = FireOriginPoint.position;
        ray.direction = CrossTargetPoint.position - FireOriginPoint.position;



        if(IsFire())
        {
           
            RigLayer.weight += Time.deltaTime / aimDuration;

            if(Physics.Raycast(ray,out hitinfo) && RigLayer.weight==1)
            {
                Debug.DrawRay(ray.origin, hitinfo.point, Color.red, 20.0f);
                muzzlEffenct.Emit(1);

                HittEffect.transform.position = hitinfo.point;
                HittEffect.transform.forward = hitinfo.normal;
                HittEffect.Emit(1);
            }
        }
        else
        {
            RigLayer.weight -= Time.deltaTime / aimDuration;
        }
    }

    private bool IsFire()
    {
        return FireAction.IsPressed();
    }

    
}
