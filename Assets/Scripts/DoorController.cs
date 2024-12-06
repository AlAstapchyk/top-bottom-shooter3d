using UnityEngine;

public class DoorController : MonoBehaviour
{
    //[SerializeField] private Animator doorAnimator;
    [SerializeField] private MeshCollider doorCollider;
    [SerializeField] private string requiredKey; 
    private bool isPlayerNearby = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HasRequiredKey(other))
            {
                isPlayerNearby = true;
                SetDoorState(true);
                //doorAnimator.SetBool("IsOpen", true); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            SetDoorState(false);
            //doorAnimator.SetBool("IsOpen", false); 
        }
    }

    private bool HasRequiredKey(Collider player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            return string.IsNullOrEmpty(requiredKey) || inventory.HasKey(requiredKey);
        }
        return false;
    }

    private void SetDoorState(bool isOpen)
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen;
        }

        Debug.Log(isOpen ? "Door is open" : "Door is closed");
    }
}
