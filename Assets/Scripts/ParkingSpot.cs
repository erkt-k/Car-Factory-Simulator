using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ParkingSpot : MonoBehaviour
{

    [SerializeField] NavMeshAgent carOnTop; 

    private void Awake()
    {
        carOnTop = null;
    }

    public bool IsAvailable() { return carOnTop == null; }

    public NavMeshAgent CarOnTop { get { return carOnTop; } }

    public void SetCarOnTop(NavMeshAgent newCar)
    {
        carOnTop = newCar;
    }
}


