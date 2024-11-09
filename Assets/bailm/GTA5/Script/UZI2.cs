using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UZI2 : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;
    public float giveDamage = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 10f;
    private float nextTimeToShoot = 0f;
    public Transform hand;
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


    private void Awake()
    {
        transform.SetParent(hand);
        Cursor.lockState = CursorLockMode.Locked;
        presentAmmunition = maximumAmmunition;
    }

    private void Update()
    {
        if (setReloading)
            return;

        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if(isMoving == false)
        {   
            if(Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
            {
                nextTimeToShoot = Time.time + 1f / fireCharge;
                Shoot();
            }
        }
        
    }
    void Shoot()
    {
        if (mag == 0)
        {
            //show aumo out text/UI
        }
        presentAmmunition--;

        if (presentAmmunition == 0)
        {
            mag--;  
        }

        muzzleSpark.Play();

        RaycastHit hitInfo;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);

            Opject obj = hitInfo.transform.GetComponent<Opject>();

            if(obj != null)
            {
                obj.objectHitDamage(giveDamage);
                GameObject metalEffectGo = Instantiate(metalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(metalEffectGo, 1f);
            }
        }
    }
    IEnumerator Reload()
    {
        setReloading = true;
        Debug.Log("Reloading...");
        //play reload sound
        yield return new WaitForSeconds(reloadingTime);
        Debug.Log("Done Reloading...");
        presentAmmunition = maximumAmmunition;
        setReloading = false;
    }
}
