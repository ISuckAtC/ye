using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKim : Enemy
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
    public float fartProjectileSpeedMin, fartProjectileSpeedMax;
    public float fartPushRange;
    public float fartPushMultiplier;
    public float fartPushVerticalBias;

    public float minionSpawnCount;

    bool startAttackT;
    bool fartAttackT;
    bool spawnAnimationT;

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
                ChangeAnimation(state);

            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }


        //if (startAttackT && animator.GetCurrentAnimatorStateInfo(0).IsName("ButtAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    AttackAnimationFinish();
        //    startAttackT = false;
        //}
        //
        //if (fartAttackT && animator.GetCurrentAnimatorStateInfo(0).IsName("Fart") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    FartAnimationFinish();
        //    fartAttackT = false;
        //}
        //
        //if (spawnAnimationT && animator.GetCurrentAnimatorStateInfo(0).IsName("Squat") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    SpawnAnimationFinish();
        //    spawnAnimationT = false;
        //}


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
                ChangeAnimation(state);

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

                        animator.SetBool("isWalking", false);
                        animator.SetBool("isFarting", false);
                        animator.SetBool("isSquatting", false);
                        animator.SetBool("isAttacking", true);

                        //if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationName"))
                        //{
                        //    // Avoid any reload.
                        //}

                        //startAttackT = true;
                        
                        break;
                    case 1:
                        Debug.Log("Kim brapp");

                        //FartNow();

                        animator.SetBool("isWalking", false);
                        animator.SetBool("isSquatting", false);
                        animator.SetBool("isAttacking", false);
                        animator.SetBool("isFarting", true);

                        

                        //fartAttackT = true;


                        break;
                    case 2:
                        Debug.Log("Kim orbiter raid");
                        

                        animator.SetBool("isWalking", false);
                        animator.SetBool("isFarting", false);
                        animator.SetBool("isAttacking", false);
                        animator.SetBool("isSquatting", true);

                        //spawnAnimationT = true;

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
                ChangeAnimation(state);

            }
        }

    }



    void ChangeAnimation(States s)
    {
        switch (s)
        {
            case States.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isFarting", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSquatting", false);

                break;
            case States.Chase:
                animator.SetBool("isFarting", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSquatting", false);
                animator.SetBool("isWalking", true);

                break;
            case States.Wait:
                animator.SetBool("isWalking", false);
                animator.SetBool("isFarting", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isSquatting", false);


                break;
            default:
                break;
        }
    }


    public void FartNow()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer = directionToPlayer.normalized;

        for (int i = 0; i < fartProjectileCount; i++)
        {
            Vector3 fartDirection = Quaternion.Euler(Random.Range(-fartSprayRadius, fartSprayRadius), Random.Range(-fartSprayRadius, fartSprayRadius), 0) * directionToPlayer;
            GameObject fart = Instantiate(fartProjectile, transform.position, Quaternion.identity);
            fart.GetComponent<Rigidbody>().AddForce(fartDirection * Random.Range(fartProjectileSpeedMin, fartProjectileSpeedMax), ForceMode.VelocityChange);
            Destroy(fart, fartProjectileLifetime);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, fartPushRange, LayerMask.GetMask("Fart"));
        foreach (Collider collider in colliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            direction += new Vector3(0, fartPushVerticalBias, 0);
            collider.GetComponent<Rigidbody>().AddForce(direction.normalized * (fartPushRange - direction.magnitude) * fartPushMultiplier, ForceMode.VelocityChange);
        }
    }


    public void SpawnMinionsNow()
    {
        for (int i = 0; i < minionSpawnCount; i++)
        {
            GameObject spawn = Instantiate(minion, transform.position, Quaternion.identity);
        }
    }

    public void ReceiveSquatEvent()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Fart"))
        {
            Debug.Log("farted");
            FartAnimationFinish();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Squat"))
        {
            Debug.Log("squated");
            SpawnAnimationFinish();
        }
    }

    public void AttackAnimationFinish()
    {
        Debug.Log("attack animation ended");
        waitTime = clapCooldown;
        state = States.Wait;
        ChangeAnimation(state);
    }

    public void FartAnimationFinish()
    {

        Debug.Log("fart animation ended");
        waitTime = fartCooldown;
        state = States.Wait;
        ChangeAnimation(state);
    }

    public void SpawnAnimationFinish()
    {
        Debug.Log("spawn animation ended");
        waitTime = spawnCooldown;
        state = States.Wait;
        ChangeAnimation(state);
    }


}
