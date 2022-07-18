using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopPlatform : MonoBehaviour
{
    public EnemyShit boss;
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            boss.StartCoroutine(boss.PhaseToOne());
            Destroy(this);
        }
    }
}
