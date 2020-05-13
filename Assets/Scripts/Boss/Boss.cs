using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

  public int health = 100;
  private enum State { idle, walk, running, jumping, falling, hurt, dead }
  public Image healthBar;
  public Text healthText;
  public bool isAttacking = false;

  public Collider2D weaponOneCol;
  public Collider2D weaponTwoCol;
  public Collider2D playerColl;
  public Rigidbody2D rb;
  public Animator anim;


  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
    anim = GetComponent<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    healthBar.fillAmount = health / 100f;
    healthText.text = health.ToString() + " %";
    if (weaponOneCol.IsTouching(playerColl) && isAttacking || weaponTwoCol.IsTouching(playerColl) && isAttacking)
    {
      GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(transform);
    }
  }

  public void TakeDamage()
  {
    health -= 10;
    GameObject.Find("LevelLogic").GetComponent<BossLevelLogic>().SpawnEagles();
    if (health <= 40)
    {
      GameObject.Find("LevelLogic").GetComponent<BossLevelLogic>().SpawnFrogs();
    }
    if (health <= 0)
    {
      anim.SetTrigger("dead");
    }
  }
}
