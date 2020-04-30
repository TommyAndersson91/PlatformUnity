﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

  public int level;
  public int health;
  public bool isWorldOneComplete;


  public PlayerData(PlayerController player)
  {
    level = player.level;
    health = player.Lives;
    isWorldOneComplete = player.isWorldOneComplete;
  }
}
