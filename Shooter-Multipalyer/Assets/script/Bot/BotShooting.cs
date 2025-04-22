using UnityEngine;

public class BotShooting : MonoBehaviour
{
    public ParticleSystem MuzzleEffect;
    public ParticleSystem HitEffect;
    public TrailRenderer trail;
    public Transform originPoint;
    public Transform checkingPoint1;
    public LayerMask EnemyLayer;
    public float fireRate=0.3f;
    public float fireRange= 15;
    public BotMoves botMoves;
    public PlayAudio playAudio;
    public AudioClip gunSfx;
    private float lastFireTime=0;

    void Start()
    {
       
    }

  
    void Update()
    {
        if (botMoves.IsFire)
        {
            if (Time.time >= lastFireTime + fireRate)
            {
                lastFireTime = Time.time;
               
                Fire();

            }
        }
    }

    private void Fire()
    {
        if(Physics.Raycast(originPoint.position,originPoint.forward,out RaycastHit hitinfo,fireRange, EnemyLayer))
        {
           
          
                MuzzleEffect.Emit(1);
               playAudio.AudioPlay(gunSfx);
             

                HitEffect.transform.position = hitinfo.point;
                hitinfo.transform.forward = hitinfo.normal;
                HitEffect.Emit(1);

                var Trail = Instantiate(trail, originPoint.position, Quaternion.identity);
                Trail.AddPosition(originPoint.position);
                trail.transform.position = hitinfo.point;

                Health health = hitinfo.collider.GetComponentInParent<Health>();
                if (health != null)
                {
                    health.Damage(4);
                }
                BotHealth botHealth = hitinfo.collider.GetComponentInParent<BotHealth>();
                if (botHealth != null)
                {
                    botHealth.Damage(4);
                }


            
            

        }


    }
   public bool IsEnemy()
    {

       if(Physics.Raycast(checkingPoint1.position, checkingPoint1.forward,out RaycastHit hit, fireRange))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Soldier"))
            {

                return true;
            }
        }
        return false;

           
            
    }
}
