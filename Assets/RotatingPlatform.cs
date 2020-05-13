using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
  public Animator anim;
  public bool isRotated = false;
  private float timer;


  private void Start()
  {
    timer = 2f;
  }

  private void Update()
  {
    if (isRotated)
    {
      timer -= Time.deltaTime;
      if (timer <= 0)
      {
        anim.SetTrigger("rotateback");
        isRotated = false;
        timer = 2f;
      }
    }
  }
  
  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player" && !isRotated)
    {
      anim.SetTrigger("rotate");
      isRotated = true;
    }
  }
}
