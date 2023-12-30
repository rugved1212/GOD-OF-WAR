using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool isPaused = false;

    public bool pauseSystem = false;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider loadingSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseSystem)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);

        }
        else
        {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);

        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }



    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit_Game()
    {
        Application.Quit();
    }
    public void Quit_to_Game()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainMenu");
        asyncOperation.allowSceneActivation = false; // Prevent automatic scene activation

        while (!asyncOperation.isDone)
        {
            // Update your loading UI or animation here if needed

            // Check if the loading is complete
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true; // Allow scene activation
            }

            yield return null;
        }
    }

    public void LoadToScene(string levelToLoad)
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
