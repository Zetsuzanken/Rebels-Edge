using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject endGamePanel;

    [Header("UI Elements")]
    public TextMeshProUGUI endGameMessageText;

    [Header("Buttons")]
    public Button restartButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (endGamePanel != null)
            endGamePanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    /// <summary>
    /// Shows the end game panel.
    /// </summary>
    /// <param name="message">The message to display.</param>
    public void ShowEndGamePanel(string message)
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
            if (endGameMessageText != null)
            {
                endGameMessageText.text = message;
            }
        }
    }

    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
