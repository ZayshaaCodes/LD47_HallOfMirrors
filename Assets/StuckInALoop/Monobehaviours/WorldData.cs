using UnityEngine;
using UnityEngine.SceneManagement;

namespace StuckInALoop
{
    public class WorldData : MonoBehaviour
    {
        public static WorldData instance;

        public LoopingWorldRenderer worldRenderer;

        public Vector2 worldUp = Vector2.up;

        public int volume = 5;

        // Start is called before the first frame update
        private void Awake()
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            instance = null;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            worldUp           = Vector2.up;
            Physics2D.gravity = new Vector2(0, -10);
        }

        public void SetWorldUp(Vector2 direction)
        {
            worldUp = direction;
        }
    }
}