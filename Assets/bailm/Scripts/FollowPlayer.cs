using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, 0); 

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
            transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);
        }
    }
}
