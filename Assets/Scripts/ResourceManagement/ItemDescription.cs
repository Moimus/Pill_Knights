using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour
{
    public Text text = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(ProcedualSwordModel model)
    {
        text.text = "Name: " + model.name + "\n"
                    + "Damage: " + model.damage;
    }
}
