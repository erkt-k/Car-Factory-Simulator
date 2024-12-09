using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    /*
    * Create Cars an put them in inspection queue (Done)
    * 
    * inspect them and put them in repairment queue (Done)
    * 
    * repair them and put them in washing queue (Done)
    * 
    * wash them and hold them to sell (Done)
    *  
    */

    [SerializeField] CarManager carManager;
    [SerializeField] List<NavMeshAgent> carsInGame;

    GameObject inspectionParks;
    
    [SerializeField] List<ParkingSpot> inspectionAvailablePark; //Available parking spots
    [SerializeField] List<ParkingSpot> inspectionBusy;  //Unavailable parking spots
    [SerializeField] List<ParkingSpot> inspectionWaiting; //processed but not able to continue spots

    bool isInspecting = false;

    private float repairmentTimer = 20;
    [SerializeField] ParkingSpot repairmentSpot;

    [SerializeField] List<ParkingSpot> repairmentAvailablePark; //Available repairment waiting parks
    [SerializeField] List<ParkingSpot> repairmentQueue; //Unavailable repairment waiting parks
    [SerializeField] List<ParkingSpot> repairmentWaiting; //repairment is done but can't continue park


    private float washingTimer = 25;    
    [SerializeField] ParkingSpot washingSpot;

    [SerializeField] List <ParkingSpot> washingAvailablePark; //available washing parks
    [SerializeField] List<ParkingSpot> washingQueue; //unavailable washing parks

    [SerializeField] List<NavMeshAgent> washingWaiting; //washed but can't continue cars.

    [SerializeField] List<NavMeshAgent> carsToSell; //cars that are ready to sell


    private float sellingTimer;
    [SerializeField] ParkingSpot sellingSpace;
    private bool isSellable = false;
    [SerializeField] private GameObject exclamationMark;


    private void Awake()
    {
        inspectionParks = GameObject.Find("Inspection Parks");
        inspectionAvailablePark[0] = inspectionParks.transform.GetChild(0).GetComponent<ParkingSpot>();
    }

    public void Start()
    {
        StartCoroutine(CreateCars());  
        StartCoroutine(SellingCounter());
    }


    private void Update()
    {
        if (inspectionBusy.Count > 0 && !isInspecting) 
        {
            ParkingSpot currentParkingSpot = inspectionBusy[0];
            
            StartCoroutine(InspectCar(currentParkingSpot.CarOnTop ,currentParkingSpot));
        }
    }

    IEnumerator CreateCars()
    {
        while (true)
        { 
            InstantiateNewCar();
            int seconds = UnityEngine.Random.Range(5, 11);
            yield return new WaitForSeconds(seconds);
        }
    }

    public void InstantiateNewCar()
    {
        if(inspectionAvailablePark.Count > 0)
        {
            ParkingSpot parkingSpot = inspectionAvailablePark[0];
            GameObject newCar = carManager.CreateCar(parkingSpot.transform.position);
            inspectionBusy.Add(parkingSpot);
            inspectionAvailablePark.Remove(parkingSpot);
            carsInGame.Add(newCar.GetComponent<NavMeshAgent>());
            parkingSpot.SetCarOnTop(newCar.GetComponent<NavMeshAgent>());
        }
    }

    IEnumerator InspectCar(NavMeshAgent car, ParkingSpot currentSpot)
    {
        isInspecting = true;
        yield return new WaitForSeconds(10);
        if (repairmentSpot.IsAvailable())
        {
            inspectionAvailablePark.Add(currentSpot);
            inspectionBusy.Remove(currentSpot);
            carManager.SendCarToPark(car, repairmentSpot);
            currentSpot.SetCarOnTop(null);
            StartCoroutine(RepairCar(car));

        } else if (repairmentAvailablePark.Count > 0) 
        {
            ParkingSpot parkSpot = repairmentAvailablePark[0];
            repairmentAvailablePark.Remove(parkSpot);
            repairmentQueue.Add(parkSpot);
            carManager.SendCarToPark(car, parkSpot);
            currentSpot.SetCarOnTop(null);

            inspectionBusy.Remove(currentSpot);
            inspectionAvailablePark.Add(currentSpot);
        } else
        {
            inspectionBusy.Remove(currentSpot);
            inspectionWaiting.Add(currentSpot);
        }
        isInspecting = false;
    }

    
    public void DecreaseRepairmentTimer() {
        repairmentTimer -= 1.5f;
    }

    IEnumerator RepairCar(NavMeshAgent car)
    { 
        yield return new WaitForSeconds(repairmentTimer);
        if (washingSpot.IsAvailable())
        {
            carManager.SendCarToPark(car, washingSpot);
            Debug.Log(washingSpot.CarOnTop);
            StartCoroutine(WashCar(car));

            repairmentSpot.SetCarOnTop(null);

            if (repairmentQueue.Count > 0)
            {
                NavMeshAgent nextCar = repairmentQueue[0].CarOnTop;
                ParkingSpot repairmentPark = repairmentQueue[0];

                repairmentPark.SetCarOnTop(null);

                carManager.SendCarToPark(nextCar, repairmentSpot);

                repairmentAvailablePark.Add(repairmentPark);
                repairmentQueue.Remove(repairmentPark);
                StartCoroutine(RepairCar(nextCar));
                /* e�er bekleyen varsa hemen onu al
                *       repairmentQueue dan ��kar ve repairmentSpot'a yolla
                *       parkingSpot 'u repairment available k�sm�na al
                */
                if (inspectionWaiting.Count > 0)
                {
                    NavMeshAgent inspectCar = inspectionWaiting[0].CarOnTop;
                    ParkingSpot inspectSpot = inspectionWaiting[0];

                    inspectSpot.SetCarOnTop(null);

                    inspectionWaiting.Remove(inspectSpot);
                    inspectionAvailablePark.Add(inspectSpot);

                    repairmentQueue.Add(repairmentPark);
                    repairmentAvailablePark.Remove(repairmentPark);

                    carManager.SendCarToPark(inspectCar, repairmentPark);
                }
                //E�ER INSPECTIONWAITING VARSA ORDAN ALIP BURDAN GE�EN�N YER�NE KOY GEL�NCE (Done)
            } 
        }
        else if (washingAvailablePark.Count > 0)
        {
            ParkingSpot parkSpot = washingAvailablePark[0];
            washingAvailablePark.Remove(parkSpot);
            washingQueue.Add(parkSpot);

            carManager.SendCarToPark(car, parkSpot);

            repairmentSpot.SetCarOnTop(null);
            if (repairmentQueue.Count > 0)
            {
                NavMeshAgent nextCar = repairmentQueue[0].CarOnTop;
                ParkingSpot repairmentPark = repairmentQueue[0];

                repairmentPark.SetCarOnTop(null);

                carManager.SendCarToPark(nextCar, repairmentSpot);

                repairmentAvailablePark.Add(repairmentPark);
                repairmentQueue.Remove(repairmentPark);
                StartCoroutine(RepairCar(nextCar));
                /* e�er bekleyen varsa hemen onu al
                *       repairmentQueue dan ��kar ve repairmentSpot'a yolla
                *       parkingSpot 'u repairment available k�sm�na al
                */
                if (inspectionWaiting.Count > 0)
                {
                    NavMeshAgent inspectCar = inspectionWaiting[0].CarOnTop;
                    ParkingSpot inspectSpot = inspectionWaiting[0];

                    inspectSpot.SetCarOnTop(null);

                    inspectionWaiting.Remove(inspectSpot);
                    inspectionAvailablePark.Add(inspectSpot);

                    repairmentQueue.Add(repairmentPark);
                    repairmentAvailablePark.Remove(repairmentPark);

                    carManager.SendCarToPark(inspectCar, repairmentPark);
                }
                //E�ER INSPECTIONWAITING VARSA ORDAN ALIP BURDAN GE�EN�N YER�NE KOY GEL�NCE (Done)
            }
        }
        else 
        {
            repairmentWaiting.Add(washingSpot);
        }

        /*e�er m�saitse oraya koy
        *     -> repairment Spot'tan al, (Done)
        *     -> repairment Spot carOnTop = null, (Done)
        *     
        *     -> washing spot'a koy, (Done)
        *     -> washing Spot carOnTop = car, (Done)
        *     
        * de�ilse ama park m�saitse park et
        *     ->m�sait park alan�n� al (Done)
        *     ->park alan�n� available queue dan ��kar (Done)
        *     ->park alan�n� washingQueue ya al (Done)
        *     ->repairment Spot'tan oraya ta�� (Done)
        *     
        *     ->repairmentSpot.CarOnTop = null (Done)
        *     
        *     ->parkSpot.CarOnTop = car (Done)
        *     
        * oras� da de�ilse bekle
        *     ->repairmentWaiting'e al (Done)
        */
    }

    public void DecreaseWashingTimer() {
        washingTimer -= 1.37f;
    }

    IEnumerator WashCar(NavMeshAgent car)
    {
        yield return new WaitForSeconds(washingTimer);
        if (carsToSell.Count < 25) 
        {
            carManager.SendCarToPark(car, sellingSpace);
            carsToSell.Add(car);
            washingSpot.SetCarOnTop(null);

            if (washingQueue.Count > 0) 
            {
                NavMeshAgent nextCar = washingQueue[0].CarOnTop;
                ParkingSpot currentSpot = washingQueue[0];

                currentSpot.SetCarOnTop(null);
                carManager.SendCarToPark(nextCar, washingSpot);
                StartCoroutine(WashCar(nextCar));

                washingQueue.Remove(currentSpot);
                washingAvailablePark.Add(currentSpot);

                if (repairmentWaiting.Count > 0) {
                    washingQueue.Add(currentSpot);
                    washingAvailablePark.Remove(currentSpot);

                    NavMeshAgent repairmentCar = repairmentSpot.CarOnTop;

                    carManager.SendCarToPark(repairmentCar, currentSpot);

                    repairmentSpot.SetCarOnTop(null);

                    if (repairmentQueue.Count > 0) 
                    {
                        ParkingSpot repairmentPark = repairmentQueue[0];
                        NavMeshAgent nextRepairmentCar = repairmentPark.CarOnTop;

                        repairmentQueue.Remove(repairmentPark);
                        repairmentAvailablePark.Add(repairmentPark);

                        carManager.SendCarToPark(nextRepairmentCar, repairmentSpot);

                        repairmentPark.SetCarOnTop(null);

                        if (inspectionWaiting.Count > 0) 
                        {
                            ParkingSpot inspectionCurrentSpot = inspectionWaiting[0];
                            NavMeshAgent inspectionCar = inspectionCurrentSpot.CarOnTop;

                            repairmentQueue.Add(repairmentPark);
                            repairmentAvailablePark.Remove(repairmentPark);

                            inspectionWaiting.Remove(inspectionCurrentSpot);
                            inspectionAvailablePark.Add(inspectionCurrentSpot);

                            carManager.SendCarToPark(inspectionCar, repairmentPark);
                        }
                    }
                }
            }
        } else
        {
            washingWaiting.Add(washingSpot.CarOnTop);
        }
        /*
        * E�er yer varsa yolla 
        *      -> arabay� yolla (Done)
        *      -> carsToSell.Add(car) (Done)
        *     
        *      -> washingSpot.CarOnTop = null (Done)
        *
        * Yoksa beklemeye al (Done)
        */
    }
    public bool SellCar()
    {
        if (isSellable && carsToSell.Count > 0) {
            NavMeshAgent car = carsToSell[0];
            isSellable = false;
            carsToSell.Remove(car);
            exclamationMark.SetActive(false);
            //sellingScreen.SetActive(true);
            Destroy(car.gameObject);
            if (car == null) 
            {
                Debug.Log("car is removed");
            } else
            {
                Debug.LogError("Car is NOT removed");
            }
            if (washingWaiting.Count > 0)
            {
                NavMeshAgent newCar = washingWaiting[0];
                carManager.SendCarToPark(newCar, sellingSpace);
                washingWaiting.Remove(newCar);
                carsToSell.Add(newCar);
                BoxCollider boxCollider = newCar.GetComponent<BoxCollider>();
                Destroy(boxCollider);
            }

            return true;
            /*UI Manager -> sat�n alma ekran�n� a�.
            * ekrandaki se�eneklerden birini se�ince ona g�re para ver.
            * sonra E�ER garajda araba varsa onu i�leme al 
            *           garaj i�in s�ra bekleyen varsa onu da garaja al 
            *       YOKSA 
            *           garaj i�in s�ra bekleyen varsa onu da garaja al
            */
        }
        return false;
    }

    IEnumerator SellingCounter() {
        while (true) {
            yield return new WaitForSeconds(sellingTimer);
            if (carsToSell.Count > 0) 
            {
                isSellable = true;
                exclamationMark.SetActive(true);
            } else {
                isSellable = false;
                exclamationMark.SetActive(false);
            }
                }
        }
}

