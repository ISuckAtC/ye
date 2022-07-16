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
    public Camera cam;

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
            //do damage 
            Debug.Log("melee hit");

            if(enemy.attachedRigidbody != null)
                enemy.attachedRigidbody.AddForce(cam.transform.forward * meleeImpactForce, ForceMode.VelocityChange);
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
