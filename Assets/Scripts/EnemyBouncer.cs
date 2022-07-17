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
    public int punchDamage;

    public float dashSpeed;
    public float dashTime;
    Vector3 rSphere;
    int help = 0;
    float dashMethTime = 3f;
    private Animator animator;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        pController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

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


        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.right.normalized, Color.red);
        Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + -gameObject.transform.right.normalized, Color.red);

    }


    void Patrol()
    {
        animator.speed = 1f;
        animator.SetBool("isPunching", false);
        animator.SetBool("isWalking", true);

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
        animator.speed = 1f;
        animator.SetBool("isPunching", false);
        animator.SetBool("isWalking", true);

        agent.isStopped = false;
        agent.SetDestination(player.transform.position);

        if (dashMethTime > 0f)
        {
            dashMethTime -= Time.deltaTime;
        }
        else
        {
            Dash();
            dashMethTime = 3f;
        }

        //InvokeRepeating(nameof(Dash), 5f, 3f);
    }

    void Dash()
    {
        rSphere = transform.position + Random.insideUnitSphere * 1.5f;
        rSphere.Normalize();

        NavMeshHit hit;

        if (NavMesh.SamplePosition(rSphere, out hit, 10f, NavMesh.AllAreas))
        {
            
        }


        //agent.isStopped = true;
        //var step = 2f * Time.deltaTime;
        Vector3 randomHorizontal = Random.Range(0.0f, 1.0f) <= 0.5 ? gameObject.transform.right.normalized * 2 : -gameObject.transform.right.normalized * 2;

        //agent.Warp(Vector3.MoveTowards(gameObject.transform.position, Random.Range(0, 1) == 0 ? gameObject.transform.position + gameObject.transform.right.normalized : gameObject.transform.position + -gameObject.transform.right.normalized, step));
        agent.Warp(Vector3.MoveTowards(gameObject.transform.position, gameObject.transform.position + randomHorizontal, 5f));
        
        //agent.Move(new Vector3(rSphere.x, hit.position.y, rSphere.z).normalized / 5 * dashSpeed * Time.deltaTime);
        //agent.Move((gameObject.transform.position + gameObject.transform.right.normalized) * dashSpeed * Time.deltaTime);
        //agent.isStopped = false;


    }

    private void Attack()
    {
        animator.speed = 1f;
        animator.SetBool("isPunching", true);
        animator.SetBool("isWalking", false);

        CancelInvoke(nameof(Dash));
        agent.SetDestination(transform.position);

        Vector3 lookVector = player.transform.position - transform.position;
        lookVector.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookVector), 10f * Time.deltaTime);


        if (!didAttack)
        {
            //ATTACK
            pController.TakeDamage(punchDamage);

            didAttack = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        didAttack = false;
    }


}
