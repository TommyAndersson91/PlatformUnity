using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;

public class FirebaseController
{
  public static User user = new User();
  private static string databaseUrl = "https://unityplatformer-1adde.firebaseio.com/users/";
  public static string idToken;

  public static void UpdateUserScore(int score)
  {
    user.userScore = score;
    RestClient.Put(databaseUrl + user.localId + ".json?auth=" + idToken, user).Then(
      response =>
      {
        Debug.Log("Updated Users score");
      }).Catch(error =>
       {
         Debug.Log(error);
       });
  }

  public static int GetUserScore()
  {
      RestClient.Get<User>(databaseUrl + user.localId + ".json?auth=" + idToken).Then(response =>
        {
         return response.userScore;
        });
        return 0;
    }
}
