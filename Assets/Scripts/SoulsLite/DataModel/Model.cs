using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class Model
{
    public static string DataFolder = "/Data"; //Folder where models are saved
    public static string FileExtension = ".g8Model";

    public Model()
    {

    }

    public virtual void export(string fileName)
    {
        Debug.Log("Writing to: " + Application.persistentDataPath);
        if (!Directory.Exists(Application.persistentDataPath + Model.DataFolder))
        {
            Directory.CreateDirectory(Application.persistentDataPath + Model.DataFolder);
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + Model.DataFolder + "/" + fileName + Model.FileExtension, toJSON());
    }

    public virtual string toJSON()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log("Model to JSON: -> " + JsonUtility.ToJson(this));
        return json;
    }
}
