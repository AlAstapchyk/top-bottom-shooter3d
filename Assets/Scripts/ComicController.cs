using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ComicController : MonoBehaviour
{
    [SerializeField] private Image[] comicPages; 
    [SerializeField] private Button nextButton;
    [SerializeField] private int nextSceneIndex;
    //[SerializeField] private GameObject loadingScreen;

    private int currentPageIndex = 0;
    private AsyncOperation sceneLoadOperation;

    void Start()
    {
        foreach (Image page in comicPages)
            page.gameObject.SetActive(false);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        sceneLoadOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        sceneLoadOperation.allowSceneActivation = false;
        ShowFirstPage();
    }

    private void ShowFirstPage()
    {
        if (comicPages.Length > 0)
        {
            comicPages[0].gameObject.SetActive(true);
            if (nextButton != null)
                nextButton.gameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        NextPage();
    }

    public void NextPage()
    {
        comicPages[currentPageIndex].gameObject.SetActive(false);
        currentPageIndex++;

        if (currentPageIndex < comicPages.Length)
        {
            comicPages[currentPageIndex].gameObject.SetActive(true);
        }
        else
        {  
            comicPages[comicPages.Length - 1].gameObject.SetActive(true);   //loadingScreen.SetActive(true);
            sceneLoadOperation.allowSceneActivation = true;
        }
    }
}
