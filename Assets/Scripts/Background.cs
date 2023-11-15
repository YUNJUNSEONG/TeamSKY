using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    private float moveSpeed = 0.02f;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed;
        if(transform.position.x < -25) {
            transform.position += new Vector3(64.8f, 0, 0);
        }
    }
}