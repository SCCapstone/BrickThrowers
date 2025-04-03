using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency
{
    private int currencyAmount = 0; // Start with total value at a scene.

    public int CurrencyAmount
    {
        get => currencyAmount;
        set => currencyAmount = value;
    }
}
