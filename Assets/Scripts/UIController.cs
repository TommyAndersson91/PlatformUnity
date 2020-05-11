using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
  public Button BackToMenu;
  public Button FireButton;
  public GameObject player;
  public Joystick joystick;


  private void Start() {
    player = GameObject.Find("Player");
    BackToMenu.onClick.AddListener(GoMenu);
    FireButton.onClick.AddListener(player.GetComponent<PlayerController>().Shoot);
  }

  private void Update() {
    if (player.GetComponent<PlayerController>().fireBalls > 0)
    {
      FireButton.gameObject.SetActive(true);
    }
    else if (player.GetComponent<PlayerController>().fireBalls == 0 && FireButton.IsActive())
    {
      FireButton.gameObject.SetActive(false);
    }
  }

  private void GoMenu()
  {
    SceneManager.LoadScene("MenuScene");
  }

}
