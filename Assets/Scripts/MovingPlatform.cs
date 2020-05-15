using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

  public float speed = 4.0f;
  public float movingRange;
  private Vector3 target;
  private Vector3 startingPos;
  private bool isFinished = false;
  public bool isHorizontal = false;
  public Rigidbody2D body;

  // Start is called before the first frame update
  void Start()
  {
    body = GetComponent<Rigidbody2D>();
    startingPos = transform.position;
    if (isHorizontal)
    {
      target = new Vector3(transform.position.x + movingRange, transform.position.y, transform.position.z);
    }
    else
    {
      target = new Vector3(transform.position.x, transform.position.y + movingRange, transform.position.z);
    }
  }

  // Update is called once per frame
  void Update()
  {
    float step = speed * Time.deltaTime;
    if (!isFinished)
    {
      transform.position = Vector2.MoveTowards(transform.position, target, step);
      if (transform.position == target)
      {
        isFinished = true;
      }
    }
    else if (isFinished)
    {
      transform.position = Vector2.MoveTowards(transform.position, startingPos, step);
      if (transform.position == startingPos)
      {
        isFinished = false;
      }
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    other.transform.SetParent(null);
  }

  private void OnCollisionStay2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      if (other.rigidbody.velocity.x > 1.2f || other.rigidbody.velocity.x < -1.2f && other.transform.parent == transform)
      {
        other.transform.SetParent(null);
      }
      if (other.transform.parent != transform)
      {
        other.transform.SetParent(transform);
      }
    }
  }


}
