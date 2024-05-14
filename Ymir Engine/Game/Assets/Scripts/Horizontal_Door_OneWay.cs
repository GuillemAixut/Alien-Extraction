using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Horizontal_Door_OneWay : YmirComponent
{
    public enum DoorState
    {
        OPENING,
        WAITING,
        CLOSING,
        CLOSED
    }

    /// <summary>
    /// Esta variable permite que, una vez la puerta ha sido abierta desde el unico lado por el que se puede abrir,
    /// se desbloquee el poder abrirla por el otro lado
    /// </summary>
    public bool unlockAfterOpen = true;

    private GameObject doorCollider;
    private GameObject doorSensor1;
    private GameObject doorSensor2;

    public float fraction = 0f;

    private DoorState currentState;
    float timer = 0;
    float animDuration = 1f;

    private GameObject lDoor;
    private Vector3 initialPos_lDoor;
    private GameObject end_lDoor;
    private GameObject rDoor;
    private Vector3 initialPos_rDoor;
    private GameObject end_rDoor;

    public void Start()
    {
        lDoor = InternalCalls.CS_GetChild(gameObject, 0);
        end_lDoor = InternalCalls.CS_GetChild(gameObject, 3);
        rDoor = InternalCalls.CS_GetChild(gameObject, 1);
        end_rDoor = InternalCalls.CS_GetChild(gameObject, 4);
        doorCollider = InternalCalls.CS_GetChild(gameObject, 5);
        doorSensor1 = InternalCalls.CS_GetChild(gameObject, 6);
        doorSensor2 = InternalCalls.CS_GetChild(gameObject, 7);
        doorSensor2.GetComponent<BackZoneDoor_Sensor>().active = false;

        initialPos_lDoor = lDoor.transform.localPosition;
        initialPos_rDoor = rDoor.transform.localPosition;

        currentState = DoorState.CLOSED;
    }

    public void Update()
    {
        switch (currentState)
        {
            case DoorState.OPENING:
                timer += Time.deltaTime;
                float fraction = Mathf.Clamp01(timer / animDuration);
                lDoor.transform.localPosition = Vector3.Lerp(initialPos_lDoor, end_lDoor.transform.localPosition, fraction);
                rDoor.transform.localPosition = Vector3.Lerp(initialPos_rDoor, end_rDoor.transform.localPosition, fraction);
                if (timer >= animDuration)
                {
                    currentState = DoorState.WAITING;
                    timer = 0;
                }
                break;
            case DoorState.WAITING:
                timer += Time.deltaTime;
                lDoor.transform.localPosition = end_lDoor.transform.localPosition;
                rDoor.transform.localPosition = end_rDoor.transform.localPosition;
                if (timer >= animDuration)
                {
                    currentState = DoorState.CLOSING;
                    timer = 0;
                }
                break;
            case DoorState.CLOSING:
                timer += Time.deltaTime;
                fraction = Mathf.Clamp01(timer / animDuration);
                lDoor.transform.localPosition = Vector3.Lerp(end_lDoor.transform.localPosition, initialPos_lDoor, fraction);
                rDoor.transform.localPosition = Vector3.Lerp(end_rDoor.transform.localPosition, initialPos_rDoor, fraction);
                if (timer >= animDuration)
                {
                    currentState = DoorState.CLOSED;
                }
                break;
            case DoorState.CLOSED:
                // Puerta cerrada, no hay acción necesaria
                break;

        }
    }

    public void ChangeDoorState()
    {
        if (!doorSensor2.GetComponent<BackZoneDoor_Sensor>().active && unlockAfterOpen) doorSensor2.GetComponent<BackZoneDoor_Sensor>().active = true;

        if (currentState == DoorState.CLOSED)
        {
            currentState = DoorState.OPENING;
            timer = 0;

            if (doorCollider != null) { InternalCalls.Destroy(doorCollider); }
        }
        else if (currentState == DoorState.CLOSING)
        {
            currentState = DoorState.OPENING;
            timer = animDuration - timer;
        }
    }
}