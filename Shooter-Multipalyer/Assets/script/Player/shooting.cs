using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
public class shooting : MonoBehaviour
{
    public float aimDuration=0.3f;
    public GunType gunType;
    public CinemachineCamera FireCamara;
    public GameObject FirstPersonCamara;
    public CinemachineCamera FirstPersonCamaraFov;
    public GameObject Scope;
    public CinemachineImpulseSource Recoil;
    public Rig RigLayer;
    public Transform FireOriginPoint;
    public Transform CrossTargetPoint;
    public ParticleSystem muzzlEffenct;
    public ParticleSystem HittEffect;
    public ParticleSystem BlodEffect;
    public TrailRenderer trail;

   

    Ray ray;
    RaycastHit hitinfo;

    private PlayAudio playAudio;
    private InputAction FireAction;
    public PlayerInput playerInput;
    public ReloadAll reloadAll;
    private float LastFireTime = 0;

    private void Awake()
    {
        playAudio = GetComponentInParent<PlayAudio>();
        reloadAll.MaxAmmo =gunType.Maxammo;
        FireAction = playerInput.actions["Fire"];
    }

    void Start()
    {
        reloadAll.CurrentAmmo = 0;
    }

    
    void Update()
    {
        ScopeOn();
        ray.origin = FireOriginPoint.position;
        ray.direction = CrossTargetPoint.position - FireOriginPoint.position;


       
        
        if(IsFire() && reloadAll.CanShoot())
        {
            FireCamara.enabled = true;
          
           
            RigLayer.weight += Time.deltaTime / aimDuration;

            if(Time.time >= LastFireTime + gunType.FireRate)
            {
                LastFireTime = Time.time;
                reloadAll.CurrentAmmo--;
                muzzlEffenct.Emit(1);
                Recoil.GenerateImpulse();

                if(gunType.IsShotGun)
                {
                    ShotGen();
                }
                else
                {
                    SingleBullet(ray);
                }
            }
           
        }
        else
        {
            FireCamara.enabled = false;

            if(!FirstPersonCamara.activeInHierarchy)
            {
                RigLayer.weight -= Time.deltaTime / aimDuration;
            }
           
        }
    }

    private void SingleBullet(Ray BulletRay)
    {
        if (Physics.Raycast(BulletRay, out hitinfo, gunType.FireDistance) && RigLayer.weight == 1)
        {


            playAudio.AudioPlay(gunType.gunSfx);

           var Trail = Instantiate(trail, ray.origin, Quaternion.identity);
            Trail.AddPosition(ray.origin);
            Trail.transform.position = hitinfo.point;

            BotHealth bothealth = hitinfo.collider.GetComponentInParent<BotHealth>();
            HitBox hitBox = hitinfo.collider.GetComponent<HitBox>();
            if(bothealth!= null && hitBox !=null)
            {
                int damage = Mathf.RoundToInt(gunType.Damage * hitBox.DamageMultiplier);
                bothealth.Damage(damage);
                BlodEffect.transform.position = hitinfo.point;
                BlodEffect.transform.forward = hitinfo.normal;
                BlodEffect.Emit(1);
            }
            else
            {
                HittEffect.transform.position = hitinfo.point;
                HittEffect.transform.forward = hitinfo.normal;
                HittEffect.Emit(1);
            }
        }
    }
    private void ShotGen()
    {
        int PelletCout = 8;
        float Angle = 5f;

        for (int i = 0; i < PelletCout; i++)
        {
            Vector3 SpreadDirection = Direction(ray.direction, Angle);
            Ray PelletRay = new Ray(ray.origin, SpreadDirection);

            SingleBullet(PelletRay);
        }

    }

    private Vector3 Direction(Vector3 OriginalDirection, float Angle)
    {
        float randomX = Random.Range(Angle, -Angle);
        float randomY = Random.Range(Angle, -Angle);
        Quaternion SpreadRotation = Quaternion.Euler(randomX, randomY, 0);

        return SpreadRotation * OriginalDirection;
    }

    private bool IsFire()
    {
        return FireAction.IsPressed();
    }

    private void ScopeOn()
    {
        if (FirstPersonCamara.activeInHierarchy && gunType.IsSniper)
        {
            Scope.SetActive(true);
            FirstPersonCamaraFov.Lens.FieldOfView = 5f;
        }
        else
        {
            Scope.SetActive(false);
            FirstPersonCamaraFov.Lens.FieldOfView = 50f;
        }

    }



}
