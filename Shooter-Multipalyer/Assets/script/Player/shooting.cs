using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
public class shooting : MonoBehaviour
{
    public float aimDuration=0.3f;
    public GunType gunType;
    public CinemachineCamera FireCamera;
    public GameObject FirstPersonCamera;
    public CinemachineCamera FirstPersonCameraFov;
    public GameObject Scope;
    public CinemachineImpulseSource Recoil;
    public Rig RigLayer;
    public Transform FireOriginPoint;
    public Transform CrossTargetPoint;
    
   

   

    Ray ray;
    RaycastHit hitinfo;

    private PlayAudio playAudio;
    private InputAction FireAction;
    public PlayerInput playerInput;
    public ReloadAll reloadAll;
    private GunFx gunFx;
    private float LastFireTime = 0;
    private PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponentInParent<PhotonView>();
        gunFx = GetComponentInParent<GunFx>();
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
        if (!photonView.IsMine) return;
        ScopeOn();
        ray.origin = FireOriginPoint.position;
        ray.direction = CrossTargetPoint.position - FireOriginPoint.position;


       
        
        if(IsFire() && reloadAll.CanShoot())
        {
            //FireCamera.enabled = true;
           
                           
            RigLayer.weight += Time.deltaTime / aimDuration;

            if(Time.time >= LastFireTime + gunType.FireRate)
            {
                LastFireTime = Time.time;
                reloadAll.CurrentAmmo--;
                gunFx.MuzzleFX();
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
            FireCamera.enabled = false;

            if(!FirstPersonCamera.activeInHierarchy)
            {
                RigLayer.weight -= Time.deltaTime / aimDuration;
            }
           
        }
    }

   

    private void SingleBullet(Ray BulletRay)
    {
        if (Physics.Raycast(BulletRay, out hitinfo, gunType.FireDistance) && RigLayer.weight == 1)
        {
            gunFx.Trail(ray.origin, hitinfo.point);

            playAudio.AudioPlay(gunType.gunSfx);

           

            BotHealth bothealth = hitinfo.collider.GetComponentInParent<BotHealth>();
            Health health = hitinfo.collider.GetComponentInParent<Health>();
            HitBox hitBox = hitinfo.collider.GetComponent<HitBox>();
            bool IsEnemy = false;
            if(hitBox !=null)
            {
                int damage = Mathf.RoundToInt(gunType.Damage * hitBox.DamageMultiplier);
                if(bothealth!=null)
                {
                    bothealth.Damage(damage);
                    IsEnemy = true;
                }
                else if(health != null)
                {
                    PhotonView targetPhotonView = health.GetComponent<PhotonView>();
                    if (targetPhotonView != null)
                    {
                       
                        targetPhotonView.RPC("ApplyDamage", targetPhotonView.Owner, damage,PhotonNetwork.LocalPlayer);
                        IsEnemy = true;
                    }
                   
                }
            }
            gunFx.HittFx(hitinfo.point, hitinfo.normal, IsEnemy);

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
        if (FirstPersonCamera.activeInHierarchy && gunType.IsSniper)
        {
            Scope.SetActive(true);
            FirstPersonCameraFov.Lens.FieldOfView = 5f;
        }
        else
        {
            Scope.SetActive(false);
            FirstPersonCameraFov.Lens.FieldOfView = 50f;
        }

    }



}
