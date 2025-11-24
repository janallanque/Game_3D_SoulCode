using UnityEngine;
using System.Collections.Generic;

public class NPC_AI : MonoBehaviour
{
    [Header("Components")]
    public List<Transform> wayPoints;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator anim;
    public LayerMask playerLayer;
    public GameObject hitbox;

    [Header ("Variables")]
    public int currentWaypointIndex = 0;
    public float speed = 2f;
    private bool isPlayerDetected = false;
    private bool onRadius;
   


    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        navMeshAgent.speed = speed;

    }

    
    void Update()
    {
        if(!isPlayerDetected)
        {
            Walking();
        }
        else
        {
            StopWalking();
            anim.SetTrigger("Attack");
        }
    }

    private void Walking()
    {
        if (wayPoints.Count == 0)
        {
            return;
        }
        float distanceToWaypoint = Vector3.Distance(
            transform.position, 
            wayPoints[currentWaypointIndex].position
            );

        if (distanceToWaypoint <= 2) 
        { 
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Count;
        }

        navMeshAgent.SetDestination(wayPoints[currentWaypointIndex].position);
        anim.SetBool("Move", true);
        onRadius = false;
    }

    private void StopWalking()
    {
        navMeshAgent.isStopped = true;
        anim.SetBool("Move", false);
        onRadius = true;
    }

   private void  OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detectado!");
            isPlayerDetected = true;
            hitbox.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player saiu do raio de interação!");
            isPlayerDetected = false;
            navMeshAgent.isStopped = false;
            hitbox.SetActive(false);
        }
    }
}
