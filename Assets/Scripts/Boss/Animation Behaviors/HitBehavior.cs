using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehavior : StateMachineBehaviour
{
  float timer = 1f;
  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {

  }

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (timer <= 0 && animator.GetComponent<Boss>().health > 0)
    {
      animator.SetTrigger("idle");
    }
    else if (animator.GetComponent<Boss>().health > 0)
    {
      timer -= Time.deltaTime;
    }
    else
    {
      // animator.GetComponent<BoxCollider2D>().enabled = false;
      animator.GetComponent<Collider2D>().enabled = false;
      animator.SetTrigger("dead");
    }
  }

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {

  }
}
