using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTime : MonoBehaviour
{   
    public float timeScale;
    public float ratio;
    public float timeInSeconds;
    public float timeCount;
    // Start is called before the first frame update
    private void Awake() 
    {   
        ratio = timeScale /1000f;
        timeInSeconds = ratio * 60;
        timeCount = timeInSeconds;
        StartCoroutine(Countdown());
    } 
    public IEnumerator Countdown()
    {
        Debug.Log(timeInSeconds);
        while (timeCount > 0)
        {
            Debug.Log("time left : " + timeCount);
            yield return new WaitForSeconds(1f);
            timeCount -= 1;
        }

        Debug.Log("Success");
        Destroy(gameObject);
    }
    
}
