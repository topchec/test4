using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    Rigidbody2D rb;
    Boss bs;
    public float timer;
    public float min_time;
    public float max_time;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        bs = animator.GetComponent<Boss>();
        timer = Random.Range(min_time, max_time);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = new Vector2(-bs.inputX * bs.m_speed, rb.velocity.y);
        //rb.MovePosition(new Vector2(bs.inputX * bs.m_speed, rb.velocity.y));
        //Vector2 pos = new Vector2(rb.position.x + bs.inputX, rb.position.y);
        //animator.transform.position = Vector2.MoveTowards(animator.transform.position, pos, bs.m_speed * Time.fixedDeltaTime);

        
        if (timer <= 0)
        {
            //rb.velocity = new Vector2(0, rb.velocity.y);
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                animator.SetTrigger("Idle");
            }
            else if (rand == 1)
            {
                animator.SetTrigger("Attack");
            }
            else if (rand == 2 && bs.m_grounded)
            {
                rb.velocity = new Vector2(2.5f * bs.inputX, bs.m_jumpForce);
            }
            //else if (rand == 2 && !bs.m_grounded)
            //{
            //    animator.SetTrigger("Idle");
            //}
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
