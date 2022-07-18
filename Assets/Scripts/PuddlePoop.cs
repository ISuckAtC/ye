using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddlePoop : MonoBehaviour
{
    public int health;
    public float puddlePlayerSpeed;
    public int damagePerTick;
    public float tickInterval;
    private float playerInitialSpeed;
    private bool isPlayerIn;
    private float tickTime;

    public void Awake()
    {
        playerInitialSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed;
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();
            pc.speed = puddlePlayerSpeed;
            isPlayerIn = true;
        }
    }
    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            tickTime += Time.deltaTime;
            if (tickTime >= tickInterval)
            {
                tickTime = 0;
                col.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            }
            
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<PlayerController>().speed = playerInitialSpeed;
            isPlayerIn = false;
        }
    }

    public void HitPuddle(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            if (isPlayerIn)
            {
                PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                pc.speed = playerInitialSpeed;
            }
        }
    }
}
