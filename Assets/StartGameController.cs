using UnityEngine;

public class StartGameController : MonoBehaviour
{
    public GameObject startPanel;  // панель с кнопкой START

    private bool gameStarted = false;

    void Start()
    {
        gameStarted = false;

        // панель старта должна быть видна при входе на сцену
        if (startPanel != null)
            startPanel.SetActive(true);
    }

    public void OnStartButtonClicked()
    {
        if (gameStarted)
            return;

        gameStarted = true;

        // пр€чем панель
        if (startPanel != null)
            startPanel.SetActive(false);

        // запускаем игру через LevelManager
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.StartGameFromButton();
        }
    }
}
