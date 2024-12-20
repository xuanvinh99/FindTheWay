using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("Item Info")]
    public int itemPrice;
    public float itemRadius =0.5f; 
    public string ItemTag;
    private GameObject ItemToPick;

    [Header("Player Info")]
    public Player2 player2;
    public Inventory inventory;
    public GameObject pickupUI;


    private void Start()
    {
        ItemToPick = GameObject.FindWithTag(ItemTag);
          pickupUI.SetActive(false);
    }

   private void Update()
    {
        // Kiểm tra khoảng cách giữa người chơi và vật phẩm
        if (Vector3.Distance(transform.position, player2.transform.position) < itemRadius)
        {
            pickupUI.SetActive(true); // Hiện giao diện người dùng
            if (Input.GetKeyDown("f")) // Kiểm tra phím "f"
            {
                HandleItemPurchase(); // Xử lý việc mua vật phẩm
            }
        }
        else
        {
            pickupUI.SetActive(false); // Ẩn giao diện khi ra ngoài bán kính
        }
    }

    private void HandleItemPurchase()
    {
        if (itemPrice > player2.playerMoney) // Kiểm tra xem có đủ tiền không
        {
            Debug.Log("Bạn không có đủ tiền.");
            // Hiện giao diện cho thiếu tiền
            return;
        }

        player2.playerMoney -= itemPrice; // Trừ tiền khi mua

        // Kích hoạt vật phẩm theo nhãn
        switch (ItemTag)
        {
            case "HandGunPickUp":
                inventory.Weapon1.SetActive(true);
                inventory.isWeapon1Picked = true;
                break;
            case "ShortGunPickUp":
                inventory.Weapon2.SetActive(true);
                inventory.isWeapon2Picked = true;
                break;
            case "UziPickUp":
                inventory.Weapon3.SetActive(true);
                inventory.isWeapon3Picked = true;
                break;
            case "BazookaPickUp":
                inventory.Weapon4.SetActive(true);
                inventory.isWeapon4Picked = true;
                break;
        }

        Debug.Log(ItemTag + " đã được mua.");
    }
}