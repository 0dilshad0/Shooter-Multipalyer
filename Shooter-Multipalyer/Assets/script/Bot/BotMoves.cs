
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class BotMoves : MonoBehaviour
{
   
    public float AttackinRange = 10f;
    public float AttackinRangeShot = 4f;
    public float followingRange = 20f;
    public float aimDuration = 0.3f;
    public Rig rigLayer;
    public Transform AimTargetPoint;
    public BotShooting botShooting;

    public bool IsFire;

    private PlayAudio playAudio;
    private Transform CurrentTarget = null;
    private NavMeshAgent agent;
    private Animator animator;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playAudio = GetComponent<PlayAudio>();
        animator = GetComponent<Animator>();
        InvokeRepeating("FindTarget", 0,1.5f);
    }

    
    void Update()
    {

        
        if(CurrentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, CurrentTarget.position);
            Vector3 lookPoint = CurrentTarget.position + Vector3.up * 1.5f;
            AimTargetPoint.position = lookPoint; 
           
            if(distance <= AttackinRangeShot)
            {
                playAudio.FootStepSoundPause();
                IsFire = true;
                agent.SetDestination(CurrentTarget.position);
                animator.SetFloat("x", 0f);


                transform.LookAt(CurrentTarget.position);
            }
            else if (distance <= AttackinRange && botShooting.IsEnemy())
            {
                playAudio.FootStepSoundPause();
                IsFire = true;
                agent.SetDestination(CurrentTarget.position);
                animator.SetFloat("x", 0f);


                transform.LookAt(CurrentTarget.position);

            }
            else if(distance <=followingRange)
            {
                playAudio.FootStepSoundPlay();
                agent.SetDestination(CurrentTarget.position);
                animator.SetFloat("x", 1f);
                IsFire = false;
            }
            else
            {
                playAudio.FootStepSoundPause();
                CurrentTarget = null;
                animator.SetFloat("x", 0f);
                IsFire = false;
            }

            Aim();
        }
      
          
      

    }

    private void FindTarget()
    {
        
        CurrentTarget = null;
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("player");
        float ClosestDistance = Mathf.Infinity;
        Transform bestTarget = null;

        foreach(GameObject player in allPlayers)
        {
            if (player == this.gameObject)
                continue;

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance<ClosestDistance)
            {
                ClosestDistance = distance;
                bestTarget = player.transform;
            }
        }


        CurrentTarget = bestTarget;

    }
    private void Aim()
    {
        if (IsFire)
        {
            rigLayer.weight += Time.deltaTime / aimDuration;
            
        }
        else
        {
            rigLayer.weight -= Time.deltaTime / aimDuration;
        }
    }
}
