using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int[] resourceCount;
    public int currencyCount = 0;
    public int specialCurrencyCount = 0;
    public ResourceUI resourceUI = null;
    public Transform dropMarker;
    public bool needsFuel = false;
    public float fuelMax = 300;
    public float fuelCount = 300;
    public List<ProcedualSwordModel> items = new List<ProcedualSwordModel>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lateInit());
    }

    // Update is called once per frame
    void Update()
    {
        depleteFuel();
    }

    IEnumerator lateInit()
    {
        yield return new WaitForSeconds(1.0f);
        resourceCount = new int[ResourceManager.instance.resourcePrefabs.Count];
        yield return null;
    }

    public void add(int id)
    {
        resourceCount[id]++;
        updateUI();
    }

    public void addCurrency(int amount)
    {
        currencyCount += amount;
        updateUI();
    }

    public void addSpecialCurrency(int amount)
    {
        specialCurrencyCount += amount;
        updateUI();
    }

    public void removeSpecialCurrency(int amount)
    {
        specialCurrencyCount -= amount;
        updateUI();
    }

    public void addFuel(float amount)
    {
        if(fuelCount + amount < fuelMax)
        {
            fuelCount += amount;
        }
        else
        {
            fuelCount = fuelMax;
        }
    }

    public void drop(int id)
    {
        if(resourceCount[id] > 0)
        {
            resourceCount[id]--;
        }
        updateUI();
    }

    public void updateUI()
    {
        if(resourceUI != null)
        {
            resourceUI.updateUI();
        }
    }

    void depleteFuel()
    {
        if(needsFuel)
        {
            fuelCount -= Time.deltaTime;
        }
    }

    public void addItem(ProcedualSwordModel model)
    {
        items.Add(model);
        updateUI();
    }

    public void removeItem(ProcedualSwordModel model)
    {
        items.Remove(model);
        updateUI();
    }
}
