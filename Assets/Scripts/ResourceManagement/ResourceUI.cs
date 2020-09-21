using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ResourceUI : MonoBehaviour
{
    public static ResourceUI instance = null;
    public Entity owner = null;
    public Inventory inventory = null;
    [System.Obsolete]
    public GameObject resourceButtonPrefab;
    public GameObject itemIconPrefab = null;
    public GameObject itemDescriptionPrefab = null;
    public GameObject canvas;
    public List<ResourceButton> resourceButtonInstances = null;
    public KeyCode hotkey = KeyCode.I;
    bool open = false;
    public Text title;
    public Text currencyCounter = null;
    public Text specialCurrencyCounter = null;
    public Text fuelCounter = null;
    public Text hpCounter = null;
    public Transform inventoryContent = null;
    public Vector2 padding = new Vector2(0, 32);
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        lifeCycle();
    }

    void lifeCycle()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("inventory");
            toggleInventory();  
        }
        updateFuelCounter();
        updateSpecialCurrencyCounter();
        updateHPCounter();
    }

    public void updateUI()
    {
        if(open)
        {
            destroyInventoryIcons();
            createInventoryIcons();
            currencyCounter.text = "x" + inventory.currencyCount.ToString() + " " + ResourceManager.instance.currencyName;
        }
    }

    public void updateFuelCounter()
    {
        if (fuelCounter != null)
        {
            fuelCounter.text = ((int)inventory.fuelCount).ToString() + " " + ResourceManager.instance.fuelName;
        }
    }

    public void updateHPCounter()
    {
        if (hpCounter != null)
        {
            hpCounter.text = ((int)owner.hpCurrent).ToString() + " HP";
        }
    }

    public void updateSpecialCurrencyCounter()
    {
        if (specialCurrencyCounter != null)
        {
            specialCurrencyCounter.text = ((int)inventory.specialCurrencyCount).ToString() + " " + ResourceManager.instance.specialCurrencyName;
        }
    }

    public void toggleInventory()
    {
        if(open)
        {
            closeInventory();
        }
        else
        {
            openInventory();
        }
    }

    public void openInventory()
    {
        title.gameObject.SetActive(true);
        //createResourceButtons();
        createInventoryIcons();
        open = true;
        updateUI();
    }

    public void closeInventory()
    {
        title.gameObject.SetActive(false);
        //destroyResourceButtons();
        destroyInventoryIcons();
        open = false;
    }

    [System.Obsolete]
    void createResourceButtons()
    {
        resourceButtonInstances = new List<ResourceButton>();
        int xOff = -Screen.width/2 + 64;
        int yOff = Screen.height / 2 - 128;
        for(int n = 0; n < inventory.resourceCount.Length; n++)
        {
            if(inventory.resourceCount[n] > 0)
            {
                GameObject instance = Instantiate(resourceButtonPrefab, canvas.transform);
                ResourceButton btn = instance.GetComponent<ResourceButton>();
                btn.resourceID = ResourceManager.instance.resourcePrefabs[n].GetComponent<Resource>().resourceID;
                btn.inventory = inventory;
                btn.GetComponent<RectTransform>().localPosition = new Vector2(xOff, yOff);
                resourceButtonInstances.Add(btn);
                btn.init(inventory.resourceCount[n]);

                if(xOff < 640)
                {
                    xOff += 64;
                }
                else
                {
                    yOff -= 64;
                    xOff = 0;
                }
            }
        }
    }

    [System.Obsolete]
    void destroyResourceButtons()
    {
        if(resourceButtonInstances != null)
        {
            foreach (ResourceButton btn in resourceButtonInstances)
            {
                Destroy(btn.gameObject);
            }
            resourceButtonInstances = null;
        }

    }

    [System.Obsolete]
    public void destroyResourceButton(ResourceButton btn)
    {
        resourceButtonInstances.Remove(btn);
        Destroy(btn.gameObject);
    }

    public void createDebugIcons(int n)
    {
        float paddingVal = padding.y - 64;
        RectTransform contentPaneRect = inventoryContent.GetComponent<RectTransform>();
        contentPaneRect.sizeDelta = new Vector2(contentPaneRect.sizeDelta.x, (64 + (n * 64)));
        for (int i = 0; i < n; i++)
        {
            GameObject instance = Instantiate(resourceButtonPrefab, inventoryContent);
            instance.GetComponent<RectTransform>().localPosition = new Vector3(transform.position.x + 64, transform.position.y + paddingVal);
            paddingVal -= padding.y;
        }
        
    }

    public void createInventoryIcons()
    {
        float paddingVal = padding.y - 64;
        RectTransform contentPaneRect = inventoryContent.GetComponent<RectTransform>();
        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject instance = Instantiate(itemIconPrefab, inventoryContent);
            instance.GetComponent<ItemIcon>().init(inventory.items[i]);
            instance.GetComponent<RectTransform>().localPosition = new Vector3(transform.localPosition.x + 100, transform.localPosition.y + paddingVal);
            paddingVal -= padding.y;
        }
        contentPaneRect.sizeDelta = new Vector2(contentPaneRect.sizeDelta.x, (64 + (inventory.items.Count * 32)));
        //Debug.Log("sizeDelta = " + contentPaneRect.sizeDelta.ToString());
    }


    public void destroyInventoryIcons()
    {
        foreach(Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }
    }

}
