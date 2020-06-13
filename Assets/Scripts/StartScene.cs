using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using FullSerializer;


public class StartScene : MonoBehaviour
{
  public InputField emailInput;
  public InputField passwordInput;

  public Text errorText;
  
  public Button signInButton;
  public Button signUpButton;

  private static fsSerializer serializer = new fsSerializer();

  User user = new User();

  private string idToken;
  private string localId;
  private string databaseUrl = "https://unityplatformer-1adde.firebaseio.com/users/";

  private void Start() {
    signInButton.onClick.AddListener(OnSignInUserButton);
    signUpButton.onClick.AddListener(OnSignUpUserButton);
  }

  public void OnSignUpUserButton()
  {
    GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("SignUp");
  }

  public void OnSignInUserButton()
  {
    SignInUser(emailInput.text, passwordInput.text);
  }

  private void RetrieveFromDatabase()
  {
    // User user = new User();
    RestClient.Get<User>(databaseUrl + localId + ".json?auth=" + idToken).Then(response =>
      {
        user = response;
        FirebaseController.user = user;
        FirebaseController.idToken = idToken;
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadMenu();
      });
  }

  public void UpdateUserScore (int score)
  {
    user.userScore = score;
    RestClient.Put(databaseUrl + localId + ".json?auth=" + idToken, user).Then(
      response =>
      {
        Debug.Log("Updated Users score");
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
        string emailVerification = "{\"idToken\":\"" + response.idToken + "\"}";
        RestClient.Post("https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=" + AuthKey.authKey,
        emailVerification)
        .Then(emailResponse =>
       {
         fsData emailVerificationData = fsJsonParser.Parse(emailResponse.Text);
         EmailConfirmationInfo emailConfirmationInfo = new EmailConfirmationInfo();
         serializer.TryDeserialize(emailVerificationData, ref emailConfirmationInfo).AssertSuccessWithoutWarnings();

         if (emailConfirmationInfo.users[0].emailVerified)
         {
           idToken = response.idToken;
           localId = response.localId;
           errorText.text ="Successfully logged in!";
           RetrieveFromDatabase();
         }
         else
         {
           errorText.text = "Please verify your email"; 
         }
       });
      }).Catch(error =>
      {
        errorText.text = ("User is not found or incorrect password");
      });
  }
}
