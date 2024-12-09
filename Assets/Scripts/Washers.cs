
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Washers : MonoBehaviour
{
    public int washerID;
    private bool isOpen = false;
    
    public float timer = 40f;
    public int gainedMoney = 1000;

    public int timerLevel = 1;
    public int moneyLevel = 1;

    public bool isAvailable;
    public GameObject currentVehicle = null;

    public int getWasherID() { return washerID; }

    public bool IsAvaliable() { return isAvailable; }

    public GameObject getCurrentVehicle() { return currentVehicle; }

    public void setNextVehicle(GameObject newVehicle) 
    { 
        currentVehicle = newVehicle;
        newVehicle.gameObject.transform.position = this.transform.position + new Vector3(0, 1, 0);
        isAvailable = false;
        StartCoroutine(ProcessCar(timer));
    }

    IEnumerator ProcessCar(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isAvailable = true;
        Destroy(currentVehicle);
        currentVehicle = null;
    }

    public void UpgradeTimer()
    {
        if(timerLevel < 500)
        {
            timerLevel++;
            timer -= 0.03f;
        }
    }

    public void UpgradeMoney()
    {
        if (moneyLevel < 500) 
        { 
            moneyLevel++;
            gainedMoney += 430;
        }
    }

    public bool IsOpen() { return isOpen; }

    public void OpenWasher()
    {
        isOpen = true;
        isAvailable = true;
    }

}
