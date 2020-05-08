using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFrog : MonoBehaviour
{
  private Rigidbody2D rb;
  private enum State { idle, jumping, falling }
  private Animator frogAnimator;
  private State state = State.idle;
  private float jumpForce = 8f;
  private float speed = 5f;
  private bool isMoving = false;
  private Collider2D coll;


  [SerializeField] private LayerMask ground;


  // Start is called before the first frame update
  void Start()
  {
    coll = GetComponent<Collider2D>();
    rb = GetComponent<Rigidbody2D>();
    frogAnimator = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
      if (!isMoving && coll.IsTouchingLayers(ground))
      {
        StartCoroutine(Movements());
      }

      if (rb.velocity.y > .1f)
      {
        state = State.jumping;
      }
      else if (rb.velocity.y < .1f)
      {
        state = State.falling;

        if (coll.IsTouchingLayers(ground))
        {
          state = State.idle;
        }
      }
      else if (Mathf.Abs(rb.velocity.x) < Mathf.Epsilon)
      {
        state = State.idle;
      }
      frogAnimator.SetInteger("state", (int)state); //sets animation based on Enumerator state 
    if (rb.position.y < -15f)
    {
      Destroy(gameObject);
    }
  }

  private IEnumerator Movements()
  {
    isMoving = true;

    // transform.Translate(Vector2.right * speed * Time.deltaTime);
    //Moving Left
    if (Random.Range(0, 2) == 0)
    {
      rb.velocity = new Vector2(speed, jumpForce);
      transform.localScale = new Vector2(-1.4f, 1.4f);
    }
    //Moving RIght
    else
    {
      rb.velocity = new Vector2(-speed, jumpForce);
      transform.localScale = new Vector2(1.4f, 1.4f);
    }
    yield return new WaitForSeconds(0.5f);
    isMoving = false;
  }
}
