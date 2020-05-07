using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{

  public float speed = 20f;
  // Start is called before the first frame update
  void Start()
  {
    GetComponent<Rigidbody2D>().velocity = transform.right * speed;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Enemy")
    {
      GameObject.Find("EnemiesController").GetComponent<Enemy>().Die(other.transform);
      Destroy(other.gameObject);
      Destroy(gameObject);
    }
  }

}
