using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    public Player2 player2;
    public Text MoneyAmountText;

    private void Update()
    {
        MoneyAmountText.text =   "" + player2.playerMoney;
    }
}
