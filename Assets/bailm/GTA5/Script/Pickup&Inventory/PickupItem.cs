using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
 [Header("Item Info")]
 public int itemPrice;
 public int itemRadius;
 public string ItemTag;
 private GameObject ItemToPick;

[Header("Player Info")]
public Player2 player2;
private void Start(){
    ItemToPick = GameObject.FindWithTag(ItemTag);
}
private void Update()
{
    if(Vector3.Distance(transform.position,player2.transform.position)<itemRadius){
        Debug.Log("Inside the radius");
    }
    if(Input.GetKeyDown("f")){
        if(itemPrice > player2.playerMoney)
        {
            Debug.Log("You are boke");
            //show UI
        }
        else{
            if(ItemTag == "HandGunPickUp"){
                player2.playerMoney -= itemPrice;

                Debug.Log(ItemTag);
            }
            else if(ItemTag == "ShortGunPickUp"){
                 player2.playerMoney -= itemPrice;

                Debug.Log(ItemTag);
            }
                  else if(ItemTag == "UziPickUp"){
                 player2.playerMoney -= itemPrice;

                Debug.Log(ItemTag);
            }
                else if(ItemTag == "BazookaPickUp"){
                 player2.playerMoney -= itemPrice;

                Debug.Log(ItemTag);
            }
            ItemToPick.SetActive(false);
            
            
        }
    }
}
}
