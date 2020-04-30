﻿using System.Collections;
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
    PlayLevelTwo.gameObject.SetActive(false);
    PlayerData data = SaveSystem.LoadPlayer(); 
    if (data != null)
    {
      Debug.Log("The Level at menu is " + data.level);
      Debug.Log("The Lives at menu is " + data.health);
      Debug.Log("World one is completed? : " + data.isWorldOneComplete);

      if (data.isWorldOneComplete == true)
     {
       PlayLevelTwo.gameObject.SetActive(true);
     }
    }
      
    
  }


  private void PlayLevel()
  {
    PlayerData data = SaveSystem.LoadPlayer();
    
    if (data != null)
    {
    SceneManager.LoadScene(data.level);
    }
    else
    {
      SceneManager.LoadScene("Level1");

    }
  } 
}


