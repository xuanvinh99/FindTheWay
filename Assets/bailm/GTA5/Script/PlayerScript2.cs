using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript2 : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float playerSprint = 3f;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;
    
    [Header("Player Health Things")]
    private float playerHealth = 120f;
    public float presentHealth;
    public GameObject playerDamage;
    public HealthBar healthBar;

    [Header("Player Script Cameras")]
    public Transform playerCamera;

    [Header("Player jumping and velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public float jumpRange = 1f;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;
    bool Player2Active = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }
    private void Awake()
    {
        Cursor.lockState =CursorLockMode.Locked;
    }
    private void Update()
    {
        if(Player2Active == true)
        {
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("PlayerControler2");
        }
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);
        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);
        playerMove();
        Jump();
        Sprint();
    }


    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
            jumpRange = 0f;
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            jumpRange = 1f;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                jumpRange = 0f;
            }
            else
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                jumpRange = 1f;
            }
        }
    }
    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        StartCoroutine(PlayerDamage());

        healthBar.SetHealth(presentHealth);

        if (presentHealth <= 0)
        {
            playerDie();
        }
    }

    private void playerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject, 1.0f);
    }

    IEnumerator PlayerDamage()
    {
        playerDamage.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        playerDamage.SetActive(false);
    }
}
