using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public ParticleSystem ExplosionFx;
    public float ExplosionTime = 4f;
    public float ExplosionRadius = 5f;
    public float ExplosionFore = 100f;
    public AudioSource ExplosionSfx;
    void Start()
    {
        ExplosionFx.Pause();
        Invoke("Explosion", ExplosionTime);
    }

    
    void Update()
    {
        
    }

    private void Explosion()
    {
        ExplosionFx.Play();
        ExplosionSfx.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        
        foreach(Collider obj in colliders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionFore, transform.position, ExplosionRadius);
            }

            Health health = obj.GetComponent<Health>();
            if(health != null)
            {
                health.Damage(100);
            }
            BotHealth botHealth = obj.GetComponent<BotHealth>();
            if(botHealth != null)
            {
                botHealth.Damage(100);
            }
        }


        Destroy(gameObject, 6f);
    }
}
