using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private int selectionId = 0;
    [SerializeField] private KeyCode pauseKey;
    public Transform itemContainer;
    public RectTransform selectionRectXform;
    private List<RectTransform> itemRects;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip selectSound;

    public AudioMixer _mixer;


    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Start()
    {
        itemRects = new List<RectTransform>();
        foreach (RectTransform xform in itemContainer)
        {
            itemRects.Add(xform);
        }

        SetSelection(selectionId);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) SetSelection((--selectionId));
        if (Input.GetKeyDown(KeyCode.S)) SetSelection(++selectionId);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)) ExecuteMenuItem(selectionId);
        if (Input.GetKeyDown(KeyCode.A)) DecrementItem(selectionId);
        if (Input.GetKeyDown(KeyCode.D)) IncrementItem(selectionId);
    }

    private void IncrementItem(int i)
    {
        if (i == 1)
        {
            WorldData.instance.volume = Mathf.Clamp(++WorldData.instance.volume, 0, 10);
            
            _mixer.SetFloat("volume",  Mathf.Lerp(-40,0, Mathf.InverseLerp(0, 10, WorldData.instance.volume)));
            audioSource.PlayOneShot(clickSound);
            UpdateVolumeText();
        }
    }

    private void DecrementItem(int i)
    {
        if (i == 1)
        {
            WorldData.instance.volume = Mathf.Clamp(--WorldData.instance.volume, 0, 10);
            
            _mixer.SetFloat("volume",  Mathf.Lerp(-40,0, Mathf.InverseLerp(0, 10, WorldData.instance.volume)));
            audioSource.PlayOneShot(clickSound);
            UpdateVolumeText();
        }
    }

    private void ExecuteMenuItem(int i)
    {
        audioSource.PlayOneShot(clickSound);
        
        StartCoroutine(DelayedExe(.5f,() =>
        {
            if (i == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
            }

            if (i == 2)
            {
                Application.Quit();
            }    
        }));
        
    }
    
    IEnumerator DelayedExe(float wait, Action action)
    {
        float t = 0;

        while (t < wait)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        
        action.Invoke();
    }

    public void UpdateVolumeText()
    {
        itemContainer.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text =
            WorldData.instance.volume.ToString();
    }
    
    private void SetSelection(int id)
    {
        var itemCount = itemContainer.childCount - 1;
        selectionId = (id + itemCount) % itemCount;

        // Debug.Log(selectionId);
        var itemRect = itemRects[selectionId + 1];

        StartCoroutine(LerpToRectTransform(itemRect));
        audioSource.PlayOneShot(clickSound);
    }

    private IEnumerator LerpToRectTransform(RectTransform itemRect)
    {
        float t = 0;
        var startPos = selectionRectXform.position;
        var startSize = selectionRectXform.sizeDelta;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 4;

            selectionRectXform.position = Vector2.Lerp(
                startPos,
                itemRect.position,
                curve.Evaluate(t));

            selectionRectXform.sizeDelta = Vector2.Lerp(
                startSize,
                itemRect.sizeDelta + new Vector2(20, 10),
                curve.Evaluate(t));

            yield return null;
        }
    }
}