using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    public virtual void TakeDamage(int damage)
    {
        //WHEN ENEMY GET HIT
        //gameObject.GetComponent<Renderer>().material.SetFloat("_EnemyHit", 1);

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
