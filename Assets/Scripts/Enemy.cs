using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject spawnOnDeath;

    public virtual bool TakeDamage(int damage)
    {
        //WHEN ENEMY GET HIT
        //gameObject.GetComponent<Renderer>().material.SetFloat("_EnemyHit", 1);

        health -= damage;
        if (health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    protected virtual void Die()
    {
        Instantiate(spawnOnDeath, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
