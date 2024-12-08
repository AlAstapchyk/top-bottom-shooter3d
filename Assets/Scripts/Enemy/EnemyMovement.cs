using NUnit.Framework;
using System;
using System.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform waypointsParent;
    private Transform[] waypoints;
    private int currentWaypointIndex;
    private WeaponHolder enemyWeaponHolder;
    private float enemyWeaponRange;
    private float distanceToPlayer;
    private bool isRaycastingForPlayer = false;
    [SerializeField] private float raycastInterval = 0.2f;
    public enum enemyState
    {
        Patrolling,
        Chasing,
        Returning,
        Trembling,
        Aiming,
        Attacking
    }
    [SerializeField] private enemyState currentState;
    
    
    void Start()
    {
        // Begin patrolling
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = enemyState.Patrolling;
        currentWaypointIndex = 0;

        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.childCount];
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                waypoints[i] = waypointsParent.GetChild(i).transform;
            }
        }

        //
        enemyWeaponHolder = GetComponentInChildren<WeaponHolder>();
        enemyWeaponRange = enemyWeaponHolder.GetCurrentWeapon().range;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRaycastingForPlayer)
        {

        }

        //Debug.Log(currentState);

        switch (currentState)
        {
            
            case enemyState.Patrolling:
                if (isRaycastingForPlayer) { if (CheckLOS()) { currentState = enemyState.Chasing; } }
                if (waypoints != null)
                {
                    Patrol();
                }
                break;
            case enemyState.Chasing:
                Chase();
                break;
            case enemyState.Trembling:
                StartCoroutine(BulletHitReaction());
                break;
            case enemyState.Attacking:
                AttackPlayer();
                break;
        }
    }

    void Patrol()
    {
        navMeshAgent.updateRotation = true;
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }
    void Chase()
    {
        navMeshAgent.updateRotation = true;
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (player != null)
        {
            if (distanceToPlayer < enemyWeaponRange && CheckLOS())
            {
                currentState = enemyState.Attacking;
            } else
            {
                navMeshAgent.SetDestination(player.position);
            }
        }

    }

    void AttackPlayer()
    {
        if (!CheckLOS()) { currentState = enemyState.Chasing; }
        navMeshAgent.updateRotation = false;
        RotateTowards(player.position);
        enemyWeaponHolder.AttackWithReload();
    }

    // Line of sight to player when player confirmed nearby by trigger enter
    bool CheckLOS()
    {
        
        Vector3 directionToPlayer = ((player.position + Vector3.up * 0.5f) - (navMeshAgent.transform.position + Vector3.up * 0.5f)).normalized;
        // Origin and target points are raised slightly because raycast was intersecting with the plane

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, 100))
        {
            if (hit.collider.CompareTag("Player"))
            {
                currentState = enemyState.Chasing;
                //Debug.Log("LOS confirmed!");
                return true;
            }
            else
            {
                //Debug.Log("Line of sight blocked by: " + hit.collider.name);
                //Debug.Log("Target: " + (player.position + Vector3.up * 0.5f) + "\nOrigin: " + (transform.position + Vector3.up * 0.5f));
                return false;
            }
        }
        return false;
    }
    void RotateTowards(Vector3 targetPosition)
    {
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * navMeshAgent.angularSpeed);
        //Debug.Log(navMeshAgent.angularSpeed);
    }

    public void TriggerBulletHitReaction()
    {
        //enemyState previousState = currentState;
        currentState = enemyState.Trembling;

    }

    //public void TriggerChaseState() { currentState = enemyState.Chasing; }

    //public void TriggerPatrolState() { currentState = enemyState.Patrolling; }

    private IEnumerator BulletHitReaction()
    {
        //Debug.Log("Trembling");
        float trembleDuration = 0.5f; // Total time to tremble
        float trembleSpeed = 30f;    // Speed of trembling
        float trembleAmount = 0.3f;  // Distance to move back and forth

        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < trembleDuration)
        {
            // Calculate a small offset
            float offset = Mathf.Sin(elapsedTime * trembleSpeed) * trembleAmount;

            // Apply the offset to the enemy's position
            transform.position = originalPosition + transform.forward * offset;

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset position and state
        transform.position = originalPosition;

    }

    public void StartRaycasting() { 
        //if (!isRaycastingForPlayer)
        //{
        //    isRaycastingForPlayer = true;
        //    InvokeRepeating(nameof(CheckLOS), 0, raycastInterval);
        //}
        isRaycastingForPlayer = true;
        }
    public void StopRaycasting() { 
        isRaycastingForPlayer = false;
        //CancelInvoke(nameof(CheckLOS));
        currentState = enemyState.Patrolling;
    }

}
    
