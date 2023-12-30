using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scene_change : MonoBehaviour
{
    public string sceneToLoad = "Scene2";
    public GameObject loadingScreen;
    public Slider loadingBar;

    private bool isLoading = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isLoading && other.CompareTag("Player"))
        {
            // Show the loading screen
            loadingScreen.SetActive(true);
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;

        // Create an async operation to load the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Allow the scene to load in the background while updating the loading progress
        while (!asyncLoad.isDone)
        {
            // Update the loading progress
            float progress = Mathf.Clamp01(asyncLoad.progress); // The progress goes from 0 to 0.9 during the scene load
            loadingBar.value = progress;

            yield return null;
        }

        isLoading = false;

        // Hide the loading screen
        loadingScreen.SetActive(false);
    }
}
