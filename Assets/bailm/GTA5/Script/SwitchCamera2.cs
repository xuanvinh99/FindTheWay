using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera2 : MonoBehaviour
{
   
    [Header("Camera to Assign")]
    public GameObject AimCam; // Camera nhắm
    public GameObject ThirdPersonCam; // Camera góc nhìn thứ ba
    public Animator animator; // Animator để điều khiển hoạt ảnh

    private void Update()
    {
        bool isAiming = Input.GetButton("Fire2"); // Kiểm tra nhấn nút nhắm
        bool isMovingForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow); // Kiểm tra di chuyển tới

        if (isAiming)
        {
            animator.SetBool("nham", true);
            AimCam.SetActive(true);
            ThirdPersonCam.SetActive(false);

            if (isMovingForward)
            {
                animator.SetBool("AimWalk", true); // Đang đi trong khi nhắm
                animator.SetBool("ShootAim", false); // Không bắn
            }
            else
            {
                animator.SetBool("AimWalk", false); // Không đi
                animator.SetBool("ShootAim", true); // Chỉ nhắm
            }
        }
        else
        {
            animator.SetBool("nham", false);
            animator.SetBool("ShootAim", false);
            animator.SetBool("AimWalk", false);

            ThirdPersonCam.SetActive(true);
            AimCam.SetActive(false);
        }

        // Kiểm tra bắn khi nhắm và đang di chuyển
        if (Input.GetButton("Fire1"))
        {
            if (isAiming && isMovingForward)
            {
                animator.SetBool("AimWalk", true); // Vẫn đi khi bắn
                animator.SetBool("ShootAim", true); // Bắn
                ThirdPersonCam.SetActive(false);
                AimCam.SetActive(true);
            }
            else if (isAiming)
            {
                animator.SetBool("AimWalk", false); // Không đi khi bắn
                animator.SetBool("ShootAim", true); // Bắn
            }
        }
    }
}