using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject endGamePanel;

    [Header("UI Elements")]
    public TextMeshProUGUI endGameMessageText;

    [Header("Buttons")]
    public Button restartButton;
    public Button exitButton;

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

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
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
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Exits to the main menu.
    /// </summary>
    private void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
