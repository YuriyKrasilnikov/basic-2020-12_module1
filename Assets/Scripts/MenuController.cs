using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public CanvasGroup[] screenCanvas;

    public void SetCurrentScreen(CanvasGroup selectScreen)
    {
        foreach (CanvasGroup scr in screenCanvas){
            Utility.SetCanvasGroupEnabled(scr, scr == selectScreen);
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        SetCurrentScreen(screenCanvas[0]);
    }
}
