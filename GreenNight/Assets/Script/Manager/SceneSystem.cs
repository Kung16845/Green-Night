using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    private int mainSceneIndex = 0;
    public int currentSceneIndex;
    public Animator transitionAnim;
    public TimeManager timeManager;
    private bool isSceneLoading = false;
    private void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
        timeManager.sceneSystem1 = this;
        timeManager.dateTime.sceneSystem = this;
       
        // Debug.Log("sceneSystem");
    }
    public void SwitchScene(int sceneIndex)
    {

        StartCoroutine(LoadScene(sceneIndex));
    }
    IEnumerator LoadScene(int sceneIndex)
    {
        transitionAnim.SetTrigger("EndScene");
        yield return new WaitForSeconds(3.0f);
        // SceneManager.LoadScene(sceneIndex);
        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // if (currentSceneIndex == 0)
        // {
        //     // SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);

        //     // Hide all root GameObjects in the main scene (Scene index 0)
        //     Scene mainScene = SceneManager.GetSceneByBuildIndex(mainSceneIndex);
        //     if (mainScene.IsValid() && mainScene.isLoaded)
        //     {
        //         foreach (GameObject go in mainScene.GetRootGameObjects())
        //         {
        //             go.SetActive(false); // Temporarily hide main scene objects
        //         }
        //     }

        //     // Load the new scene additively
        //     SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        // }
        // else
        // {
        //     ReturnToMainScene();
        //     // SceneManager.UnloadSceneAsync(currentSceneIndex);
        // }
        if (sceneIndex == mainSceneIndex)
        {
            Debug.LogWarning("Main scene is already loaded. Use ReturnToMainScene instead.");
            yield break;
        }

        // Hide all root GameObjects in the main scene (Scene index 0)
        Scene mainScene = SceneManager.GetSceneByBuildIndex(mainSceneIndex);
        if (mainScene.IsValid() && mainScene.isLoaded)
        {
            foreach (GameObject go in mainScene.GetRootGameObjects())
            {
                go.SetActive(false); // Temporarily hide main scene objects
            }
        }

        // Load the new scene additively
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        currentSceneIndex = sceneIndex;
    }


    public void ReturnToMainScene()
    {
        // Unload all additive scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.buildIndex != mainSceneIndex && scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // Show all root GameObjects in the main scene
        Scene mainScene = SceneManager.GetSceneByBuildIndex(mainSceneIndex);
        if (mainScene.IsValid() && mainScene.isLoaded)
        {
            foreach (GameObject go in mainScene.GetRootGameObjects())
            {
                go.SetActive(true); // Restore main scene objects
            }
        }


    }
}
