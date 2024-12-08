using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject comicPanel;

    public void StartButton()
    {
        ShowComic();
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit(); 
        #endif
    }

    private void ShowComic()
    {
        if (comicPanel != null)
        {
            comicPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Comic panel is not assigned in the ButtonManager!");
        }
    }
}
