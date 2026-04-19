using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attack : StateMachineBehaviour
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
        timer = Random.Range(1, 3)*0.75f;
        if (!bs.m_isJump && bs.m_grounded)
        {
            if (bs.player.position.x < bs.transform.position.x)
            {
                bs.inputX = -1;
            }
            else
            {
                bs.inputX = 1;
            }
        }
        
        bs.transform.localScale = new Vector3(3.0f*bs.inputX, 3.0f, 1.0f);
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
        {
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                animator.SetTrigger("Idle");
            }
            else if (rand == 1)
            {
                animator.SetTrigger("Run");
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
