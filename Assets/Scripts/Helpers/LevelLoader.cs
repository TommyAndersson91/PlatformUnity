using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
  public Animator transisition;

  public float transisitionTime = 1f;
  // Update is called once per frame
  void Update()
  {

  }

  public void LoadNextLevel()
  {
    for (int i = 0; i < Constants.Levels.Length; i++)
    {
      
      if (SceneManager.GetActiveScene().name == Constants.Levels[i] && i != Constants.Levels.Length - 1)
      {
        StartCoroutine(LoadLevelName(Constants.Levels[i+1]));
        return;
      }
      else if (i == Constants.Levels.Length - 1)
      {
        LoadMenu();
        return;
      }
    }
  }

  public void LoadLevel(string levelName)
  {
    SceneManager.LoadScene(levelName);
  }

  public void LoadMenu()
  {
    StartCoroutine(LoadLevel(0));
  }

  IEnumerator LoadLevel(int levelIndex)
  {
    transisition.SetTrigger("start");
    yield return new WaitForSeconds(transisitionTime);
    SceneManager.LoadScene(levelIndex);
  }

  IEnumerator LoadLevelName(string levelName)
  {
    transisition.SetTrigger("start");
    yield return new WaitForSeconds(transisitionTime);
    SceneManager.LoadScene(levelName);
  }
}
