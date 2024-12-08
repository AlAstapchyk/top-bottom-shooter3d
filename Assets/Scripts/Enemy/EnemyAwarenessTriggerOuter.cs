using UnityEngine;

public class EnemyAwarenessTriggerOuter : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = this.GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyMovement.StopRaycasting();
            //Debug.Log("Player left Awareness...");
            //enemyMovement.TriggerPatrolState();
        }
    }
}
