using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToHit : MonoBehaviour
{
    public float ObjectHealth = 100f;

    public void ObjectHitDamage(float amount)
    {
        ObjectHealth -= amount;
        if (ObjectHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
