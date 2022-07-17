using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOfficer : Enemy
{

    public float speed;
    private NavMeshAgent agent;
    private GameObject player;

    [SerializeField] LayerMask groundLayer, playerLayer;

    //Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack
    bool didAttack;
    public float timeBetweenAttacks;

    //Check
    public float sightRange, attackRange;
    public bool inSightRange, inAttackRange;

    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    public float bulletSpeed;
    public float bulletLifeTime;
    public LayerMask enemyLayers;
    public float ballRadius;
    private GameObject bulletClone;
    int help = 0;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        agent.speed = speed;
    }

    public void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!inSightRange && !inAttackRange)
        {
            Patrol();
        }

        if (inSightRange && !inAttackRange)
        {
            Chase();
        }

        if (inAttackRange && inSightRange)
        {
            Attack();
        }


        if (didAttack)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(bulletClone.transform.position, ballRadius, enemyLayers);
            foreach (Collider pl in hitEnemies)
            {
                Debug.Log(pl.attachedRigidbody.name);
                //Check if it has rigidBody
                if (pl.attachedRigidbody != null)
                {
                    
                    if (LayerMask.LayerToName(pl.gameObject.layer) == "Player")
                    {
                        if (help == 0)
                        {
                            pl.GetComponent<PlayerController>().health -= 10;
                            help = 1;
                        }

                    }
                }


            }

        }
        


    }


    void Patrol()
    {
        if (!walkPointSet)
            RandomPatrol();

        if (walkPointSet)
        {
            agent.isStopped = false;
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToPointSet = transform.position - walkPoint;

        if (distanceToPointSet.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }


    void RandomPatrol()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }

    }

    private void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
    }

    private void Attack()
    {

        

        agent.SetDestination(transform.position);

        transform.LookAt(player.transform.position);


        if (!didAttack)
        {

            bulletClone = Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);


            bulletClone.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.transform.forward * bulletSpeed;


            Destroy(bulletClone, bulletLifeTime);

            didAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        didAttack = false;
        help = 0;
    }

}
