using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OpossumController : MonoBehaviour
{
 private float speed = 15f;
 public float distance;
 private bool movingLeft = true;

  [SerializeField] private LayerMask ground;

private void Start() {
}

 public Transform groundsDetection; 

 private void Update() {
    if (GetComponent<Renderer>().isVisible)
    {
    
   transform.Translate(Vector2.left * speed * Time.deltaTime);

    RaycastHit2D groundInfo = Physics2D.Raycast(groundsDetection.position, Vector2.down, distance, ground);

   if (groundInfo.collider == false)
   {
     if(movingLeft == true)
     {
       transform.eulerAngles = new Vector3(0, -180, 0);
       movingLeft = false;
     }
       else
       {
        transform.eulerAngles = new Vector3(0, 0, 0);
           movingLeft = true;
       }
     }
   }
 }
 }

