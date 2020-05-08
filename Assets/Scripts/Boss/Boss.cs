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

  public Collider2D weaponOneCol;
  public Collider2D weaponTwoCol;
  public Collider2D playerColl;
  public Rigidbody2D rb;


  // Start is called before the first frame update
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    playerColl = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider2D>();
  }

  // Update is called once per frame
  void Update()
  {
    healthBar.fillAmount = health / 100f;
    healthText.text = health.ToString() + " %";
    if (weaponOneCol.IsTouching(playerColl) || weaponTwoCol.IsTouching(playerColl))
    {
      Debug.Log("Hitting the player");
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
  }

  // private void OnCollisionEnter2D(Collision2D other) {
  //   if (other.gameObject.tag == "Player")
  //   {

  //   }

  // }
}
