using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public GameObject spawnOnDeath;
    public bool invincible = false;

    public virtual bool TakeDamage(int damage)
    {

        if (!invincible)
        {
            health -= damage;
        }
        

        if (health <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    protected virtual void Die()
    {
        if (spawnOnDeath) Instantiate(spawnOnDeath, transform.position, transform.rotation);
        SoundController.sounds.PlaySound(SoundController.sounds.Death, transform.position);
        Destroy(gameObject);
    }
}
