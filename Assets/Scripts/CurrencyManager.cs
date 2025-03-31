using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private Currency playerCurrency = new Currency();
    private IDataService dataService = new JsonDataService();

    public void SerializeJson()
    {
        if (dataService.SaveData("currency.json", playerCurrency))
        {
            Debug.Log("Data saved successfully");
        }
        else
        {
            Debug.Log("Data failed to save");
        }
    }

    private void Start()
    {
        playerCurrency.currencyAmount = 100;
        SerializeJson(); 
    }
}
