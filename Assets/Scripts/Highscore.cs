using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using FullSerializer;
using System.Linq;
using TMPro;

public class Highscore : MonoBehaviour
{

  public GameObject topPlayersDisplay;
  public GameObject userObject;

  public InputField searchField;

  public Button searchButton;
  public Button backButton;

  public TextMeshProUGUI searchText;

  private string databaseUrl = "https://unityplatformer-1adde.firebaseio.com/users/";

  private User user = new User();

  private List<User> scores;

  private fsSerializer serializer = new fsSerializer();

  private Dictionary<string, User> users = null;

  void Start()
  {
    scores = new List<User>();
    RetrieveFromDatabase();
    searchButton.onClick.AddListener(RetrieveScore);
    backButton.onClick.AddListener(BackToMenu);
  }

  private void BackToMenu()
  {
    GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadMenu();
  }

  private void RetrieveScore()
  {
    foreach (var score in scores)
    {
      bool ignoreCase = score.userName.Contains(searchField.text, StringComparison.OrdinalIgnoreCase);
      if (searchField.text == score.userName || ignoreCase)
      {
        searchText.text = "Player: " + score.userName + "\n" + "Score: " + score.userScore;
        return;
      }
    }
    searchText.text = "The player was not found";
  }

  private void RetrieveFromDatabase()
  {
    Debug.Log("Retrieving");
    // User user = new User();
    RestClient.Get(databaseUrl + ".json").Then(response =>
      {
        fsData userData = fsJsonParser.Parse(response.Text);
        
        serializer.TryDeserialize(userData, ref users);

        foreach (var user in users.Values)
        {
          scores.Add(user);
        }
        Debug.Log("Successfully retrieved");
        SortScores();
      }).Catch(error =>
      {
        Debug.Log(error);
      });

  }

  private void SortScores()
  {
    List<User> SortedList = scores.OrderBy(o => o.userScore).ToList();
    SortedList.Reverse();
    for (int i = 0; i < scores.Count; i++)
    {
      GameObject userobj = Instantiate(userObject, topPlayersDisplay.transform);
      userobj.GetComponent<UserPrefab>().username.text = SortedList[i].userName;
      userobj.GetComponent<UserPrefab>().score.text = SortedList[i].userScore.ToString();
    }
  }
}
