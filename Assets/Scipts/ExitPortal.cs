using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshRenderer))]
public class ExitPortal : MonoBehaviour, IDestroyOnClone
{
    [SerializeField] private MeshRenderer portalMeshRenderer;
    [SerializeField] private MeshRenderer iconMeshRenderer;

    [SerializeField] private AudioSource audioSource;

    public bool active = false;

    public int nextSceneId = -1;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (active)
        {
            Enable();
        }
        else
        {
            Disable();
        }
    }

    private void Update()
    {
        if (!active) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var currentScene = SceneManager.GetActiveScene().buildIndex;

            if (nextSceneId < 0) nextSceneId = ++currentScene;
            if (nextSceneId >= sceneCount) nextSceneId = 0;

            SceneManager.LoadScene(nextSceneId);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (active && other.GetComponent<Player>() != null)
        {
            iconMeshRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (active && other.GetComponent<Player>() != null)
        {
            iconMeshRenderer.enabled = false;
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
        portalMeshRenderer.enabled = false;
        audioSource.Stop();
        active = false;
    }
}