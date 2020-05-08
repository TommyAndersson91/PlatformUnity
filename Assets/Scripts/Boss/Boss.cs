using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

  public int health = 50;
  private enum State { idle, walk, running, jumping, falling, hurt, dead }
  public Image healthBar;
  public Text healthText; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount =  health / 100f;
        healthText.text = health.ToString() + " %";
    }
}
