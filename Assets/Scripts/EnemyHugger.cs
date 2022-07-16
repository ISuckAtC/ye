using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHugger : Enemy
{
    public float speed;
    public float initialSpeed;
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerController pController;

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
    public bool grabbingPlayer;
    bool checkGrab;
    float grabCooldown = 0f;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        pController = player.GetComponent<PlayerController>();

        agent.speed = speed;
        initialSpeed = pController.speed;
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


        if (checkGrab)
        {
            grabCooldown -= Time.deltaTime;
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

        

        if (grabCooldown <= 0)
        {
            checkGrab = false;

            agent.SetDestination(transform.position);

            transform.LookAt(player.transform.position);


            if (!didAttack)
            {

                if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 2)
                {

                    //GRAB PLAYER
                    //MAYBE REMOVE THE ABIITY TO MOVE
                    //CHANGE SPEED TO 0?
                    pController.speed = 0.2f;

                    didAttack = true;
                    grabbingPlayer = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }
            }

        }

    }

    private void ResetAttack()
    {

        didAttack = false;
        grabbingPlayer = false;
        pController.speed = initialSpeed;
        checkGrab = true;
        grabCooldown = 2f;
    }
}
