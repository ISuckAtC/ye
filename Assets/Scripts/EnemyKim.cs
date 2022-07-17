using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKim : MonoBehaviour
{
    public enum States
    {
        Idle,
        Chase,
        Attack,
        Wait
    }


    public float waitTimeMin, waitTimeMax;
    public float speed;
    public float sightRange, attackRange;
    public GameObject minion;
    public GameObject fartProjectile;
    private NavMeshAgent agent;
    private GameObject player;
    public States state;
    private float waitTime;
    private Animator animator;


    public int clapDamage, fartDamage;

    public float clapCooldown, fartCooldown, spawnCooldown;
    public float fartSprayRadius;
    public float fartProjectileCount;
    public float fartProjectileLifetime;

    public float minionSpawnCount;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == States.Wait)
        {
            if (waitTime <= 0)
            {
                state = States.Idle;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (state == States.Wait) return;
        if (state == States.Attack) return;




        if (state == States.Chase)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > sightRange)
            {
                agent.isStopped = true;
                player = null;
                state = States.Idle;
            }
            else
            {
                agent.SetDestination(player.transform.position);
                agent.isStopped = false;
            }

            if (player && Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                state = States.Attack;
                agent.isStopped = true;
                int rand = Random.Range(0, 3);
                switch (rand)
                {
                    case 0:
                        Debug.Log("Kim klap attakk");
                        player.GetComponent<PlayerController>().TakeDamage(clapDamage);
                        

                        // run next code after attack animation finishes (in animation state script or callback)

                        waitTime = clapCooldown;
                        state = States.Wait;

                        break;
                    case 1:
                        Debug.Log("Kim brapp");
                        Vector3 directionToPlayer = player.transform.position - transform.position;
                        directionToPlayer = directionToPlayer.normalized;

                        for (int i = 0; i < fartProjectileCount; i++)
                        {
                            Vector3 fartDirection = Quaternion.Euler(Random.Range(-fartSprayRadius, fartSprayRadius), Random.Range(-fartSprayRadius, fartSprayRadius), 0) * directionToPlayer;
                            GameObject fart = Instantiate(fartProjectile, transform.position, Quaternion.identity);
                            fart.GetComponent<Rigidbody>().AddForce(fartDirection, ForceMode.VelocityChange);
                            Destroy(fart, fartProjectileLifetime);
                        }


                        // run next code after attack animation finishes (in animation state script or callback)

                        waitTime = fartCooldown;
                        state = States.Wait;


                        break;
                    case 2:
                        Debug.Log("Kim orbiter raid");
                        for (int i = 0; i < minionSpawnCount; i++)
                        {
                            GameObject spawn = Instantiate(minion, transform.position, Quaternion.identity);
                        }


                        // run next code after attack animation finishes (in animation state script or callback)

                        waitTime = spawnCooldown;
                        state = States.Wait;
                        
                        
                        break;
                }
            }
        }
        else
        {
            Collider[] overlaps = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
            if (overlaps.Length > 0)
            {
                player = overlaps[0].gameObject;
                state = States.Chase;
            }
        }
        
    }
}
