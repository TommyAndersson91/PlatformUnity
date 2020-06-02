using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{
  public string userName;
  public int userScore;
  public string localId;


  public User()
  {
    userName = PlayerScore.playerName;
    userScore = 0;
    localId = PlayerScore.localId;
  }
  
  public User(string username, string localId, int userScore = 0)
  {
    this.userName = username;
    this.userScore = userScore;
    this.localId = localId;
  }
}
