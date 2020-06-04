using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;

public class SignUp : MonoBehaviour
{
  public InputField emailInput;
  public InputField usernameInput;
  public InputField passwordInput;
  public InputField passwordReInput;

  public Button signUpButton;
  public Button backButton;

  public Text errorText;

  private string localId;
  private string idToken;
  private string databaseUrl = "https://unityplatformer-1adde.firebaseio.com/users/";

  User user = new User();



  void Start()
  {
    signUpButton.onClick.AddListener(OnSignUpButton);
    backButton.onClick.AddListener(BackToStart);
  }

  private void BackToStart()
  {
    GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("StartScene");
  }

  public void OnSignUpButton()
  {
    SignUpUser(emailInput.text, usernameInput.text, passwordInput.text);
  }

  private void SignUpUser(string email, string username, string password)
  {
    if (password.Length < 3)
    {
      errorText.text = "The minimum length of the password is 3 characters";
      return;
    }
    if (password != passwordReInput.text)
    {
      errorText.text = "The passwords dont match";
      return;
    }
    string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
    RestClient.Post<SignResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey.authKey, userData).Then(
      response =>
      {
        string emailVerification = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + response.idToken + "\"}";
        RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=" + AuthKey.authKey, emailVerification);
        localId = response.localId;
        // idToken = response.idToken;
        errorText.text = "Successfully signed up, check your email for confirmation";
        PostToDatabase(username, localId, default, response.idToken);
        StartCoroutine(GoStartScene());
      }).Catch(error =>
      {
        errorText.text = error.Message;
      });
  }

  private void PostToDatabase(string username, string localId, int score = 0, string idTokenTemp = "")
  {
    if (idTokenTemp == "")
    {
      idTokenTemp = idToken;
    }
    user = new User(username, localId, score);
    Debug.Log(idToken);
    RestClient.Put(databaseUrl + localId + ".json?auth=" + idTokenTemp, user).Then(
      response =>
      {
        Debug.Log("New user posted to database");
      }).Catch(error =>
       {
         Debug.Log(error);
       });
  }

  IEnumerator GoStartScene()
  {
    yield return new WaitForSeconds(2f);
    GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("StartScene");
  }
}
