using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWarriorController : MonoBehaviour
{
  private float speed = 5f;
  public float jumpForce;
  public float health;
  public float groundDetect = 3f;
  public Animator anim;
  float distance = 5f;
  Collider2D playerColl;
  public GameObject eyes;
  private bool movingLeft = true;
  [SerializeField] private LayerMask ground;
  // Start is called before the first frame update
  void Start()
  {
    anim = GetComponent<Animator>();
    playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
  }

  // Update is called once per frame
  void Update()
  {
    RaycastHit2D hit = Physics2D.Raycast((Vector2)eyes.transform.position, (Vector2)transform.right, distance);
    if (hit.collider != null)
    {
      Debug.Log(hit.collider.name);
      Debug.DrawLine(eyes.transform.position, hit.point, Color.red);
    }
    else
    {
      Debug.DrawLine(eyes.transform.position, eyes.transform.position + transform.right * distance, Color.green);
    }

    transform.Translate(Vector2.right * speed * Time.deltaTime);

    RaycastHit2D groundInfo = Physics2D.Raycast(eyes.transform.position, Vector2.down, groundDetect, ground);

    if (groundInfo.collider == false)
    {
      anim.SetTrigger("Idle");
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
    else
    {
      anim.SetTrigger("Walking");
    }
  }
}
