using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPoop : Enemy
{
    public int damage;
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
