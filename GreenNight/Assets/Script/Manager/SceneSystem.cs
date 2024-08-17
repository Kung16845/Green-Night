using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{   
    public void SwitchScene(string sceneName)
    {
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
