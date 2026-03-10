using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
