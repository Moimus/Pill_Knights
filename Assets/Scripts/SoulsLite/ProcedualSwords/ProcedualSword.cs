using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedualSword : MonoBehaviour
{
    public ProcedualSwordModel swordModel = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(Entity owner)
    {
        generateModel(owner);
    }

    public void initFromModel(Entity owner, ProcedualSwordModel model)
    {
        GameObject prefabBlade = Resources.Load<GameObject>("Prefabs/ProcedualSwords/Blades/" + model.bladePrefabName);
        GameObject prefabBar = Resources.Load<GameObject>("Prefabs/ProcedualSwords/Bars/" + model.barPrefabName);
        GameObject prefabHandle = Resources.Load<GameObject>("Prefabs/ProcedualSwords/Handles/" + model.handlePrefabName);

        GameObject blade = Instantiate(prefabBlade, transform);
        GameObject bar = Instantiate(prefabBar, transform);
        GameObject handle = Instantiate(prefabHandle, transform);

        BoxCollider coll = blade.GetComponent<BoxCollider>();
        BoxCollider collCopy = gameObject.AddComponent<BoxCollider>();
        collCopy.size = coll.size;
        collCopy.center = coll.center;
        collCopy.isTrigger = true;
        Destroy(coll);

        Weapon weapon = gameObject.AddComponent<Weapon>();
        weapon.owner = owner;
        weapon.damage = model.damage;
        weapon.range = model.range;
        swordModel = model;

        addModifiers(blade.GetComponent<Modifier>());
    }

    void generateModel(Entity owner)
    {
        GameObject[] prefabsBlades = Resources.LoadAll<GameObject>("Prefabs/ProcedualSwords/Blades/");
        GameObject[] prefabsBars = Resources.LoadAll<GameObject>("Prefabs/ProcedualSwords/Bars/");
        GameObject[] prefabsHandles = Resources.LoadAll<GameObject>("Prefabs/ProcedualSwords/Handles/");

        int rollBlade = Random.Range(0, prefabsBlades.Length);
        int rollBar = Random.Range(0, prefabsBars.Length);
        int rollHandle = Random.Range(0, prefabsHandles.Length);

        GameObject blade = Instantiate(prefabsBlades[rollBlade], transform);
        GameObject bar = Instantiate(prefabsBars[rollBar], transform);
        GameObject handle = Instantiate(prefabsHandles[rollHandle], transform);

        BoxCollider coll = blade.GetComponent<BoxCollider>();
        BoxCollider collCopy = gameObject.AddComponent<BoxCollider>();
        collCopy.size = coll.size;
        collCopy.center = coll.center;
        collCopy.isTrigger = true;
        Destroy(coll);

        Weapon weapon = gameObject.AddComponent<Weapon>();
        weapon.owner = owner;
        weapon.range = 1.0f;
        swordModel = new ProcedualSwordModel(prefabsBlades[rollBlade].name, prefabsBars[rollBar].name, prefabsHandles[rollHandle].name, weapon.damage, weapon.range);
        addModifiers(blade.GetComponent<Modifier>());
    }

    void addModifiers(Modifier bladeMod)
    {
        string newName = bladeMod.nameMod + " Sword";
        name = newName;
        swordModel.name = newName;

        Destroy(bladeMod);
    }
}
