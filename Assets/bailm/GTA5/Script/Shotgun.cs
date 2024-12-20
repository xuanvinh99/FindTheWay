using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    // Rifle movement
    public int maxReloads = 10; // Giới hạn số lần nạp đạn
    private int remainingReloads; 
    
    [Header("Player Settings")]
    public float turnCalmTime = 0.1f;
    public Transform playerBody;
    private Coroutine reloadCoroutine;

    [Header("Player Movement")]
    public float playerSpeed = 1.1f;
    public float playerSprint = 2f;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Script Cameras")]
    public Transform playerCamera;

    [Header("Player jumping and velocity")]
    float turnCalmVelocity;
    public float jumpRange = 1f;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    // Rifle shooting var
    [Header("Rifle Things")]
    public Camera cam;
    public float giveDamage = 5050f;
    public float shootingRange = 50f;
    public float fireCharge = 30f;
    private float nextTimeToShoot = 1.55f;
    public Transform hand;
    public Transform PlayerTransform;
    public bool isMoving;

    [Header("Rifle Ammunition and shooting")]
    private int maximumAmmunition = 1000; // Số viên trong một lần nạp
    public int mag = 100; // Tổng số viên đạn trong kho
    private int presentAmmunition;
    private int bulletsFired = 0; // Số viên đạn đã bắn
    public float reloadingTime = 1.11f;
    private bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject metalEffect;

    [Header("Sounds && UI")]
    bool ShotgunActive = true;
    
    private void Awake()
    {
        transform.SetParent(hand);
        Cursor.lockState = CursorLockMode.Locked;
        presentAmmunition = maximumAmmunition;
        remainingReloads = maxReloads; 
    }

    private void Update()
    {
    RotateTowardsMouse(); 
    if (ShotgunActive)
    {
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("ShotgunAnimator");
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

    // Kiểm tra nếu đang nạp đạn
    if (setReloading)
    {
        return; // Nếu đang nạp đạn, không cho phép bắn
    }


    // Kiểm tra xem có đạn không
    if (presentAmmunition <= 0 && mag > 0)
    {
        StartCoroutine(Reload());
        return;
    }

    // Kiểm tra bắn
    if (!isMoving && Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
    {
        if (presentAmmunition > 0) // Kiểm tra có đạn trước khi bắn
        {
            animator.SetBool("Shoot", true);
            nextTimeToShoot = Time.time + 1.5f / fireCharge;
            Shoot();
            bulletsFired++;

            // Kiểm tra nếu đã bắn 10 viên
            if (bulletsFired >= 10)
            {
                Debug.Log("Đã bắn 10 viên, cần nạp lại!");
                StartCoroutine(Reload());
            }
        }
        else
        {
            Debug.Log("Hết đạn, không thể bắn!");
        }
    }
    else
    {
        animator.SetBool("Shoot", false);
    }    
    }

    void RotateTowardsMouse()
    {
        if (playerCamera == null || playerBody == null)
        {
            Debug.LogError("playerCamera or playerBody is null");
            return;
        }

        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, playerBody.position);

        if (plane.Raycast(ray, out float hit))
        {
            Vector3 mousePosition = ray.GetPoint(hit);
            Vector3 direction = mousePosition - playerBody.position;
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, turnCalmTime);
            }
        }
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;
        animator.SetBool("Reload", false);
        
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
            animator.SetBool("WalkForward", true);
            animator.SetBool("RunForward", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(PlayerTransform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            PlayerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            Vector3 targetVelocity = moveDirection.normalized * playerSpeed;
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10f);
        }
        else
        {
            velocity = Vector3.zero;
            isMoving = false;
            animator.SetBool("WalkForward", false);
            animator.SetBool("RunForward", false);
        }
        
        cC.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("IdleAim", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.SetBool("IdleAim", true);
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && onSurface)
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("WalkForward", false);
                animator.SetBool("RunForward", true);
                isMoving = true;

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(PlayerTransform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                PlayerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
            }
        }
        else
        {
            animator.SetBool("RunForward", false);
            if (isMoving)
            {
                animator.SetBool("WalkForward", true);
            }
            else
            {
                animator.SetBool("WalkForward", false);
            }
            isMoving = false;
        }       
    }

    void Shoot()
    {
        if (mag == 0)
        {
            return; // Hiển thị thông báo hết đạn
        }

        presentAmmunition--;

        if (presentAmmunition == 0 && mag > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        muzzleSpark.Play();

        RaycastHit hitInfo;
        Vector3 shootDirection = playerCamera.transform.forward;

        if (Physics.Raycast(playerCamera.transform.position, shootDirection, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            Opject obj = hitInfo.transform.GetComponent<Opject>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            Zombie2 zombie2 = hitInfo.transform.GetComponent<Zombie2>();

            if (obj != null)
            {
                obj.objectHitDamage(giveDamage);
                GameObject metalEffectGo = Instantiate(metalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(metalEffectGo, 1f);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamage);
                GameObject goreEffectGo = Instantiate(metalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if (zombie2 != null)
            {
                zombie2.zombieHitDamage(giveDamage);
                GameObject goreEffectGo = Instantiate(metalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
        }
    }

   IEnumerator Reload()
{
      if (remainingReloads <= 0)
        {
            Debug.Log("Không thể nạp đạn nữa!");
            yield break; // Dừng coroutine nếu không còn nạp đạn
        }

        playerSpeed = 0f;
        playerSprint = 0f;
        setReloading = true;
        animator.SetBool("Reload", true);
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadingTime);

        if (setReloading)
        {
            Debug.Log("Done Reloading...");
            animator.SetBool("Reload", false);
            presentAmmunition = maximumAmmunition; // Đặt lại đạn
            remainingReloads--; // Giảm số lần nạp đạn còn lại
            playerSpeed = 1.1f;
            playerSprint = 5f;
            setReloading = false;
        }
}
}