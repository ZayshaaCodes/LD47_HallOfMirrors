using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldData : MonoBehaviour
{

    public static WorldData instance;

    public LoopingWorldRenderer worldRenderer;
    
    public Vector2 worldUp = Vector2.up;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        instance = null;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        
        
        if (instance != this && instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this);
        }


    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        worldUp = Vector2.up;
        Physics2D.gravity = new Vector2(0,-10);
    }

    public void SetWorldUp(Vector2 direction)
    {
        worldUp = direction;
    }
}
