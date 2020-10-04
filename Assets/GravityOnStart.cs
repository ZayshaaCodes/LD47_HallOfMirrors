using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOnStart : MonoBehaviour
{
    [SerializeField] private Vector2 startGravity;

    void Start()
    {
        Physics2D.gravity = startGravity;
    }
}