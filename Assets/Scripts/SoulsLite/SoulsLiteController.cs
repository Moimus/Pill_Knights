using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SoulsLiteController : MonoBehaviour
{
    public static SoulsLiteController instance = null;
    public SoulsLiteCharacter character;
    bool overrideControls = false;
    public Transform camRotator;
    public Camera mainCam;
    public float controlStickDeadZone = 0.8f;
    public float leftStickInputX = 0f;
    public float leftSickInputY = 0f;
    public float rightSickInputX = 0f;
    public float rightSickInputY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        string[] n = Input.GetJoystickNames();
        foreach (string i in n)
        {
            Debug.Log(i.ToString());
        }
        StartCoroutine(lateInit());
    }

    // Update is called once per frame
    void Update()
    {
        if (!overrideControls)
        {
            StartCoroutine(checkInput());
        }
    }

    public virtual IEnumerator lateInit()
    {
        yield return new WaitForSeconds(0.5f);
        loadPlayer();
        yield return null;
    }

    IEnumerator checkInput()
    {
        //Button controls
        if (Input.GetButton("P1Abutton"))
        {
            Debug.Log("A-Button down");
        }
        else if (Input.GetButtonDown("P1Bbutton"))
        {
            Debug.Log("B-Button down");
            character.setBlocking();
        }

        if (Input.GetButtonDown("P1Xbutton"))
        {
            Debug.Log("X-Button down");
            character.SetAttacking();
        }

        if (Input.GetButtonDown("P1Ybutton"))
        {
            Debug.Log("Y-Button down");
        }

        if (Input.GetButtonDown("P1RBbutton"))
        {
            Debug.Log("RB-Button down");
        }

        //Left stick controls
        leftStickInputX = Input.GetAxis("P1LeftStickHorizontal");
        leftSickInputY = Input.GetAxis("P1LeftStickVertical");
        if (leftStickInputX < -controlStickDeadZone)
        {
            Debug.Log("left stick Horizontal Axis: " + leftStickInputX);
        }
        else if (leftStickInputX > controlStickDeadZone)
        {
            Debug.Log("left stick Horizontal Axis: " + leftStickInputX);
        }

        if (leftSickInputY > controlStickDeadZone)
        {
            Debug.Log("left stick vertical Axis: " + leftSickInputY);
            character.SetWalkBackward();
        }
        else if (leftSickInputY < -controlStickDeadZone)
        {
            Debug.Log("left stick vertical Axis: " + leftSickInputY);
            if(Input.GetAxis("P1LeftTrigger") == 1)
            {
                character.SetRun();
            }
            else
            {
                character.SetWalkForward();
            }
        }
        else if((leftStickInputX < controlStickDeadZone && leftStickInputX > -controlStickDeadZone) && (leftSickInputY < controlStickDeadZone && leftSickInputY > -controlStickDeadZone))
        {
            //character.animationState = (int)SoulsLiteCharacter.animationStates.idle;
            character.SetIdle();
        }

        //Right stick controls
        if (Input.GetAxis("P1RightStickHorizontal") < -controlStickDeadZone && Input.GetButton("P1SelectButton"))
        {
            rotateCamera(false);
        }
        else if(Input.GetAxis("P1RightStickHorizontal") > controlStickDeadZone && Input.GetButton("P1SelectButton"))
        {
            rotateCamera(false);
        }

        else if (Input.GetAxis("P1RightStickHorizontal") < -controlStickDeadZone)
        {
            Debug.Log("right stick Horizontal Axis: " + Input.GetAxis("P1RightStickHorizontal"));
            character.rotate(Input.GetAxis("P1RightStickHorizontal"));
        }
        else if (Input.GetAxis("P1RightStickHorizontal") > controlStickDeadZone)
        {
            Debug.Log("right stick Horizontal Axis: " + Input.GetAxis("P1RightStickHorizontal"));
            character.rotate(Input.GetAxis("P1RightStickHorizontal"));
        }

        if (Input.GetAxis("P1RightStickVertical") > controlStickDeadZone)
        {
            Debug.Log("right stick Vertical Axis: " + Input.GetAxis("P1RightStickVertical"));
        }
        else if (Input.GetAxis("P1RightStickVertical") < -controlStickDeadZone)
        {
            Debug.Log("right stick Vertical Axis: " + Input.GetAxis("P1RightStickVertical"));
        }

        //Trigger controls
        if (Input.GetAxis("P1LeftTrigger") == 1)
        {
            Debug.Log("left trigger Axis: " + Input.GetAxis("P1LeftTrigger"));
            if (leftSickInputY < -controlStickDeadZone)
            {
                character.SetRun();
            }
        }
        else if (Input.GetAxis("P1RightTrigger") == 1)
        {
            Debug.Log("right trigger Axis: " + Input.GetAxis("P1RightTrigger"));
        }

        yield return null;
    }

    void rotateCamera(bool vertical)
    {
        if(!vertical)
        {
            camRotator.transform.Rotate(((Vector3.up * Input.GetAxis("P1RightStickHorizontal")) * Time.deltaTime * 100));
        }
        else
        {
            camRotator.transform.Rotate(((transform.right * Input.GetAxis("P1RightStickVertical")) * Time.deltaTime * 100));
        }

    }

    public void savePlayer()
    {
        Inventory inv = GetComponent<Inventory>();
        PlayerModel model = new PlayerModel(character.hpMax, character.hpCurrent, inv.currencyCount, inv.specialCurrencyCount, (int)inv.fuelCount, character.weapon.transform.GetComponent<ProcedualSword>().swordModel, character.GetComponent<Inventory>().items);
        model.export(PlayerModel.playerSaveFile);
    }

    public void loadPlayer()
    {
        try
        {
            PlayerModel model = PlayerModel.import(PlayerModel.playerSaveFile);
            Inventory inv = character.transform.GetComponent<Inventory>();
            inv.items = model.weaponInventory;
            character.hpCurrent = model.hpCurrent;
            character.hpMax = model.hpMax;
            inv.currencyCount = model.currencyCount;
            inv.specialCurrencyCount = model.specialCurrencyCount;
            inv.fuelCount = model.fuelCount;
            character.loadWeapon(model.swordModel);
        }
        catch(FileNotFoundException fne)
        {
            character.equipRandomWeapon();
        }

    }

}
