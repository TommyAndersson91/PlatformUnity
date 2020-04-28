using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
  [SerializeField] private Button PlayLevelOne;
  [SerializeField] private Button PlayLevelTwo;
  

  private void Start()
  {
    PlayLevelOne.onClick.AddListener(PlayLevel);
    PlayLevelTwo.onClick.AddListener(PlayLevel);
  }

  private void PlayLevel()
  {

    SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
  }

 
}


