using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BusinessController : MonoBehaviour
{
    public int index;
    public BusinessData baseData;
    
    [Header("TMP")]
    [SerializeField] private TMP_Text nameTMP;
    [SerializeField] private TMP_Text levelTMP;
    [SerializeField] private TMP_Text incomeTMP;
    [SerializeField] private TMP_Text levelUpPriceTMP;
    [Space]
    [SerializeField] private TMP_Text upgrade1NameTMP;
    [SerializeField] private TMP_Text upgrade1IncomeTMP;
    [SerializeField] private TMP_Text upgrade1PriceTMP;
    [Space]
    [SerializeField] private TMP_Text upgrade2NameTMP;
    [SerializeField] private TMP_Text upgrade2IncomeTMP;
    [SerializeField] private TMP_Text upgrade2PriceTMP;

    [Header("Buttons")] 
    [SerializeField] private Button levelUpButton;
    [SerializeField] private Button upgrade1Button;
    [SerializeField] private Button upgrade2Button;

    [Header("Progress Bar")] 
    [SerializeField] private Image progressBar;
    public float currentBarValue;
    public float delayBarValue;
    
    [Header("Parameters")]
    public int level;
    public int income;
    public int levelUpPrice;
    
    [Header("Upgrade 1")]
    public bool isUpgrade1Used;
    public int upgrade1Income;

    [Header("Upgrade 2")] 
    public bool isUpgrade2Used;
    public int upgrade2Income;

    void Awake()
    {
        BalanceController.Instance.BalanceChangedAction += CheckButtonAccessibility;

        levelUpButton.onClick.AddListener(LevelUp);
        upgrade1Button.onClick.AddListener(OnUpgrade1Button);
        upgrade2Button.onClick.AddListener(OnUpgrade2Button);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // Load/Initializations
        nameTMP.text = baseData.businessName;

        currentBarValue = PlayerPrefs.GetFloat($"currentBarValue{index}", 0);
        delayBarValue = baseData.baseDelay;
        
        var levelValue = index == 0 ? 1 : 0; 
        level = PlayerPrefs.GetInt($"level{index}", levelValue);
        income = PlayerPrefs.GetInt($"income{index}", baseData.baseIncome);
        UpdateIncomeTMP();
        levelUpPrice = PlayerPrefs.GetInt($"levelUpPrice{index}", baseData.baseLevelUpPrice);
        UpdateLevel();

        upgrade1NameTMP.text = baseData.baseUpgrade1Name;
        isUpgrade1Used = PlayerPrefsExtra.GetBool($"isUpgrade1Used{index}", false);
        if (isUpgrade1Used) upgrade1Income = baseData.baseUpgrade1Income;
        upgrade1IncomeTMP.text = "+" + baseData.baseUpgrade1Income + "%";
        UpdateUpgrade1();

        upgrade2NameTMP.text = baseData.baseUpgrade2Name;
        isUpgrade2Used = PlayerPrefsExtra.GetBool($"isUpgrade2Used{index}", false);
        if (isUpgrade2Used) upgrade2Income = baseData.baseUpgrade2Income;
        upgrade2IncomeTMP.text = "+" + baseData.baseUpgrade2Income + "%";
        UpdateUpgrade2();
        
        CheckButtonAccessibility();

        if (level >= 1) StartCoroutine(ProgressBarRoutine());
    }

    public IEnumerator ProgressBarRoutine()
    {
        while (true)
        {
            currentBarValue += Time.deltaTime;
            progressBar.fillAmount = currentBarValue / delayBarValue;
            
            if (currentBarValue >= delayBarValue)
            {
                AudioBase.Instance.CoinCollection();
                currentBarValue = 0;
                BalanceController.Instance.Balance += income;
            }

            yield return null;
        }
    }

    public void CheckButtonAccessibility()
    {
        levelUpButton.interactable = BalanceController.Instance.PurchaseAvailability(levelUpPrice);
        upgrade1Button.interactable = BalanceController.Instance.PurchaseAvailability(baseData.baseUpgrade1Price) &&
                                      !isUpgrade1Used;
        upgrade2Button.interactable = BalanceController.Instance.PurchaseAvailability(baseData.baseUpgrade2Price) &&
                                      !isUpgrade2Used;
    }

    public void LevelUp()
    {
        AudioBase.Instance.ClickButton();
        
        level++;
        
        BalanceController.Instance.Balance -= levelUpPrice;
        levelUpPrice = (level + 1) * baseData.baseLevelUpPrice;
        
        UpdateIncome();
        UpdateLevel();

        if (level == 1) StartCoroutine(ProgressBarRoutine());
    }

    public void OnUpgrade1Button()
    {
        AudioBase.Instance.ClickButton();
        
        isUpgrade1Used = true;
        BalanceController.Instance.Balance -= baseData.baseUpgrade1Price;
        upgrade1Income = baseData.baseUpgrade1Income;
        upgrade1Button.interactable = !isUpgrade1Used;
        
        UpdateIncome();
        UpdateUpgrade1();
    }
    
    public void OnUpgrade2Button()
    {
        AudioBase.Instance.ClickButton();
        
        isUpgrade2Used = true;
        BalanceController.Instance.Balance -= baseData.baseUpgrade2Price;
        upgrade2Income = baseData.baseUpgrade2Income;
        upgrade2Button.interactable = !isUpgrade2Used;
        
        UpdateIncome();
        UpdateUpgrade2();
    }

    #region Updates

    private void UpdateIncome()
    {
        income = level * baseData.baseIncome * (1 + upgrade1Income + upgrade2Income);
        UpdateIncomeTMP();
    }

    private void UpdateIncomeTMP()
    {
        incomeTMP.text = income + "$";
    }
    
    private void UpdateLevel()
    {
        levelTMP.text = level.ToString();
        UpdateLevelUpPriceTMP();
    }
    
    private void UpdateLevelUpPriceTMP()
    {
        levelUpPriceTMP.text = levelUpPrice + "$";
    }
    
    private void UpdateUpgrade1()
    {
        upgrade1PriceTMP.text = isUpgrade1Used ? "Bought" : baseData.baseUpgrade1Price + "$";
    }
    
    private void UpdateUpgrade2()
    {
        upgrade2PriceTMP.text = isUpgrade2Used ? "Bought" : baseData.baseUpgrade2Price + "$";
    }

    #endregion

    #region Save

    private void Save()
    {
        PlayerPrefs.SetInt($"level{index}", level);
        PlayerPrefs.SetInt($"income{index}", income);
        PlayerPrefs.SetInt($"levelUpPrice{index}", levelUpPrice);
        PlayerPrefs.SetFloat($"currentBarValue{index}", currentBarValue);
        PlayerPrefsExtra.SetBool($"isUpgrade1Used{index}", isUpgrade1Used);
        PlayerPrefsExtra.SetBool($"isUpgrade2Used{index}", isUpgrade2Used);
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
}
