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
    public float meleeRange;
    public int meleeDamage;
    public GameObject poopPuddle;
    public float meleeAttackCD;
    public GameObject fakeShitter;
    public float puddleChanceOnAttack = 100f;
    public float slamHitHeight;
    public float slamHitRange;
    public float meleeAoe;

    private bool isRight;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        animatoranimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
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
        yield return new WaitForEndOfFrame();

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




        if (Vector3.Distance(transform.position, player.transform.position) < meleeRange)
        {
            if (state == EnemyKim.States.Attack) return;

            Vector3 pos = transform.position;
            transform.LookAt(player.transform, Vector3.up);

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            if (state == EnemyKim.States.Wait) return;


            state = EnemyKim.States.Wait;


            // CALL ATTACK ANIMATION HERE, MAKE SURE ANIMATION CALLS BACK TO AttackFinish()

            isRight = Random.Range(0, 2) == 0;

            animatoranimator.SetBool(isRight ? "isLeft" : "isRight", true);
        }
    }

    public void AttackStopMove()
    {
        state = EnemyKim.States.Attack;
    }

    public void AttackHit()
    {
        Vector3 hitPosition = transform.position + (transform.forward * slamHitRange);

        hitPosition.y = slamHitHeight;

        GameObject a = new GameObject("Slam");
        a.transform.position = hitPosition;

        Collider[] overlaps = Physics.OverlapSphere(hitPosition, meleeAoe, LayerMask.GetMask("Player"));
        if (overlaps.Length > 0)
        {
            Debug.Log("hit " + overlaps[0].name);
            player = overlaps[0].gameObject;
            player.GetComponent<PlayerController>().TakeDamage(meleeDamage);
        }

        if (Random.Range(0f, 100f) < puddleChanceOnAttack)
        {
            Instantiate(poopPuddle, hitPosition, Quaternion.identity);
        }
    }

    //gets called when animation finishes
    public void AttackFinish()
    {
        waitTime = meleeAttackCD;
        state = EnemyKim.States.Wait;
        animatoranimator.SetBool("isRight", false);
        animatoranimator.SetBool("isLeft", false);
    }
}
