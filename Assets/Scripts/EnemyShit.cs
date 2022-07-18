using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShit : Enemy
{
    public int phase;
    public GameObject fallPoop;
    public GameObject rollPoop;
    private Animator animatoranimator;
    private MeshRenderer renderer;
    private Rigidbody rigidbody;
    public float speed;
    public float fallAttackCD;
    public float rollAttackCD;
    public BoxCollider fallSpawnArea;
    public float waitTime;
    [Tooltip("If it doesnt spawn fall poop, it will spawn roll poop")]
    public float fallPoopChance;
    public float fallPoopInitiateSpeed;
    public float fallPoopSpawnHeight;

    public Vector3 rollPoopSpawnSpeed;
    public BoxCollider rollSpawnArea;
    public float rollPoopLifetime;

    public Transform antiClutter;

    public Transform phaseTwoSpawn;
    public float phaseTwoSpawnSpeed;

    public EnemyKim.States state;
    private GameObject player;
    public float sightRange;
    public float meleeRange;
    public int meleeDamage;
    public GameObject poopPuddle;
    public float meleeAttackCD;
    public float puddleAttackCD;
    public float puddleChancePerSecond;
    private NavMeshAgent agent;
    public GameObject fakeShitter;
    public float puddleChanceOnAttack;



    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        rigidbody = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                if (state == EnemyKim.States.Wait)
                {
                    state = EnemyKim.States.Idle;
                }
            }
            return;
        }

        if (phase == 0)
        {
            PhaseOne();
        }
        else if (phase == 1)
        {
            PhaseTwo();
        }
    }

    public IEnumerator PhaseToOne()
    {
        phase = -1;
        int secure = 0;
        while (true)
        {
            fakeShitter.transform.position = Vector3.MoveTowards(fakeShitter.transform.position, phaseTwoSpawn.position, phaseTwoSpawnSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            if (Vector3.Distance(fakeShitter.transform.position, phaseTwoSpawn.position) < 0.1f)
            {
                break;
            }
            if (secure++ > 1000)
            {
                break;
            }
        }
        GetComponent<Collider>().enabled = true;
        rigidbody.isKinematic = false;
        renderer.enabled = true;
        Destroy(fakeShitter);

        phase = 1;
    }

    void PhaseOne()
    {
        float chance = Random.Range(0f, 100f);
        if (chance < fallPoopChance)
        {
            StartCoroutine(SpawnFallPoop());
        }
        else
        {
            waitTime = rollAttackCD;
            Vector3 startPosition = new Vector3(Random.Range(rollSpawnArea.bounds.min.x, rollSpawnArea.bounds.max.x), rollSpawnArea.bounds.center.y, Random.Range(rollSpawnArea.bounds.min.z, rollSpawnArea.bounds.max.z));
            GameObject poop = Instantiate(rollPoop, startPosition, Quaternion.identity);
            poop.GetComponent<Rigidbody>().velocity = rollPoopSpawnSpeed;
            Destroy(poop, rollPoopLifetime);
            poop.transform.SetParent(antiClutter);
        }
    }

    IEnumerator SpawnFallPoop()
    {
        waitTime = fallAttackCD;
        GameObject poop = Instantiate(fallPoop, transform.position, Quaternion.identity);
        Vector3 startPosition = new Vector3(Random.Range(fallSpawnArea.bounds.min.x, fallSpawnArea.bounds.max.x), fallPoopSpawnHeight, Random.Range(fallSpawnArea.bounds.min.z, fallSpawnArea.bounds.max.z));

        poop.transform.SetParent(antiClutter);

        int secure = 0;
        while (true)
        {
            poop.transform.position = Vector3.MoveTowards(poop.transform.position, startPosition, fallPoopInitiateSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
            if (Vector3.Distance(poop.transform.position, startPosition) < 0.1f)
            {
                break;
            }
            if (secure++ > 1000)
            {
                break;
            }
        }

        Rigidbody rb = poop.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        Collider col = poop.GetComponent<Collider>();
        col.enabled = true;
    }

    void PhaseTwo()
    {
        if (state == EnemyKim.States.Wait) return;
        if (state == EnemyKim.States.Attack) return;




        if (state == EnemyKim.States.Chase)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > sightRange)
            {
                agent.isStopped = true;
                player = null;
                state = EnemyKim.States.Idle;
            }
            else
            {
                agent.SetDestination(player.transform.position);
                agent.isStopped = false;
            }

            float randPuddle = Random.Range(0f, 1f);
            if (randPuddle < Time.deltaTime)
            {
                randPuddle = Random.Range(0f, 100f);


                if (randPuddle < puddleChancePerSecond)
                {
                    /*
                    // spawn puddle

                    GameObject puddle = Instantiate(poopPuddle, transform.position, Quaternion.identity);
                    puddle.transform.SetParent(antiClutter);
                    state = EnemyKim.States.Attack;
                    agent.isStopped = true;

                    // code after finished animation

                    state = EnemyKim.States.Wait;
                    waitTime = puddleAttackCD;
                    */
                }
            }

            if (player && Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
            {
                state = EnemyKim.States.Attack;
                agent.isStopped = true;
                int rand = Random.Range(0, 1);
                switch (rand)
                {
                    case 0:
                        player.GetComponent<PlayerController>().TakeDamage(meleeDamage);

                        state = EnemyKim.States.Attack;
                        Debug.Log("aa");
                        animatoranimator.SetBool("isRight", true);

                        // run next code when attack hits

                        float frand = Random.Range(0f, 100f);
                        if (frand < puddleChanceOnAttack)
                        {
                            GameObject puddle = Instantiate(poopPuddle, transform.position, Quaternion.identity);
                            puddle.transform.SetParent(antiClutter);
                        }

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
                state = EnemyKim.States.Chase;
            }
        }
    }

    //gets called when animation finishes
    public void AttackFinish()
    {
        waitTime = meleeAttackCD;
        state = EnemyKim.States.Wait;
        animatoranimator.SetBool("isRight", false);
    }
}
