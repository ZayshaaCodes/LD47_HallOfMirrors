using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        pauseMenu.UpdateVolumeText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var pauseMenuActive = pauseMenu.gameObject.activeInHierarchy;
            pauseMenu.gameObject.SetActive(!pauseMenuActive);
            Time.timeScale = pauseMenuActive ? 1 : 0;
        }
    }
}