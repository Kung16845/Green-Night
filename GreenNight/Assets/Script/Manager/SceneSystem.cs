using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public Animator transitionAnim;
    public TimeManager timeManager;
    private void Start() 
    {
        timeManager = FindObjectOfType<TimeManager>();
        timeManager.sceneSystem1 = this;
        timeManager.dateTime.sceneSystem = this;
        Debug.Log("sceneSystem"); 
    }
    public void SwitchScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }
    IEnumerator LoadScene(string sceneName)
    {
        transitionAnim.SetTrigger("EndScene");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
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
