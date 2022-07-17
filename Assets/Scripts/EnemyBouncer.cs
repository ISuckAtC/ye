using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBouncer : Enemy
{

    public float speed;
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

    public float dashSpeed;
    public float dashTime;
    Vector3 rSphere;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        pController = player.GetComponent<PlayerController>();

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
        CancelInvoke(nameof(Dash));
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


        InvokeRepeating(nameof(Dash), 5f, 3f);
    }

    void Dash()
    {
        rSphere = transform.position + Random.insideUnitSphere * 1.5f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(rSphere, out hit, 10f, NavMesh.AllAreas))
        {
            
        }


        agent.isStopped = true;
        Debug.Log("dooodge");
        agent.Move(new Vector3(rSphere.x, hit.position.y, rSphere.z).normalized * dashSpeed * Time.deltaTime);
        //agent.Move(gameObject.transform.right * dashSpeed * Time.deltaTime);
        agent.isStopped = false;


    }

    private void Attack()
    {

        CancelInvoke(nameof(Dash));
        agent.SetDestination(transform.position);

        transform.LookAt(player.transform.position);


        if (!didAttack)
        {
            //ATTACK
            pController.TakeDamage(10);

            didAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        didAttack = false;
    }


}
