using UnityEngine;
using UnityEngine.SceneManagement;

namespace StuckInALoop
{
    public class ExitPortal : MonoBehaviour, IDestroyOnClone
    {
        [SerializeField] private MeshRenderer portalMeshRenderer;
        [SerializeField] private MeshRenderer iconMeshRenderer;
        [SerializeField] private AudioSource  audioSource;

        public bool active;
        public int  nextSceneId = -1;

        private bool _playerTouching;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (active)
                Enable();
            else
                Disable();
        }

        private void Update()
        {
            if (!active) return;
            if (Input.GetKeyDown(KeyCode.E))
                if (_playerTouching)
                {
                    var sceneCount   = SceneManager.sceneCountInBuildSettings;
                    var currentScene = SceneManager.GetActiveScene().buildIndex;

                    if (nextSceneId < 0) nextSceneId           = ++currentScene;
                    if (nextSceneId >= sceneCount) nextSceneId = 0;

                    SceneManager.LoadScene(nextSceneId);
                }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (active && other.GetComponent<Player>() != null)
            {
                _playerTouching          = true;
                iconMeshRenderer.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var player                  = other.GetComponent<Player>();
            if (player) _playerTouching = false;

            if (active && player != null) iconMeshRenderer.enabled = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (active && other.GetComponent<Player>() != null)
            {
                _playerTouching          = true;
                iconMeshRenderer.enabled = true;
            }
        }

        public void Enable()
        {
            portalMeshRenderer.enabled = true;
            audioSource.Play();
            active = true;
        }

        public void Disable()
        {
            audioSource.Stop();

            portalMeshRenderer.enabled = false;
            active                     = false;
            _playerTouching            = false;
        }
    }
}