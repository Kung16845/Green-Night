using UnityEngine;

public class DontDestroy : MonoBehaviour
{   
    private void Start() {
        DontDestroyOnLoad(this);
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
