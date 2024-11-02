using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static bool instanceExists = false;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Awake()
    {
        if (instanceExists)
        {
            Destroy(this.gameObject); // Destroy duplicates upon reloading the scene
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instanceExists = true; // Set flag to indicate an instance exists
        }
    }
}
