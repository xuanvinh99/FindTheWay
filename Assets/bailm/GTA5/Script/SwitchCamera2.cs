using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera2 : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject ThirdPersonCam;
    public Animator animator;

    private void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("AimWalk", true);
            animator.SetBool("ShootAim", false);
            ThirdPersonCam.SetActive(false);
            AimCam.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            animator.SetBool("AimWalk", true);
            animator.SetBool("ShootAim", true);
            ThirdPersonCam.SetActive(false);
            AimCam.SetActive(true);
        }
        else
        {
            animator.SetBool("AimWalk", false);
            animator.SetBool("ShootAim", false);
            
            ThirdPersonCam.SetActive(true);
            AimCam.SetActive(false);
        }

         if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("AimWalk", true);
            animator.SetBool("ShootAim", false);
            ThirdPersonCam.SetActive(false);
            AimCam.SetActive(true);
        }
        
         else
        {
            animator.SetBool("AimWalk", false);
            animator.SetBool("ShootAim", false);
            
            ThirdPersonCam.SetActive(true);
            AimCam.SetActive(false);
        }

    }
}