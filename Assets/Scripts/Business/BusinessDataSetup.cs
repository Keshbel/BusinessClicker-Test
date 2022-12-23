using System.Collections.Generic;
using UnityEngine;

public class BusinessDataSetup : MonoBehaviour
{
    public List<BusinessController> business;
    public BusinessConfig dataConfig;
    
    private void Awake()
    {
        //PlayerPrefs.DeleteAll(); //удаление данных

        if (business.Count != dataConfig.datas.Count) return;
        
        for (var index = 0; index < business.Count; index++)
        {
            business[index].index = index;
            business[index].baseData = dataConfig.datas[index];
        }
    }
}
