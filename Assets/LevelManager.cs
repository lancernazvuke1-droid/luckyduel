using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Базовые параметры")]
    public int startLevel = 1;
    public int baseAmmo = 10;
    public int baseTargetsToHit = 5;
    public float baseTime = 20f;

    [Header("Рост сложности")]
    public int ammoPerLevel = 2;
    public int targetsPerLevel = 2;
    public float timeChangePerLevel = -1f;

    [Header("UI")]
    public Text levelText;
    public Text ammoText;
    public Text targetsText;
    public Text timeText;
    public Text bestLevelText;

    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Связи")]
    public Shooter shooter;

    int currentLevel;
    int currentAmmo;
    int targetsToHit;
    int targetsHit;
    float timeLeft;
    bool levelActive;

    int bestLevel = 1;
    const string BEST_LEVEL_KEY = "BestLevel";

    public bool IsLevelActive => levelActive;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // загружаем лучший уровень
            bestLevel = PlayerPrefs.GetInt(BEST_LEVEL_KEY, 1);

            // при запуске сцены игра ещё НЕ началась
            currentLevel = startLevel;
            levelActive = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // просто обновим UI (уровень, лучший уровень и т.п.)
        UpdateUI();

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    void Update()
    {
        if (!levelActive)
            return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            CheckLoseByTime();
        }

        UpdateUI();
    }

    /// <summary>
    /// Этот метод вызывается КНОПКОЙ START.
    /// </summary>
    public void StartGameFromButton()
    {
        if (levelActive) return; // уже идёт уровень

        currentLevel = startLevel; // на всякий случай
        StartLevel();
    }

    public void StartLevel()
    {
        levelActive = true;

        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        targetsHit = 0;

        currentAmmo = baseAmmo + (currentLevel - 1) * ammoPerLevel;
        targetsToHit = baseTargetsToHit + (currentLevel - 1) * targetsPerLevel;
        timeLeft = baseTime + (currentLevel - 1) * timeChangePerLevel;
        if (timeLeft < 5f) timeLeft = 5f;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (levelText != null)
            levelText.text = "LEVEL: " + currentLevel;

        if (ammoText != null)
            ammoText.text = "AMMO: " + currentAmmo;

        if (targetsText != null)
            targetsText.text = "DUCK: " + targetsHit + " / " + targetsToHit;

        if (timeText != null)
            timeText.text = "TIME: " + timeLeft.ToString("0.0");

        if (bestLevelText != null)
            bestLevelText.text = "BEST LVL: " + bestLevel;
    }

    public bool TrySpendAmmo()
    {
        if (!levelActive)
            return false;

        if (currentAmmo <= 0)
            return false;

        currentAmmo--;

        if (currentAmmo <= 0 && targetsHit < targetsToHit)
        {
            Lose("Патроны закончились");
        }

        UpdateUI();
        return true;
    }

    public void OnTargetHit()
    {
        if (!levelActive)
            return;

        targetsHit++;

        if (targetsHit >= targetsToHit)
        {
            Win();
        }

        UpdateUI();
    }

    void CheckLoseByTime()
    {
        if (!levelActive)
            return;

        if (targetsHit < targetsToHit)
        {
            Lose("Время вышло");
        }
    }

    void Win()
    {
        levelActive = false;
        ClearAllTargets();

        if (currentLevel > bestLevel)
        {
            bestLevel = currentLevel;
            PlayerPrefs.SetInt(BEST_LEVEL_KEY, bestLevel);
            PlayerPrefs.Save();
        }

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayWin();

        UpdateUI();

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    void Lose(string reason)
    {
        levelActive = false;
        Debug.Log("Поражение: " + reason);

        ClearAllTargets();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayLose();

        UpdateUI();

        if (losePanel != null)
            losePanel.SetActive(true);
    }

    void ClearAllTargets()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (var t in targets)
        {
            Destroy(t);
        }
    }

    public void NextLevel()
    {
        currentLevel++;
        StartLevel();
    }

    public void RestartLevel()
    {
        StartLevel();
    }

    public void ResetBestLevel()
    {
        bestLevel = 1;
        PlayerPrefs.SetInt(BEST_LEVEL_KEY, bestLevel);
        PlayerPrefs.Save();
        UpdateUI();
    }
}
