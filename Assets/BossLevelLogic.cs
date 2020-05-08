using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelLogic : MonoBehaviour
{
  public GameObject eagle;
  public GameObject frog;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void SpawnEagles()
  {
    Vector2 RandomPos = new Vector2(Random.Range(-35f, 25f), 30f);
    Vector2 RandomPosTwo = new Vector2(Random.Range(-35f, 25f), 30f);

    Instantiate(eagle, RandomPos, transform.rotation);
    Instantiate(eagle, RandomPosTwo, transform.rotation);
  }

  public void SpawnFrogs()
  {
    // Vector2 RandomPos = new Vector2(Random.Range(-35, 25), 20f);
    // Vector2 RandomPosTwo = new Vector2(Random.Range(-35, 25), 20f);

    Instantiate(frog, new Vector2(Random.Range(-35f, 25f), 20f), transform.rotation);
    Instantiate(frog, new Vector2(Random.Range(-35f, 25f), 20f), transform.rotation);
    Instantiate(frog, new Vector2(Random.Range(-35f, 25f), 20f), transform.rotation);
    Instantiate(frog, new Vector2(Random.Range(-35f, 25f), 20f), transform.rotation);
  }
}
