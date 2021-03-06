﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevelLogic : MonoBehaviour
{
  public GameObject eagle;
  public GameObject frog;
  public GameObject cherry;
  float timer = 6f;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (timer <= 0)
    {
      Instantiate(cherry, new Vector2(Random.Range(-30f, 20f), 5f), transform.rotation);
      timer = 6f;
    }
    else
    {
      timer -= Time.deltaTime;
    }
  }

  public void SpawnEagles()
  {
    for (int i = 0; i < Random.Range(0, 4); i++)
    {
      Instantiate(eagle, new Vector2(Random.Range(-35f, 25f), 30f), transform.rotation);
    }
  }

  public void SpawnFrogs()
  {
    for (int i = 0; i < Random.Range(2, 7); i++)
    {
      Instantiate(frog, new Vector2(Random.Range(-35f, 25f), 20f), transform.rotation);

    }
  }
}
