using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    ProcedualSwordModel model = null;
    public Text text = null;
    public GameObject description = null;

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
        this.model = model;
        text.text = model.name;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(model.bladePrefabName);
        Inventory inv = SoulsLiteController.instance.character.GetComponent<Inventory>();
        inv.addItem(SoulsLiteController.instance.character.weapon.GetComponent<ProcedualSword>().swordModel);
        inv.removeItem(model);
        SoulsLiteController.instance.character.loadWeapon(model);
        Destroy(description);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(description == null)
        {
            description = Instantiate(ResourceUI.instance.itemDescriptionPrefab, ResourceUI.instance.canvas.transform);
            ItemDescription itemDescription = description.GetComponent<ItemDescription>();
            RectTransform rect = description.GetComponent<RectTransform>();
            rect.position = new Vector2(Input.mousePosition.x + 128, Input.mousePosition.y);
            itemDescription.init(model);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(description != null)
        {
            Destroy(description);
        }
        Debug.Log("MouseExit");
    }
}
