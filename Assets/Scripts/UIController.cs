using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
  private Button BackToMenu;

  private void Start() {
    BackToMenu = transform.GetChild(3).GetComponent<Button>();
    BackToMenu.onClick.AddListener(GoMenu);
  }

  private void GoMenu()
  {
    SceneManager.LoadScene("MenuScene");
  }

}
