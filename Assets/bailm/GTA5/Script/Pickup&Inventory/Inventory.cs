using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Item slots")]
    public GameObject Weapon1;
    public bool isWeapon1Picked = false;
    public bool isWeapon1Active = false;
    public GameObject Weapon2;
    public bool isWeapon2Picked = false;
    public bool isWeapon2Active = false;
    public GameObject Weapon3;
    public bool isWeapon3Picked = false;
    public bool isWeapon3Active = false;
    public GameObject Weapon4;
    public bool isWeapon4Picked = false;
    public bool isWeapon4Active = false;

    [Header("Weapon to Use")]
    public GameObject HandGun1;
    public GameObject HandGun2;
    public GameObject Shotgun;
    public GameObject UZI;
    public GameObject UZI2;
    public GameObject Bazooka;
        bool Player2Active = true;

    [Header("Scripts")]
    public PlayerScript2 playerScript2;
    public Shotgun shotgunScript2;
    public HandGun handgun1Script2;
    public HandGun2 handgun2Script2;
    public UZI uziScript2;
    public UZI2 uzi2Script2;
    public Bazooka bazooka;

    [Header("Inventory")]
    public GameObject inventoryPanel;
    bool isPause = false;

    public SwitchCamera2 switchCamera2;
    public GameObject AimCam;
    public GameObject ThirdPersonCamera;

    private void Update()
    {
        if (Input.GetKeyDown("1") && isWeapon1Picked == true)
        {
            isWeapon1Active = true;
            isWeapon2Active = false;
            isWeapon3Active = false;
            isWeapon4Active = false;
            isRifleACtive();
        }
        else if (Input.GetKeyDown("2") && isWeapon2Picked == true)
        {
            isWeapon1Active = false;
            isWeapon2Active = true;
            isWeapon3Active = false;
            isWeapon4Active = false;
            isRifleACtive();
        }
        else if (Input.GetKeyDown("3") && isWeapon3Picked == true)
        {
            isWeapon1Active = false;
            isWeapon2Active = false;
            isWeapon3Active = true;
            isWeapon4Active = false;
            isRifleACtive();
        }
        else if (Input.GetKeyDown("4") && isWeapon4Picked == true)
        {
            isWeapon1Active = false;
            isWeapon2Active = false;
            isWeapon3Active = false;
            isWeapon4Active = true;
            isRifleACtive();
        }
         else if (Input.GetKeyDown("q") ){
             isWeapon1Active = false;
            isWeapon2Active = false;
            isWeapon3Active = false;
            isWeapon4Active = false;

            // Kích hoạt lại điều khiển của nhân vật
            playerScript2.GetComponent<PlayerScript2>().enabled = true; 

            // Tắt tất cả các vũ khí
            HandGun1.SetActive(false);
            HandGun2.SetActive(false);
            Shotgun.SetActive(false);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(false);
            
        
         }
        else if (Input.GetKeyDown("tab"))
        {
            if (isPause)
            {
                hideInvetory();
            }
            else
            {
                showInventory();
            }
        }

    }

    void isRifleACtive()
    {
        if (isWeapon1Active == true)
        {
            HandGun1.SetActive(true);
            HandGun2.SetActive(true);
            Shotgun.SetActive(false);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(false);

            playerScript2.GetComponent<PlayerScript2>().enabled = false;
            shotgunScript2.GetComponent<Shotgun>().enabled = false;
            handgun1Script2.GetComponent<HandGun>().enabled = true;
            handgun2Script2.GetComponent<HandGun2>().enabled = true;
            uziScript2.GetComponent<UZI>().enabled = false;
            uzi2Script2.GetComponent<UZI2>().enabled = false;
            bazooka.GetComponent<Bazooka>().enabled = false;
        }
        else if (isWeapon2Active == true)
        {
            HandGun1.SetActive(false);
            HandGun2.SetActive(false);
            Shotgun.SetActive(true);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(false);

            playerScript2.GetComponent<PlayerScript2>().enabled = false;
            shotgunScript2.GetComponent<Shotgun>().enabled = true;
            handgun1Script2.GetComponent<HandGun>().enabled = false;
            handgun2Script2.GetComponent<HandGun2>().enabled = false;
            uziScript2.GetComponent<UZI>().enabled = false;
            uzi2Script2.GetComponent<UZI2>().enabled = false;
            bazooka.GetComponent<Bazooka>().enabled = false;
        }
        else if (isWeapon3Active == true)
        {
            HandGun1.SetActive(true);
            HandGun2.SetActive(true);
            Shotgun.SetActive(false);
            UZI.SetActive(true);
            UZI2.SetActive(true);
            Bazooka.SetActive(false);

            playerScript2.GetComponent<PlayerScript2>().enabled = false;
            shotgunScript2.GetComponent<Shotgun>().enabled = false;
            handgun1Script2.GetComponent<HandGun>().enabled = false;
            handgun2Script2.GetComponent<HandGun2>().enabled = false;
            uziScript2.GetComponent<UZI>().enabled = true;
            uzi2Script2.GetComponent<UZI2>().enabled = true;
            bazooka.GetComponent<Bazooka>().enabled = false;
        }
        else if (isWeapon4Active == true)
        {
            HandGun1.SetActive(false);
            HandGun2.SetActive(false);
            Shotgun.SetActive(false);
            UZI.SetActive(false);
            UZI2.SetActive(false);
            Bazooka.SetActive(true);

            playerScript2.GetComponent<PlayerScript2>().enabled = false;
            shotgunScript2.GetComponent<Shotgun>().enabled = false;
            handgun1Script2.GetComponent<HandGun>().enabled = false;
            handgun2Script2.GetComponent<HandGun2>().enabled = false;
            uziScript2.GetComponent<UZI>().enabled = false;
            uzi2Script2.GetComponent<UZI2>().enabled = false;
            bazooka.GetComponent<Bazooka>().enabled = true;
        }
    }

    void showInventory()
    {
        switchCamera2.GetComponent<SwitchCamera2>().enabled = false;
        ThirdPersonCamera.SetActive(false);
        AimCam.SetActive(false);

        inventoryPanel.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }

    void hideInvetory()
    {
        switchCamera2.GetComponent<SwitchCamera2>().enabled = true;
        ThirdPersonCamera.SetActive(true);
        AimCam.SetActive(true);

        inventoryPanel.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }
}