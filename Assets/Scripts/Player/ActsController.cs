using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActsController : MonoBehaviour
{
    [SerializeField] HealthHearts playerHearts;
    [SerializeField] public float actIndex;
    private PlayerMovement movement;
    public Transform respawnPoint;
    [SerializeField] private ComicController endingComic;
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        actIndex = 1;
        //playerHearts.OnPlayerDeath.AddListener(HandlePlayerDeath);
        EventManager.Instance.Health.OnPlayerDied.AddListener(HandlePlayerDeath);
    }

    async void HandlePlayerDeath()
    {

        Debug.Log("handling death");
        if (actIndex == 1)
        {
            //Debug.Log(respawnPoint.position);
            //movement.Teleport(respawnPoint.position);
            //playerHearts.SetHeartsToMax();
            await Task.Delay(1500);
            SceneManager.LoadScene("MainScene");
        }
        else if (actIndex == 2)
        {
            Debug.Log("Koniec");
            //endingComic.
            endingComic.gameObject.SetActive(true);
            //SceneManager.LoadScene(0);
        }
    }

    public void SetAct(int actIndex)
        { this.actIndex = actIndex; }
    
}
