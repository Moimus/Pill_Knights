using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : Entity
{
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
    //public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        calcAspectRatio();
        calcDeadZones();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();
        roll();
        autoAnimSpeed();
    }

    public override void init()
    {
        base.init();
    }

    void checkInput()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speedCurrent);
        if (Input.GetMouseButton(1))
        {
            followMouse();
        }

        if(Input.GetKey(KeyCode.W))
        {
            speedUp();
        }
        else
        {
            slowDown();
        }

        if(Input.GetKey(KeyCode.Q))
        {
            rollLeft();
        }
        else if(Input.GetKey(KeyCode.E))
        {
            rollRight();
        }
        else
        {
            rollIdle();
        }
    }

    void autoAnimSpeed()
    {
        if(speedCurrent > 1 && animator.speed < 3)
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
        deadZoneXPositive = Screen.width / 2 + (Screen.width /2) / 10;
        deadZoneXNegative = Screen.width / 2 - (Screen.width / 2) / 10;
        deadZoneYPositive = Screen.height / 2 + (Screen.width / 2) / 10;
        deadZoneYNegative = Screen.height / 2 - (Screen.width / 2) / 10;
    }

    public void followMouse()
    {
        if (Input.mousePosition.x < deadZoneXNegative)
        {
            float mouseMidDistance = Screen.width /2  - Input.mousePosition.x;
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
            float mouseMidDistance = Screen.height /2 - Input.mousePosition.y;
            float smoothFactor = mouseMidDistance / Screen.height;
            //Debug.Log(smoothFactor + "= " + mouseMidDistance + "/" + screenWidthHalf);
            yawDown(smoothFactor);
        }
        else if (Input.mousePosition.y > deadZoneYPositive)
        {
            float mouseMidDistance = Screen.height /2  - Input.mousePosition.y;
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
        if(!Math.valueIsBetween(rollSpeedCurrent,-10f,10f))
        {
            transform.Rotate(Vector3.forward, rollSpeedCurrent * Time.deltaTime);
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Debug.Log("zeroing Vel");
        }
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
}
