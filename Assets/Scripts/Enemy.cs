using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public GameObject DeathFx;
  GameObject died;


public void Die(Transform obj)
{
 died = Instantiate(DeathFx, obj.position, Quaternion.identity);
StartCoroutine(AnimTime());
}
IEnumerator AnimTime()
{
  yield return new WaitForSeconds(0.5f);
  Destroy(died);
}
}
