using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Settings")]
    public Transform Destination;
    public float Vehicle_Velocity;

    public bool AutoLaunch;
    [Tooltip("Only used when AutoLaunch is true")]
    public float Delay_Max;
    [Tooltip("Only used when AutoLaunch is true")]
    public float Delay_Min;

    [Header("Pooled Vehicles")]
    public List<GameObject> VehiclePool;

    /// <summary>
    /// Queue used for available pooled vehicles
    /// </summary>
    Queue<VehicleController> _availableVehicles;

    /// <summary>
    /// Manage the AutomateLaunchingRoutine coroutine
    /// </summary>
    Coroutine _autoLaunchRoutine;

    /// Temporary manual launching
    //private void Update()
    //{
    //    if (Input.GetKeyDown("k"))
    //    {
    //        LaunchNextAvailable();
    //    }
    //}

    /// <summary>
    /// Called by Unity when the GameObject is first instantiated
    /// </summary>
    private void Start()
    {
        _availableVehicles = new Queue<VehicleController>();

        //Enqueue VehicleControllers from VehiclePool list
        foreach(GameObject go in VehiclePool)
        {
            VehicleController controller = go.GetComponent<VehicleController>();
            if(controller != null)
            {
                _availableVehicles.Enqueue(controller);
            }
        }

        if (AutoLaunch)
        {
            _autoLaunchRoutine = StartCoroutine(AutomateLaunchingRoutine());
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
        //Random value is obtained from Sin function using time since application start
        float delay = Mathf.Lerp(Delay_Max, Delay_Min, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup)));

        //Random.Range version. UnityEngine.Random is global.
        //float delay = Mathf.Lerp(Delay_Max, Delay_Min, Random.Range(0.0f, 1.0f));

        while (AutoLaunch)
        {
            delay -= Time.deltaTime;

            if(delay <= 0.0f)
            {
                LaunchNextAvailable();

                //Sin version
                delay = Mathf.Lerp(Delay_Max, Delay_Min, Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup)));
                //Random.Range version
                //delay = Mathf.Lerp(Delay_Max, Delay_Min, Random.Range(0.0f, 1.0f));
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
        _availableVehicles.Enqueue(controller);
    }
}
