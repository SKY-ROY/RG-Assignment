using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float newXPos = 0f;
    private float moveX;
    private float moveY;
    private float colliderHeight;
    private float colliderCenterY;
    private CharacterController m_char;
    private Animator m_Animator;

    [HideInInspector]
    public bool swipeLeft, swipeRight, swipeUp, swipeDown;
    
    public SIDE m_side = SIDE.Mid;
    public float xValue;
    public float jumpSpeed = 7.5f;
    public float dodgeSpeed = 10f;
    public float fwdMovementSpeed = 7.5f;
    public float slideDuration = 0.5f;
    public bool inJump;
    public bool inRoll;

    void Start()
    {
        InititalizeReference();
        ResetPosition();
    }

    void Update()
    {
        MovePlayer();
    }

    private void InititalizeReference()
    {
        m_char = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        colliderHeight = m_char.height;
        colliderCenterY = m_char.center.y;
    }

    private void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
    
    private void MovePlayer()
    {
        swipeLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        swipeRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        swipeUp = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        swipeDown = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        
        if(swipeLeft && !inRoll)
        {
            if(m_side == SIDE.Mid)
            {
                newXPos = -xValue;
                m_side = SIDE.Left;
                m_Animator.Play("DodgeLeft");
            }
            else if(m_side == SIDE.Right)
            {
                newXPos = 0f;
                m_side = SIDE.Mid;
                m_Animator.Play("DodgeLeft");
            }
        }
        else if (swipeRight && !inRoll)
        {
            if (m_side == SIDE.Mid)
            {
                newXPos = xValue;
                m_side = SIDE.Right;
                m_Animator.Play("DodgeRight");
            }
            else if (m_side == SIDE.Left)
            {
                newXPos = 0f;
                m_side = SIDE.Mid;
                m_Animator.Play("DodgeRight");
            }
        }

        Vector3 moveVector = new Vector3(moveX - transform.position.x, moveY * Time.deltaTime, fwdMovementSpeed * Time.deltaTime);
        moveX = Mathf.Lerp(moveX, newXPos, Time.deltaTime * dodgeSpeed);
        m_char.Move(moveVector);
        
        Jump();
        Slide();
    }

    public void Jump()
    {
        if(m_char.isGrounded)
        {
            if(m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Fall"))
            {
                m_Animator.Play("Land");
                inJump = false;
            }
            if(swipeUp)
            {
                moveY = jumpSpeed;
                m_Animator.CrossFadeInFixedTime("Jump", 0.1f);
                inJump = true;
            }
        }
        else
        {
            moveY = moveY - jumpSpeed * 2 * Time.deltaTime;
            if(m_char.velocity.y<-0.1f)
            {
                m_Animator.Play("Fall");
            }
        }
    }
    internal float slideCounter;
    public void Slide()
    {
        slideCounter -= Time.deltaTime;
        if(slideCounter <= 0f)
        {
            slideCounter = 0f;
            m_char.center = new Vector3(0, colliderCenterY, 0);
            m_char.height = colliderHeight;
            inRoll = false;
        }
        if(swipeDown)
        {
            slideCounter = slideDuration;
            moveY = moveY - 10f;

            m_char.center = new Vector3(0, colliderCenterY / 2, 0);
            m_char.height = colliderHeight / 2;

            m_Animator.CrossFadeInFixedTime("Slide", 0.1f);
            inRoll = true;
            inJump = false;
        }
    }
}
