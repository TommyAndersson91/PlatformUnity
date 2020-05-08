using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBehavior : StateMachineBehaviour
{
  public float speed;

  public float timer;
  public float minTime;
  public float maxTime;
  public float attackRange;

  private Transform playerPos;

  // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    timer = Random.Range(minTime, maxTime);
    playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    speed = 6f;
    attackRange = 3f;
  }

  // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (timer <= 0)
    {
      animator.SetTrigger("idle");
    }
    else
    {
      timer -= Time.deltaTime;
    }
    Vector2 target = new Vector2(playerPos.position.x, animator.transform.position.y);
    animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);
    if (target.x > animator.transform.position.x)
    {
      animator.transform.localScale = new Vector2(0.4f, 0.4f);
    }
    else
    {
      animator.transform.localScale = new Vector2(-0.4f, 0.4f);
    }
    float distance = Vector2.Distance(playerPos.position, animator.transform.position);
    if (distance < attackRange)
    {
      if (Random.Range(0, 2) == 0)
      {
        animator.SetTrigger("attack1");
      }
      else
      {
        animator.SetTrigger("attack3");
      }
    }
  }

  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {

  }


}
