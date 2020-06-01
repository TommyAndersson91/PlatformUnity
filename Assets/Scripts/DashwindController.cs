using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashwindController : MonoBehaviour
{
  public float timer = 1f;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (timer <= 0)
    {
      Destroy(gameObject);
    }
    else
    {
      timer -= Time.deltaTime;
    }
  }
}
