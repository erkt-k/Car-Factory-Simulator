using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CarManager : MonoBehaviour
{
    [SerializeField] List<GameObject> cars = new List<GameObject>();
    [SerializeField] GameObject carsParent;

    public GameObject CreateCar(Vector3 destination)
    {
        Debug.Log(cars.Count);
        int randomIndex = Random.Range(1, cars.Count);
        GameObject newCar = Instantiate(cars[randomIndex], this.transform.position, Quaternion.identity, carsParent.transform);
        newCar.GetComponent<NavMeshAgent>().SetDestination(destination);
        return newCar;
    }

    public void SendCarToPark(NavMeshAgent car, ParkingSpot parkingSpace)
    {
        parkingSpace.SetCarOnTop(car);
        car.SetDestination(parkingSpace.transform.position);
    }

    

}
