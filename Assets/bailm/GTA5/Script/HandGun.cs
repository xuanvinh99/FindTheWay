using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
    public int maxReloads = 3; // Giới hạn số lần nạp đạn
    private int remainingReloads; // Số lần nạp đạn còn lại
    [Header("Player Settings")]
    public float turnCalmTime = 0.1f;
    public Transform playerBody;
    private Coroutine reloadCoroutine;

    // Rifle movement
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
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 10f;
    private float nextTimeToShoot = 0f;
    public Transform hand;
    public Transform PlayerTransform;
    public HandGun2 handgun2;
    public bool isMoving;

    [Header("Rifle Ammunition and shooting")]
    private int maximumAmmunition = 25;
    public int mag = 10;
    private int presentAmmunition;
    public float reloadingTime = 4.3f;
    private bool setReloading = false;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject metalEffect;

    [Header("Sounds && UI")]
    bool HandgunActive = true;

    private void Awake()
    {
        transform.SetParent(hand);
        Cursor.lockState = CursorLockMode.Locked;
        presentAmmunition = maximumAmmunition;
        remainingReloads = maxReloads; // Khởi tạo số lần nạp đạn
    }

    void RotateTowardsMouse()
    {
        if (playerCamera == null || playerBody == null)
        {
            Debug.LogError("playerCamera or playerBody is null");
            return;
        }

        Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, playerBody.position); // Sử dụng vị trí của playerBody

        if (plane.Raycast(ray, out float hit))
        {
            Vector3 mousePosition = ray.GetPoint(hit);
            Vector3 direction = mousePosition - playerBody.position; // Tính toán hướng từ playerBody

            direction.y = 0; // Đảm bảo không thay đổi chiều y

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, turnCalmTime);
            }
        }
    }

    private void Update()
    {
        RotateTowardsMouse(); // Gọi hàm quay theo chuột

        if (HandgunActive)
        {
            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("GunAnimator");
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

        // Kiểm tra nếu đang reload
        if (setReloading)
        {
            // Kiểm tra nếu có phím di chuyển được nhấn
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                if (reloadCoroutine != null)
                {
                    reloadingTime = 0f;
                    StopCoroutine(reloadCoroutine); // Hủy quá trình reload
                    reloadCoroutine = null; // Đặt lại coroutine
                }
                setReloading = false;
                animator.SetBool("Reload", false); // Ngừng animation reload
            }
            return; // Nếu đang reload thì không cần xử lý thêm
        }

        if (Input.GetKeyDown(KeyCode.R) && presentAmmunition < maximumAmmunition)
        {
            reloadCoroutine = StartCoroutine(Reload()); // Lưu coroutine
            return;
        }

        if (presentAmmunition <= 0)
        {
            reloadCoroutine = StartCoroutine(Reload()); // Lưu coroutine
            return;
        }

        if (!isMoving)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
            {
                animator.SetBool("Shoot", true);
                nextTimeToShoot = Time.time + 1f / fireCharge;

                Shoot();
            }
            else
            {
                animator.SetBool("Shoot", false);
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
            animator.SetBool("WalkForward", true);
            animator.SetBool("RunForward", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(PlayerTransform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            PlayerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Cập nhật vận tốc
            Vector3 targetVelocity = moveDirection.normalized * playerSpeed;
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * 10f); // Điều chỉnh độ mượt
        }
        else
        {
            // Nếu không có di chuyển, đặt vận tốc về 0
            velocity = Vector3.zero;
            animator.SetBool("WalkForward", false);
            animator.SetBool("RunForward", false);
            jumpRange = 1f; // Có thể nhảy khi đứng yên/-strong/-heart:>:o:-((:-h isMoving = false;
            handgun2.isMoving = false;
        }

        // Di chuyển CharacterController
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
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(PlayerTransform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                PlayerTransform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                jumpRange = 0f;
                isMoving = true;
                handgun2.isMoving = true;
                setReloading = false;
                animator.SetBool("Reload", false);
            }
            else
            {
                animator.SetBool("WalkForward", true);
                animator.SetBool("RunForward", false);
                jumpRange = 1f;
                isMoving = false;
                handgun2.isMoving = false;
            }
        }
    }

    void Shoot()
    {
        if (mag == 0)
        {
            // Hiển thị thông báo hết đạn
            return;
        }

        presentAmmunition--;

        if (presentAmmunition == 0)
        {
            mag--;
            if (mag < 0)
            {
                Debug.Log("Hết đạn trong băng!");
                return; // Không thể bắn nếu không còn đạn
            }
        }
        muzzleSpark.Play();
        setReloading = false;
        animator.SetBool("Reload", false);

        RaycastHit hitInfo;
        Vector3 shootDirection = playerCamera.transform.forward; // Lấy hướng bắn từ camera
        RotateTowardsMouse();
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