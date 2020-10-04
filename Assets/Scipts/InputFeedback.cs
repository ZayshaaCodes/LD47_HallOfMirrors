using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFeedback : MonoBehaviour
{
    private SimpleSprite indicatorSprite;


    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        indicatorSprite = GetComponent<SimpleSprite>();
    }

    void Update()
    {
        var up = cam.transform.up;

        Vector2 d = Vector2.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) d += Vector2.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) d += Vector2.down;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) d += Vector2.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) d += Vector2.right;

        if (d == Vector2.zero)
        {
            indicatorSprite.mr.enabled = false;
        }
        else if (indicatorSprite.mr.enabled == false)
        {
            indicatorSprite.mr.enabled = true;
        }

        transform.rotation = Quaternion.LookRotation(Vector3.forward, d) * Quaternion.LookRotation(Vector3.forward, up);
    }
}