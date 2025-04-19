
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class BotMoves : MonoBehaviour
{
   
    public float AttackinRange = 10f;
    public float followingRange = 20f;
    public float aimDuration = 0.3f;
    public Rig rigLayer;
    public Transform AimTargetPoint;
  

    public bool IsFire;

   
    private Transform CurrentTarget = null;
    private NavMeshAgent agent;
    private Animator animator;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
           
            if (distance <= AttackinRange)
            {
                agent.SetDestination(CurrentTarget.position);
                animator.SetFloat("x", 0f);
                IsFire = true;

                transform.LookAt(CurrentTarget.position);



            }
            else if(distance <=followingRange)
            {
                agent.SetDestination(CurrentTarget.position);
                animator.SetFloat("x", 1f);
                IsFire = false;
            }
            else
            {
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
