using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
public class PlayerScore : MonoBehaviour
{

  public Text scoreText;
  public int playerScore;
  public static string playerName;
  public Button Submit;
  public Button GetScore;

  // Start is called before the first frame update
  void Start()
  {
    playerName = "Tommy";
    Submit.onClick.AddListener(OnSubmit);
    GetScore.onClick.AddListener(OnGetScore);
  }


  private void PostToDatabase()
  {
    User user = new User();
    RestClient.Put("https://unityplatformer-1adde.firebaseio.com/" + playerName + ".json", user);
  }

  private void RetrieveFromDatabase()
  {
    User user = new User();
    RestClient.Get<User>("https://unityplatformer-1adde.firebaseio.com/" + playerName + ".json").Then(response =>
    {
     user = response;
     UpdateScore(user);
    });
  }

  void OnSubmit()
  {
    PostToDatabase();
  }

  void OnGetScore()
  {
    RetrieveFromDatabase();
  }
  
  private void UpdateScore(User user) 
  {
    scoreText.text = user.userScore;
  }

  private void SignUpUser(string email, string username, string password)
  {
    string userData = "{email: }";
    RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key= AIzaSyAspUe9tPOuLJUF_vqHhup9GItTZ-5c9wg", userData);
  }

  private void SignInUser(string username, string password)
  {

  }
}
