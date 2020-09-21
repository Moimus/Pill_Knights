using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public int faction = 0;
    public float speedMax = 3f;
    public float acceleration = 1f;
    public float deceleration = 1f;
    public float speedCurrent = 0f;
    int deadZoneXPositive; //TODO move this to KeyboardController
    int deadZoneXNegative; //TODO move this to KeyboardController
    int deadZoneYPositive; //TODO move this to KeyboardController
    int deadZoneYNegative; //TODO move this to KeyboardController
    int displayAspectRatio; //TODO move this to KeyboardController
    public float rollSpeedMax = 100; //Maximum roll speed
    public float rollSpeedCurrent = 0f; //current roll speed
    public float rollAcceleration = 200f; //rate of accelleration
    public float rollDeceleration = 400f; //rate of deceleration
    public float yawSensitivity = 0.5f;
    public float yawSpeed = 200; //Controller only
    public Camera mainCamera;
    public Animator animator;
    public GameObject meleeAttackPrefab;
    public GameObject meleeAttackMarker;
    public float aggroRange = 10f;
    public float attackRange = 0.3f;
    public float attackDuration = 1f;
    public float attackDelay = 0.3f;
    public bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        calcAspectRatio();
        calcDeadZones();
    }

    // Update is called once per frame
    void Update()
    {
        move();
        //checkInput();
        //roll();
       // autoAnimSpeed();
    }

    void checkInput()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speedCurrent);
        if (Input.GetMouseButton(1))
        {
            followMouse();
        }

        if (Input.GetKey(KeyCode.W))
        {
            speedUp();
        }
        else
        {
            slowDown();
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rollLeft();
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rollRight();
        }
        else
        {
            rollIdle();
        }
    }

    public void playAnimation(string name)
    {
        if(animator != null)
        {
            animator.Play(name, 0);
        }
    }

    void move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speedCurrent);
    }

    void autoAnimSpeed()
    {
        if (speedCurrent > 1 && animator.speed < 3)
        {
            animator.speed = 3;
        }
        else if (speedCurrent < 1)
        {
            animator.speed = 2;
        }
    }

    void calcAspectRatio()
    {
        if (mainCamera != null)
        {
            displayAspectRatio = (int)mainCamera.aspect * Screen.width / Screen.height;
        }
    }

    void calcDeadZones()
    {
        deadZoneXPositive = Screen.width / 2 + (Screen.width / 2) / 10;
        deadZoneXNegative = Screen.width / 2 - (Screen.width / 2) / 10;
        deadZoneYPositive = Screen.height / 2 + (Screen.width / 2) / 10;
        deadZoneYNegative = Screen.height / 2 - (Screen.width / 2) / 10;
    }

    public void followMouse()
    {
        if (Input.mousePosition.x < deadZoneXNegative)
        {
            float mouseMidDistance = Screen.width / 2 - Input.mousePosition.x;
            float smoothFactor = mouseMidDistance / Screen.width;
            //Debug.Log(smoothFactor + "= " + mouseMidDistance + "/" + screenWidthHalf);
            yawLeft(smoothFactor);
        }
        else if (Input.mousePosition.x > deadZoneXPositive)
        {
            float mouseMidDistance = Screen.width / 2 - Input.mousePosition.x;
            float smoothFactor = mouseMidDistance / Screen.width;
            //Debug.Log(-smoothFactor + "= " + mouseMidDistance + "/" + screenWidthHalf);
            yawRight(-smoothFactor);
        }

        if (Input.mousePosition.y < deadZoneYNegative)
        {
            float mouseMidDistance = Screen.height / 2 - Input.mousePosition.y;
            float smoothFactor = mouseMidDistance / Screen.height;
            //Debug.Log(smoothFactor + "= " + mouseMidDistance + "/" + screenWidthHalf);
            yawDown(smoothFactor);
        }
        else if (Input.mousePosition.y > deadZoneYPositive)
        {
            float mouseMidDistance = Screen.height / 2 - Input.mousePosition.y;
            float smoothFactor = mouseMidDistance / Screen.height;
            //Debug.Log(-smoothFactor + "= " + mouseMidDistance + "/" + screenWidthHalf);
            yawUp(-smoothFactor);
        }
    }

    public void yawDown(float factor)
    {
        transform.Rotate(Vector3.right, yawSpeed * factor * yawSensitivity * Time.deltaTime);
    }

    public void yawUp(float factor)
    {
        transform.Rotate(-Vector3.right, yawSpeed * factor * yawSensitivity * Time.deltaTime);
    }

    public void yawLeft(float factor)
    {
        transform.Rotate(-Vector3.up, yawSpeed * factor * yawSensitivity * Time.deltaTime);
    }

    public void yawRight(float factor)
    {
        transform.Rotate(Vector3.up, yawSpeed * factor * yawSensitivity * Time.deltaTime);
    }

    public void roll()
    {
        transform.Rotate(Vector3.forward, rollSpeedCurrent * Time.deltaTime);
    }

    public void rollRight()
    {
        if (rollSpeedCurrent > -rollSpeedMax)
        {
            rollSpeedCurrent -= Time.deltaTime * rollAcceleration;
        }
    }

    public void rollLeft()
    {
        if (rollSpeedCurrent < rollSpeedMax)
        {
            rollSpeedCurrent += Time.deltaTime * rollAcceleration;
        }
    }

    public void rollIdle()
    {
        if (rollSpeedCurrent > 0 && rollSpeedCurrent > 0.01f)
        {
            rollSpeedCurrent -= Time.deltaTime * rollDeceleration;
        }
        else if (rollSpeedCurrent < 0 && rollSpeedCurrent < -0.01f)
        {
            rollSpeedCurrent += Time.deltaTime * rollDeceleration;
        }
        else if (Math.valueIsBetween(rollSpeedCurrent, -0.01f, 0.01f))
        {

        }
        else
        {

        }
    }

    public void speedUp()
    {
        if (speedCurrent < speedMax)
        {
            speedCurrent += 1 * Time.deltaTime * acceleration;
        }
    }

    public void slowDown()
    {
        if (speedCurrent > 0)
        {
            speedCurrent -= 1 * Time.deltaTime * deceleration;

        }
    }

    public void lookAt(Vector3 point)
    {
        float step = yawSpeed * 0.01f * Time.deltaTime;
        Quaternion rotQuat = Quaternion.LookRotation(point - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotQuat, step);
    }

    public IEnumerator attackMelee()
    {
        attacking = true;
        yield return new WaitForSeconds(attackDelay);
        GameObject atk = Instantiate(meleeAttackPrefab, meleeAttackMarker.transform.position, transform.rotation);
        playAnimation("attack");
        yield return new WaitForSeconds(attackDuration);
        playAnimation("idle");
        attacking = false;
        yield return null;
    }
}
