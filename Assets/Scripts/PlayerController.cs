using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  private Rigidbody2D rb;
  private Animator playerAnimator;
  private enum State { idle, running, jumping, falling, hurt }
  private State state = State.idle;
  private Collider2D coll;
  private bool colliding = false;
  private bool isHurt = false;
  private delegate void UpdateCherries(int value);
  private UpdateCherries onUpdateCherries;

  private delegate void UpdateLives(int value);
  private UpdateLives OnUpdateLives;

  [SerializeField] private LayerMask ground;
  [SerializeField] private float speed = 7f;
  private float jumpForce = 16f;
  [SerializeField] private Text cherryText;
  [SerializeField] private float hurtForce = 20f;
  [SerializeField] private GameObject Heart;
  [SerializeField] private Transform HeartsHolder;
  [SerializeField] private Collider2D feetColl;

  private int cherries;
  public int Cherries
  {
    get { return cherries; }
    set
    {
      cherries = value;
      onUpdateCherries.Invoke(value);
    }
  }

  [SerializeField]
  private int lives;

  public int Lives
  {
    get { return lives; }
    set
    {
      lives = value;
      OnUpdateLives.Invoke(value);
    }
  }

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    playerAnimator = GetComponent<Animator>();
    coll = GetComponent<Collider2D>();
    onUpdateCherries += UpdateCherryText;
    OnUpdateLives += UpdateHeartsGrid;
    for (int i = 0; i < Lives; i++)
    {
      GameObject heart = Instantiate(Heart ,new Vector3(0, 0, 10), Quaternion.identity) as GameObject;
      heart.transform.SetParent(HeartsHolder);
    }

  }

  private void UpdateHeartsGrid(int value)
  {
    if (value == 0)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    if (HeartsHolder.childCount > value && value > 0)
    {
      Destroy(HeartsHolder.GetChild(HeartsHolder.childCount - 1).gameObject);
    }
    else if (transform.childCount < value)
    {
      GameObject heart = Instantiate(Heart, new Vector3(0, 0, 10), Quaternion.identity) as GameObject;
      heart.transform.SetParent(HeartsHolder);
    }

  }

  private void UpdateCherryText(int value)
  {
    cherryText.text = value.ToString();
  }

  private void Update()
  {
    if (!colliding)
    {
      Movement();
      AnimationState();
    }
    //sets animation based on Enumerator state
    playerAnimator.SetInteger("state", (int)state);
    if (rb.position.y < -15f)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Enemy")
    {

      if (state == State.falling && coll.bounds.min.y > other.rigidbody.GetComponent<Collider2D>().bounds.max.y)
      {
        Destroy(other.gameObject);
      }
      else if (state != State.hurt)
      {
        Lives -= 1;
        colliding = true;
        state = State.hurt;
        if (other.gameObject.transform.position.x > transform.position.x)
        {
          // Enemy is to my right. Therefore i should be damaged and move left
          rb.velocity = new Vector2(-hurtForce, hurtForce);
          transform.localScale = new Vector2(-1, 1);

        }
        else
        {
          //Enemy is to my left. Therefore i should be damaged and move right
          rb.velocity = new Vector2(hurtForce, hurtForce);
          transform.localScale = new Vector2(1, 1);
        }
      }
    }
    else if (other.gameObject.tag == "House")
    {
      if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex+1) != null)
      {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
      }
    }
  }

  private void OnCollisionExit2D(Collision2D other)
  {
    StartCoroutine(Wait());

  }

  private IEnumerator Wait()
  {
    yield return new WaitForSeconds(0.5f);
    colliding = false;
    yield break;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Collectible")
    {
      Destroy(other.gameObject);
      Cherries += 1;
    }
    else if (other.gameObject.tag == "Shrooms" && Lives < 3)
    {
      Lives++;
      Destroy(other.gameObject);
    }
  }

  private void Movement()
  {
    float hDirection = Input.GetAxis("Horizontal");

    //Moving Left
    if (hDirection < 0)
    {
      rb.velocity = new Vector2(-speed, rb.velocity.y);
      transform.localScale = new Vector2(-1, 1);
    }
    //Moving RIght
    else if (hDirection > 0)
    {
      rb.velocity = new Vector2(speed, rb.velocity.y);
      transform.localScale = new Vector2(1, 1);
    }
    //Jumping
    if (Input.GetButtonDown("Jump") && feetColl.IsTouchingLayers(ground))
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      state = State.jumping;
    }

  }

  private void AnimationState()
  {
    if (state == State.jumping || state == State.hurt)
    {
      if (rb.velocity.y < .1f)
      {
        state = State.falling;
      }
      else if (state == State.falling)
      {
        if (coll.IsTouchingLayers(ground) || feetColl.IsTouchingLayers(ground))
        {
          state = State.idle;
        }
      }
    }
    else if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon && coll.IsTouchingLayers(ground))
    {
      //Moving
      state = State.running;
    }
    else if (Mathf.Abs(rb.velocity.x) < Mathf.Epsilon)
    {
      state = State.idle;
    }
  }
}
