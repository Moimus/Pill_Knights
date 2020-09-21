using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance = null;
    public List<GameObject> resourcePrefabs = new List<GameObject>();
    public GameObject procedualWeaponPrefab = null;
    public GameObject weaponDropPrefab = null;
    public GameObject currencyPrefab;
    public string currencyName = "Coins";
    public string fuelName = "fuel";
    public string specialCurrencyName = "special";

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        instance = this;
    }
}
