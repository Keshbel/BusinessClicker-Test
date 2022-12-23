using System;
using UnityEngine;

[Serializable]
public class BusinessData
{
    [Header("MainParameters")]
    public string businessName;
    public int baseDelay;
    public int baseLevelUpPrice;
    public int baseIncome;

    [Header("Upgrade 1")]
    public string baseUpgrade1Name;
    public int baseUpgrade1Price;
    public int baseUpgrade1Income;

    [Header("Upgrade 2")]
    public string baseUpgrade2Name;
    public int baseUpgrade2Price;
    public int baseUpgrade2Income;
}
