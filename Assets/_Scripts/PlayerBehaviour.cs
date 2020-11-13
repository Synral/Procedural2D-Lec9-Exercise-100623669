using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public InputActionMap input;
    public bool isGrounded;
    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    float _moveSpeed = 10.0f;
    float _jumpStrength = 1.0f;
    float _direction = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _Move(_direction);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<float>();
        
        if (_direction <0.0f)
            m_spriteRenderer.flipX = true;
        else if (_direction>0.0f)
            m_spriteRenderer.flipX = false;
    }
    void _Move(float temp)
    {
        m_rigidBody2D.AddForce(new Vector2(_moveSpeed * temp, 0.0f));
        if (m_rigidBody2D.velocity.magnitude == 0.0f)
            m_animator.SetInteger("AnimState", 0);
        else if (isGrounded)
            m_animator.SetInteger("AnimState", 1);
        else
            m_animator.SetInteger("AnimState", 2);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        float temp = context.ReadValue<float>();
        if (temp>0.0f && isGrounded)
        {
            m_rigidBody2D.AddForce(new Vector2(0.0f, Mathf.Sqrt(_jumpStrength * Physics.gravity.y * -2.0f)), ForceMode2D.Impulse);
            m_animator.SetInteger("AnimState", 2);
        }
    }
    
    public void OnGenerate(InputAction.CallbackContext context)
    {
        float temp = context.ReadValue<float>();
        if (temp>0.0f)
            GameObject.Find("WorldController").GetComponent<TileController>().Generate();
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        isGrounded = true;
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        isGrounded = false;
    }
}
