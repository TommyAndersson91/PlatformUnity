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
    if (SceneManager.GetActiveScene().buildIndex+1 < SceneManager.sceneCountInBuildSettings)
    {
      StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    else
    {
      LoadMenu();
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
}
