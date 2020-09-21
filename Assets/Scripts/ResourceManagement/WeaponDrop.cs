using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public ProcedualSwordModel model = null;
    public Transform weaponHolder = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        weaponHolder.Rotate(transform.up * Time.deltaTime * 10);
    }

    public void init(ProcedualSwordModel weaponModel)
    {
        model = weaponModel;
        GameObject sword = Instantiate(ResourceManager.instance.procedualWeaponPrefab, weaponHolder);
        ProcedualSword proSword = sword.GetComponent<ProcedualSword>();
        proSword.initFromModel(null ,model);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Inventory>() != null)
        {
            Inventory inv = other.GetComponent<Inventory>();
            inv.addItem(model);
            Destroy(gameObject);
        }
    }
}
