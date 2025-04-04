using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static Currency playerCurrency = new Currency();
    private static IDataService dataService = new JsonDataService();
    private const string CURRENCY_PATH = "currency.json";

    private void SerializeJson()
    {
        if (dataService.SaveData(CURRENCY_PATH, playerCurrency))
        {
            Debug.Log("Data saved successfully");
        }
        else
        {
            Debug.Log("Data failed to save");
        }
    }

    private void LoadJsonCurrency()
    {
        try
        {
            playerCurrency = dataService.LoadData<Currency>(CURRENCY_PATH);
            
        }
        catch (Exception e)
        {
            Debug.LogError("Could not load file!");
            throw e;
        }
    }

    public Currency ReturnCurrency()
    {
        return playerCurrency;
    }
}
