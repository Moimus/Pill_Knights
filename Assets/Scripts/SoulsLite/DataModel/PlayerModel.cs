using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerModel : Model
{
    public static string playerSaveFile = "playerSave";
    public int hpMax;
    public int hpCurrent;
    public int currencyCount;
    public int specialCurrencyCount;
    public int fuelCount;
    public ProcedualSwordModel swordModel = null;
    public List<ProcedualSwordModel> weaponInventory = new List<ProcedualSwordModel>();

    public PlayerModel(int hpMax, int hpCurrent, int currencyCount, int specialCurrencyCount, int fuelCount, ProcedualSwordModel swordModel, List<ProcedualSwordModel> weaponInventory)
    {
        this.hpMax = hpMax;
        this.hpCurrent = hpCurrent;
        this.currencyCount = currencyCount;
        this.specialCurrencyCount = specialCurrencyCount;
        this.fuelCount = fuelCount;
        this.swordModel = swordModel;
        this.weaponInventory = weaponInventory;
    }

    public static PlayerModel import(string fileName)
    {
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + Model.DataFolder + "/" + fileName + Model.FileExtension);
        Debug.Log("imported -> " + json);
        PlayerModel model = JsonUtility.FromJson<PlayerModel>(json);
        return model;
    }
}   
