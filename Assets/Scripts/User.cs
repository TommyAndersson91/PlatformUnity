using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User 
{
   public string userName;
   public string userScore;

   public User()
   {
    PlayerData data = SaveSystem.LoadPlayer();
     userName = PlayerScore.playerName;
     userScore = data.level.ToString();

   }
}
