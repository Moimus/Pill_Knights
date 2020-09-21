using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public string spellPrefabFolder = "Prefabs/Spells/Player";
    public KeyCode hotkeySpell = KeyCode.R;
    public GameObject[] availableSpells;
    public GameObject selectedSpell;
    public Transform spellMarker;
    int selectedSpellIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
    }

    void init()
    {
        loadSpells();
    }

    void loadSpells()
    {
        availableSpells = Resources.LoadAll<GameObject>(spellPrefabFolder);
        selectedSpell = availableSpells[0];
    }

    void checkInput()
    {
        if(Input.GetKeyDown(hotkeySpell))
        {
            castSpell();
        }
    }

    void incrementSelectedSpell()
    {
        if (selectedSpellIndex < availableSpells.Length - 1)
        {
            selectedSpellIndex++;
            selectedSpell = availableSpells[selectedSpellIndex];
        }
        else
        {
            selectedSpellIndex = 0;
            selectedSpell = availableSpells[selectedSpellIndex];
        }

    }

    public void castSpell()
    {
        if(gameObject.tag == "Player")
        {
            if (GetComponent<Inventory>() != null)
            {
                Inventory inv = GetComponent<Inventory>();
                if(inv.specialCurrencyCount >= availableSpells[selectedSpellIndex].GetComponent<Spell>().specialCurrencyCost)
                {
                    GameObject spellInstance = Instantiate(selectedSpell, spellMarker.position, Quaternion.Euler(0, spellMarker.rotation.y, 0));
                    inv.removeSpecialCurrency(availableSpells[selectedSpellIndex].GetComponent<Spell>().specialCurrencyCost);
                }
                else
                {
                    AnnounceUI.instance.spawnMessage("Not enogh " + ResourceManager.instance.specialCurrencyName);
                }
            }
        }
        else
        {
            GameObject spellInstance = Instantiate(selectedSpell, spellMarker.position, Quaternion.Euler(0, spellMarker.rotation.y, 0));
        }
    }
}
