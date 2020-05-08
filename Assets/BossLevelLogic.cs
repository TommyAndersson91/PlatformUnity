using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelLogic : MonoBehaviour
{
  public Transform eagleSpawnOne;
  public Transform eagleSpawnTwo;
  public GameObject eagle;

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
    Vector2 RandomPos = new Vector2(Random.Range(-35, 25), 30f);
    Vector2 RandomPosTwo = new Vector2(Random.Range(-35, 25), 30f);

    GameObject eagleOne = Instantiate(eagle, RandomPos, transform.rotation);
    GameObject eagleTwo = Instantiate(eagle, RandomPosTwo, transform.rotation);

  }
}
