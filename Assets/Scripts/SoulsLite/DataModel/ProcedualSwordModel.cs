using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProcedualSwordModel : ItemModel
{
    public string bladePrefabName = null;
    public string barPrefabName = null;
    public string handlePrefabName = null;
    public int damage = 0;
    public float range = 0;

    public ProcedualSwordModel(string bladePrefabName, string barPrefabName, string handlePrefabName, int damage, float range)
    {
        this.bladePrefabName = bladePrefabName;
        this.barPrefabName = barPrefabName;
        this.handlePrefabName = handlePrefabName;
        this.damage = damage;
        this.range = range;
    }

    public static ProcedualSwordModel import(string fileName)
    {
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + Model.DataFolder + "/" + fileName + Model.FileExtension);
        Debug.Log("imported -> " + json);
        ProcedualSwordModel model = JsonUtility.FromJson<ProcedualSwordModel>(json);
        return model;
    }
}
