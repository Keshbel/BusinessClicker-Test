using System;
using TMPro;
using UnityEngine;

public class BalanceController : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceTMP;

    [SerializeField] private int balance;
    public int Balance
    {
        get => balance;
        set
        {
            balance = value;
            if (balance < 0) balance = 0;
            BalanceChangedAction?.Invoke();
            balanceTMP.text = value + "$";
        }
    }

    public Action BalanceChangedAction;

    private void Start()
    {
        Balance = PlayerPrefs.GetInt("Balance", 150);
    }

    public bool PurchaseAvailability(int value)
    {
        return Balance - value >= 0;
    }

    #region Save

    private void Save()
    {
        PlayerPrefs.SetInt("Balance", Balance);
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    #endregion

    #region Singleton

    public static BalanceController Instance;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    #endregion
}
