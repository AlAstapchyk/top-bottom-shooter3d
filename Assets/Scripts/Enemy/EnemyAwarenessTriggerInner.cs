using UnityEngine;

public class EnemyAwarenessTriggerInner : MonoBehaviour
{
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = this.GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered Awareness!!!");
            enemyMovement.StartRaycasting();
            //enemyMovement.TriggerChaseState();
            //Debug.Log("Trigger enter");
        }
    }
}
