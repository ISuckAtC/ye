using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public Animator anim;
    public GameObject attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public float meleeImpactForce = 2f;

    public int damageValue = 3;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.attachedRigidbody.name);
            //Check if it has rigidBody
            if (enemy.attachedRigidbody != null && LayerMask.LayerToName(enemy.gameObject.layer) == "Enemy")
            {
                enemy.attachedRigidbody.AddForce(cam.transform.forward * meleeImpactForce, ForceMode.VelocityChange);
                bool isEnemyDead = enemy.gameObject.GetComponent<Enemy>().TakeDamage(damageValue);
                if (isEnemyDead)
                    nextAttackTime = 0f;
            }


        }

        if (hitEnemies.Length == 0)
        {
            Debug.Log("test to see if failed");
            //Reduces attack rate to a minimum of 1f, values should be adjusted
            attackRate = Mathf.Max(attackRate * 0.90f, 1f);
            
            //TODO figure out the appropiate ammount to reduce animation speed
            anim.speed = Mathf.Max(attackRate * 0.90f, 1f);


        }
    }

    /* void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.transform.position, attackRange);
    } */
}
