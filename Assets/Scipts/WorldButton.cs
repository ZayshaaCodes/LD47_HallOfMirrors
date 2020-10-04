using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour
{
    [SerializeField] private float pushDistance = .1f;
    [SerializeField] private SimpleSprite buttonObject;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enableClip;
    [SerializeField] private AudioClip disableClip;
    
    [ColorUsage(true, true)] public Color PressedColor = Color.blue;
    [ColorUsage(true, true)] public Color DepressedColor = new Color(0.8f, 0.8f, 0.8f);

    [SerializeField] private bool playerTriggerOnly = false;
    [SerializeField] private bool isToggleButton = false;
    [SerializeField] private bool pressed;

    private List<ButtonPresser> touching = new List<ButtonPresser>();
    private Vector3 initialButtonPosition;

    public UnityEvent ButtonActivateEvent;
    public UnityEvent ButtonDeactivateEvent;

    private Coroutine cor;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        initialButtonPosition = buttonObject.transform.position;
        if (pressed)
        {
            PressButton();
        }
    }

    private void OnValidate()
    {
        if (buttonObject != null)
        {
            buttonObject.SetColor(DepressedColor);
        }
    }

    private IEnumerator LerpToPosition(Vector3 pos, float speed)
    {
        float d = 1;
        while (d > .001f)
        {
            d = Vector3.Distance(buttonObject.transform.position, pos);

            buttonObject.transform.position = Vector3.MoveTowards(buttonObject.transform.position, pos, speed);

            yield return null;
        }
    }

    public void PressButton()
    {
        // buttonObject.transform.position += new Vector3(0, -pushDistance, 0);

        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(LerpToPosition(initialButtonPosition - transform.up * pushDistance, 3 * Time.deltaTime));

        buttonObject.SetColor(PressedColor);

        audioSource.PlayOneShot(enableClip);
        
        ButtonActivateEvent.Invoke();
    }

    public void DepressButton()
    {
        // buttonObject.transform.position += new Vector3(0, pushDistance, 0);

        buttonObject.SetColor(DepressedColor);
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(LerpToPosition(initialButtonPosition, 3 * Time.deltaTime));

        audioSource.PlayOneShot(disableClip);
        
        ButtonDeactivateEvent.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var bp = other.GetComponent<ButtonPresser>();
        var player = other.GetComponent<Player>();
        if (bp != null && (playerTriggerOnly ? player != null : true))
        {
            touching.Add(bp);
            if (touching.Count == 1) PressButton();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var bp = other.GetComponent<ButtonPresser>();
        var player = other.GetComponent<Player>();
        if (bp != null && (playerTriggerOnly ? player != null : true))
        {
            touching.Remove(bp);
            if (touching.Count == 0 && !isToggleButton) DepressButton();
        }
    }
}