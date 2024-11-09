using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Opject : MonoBehaviour
{
    public float objectHealth = 120f;

    public void objectHitDamage(float amount)
    {
        objectHealth -= amount;
        if (objectHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}

