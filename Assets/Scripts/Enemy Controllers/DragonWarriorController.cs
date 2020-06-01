using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWarriorController : MonoBehaviour
{
  private float speed = 5f;
  private float attackRange = 4f;
  public float jumpForce = 11f;
  public float health;
  public float groundDetect = 3f;
  private float idleTimer = 5f;
  private float fireballTimer = 3f;
  public float maxFovAngle = 45f;
  private float dashwindCooldown = 3f;
  public Animator anim;
  float distance = 7f;
  Collider2D playerColl;
  RaycastHit2D groundInfo;
  public Collider2D groundColl;
  public GameObject eyes;
  public GameObject fireballPrefab;
  public GameObject DashwindPrefab;
  private bool movingLeft = true;
  private bool isPlayerDetected = false;
  private enum State { idle, walking, attacking, striking, jumping }
  private State state = State.walking;
  private Rigidbody2D body;
  [SerializeField] private LayerMask ground;
  // Start is called before the first frame update

  // float distanceToPlayer;
  float visionAngle;

  void Start()
  {
    anim = GetComponent<Animator>();
    playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    body = GetComponent<Rigidbody2D>();
  }

  void ShootFireball()
  {
    GameObject fireball = Instantiate(fireballPrefab, eyes.transform.position, transform.rotation);
  }

  // Update is called once per frame
  void Update()
  {
    groundInfo = Physics2D.Raycast(eyes.transform.position, Vector2.down, groundDetect, ground);
    if (GetComponent<Renderer>().isVisible)
    {
      anim.SetInteger("state", (int)state);
      Vector2 fovRadius = eyes.transform.right * distance;
      // distanceToPlayer = Vector2.Distance(eyes.transform.position - playerColl.transform.position, fovRadius);
      visionAngle = Vector2.Angle(playerColl.transform.position - eyes.transform.position, fovRadius);

      if (visionAngle < maxFovAngle && !isPlayerDetected)
      {
        isPlayerDetected = true;
      }
      if (isPlayerDetected)
      {
        DetectedBehaviour();
      }
      else
      {
        UndetectedBehaviour();
      }
    }
    else
    {
      state = State.walking;
      isPlayerDetected = false;
    }
  }

  IEnumerator DashwindTime()
  {
    float timePassed = 0;
    while (timePassed < 0.5f)
    {
      if (playerColl.transform.position.x < transform.position.x && groundInfo.collider)
      {
        body.velocity = new Vector2(speed + 4f, body.velocity.y);
      }
      else if (playerColl.transform.position.x > transform.position.x && groundInfo.collider)
      {
        body.velocity = new Vector2(-speed + (-4f), body.velocity.y);
      }
      timePassed += Time.deltaTime;
      yield return null;
    }
  }

  void DetectedBehaviour()
  {
    float step = speed * Time.deltaTime;
    float distancePlayer = Vector2.Distance(playerColl.transform.position, transform.position);

    if (groundInfo.collider == false)
    {
      body.velocity = transform.forward * body.velocity.magnitude;
    }

    if (playerColl.transform.position.y > (transform.position.y + 1f) && distancePlayer < 4f && dashwindCooldown > 2.9f)
    {
      dashwindCooldown = 0f;
      Instantiate(DashwindPrefab, groundColl.transform.position, transform.rotation);
      StartCoroutine(DashwindTime());
    }
    else
    {
      body.position = Vector2.MoveTowards(transform.position, playerColl.transform.position, step);
    }
    if (dashwindCooldown < 3f)
    {
      dashwindCooldown += Time.deltaTime;
    }

    if (playerColl.transform.position.x < transform.position.x)
    {
      transform.eulerAngles = new Vector3(0, -180, 0);
    }
    else if (playerColl.transform.position.x > transform.position.x)
    {
      transform.eulerAngles = new Vector3(0, 0, 0);
    }

    if (playerColl.transform.position.y > body.position.y && groundColl.IsTouchingLayers(ground))
    {
      state = State.jumping;
      body.velocity = new Vector2(body.velocity.x, jumpForce);
    }
    else if (!groundColl.IsTouchingLayers(ground) && body.position.y <= playerColl.transform.position.y)
    {
      state = State.jumping;
      body.velocity = new Vector2(body.velocity.x, jumpForce);
    }
    else if (groundColl.IsTouchingLayers(ground) && state == State.jumping)
    {
      state = State.walking;
    }

    if (distancePlayer < attackRange && state != State.striking)
    {
      state = State.striking;
    }
    if (distancePlayer > attackRange && state == State.striking)
    {
      state = State.walking;
    }

    if (fireballTimer <= 0)
    {
      ShootFireball();
      fireballTimer = 3f;
    }
    else
    {
      fireballTimer -= Time.deltaTime;
    }
  }

  void UndetectedBehaviour()
  {

    if (idleTimer <= 0 && state == State.walking)
    {
      StartCoroutine(IdleWaiting());
    }
    else if (!isPlayerDetected)
    {
      idleTimer -= Time.deltaTime;
      transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    if (groundInfo.collider == false)
    {
      if (movingLeft == true)
      {
        transform.eulerAngles = new Vector3(0, -180, 0);
        movingLeft = false;
      }
      else
      {
        transform.eulerAngles = new Vector3(0, 0, 0);
        movingLeft = true;
      }
    }
  }

  IEnumerator IdleWaiting()
  {
    state = State.idle;
    yield return new WaitForSeconds(Random.Range(1, 3));
    state = State.walking;
    idleTimer = 5f;
  }

  void Attacking()
  {

  }
}
