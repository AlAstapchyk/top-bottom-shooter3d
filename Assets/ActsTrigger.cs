using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsTrigger : MonoBehaviour
{
    [SerializeField] private ActsController controller;
    [SerializeField] private ComicController comic;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger entered");
        if (other.CompareTag("Player")) {
            if (controller.actIndex == 1)
            {
                //Debug.Log("Player triggered");
                controller.SetAct(2);
                comic.gameObject.SetActive(true);
                
            }
        }   
    }
}
