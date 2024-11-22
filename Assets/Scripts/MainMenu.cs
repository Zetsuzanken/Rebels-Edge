using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject fadeCanvas;
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Start()
    {
        fadeCanvas.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void NewGame()
    {
        StartCoroutine(FadeOut("Level1"));
    }

    public void QuitGame()
    {
        StartCoroutine(FadeOutQuit());
    }

    private IEnumerator FadeOut(string sceneName)
    {
        fadeCanvas.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeOutQuit()
    {
        fadeCanvas.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        Application.Quit();
    }
}
