using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOfficer : Enemy
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

        if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 8)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
        }




    }
}
