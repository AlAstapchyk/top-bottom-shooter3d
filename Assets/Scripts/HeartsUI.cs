using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour {
    [SerializeField] private Image[] heartImages; // Array of heart UI objects
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private HealthHearts healthHearts;
    

    private void Start()
    {
        // EventManager.Instance will be null here if it's Awake or OnEnable instead of Start. Load order issue.
        if (healthHearts != null)
        {
            EventManager.Instance.Health.OnPlayerHeartsChanged.AddListener(UpdateHearts);
        }
    }

    private void UpdateHearts(int currentHearts)
    {
        //Debug.Log("Current hearts: " + currentHearts);


        for (int i = 0; i < heartImages.Length; i++)
        {

            heartImages[i].sprite = i < currentHearts ? fullHeart : emptyHeart;
        }
    }


}