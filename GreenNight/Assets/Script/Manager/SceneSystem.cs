using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
     private int mainSceneIndex = 0;
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(currentSceneIndex == 0)
        {
            SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
        }
        else 
        {
            SceneManager.UnloadSceneAsync(currentSceneIndex);
        }

    }
    // private void Update() {
    //     if(dateTime.hour == 18 && dateTime.isDayNight)
    //     {
    //         SwitchScene("DefendSceneFare");

    //     }
    //     else if(dateTime.hour == 6 )
    //     {
    //         SwitchScene("TownBaseScene");
    //     }
    // }
}
