using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController_Script : MonoBehaviour
{
    [SerializeField]
    private float m_JumpForce = 400f;                           // Amount of force added when the player jumps.
    [Range(0, 1)]
    [SerializeField]
    private float m_CrouchSpeed = .36f;         // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)]
    [SerializeField]
    private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
    [SerializeField]
    private LayerMask m_WhatIsGround;                           // A mask determining what is ground to the character
    [SerializeField]
    private Transform m_GroundCheck;                            // A position marking where to check if the player is grounded.
    [SerializeField]
    private Transform m_CeilingCheck;                           // A position marking where to check for ceilings
    [SerializeField]
    private Collider2D m_CrouchDisableCollider;             // A collider that will be disabled when crouching

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;


    public StatusManager StatusMng;

    public GameObject myWeapon;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        //m_Grounded = false;

        //// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        //// This can be done using layers instead but Sample Assets will not overwrite your project settings.
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    if (colliders[i].gameObject != gameObject)
        //        m_Grounded = true;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            m_Grounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MpItem") && StatusMng.Mp < 70)
        {
            Destroy(collision.gameObject);
            StatusMng.ControlMp(30);
        }

        if (collision.CompareTag("HpItem") && StatusMng.Hp < 70)
        {
            Destroy(collision.gameObject);
            StatusMng.ControlHp(30);
        }

        if (collision.CompareTag("StaminaItem") && StatusMng.Stamina < 70)
        {
            Destroy(collision.gameObject);
            StatusMng.ControlStamina(30);
        }


    }



    IEnumerator SetOffWeapon()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("ASDASDSDADSADS");
        myWeapon.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Atk()
    {
        if (StatusMng.Stamina < 15)
            return;

        StatusMng.ControlStamina(-10);
        GetComponent<Animator>().SetTrigger("isAtk");
        GetComponent<Animator>().SetTrigger("goIdle");

        myWeapon.GetComponent<BoxCollider2D>().enabled = true;
        StartCoroutine(SetOffWeapon());
    }

    
    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        //if (!crouch)
        //{
        //    // If the character has a ceiling preventing them from standing up, keep them crouching
        //    if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        //    {
        //        crouch = true;
        //    }
        //}

        //only control the player if grounded or airControl is turned on

        // If crouching

        if (crouch)
        {
            // Reduce the speed by the crouchSpeed multiplier
            move *= m_CrouchSpeed;

            // Disable one of the colliders when crouching
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            // Enable the collider when not crouching
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;
        }

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
        if (m_Grounded)
        {
            GetComponent<Animator>().SetBool("isJump", false);
        }
        if (move != 0)
        {
            GetComponent<Animator>().SetBool("isRun", true);
        }
        if(jump)
        {
            GetComponent<Animator>().SetBool("isJump", true);
        }
        if(move == 0)
        {
            GetComponent<Animator>().SetBool("isRun", false);
        }
       


        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    
    
}

