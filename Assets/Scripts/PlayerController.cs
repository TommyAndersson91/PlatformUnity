using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
  public Rigidbody2D rb;
  private Animator playerAnimator;
  private enum State { idle, running, jumping, falling, hurt }
  private State state = State.idle;
  private Collider2D coll;
  private bool colliding = false;
  private bool isHurt = false;
  private bool isCollided = false;
  private bool isDeadByFalling = false;
  public bool isWorldOneComplete = false;
  private bool isJumping;

  public int level;

  private float jumpTimeCounter;
  public float jumpTime = 0.3f;

  float fJumpPressedRemember = 0;
  [SerializeField]
  float fJumpPressedRememberTime = 0.2f;

  float fGroundedRemember = 0;
  [SerializeField]
  float fGroundedRememberTime = 0.2f;

  private delegate void UpdateLives(int value);
  private UpdateLives OnUpdateLives;

  private delegate void UpdateCherries(int value);
  private UpdateCherries onUpdateCherries;

  [SerializeField] private LayerMask ground;
  [SerializeField] private float speed = 7f;
  private float jumpForce = 11f;
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

  private void Awake()
  {
    onUpdateCherries += UpdateCherryText;
    OnUpdateLives += UpdateHeartsGrid;
    LoadPlayer();
  }

  private void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    playerAnimator = GetComponent<Animator>();
    coll = GetComponent<Collider2D>();
  }

  private void Update()
  {
    fGroundedRemember -= Time.deltaTime;
    if (feetColl.IsTouchingLayers(ground))
    {
      fGroundedRemember = fGroundedRememberTime;
    }

    fJumpPressedRemember -= Time.deltaTime;
    if (Input.GetButtonDown("Jump"))
    {
      fJumpPressedRemember = fJumpPressedRememberTime;
    }
    if (!colliding)
    {
      Movement();
      AnimationState();
    }
    //sets animation based on Enumerator state
    playerAnimator.SetInteger("state", (int)state);
    if (rb.position.y < -15f)
    {
      isDeadByFalling = true;
      Lives -= 1;
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
    if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
    {
      fJumpPressedRemember = 0;
      fGroundedRemember = 0;
      isJumping = true;
      rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      jumpTimeCounter = jumpTime;
      state = State.jumping;
    }
    if (jumpTimeCounter > 0 && isJumping && Input.GetKey(KeyCode.Space))
    {
      rb.velocity = new Vector2(rb.velocity.x, jumpForce);
      jumpTimeCounter -= Time.deltaTime;
    }
    else
    {
      isJumping = false;
    }

    if (Input.GetKeyUp(KeyCode.Space))
    {
      isJumping = false;
    }
  }

  private void AnimationState()
  {
    if (rb.velocity.y < .1f && !feetColl.IsTouchingLayers(ground))
    {
      state = State.falling;
    }
    if (Mathf.Abs(rb.velocity.x) < 1f && feetColl.IsTouchingLayers(ground) && state != State.jumping)
    {
      state = State.idle;
    }
    if (Mathf.Abs(rb.velocity.x) > 1f && feetColl.IsTouchingLayers(ground) && state != State.jumping)
    {
      state = State.running;
    }
  }

  public void SavePlayer()
  {
    SaveSystem.SavePlayer(this);
  }

  public void LoadPlayer()
  {
    PlayerData data = SaveSystem.LoadPlayer();

    level = data.level;
    Lives = data.health;
    isWorldOneComplete = data.isWorldOneComplete;
  }

  private void UpdateHeartsGrid(int value)
  {
    if (value == 0 && SceneManager.GetActiveScene().buildIndex != 0)
    {
      level = 1;
      Lives = 3;
      SceneManager.LoadScene("Level1");
    }
    else if (value > 0 && isDeadByFalling)
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      isDeadByFalling = false;
    }

    if (HeartsHolder.childCount > value && value > 0 && SceneManager.GetActiveScene().buildIndex != 0)
    {
      Destroy(HeartsHolder.GetChild(HeartsHolder.childCount - 1).gameObject);
    }
    else if (HeartsHolder.childCount < value && SceneManager.GetActiveScene().buildIndex != 0)
    {
      for (int i = 0; i < value; i++)
      {
        if (HeartsHolder.childCount == value)
        {
          return;
        }
        GameObject heart = Instantiate(Heart, new Vector3(0, 0, 10), Quaternion.identity) as GameObject;
        heart.transform.SetParent(HeartsHolder);
      }

    }

  }

  private void UpdateCherryText(int value)
  {
    if (value < 5)
    {
      cherryText.text = value.ToString();
    }
    else
    {
      Lives += 1;
      Cherries = 0;
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Enemy")
    {

      if (state == State.falling && coll.bounds.min.y > other.rigidbody.GetComponent<Collider2D>().bounds.center.y)
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
      if (!SceneManager.GetActiveScene().name.Contains("3"))
      {
        level = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Level saved as " + level);
        SavePlayer();
      }
      else
      {
        isWorldOneComplete = true;
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
    if (!isCollided)
    {
      isCollided = true;
      if (other.tag == "Collectible")
      {
        Destroy(other.gameObject);
        Cherries += 1;
      }
      else if (other.gameObject.tag == "Shrooms" && Lives < 10)
      {
        Destroy(other.gameObject);
        Lives += 1;
      }
      else if (other.gameObject.tag == "Crank")
      {
        if (GameObject.Find("Block"))
        {
          Sprite sp = Resources.Load<Sprite>("Sunnyland/artwork/Environment/props/crank-up");
          other.gameObject.GetComponent<SpriteRenderer>().sprite = sp;
          Destroy(GameObject.Find("Block").gameObject);
        }
      }
      else if (other.gameObject.tag == "Spikes")
      {
        state = State.hurt;
        Lives -= 1;
        colliding = true;
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
      StartCoroutine(TriggerWait());
    }
  }

  private void OnTriggerExit2D(Collider2D other)
  {

  }

  private IEnumerator TriggerWait()
  {
    yield return new WaitForSeconds(0.5f);
    isCollided = false;
    colliding = false;
  }

  private void OnDestroy()
  {
    SavePlayer();
  }
}
