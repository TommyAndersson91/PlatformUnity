using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
  public float speed = 10f;
  public Animator anim;
  // Start is called before the first frame update
  void Start()
  {
    GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    anim = GetComponent<Animator>();

  }

  private void Update()
  {
    if (GetComponent<Renderer>().isVisible == false)
    {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(transform);
      GetComponent<Rigidbody2D>().velocity = Vector2.zero;
      StartCoroutine(Explode());
    }
    else if (other.transform.name == "Foreground")
    {
      GetComponent<Rigidbody2D>().velocity = Vector2.zero;
      StartCoroutine(Explode());
    }
  }

  IEnumerator Explode()
  {
    anim.Play("Exploding");
    yield return new WaitForSeconds(1f);
    Destroy(gameObject);
  }
}
