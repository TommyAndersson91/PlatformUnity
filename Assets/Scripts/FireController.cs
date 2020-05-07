using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

  public float speed = 20f;
  public Rigidbody2D rigidbody;
  // Start is called before the first frame update
  void Start()
  {
    rigidbody.velocity = transform.right * speed;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    
  }

}
