using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
  public float attackRange;
  public Transform playerPos;

  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    animator.GetComponent<Boss>().isAttacking = true;
    attackRange = 3.5f;
    playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
  }

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    Vector2 target = new Vector2(playerPos.position.x, animator.transform.position.y);
    float distance = Vector2.Distance(playerPos.position, animator.transform.position);
    if (distance > attackRange)
    {
      animator.SetTrigger("idle");
    }
  }

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    animator.GetComponent<Boss>().isAttacking = false;
  }
}
