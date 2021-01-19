using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


internal sealed class GameController : MonoBehaviour
{
    public Button attackButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

    public TextMeshProUGUI statusTextMenu;
    public string winText;
    public string lostText;

    public CanvasGroup controlPanel;
    public CanvasGroup gameCanvas;
    public CanvasGroup menuCanvas;
    
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    Character FirstAliveCharacter(Character[] characters)
    {
        return characters.FirstOrDefault(character => !character.IsDead());
    }

    void PlayerWon()
    {
        Debug.Log("Player won.");
        EndMenu(winText);
    }

    void PlayerLost()
    {
        Debug.Log("Player lost.");
        EndMenu(lostText);
    }

    void EndMenu(string text){
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        ChangeStatusText(statusTextMenu, text);
        PauseGame();
        Time.timeScale = 0.25f;
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacter) == null) {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null) {
            PlayerWon();
            return true;
        }

        return false;
    }
    
    public void ChangeStatusText(TextMeshProUGUI view, string text="")
    {   

        view.gameObject.SetActive(text!="");
        view.text = text;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        Utility.SetCanvasGroupEnabled(gameCanvas, false);
        Utility.SetCanvasGroupEnabled(menuCanvas, true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
        Utility.SetCanvasGroupEnabled(menuCanvas, false);
        Utility.SetCanvasGroupEnabled(gameCanvas, true);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //[ContextMenu("Player Attack")]
    public void PlayerAttack()
    {
        waitingForInput = false;
    }

    //[ContextMenu("Next Target")]
    public void NextTarget()
    {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++) {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead()) {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter)
            {
                if (player.IsDead())
                    continue;

                currentTarget = FirstAliveCharacter(enemyCharacter);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                Utility.SetCanvasGroupEnabled(controlPanel, true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;

                Utility.SetCanvasGroupEnabled(controlPanel, false);
                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.target = currentTarget.transform;
                player.AttackEnemy();

                while (!player.IsIdle())
                    yield return null;
            }

            foreach (var enemy in enemyCharacter)
            {
                if (enemy.IsDead())
                    continue;

                Character target = FirstAliveCharacter(playerCharacter);
                if (target == null)
                    break;

                enemy.target = target.transform;
                enemy.AttackEnemy();

                while (!enemy.IsIdle())
                    yield return null;
            }
        }
    }
    
    void Start()
    {
        Time.timeScale = 1;
        attackButton.onClick.AddListener(PlayerAttack);
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        exitButton.onClick.AddListener(ExitGame);
        Utility.SetCanvasGroupEnabled(controlPanel, false);
        ChangeStatusText(statusTextMenu);
        Utility.SetCanvasGroupEnabled(menuCanvas, false);
        Utility.SetCanvasGroupEnabled(gameCanvas, true);
        StartCoroutine(GameLoop());
    }
}