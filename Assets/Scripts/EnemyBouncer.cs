using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBouncer : Enemy
{
    public float speed;
    private NavMeshAgent agent;
    private GameObject player;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        agent.speed = speed;
    }

    public void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
