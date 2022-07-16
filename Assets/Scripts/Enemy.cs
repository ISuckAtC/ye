using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    public virtual bool TakeDamage(int damage)
    {
        //WHEN ENEMY GET HIT
        //gameObject.GetComponent<Renderer>().material.SetFloat("_EnemyHit", 1);

        Debug.Log("got damages");
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
        Destroy(gameObject);
    }
}
