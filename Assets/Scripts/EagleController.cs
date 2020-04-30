﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : MonoBehaviour
{
  public float speed = 1.0f;
  private Vector2 target;
  
  private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    if (GetComponent<Renderer>().isVisible)
    {
    target = GameObject.Find("Player").GetComponent<PlayerController>().rb.position;
    float step = speed * Time.deltaTime;

    // move sprite towards the target location
    transform.position = Vector2.MoveTowards(transform.position, target, step);
    }
    }

    private void OnGUI() {
      
    }

    private void Movement()
    {

    }
}
