using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject endGamePanel;

    [Header("Buttons")]
    public Button restartButton;

    void Awake()
    {
        // Implement Singleton pattern
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
            endGamePanel.SetActive(false); // Ensure it's hidden at start

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    /// <summary>
    /// Shows the end game panel.
    /// </summary>
    public void ShowEndGamePanel()
    {
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true);
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
