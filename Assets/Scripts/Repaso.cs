using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //****

public class Repaso : MonoBehaviour
{

//Instalar window AI Navigation
    enum State
    {
        Patrolling,
        Chasing,
        Attacking
    }

    private State currentState;
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float detectionRange = 15;
    [SerializeField] private float attackRange = 5;    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;  //tiene qeu tener el tag de player
    }

    void Start()
    {
        SetRandomPoint();
        currentState = State.Patrolling;

    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;

            case State.Chasing:
                Chase();
            break;

            case State.Attacking:
                Attack();
            break;

        }
    }

    void Patrol()
    {
        if(IsInRange(detectionRange) == true)
        {
            currentState = State.Chasing;
        }

        if(agent.remainingDistance < 0.5f)
        {
            SetRandomPoint();
        }
    }

    void Chase()
    {
         if(IsInRange(detectionRange) == false)
        {
            SetRandomPoint();
            currentState = State.Chasing;
        }

        if(IsInRange(attackRange) == true)
        {
            currentState = State.Attacking;
        }
        agent.destination = player.position;
    }

    void Attack()
    {
        Debug.Log("Atacando");

        currentState = State.Chasing;
    }
    void SetRandomPoint()
    {
        agent.destination = patrolPoints[Random.Range(0,patrolPoints.Length - 1)].position;
    }

    bool IsInRange(float range)
    {
        if(Vector3.Distance(transform.position, player.position) < attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        foreach(Transform point in patrolPoints)
        {
            Gizmos.DrawWireSphere(point.position, 0.5f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

    }
}
