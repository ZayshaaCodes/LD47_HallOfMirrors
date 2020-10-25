using TMPro;
using UnityEngine;

public class fpsCounter : MonoBehaviour
{
    private TMP_Text _text;


    private float t = .5f;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        t -= Time.deltaTime;

        if (t < 0)
        {
            t += .5f;
            var rate = 1 / Time.deltaTime;
            _text.text  = rate.ToString("F0");
            _text.color = Color.Lerp(Color.red, Color.green, Mathf.InverseLerp(30, 120, rate));
        }
    }
}