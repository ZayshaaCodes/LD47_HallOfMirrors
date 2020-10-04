using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MeshRenderer))]
public class ExitPortal : MonoBehaviour
{
    [SerializeField] private MeshRenderer portalMeshRenderer;
    [SerializeField] private MeshRenderer iconMeshRenderer;

    [SerializeField] private AudioSource audioSource;

    public bool active = false;

    public int nextSceneId;
    
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
            //add fadeout
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