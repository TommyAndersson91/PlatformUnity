using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Proyecto26;
using FullSerializer;
public class PlayerScore : MonoBehaviour
{

  public Text scoreText;
  public int playerScore;
  public static string playerName;
  public Button Submit;
  public Button GetScore;

  public string idToken;
  public static string localId;

  private bool isLoggedIn = false;

  User user = new User();

  public Button SignUpButton;
  public Button SignInButton;

  public InputField getScoreText;

  public InputField usernameText;
  public InputField emailText;
  public InputField passwordText;

  private string getLocalId;

  public static fsSerializer serializer = new fsSerializer();

  private string databaseUrl = "https://unityplatformer-1adde.firebaseio.com/users/";

  // Start is called before the first frame update
  void Start()
  {

    Submit.onClick.AddListener(OnSubmit);
    GetScore.onClick.AddListener(OnGetScore);
    SignInButton.onClick.AddListener(SignInUserButton);
    SignUpButton.onClick.AddListener(SignUpUserButton);

  }

  public void SignUpUserButton()
  {
    SignUpUser(emailText.text, usernameText.text, passwordText.text);
  }

  public void SignInUserButton()
  {
    SignInUser(emailText.text, passwordText.text);
  }

  private void PostToDatabase(string username, string localId, int score = 0)
  {
    user = new User(username, localId, score);
    // if (emptyScore)
    // {
    //   user.userScore = 0;
    // }
    RestClient.Put(databaseUrl + localId + ".json?auth=" + idToken, user).Then(
      response =>
      {
        Debug.Log("New user posted to database");
      }).Catch(error =>
       {
         Debug.Log(error);
       });
  }

  private void RetrieveFromDatabase()
  {
    // User user = new User();
    RestClient.Get<User>(databaseUrl + "/" + getLocalId + ".json?auth=" + idToken).Then(response =>
      {
        user = response;
        UpdateScore();
      });
  }

  private void UpdateScore()
  {
    scoreText.text = "Score: " + user.userScore;
  }

  void OnSubmit()
  {
    PostToDatabase(usernameText.text, localId, 0);
  }

  void OnGetScore()
  {
    GetLocalId();
  }

  private void UpdateScore(User user)
  {
    scoreText.text = user.userScore.ToString();
  }

  private void GetLocalId()
  {
    RestClient.Get(databaseUrl + ".json?auth=" + idToken).Then(response =>
       {
         var username = getScoreText.text;

         fsData userData = fsJsonParser.Parse(response.Text);
         Dictionary<string, User> users = null;
         serializer.TryDeserialize(userData, ref users);

         foreach (var user in users.Values)
         {
           if (user.userName == username)
           {
             getLocalId = user.localId;
             RetrieveFromDatabase();
             break;
           }
         }
       }).Catch(error =>
       {
         Debug.Log(error);
       });
  }

  private void SignUpUser(string email, string username, string password)
  {
    string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
    RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey.authKey, userData).Then(
      response =>
      {
        idToken = response.idToken;
        localId = response.localId;
        Debug.Log("Successfully signed up");
        PostToDatabase(username, localId);
      }).Catch(error =>
      {
        Debug.Log(error);
      });
  }

  private void SignInUser(string email, string password)
  {
    string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
    RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey.authKey, userData).Then(
      response =>
      {
        idToken = response.idToken;
        localId = response.localId;
        Debug.Log("Successfully logged in");
        isLoggedIn = true;
      }).Catch(error =>
      {
        Debug.Log(error);
      });
  }
}
