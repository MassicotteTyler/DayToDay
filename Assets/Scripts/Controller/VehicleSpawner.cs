using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public Transform Destination;
    public float Vehicle_Velocity;

    public bool AutoLaunchOnStart;
    [Tooltip("Only used when AutoLaunch is true")]
    public float Delay_Max;
    [Tooltip("Only used when AutoLaunch is true")]
    public float Delay_Min;

    [Header("Pooled Vehicles")]
    public GameObject VehiclePool;

    /// <summary>
    /// Queue used for available pooled vehicles
    /// </summary>
    Queue<VehicleController> _availableVehicles;

    /// <summary>
    /// Manage the AutomateLaunchingRoutine coroutine
    /// </summary>
    Coroutine _autoLaunchRoutine;

    /// <summary>
    /// Called by Unity when the GameObject is first instantiated
    /// </summary>
    private void Start()
    {
        _availableVehicles = new Queue<VehicleController>();

        //Enqueue VehicleControllers from VehiclePool list
        foreach(Transform trans in VehiclePool.transform)
        {
            VehicleController controller = trans.GetComponent<VehicleController>();
            if(controller != null)
            {
                // Put a VehicleController into the pool
                ReleaseReusable(controller);
            }
        }

        if (AutoLaunchOnStart)
        {
            AutoLaunch(true);
        }
    }

    /// <summary>
    /// Start/Stop automated launching
    /// </summary>
    /// <param name="state"></param>
    public void AutoLaunch(bool state)
    {
        if (state)
        {
            //Start the coroutine if it is not running
            if(_autoLaunchRoutine == null)
            {
                _autoLaunchRoutine = StartCoroutine(AutomateLaunchingRoutine());
            }
        }
        else
        {
            //Stop coroutine if there is one running
            if(_autoLaunchRoutine != null)
            {
                StopCoroutine(_autoLaunchRoutine);
                _autoLaunchRoutine = null;
            }
        }
    }

    /// <summary>
    /// Automate launching pooled vehicles.
    /// Will only stop when AutoLaunch is set to false
    /// </summary>
    /// <returns></returns>
    IEnumerator AutomateLaunchingRoutine()
    {
        //Delay between launching vehicles.
        float delay = Mathf.Lerp(Delay_Max, Delay_Min, Random.Range(0.0f, 1.0f));

        while (true)
        {
            delay -= Time.deltaTime;

            if(delay <= 0.0f)
            {
                LaunchNextAvailable();

                //Reset delay
                delay = Mathf.Lerp(Delay_Max, Delay_Min, Random.Range(0.0f, 1.0f));
            }

            yield return null;
        }
    }

    /// <summary>
    /// Launch the next available vehicle gameobject
    /// </summary>
    public void LaunchNextAvailable()
    {
        //No destination or vehicles
        if (Destination == null || _availableVehicles.Count == 0) return;

        //Get vehicle controller
        VehicleController controller = _availableVehicles.Dequeue();

        //Enable gameobject
        controller.gameObject.SetActive(true);

        //Set controller variables
        controller.SetDestination(Destination);
        controller.SetVelocity(Vehicle_Velocity);

        //Set action for onDestinationReached event
        controller.onDestinationReached += ReleaseReusable;

        //Launch the vehicle
        controller.Launch();
    }

    /// <summary>
    /// Put a VehicleController back into the queue
    /// </summary>
    /// <param name="controller">Controller to put back into queue</param>
    public void ReleaseReusable(VehicleController controller)
    {
        if (controller == null) return;

        //Move to starting position (this script's transform)
        controller.TranslateTo(transform);
        controller.gameObject.SetActive(false);
        _availableVehicles.Enqueue(controller);
    }
}
