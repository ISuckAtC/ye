using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBouncer : Enemy
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
            //ATTACK

            didAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        didAttack = false;
    }



}
