using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    /// <summary>
    /// Animator component of the vehicle
    /// </summary>
    public Animator VehicleAnimator;

    /// <summary>
    /// On destination reached event
    /// </summary>
    public delegate void OnDesitnationReached(VehicleController controller);
    public OnDesitnationReached onDestinationReached;

    /// <summary>
    /// Target destination on launch
    /// </summary>
    Transform _destination;

    /// <summary>
    /// Constant velocity when the vehicle moves
    /// </summary>
    float _velocity;

    /// <summary>
    /// Manage the movement coroutine
    /// </summary>
    Coroutine _moveRoutine;

    /// <summary>
    /// Set the destination of this vehicle
    /// </summary>
    /// <param name="target">Destination</param>
    public void SetDestination(Transform target)
    {
        _destination = target;
    }

    /// <summary>
    /// Set the vehicle's velocity
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocity(float velocity)
    {
        _velocity = velocity;
    }

    /// <summary>
    /// Starts the process of moving the vehicle.
    /// Manages the movement coroutine.
    /// </summary>
    public void Launch()
    {
        //Don't launch if there is no destination set
        if(_destination == null) return;

        //Stop any previous movement routine
        if(_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
        }

        //Start movement routine
        _moveRoutine = StartCoroutine(MoveToDestination());
    }

    /// <summary>
    /// Start moving towards _destination
    /// </summary>
    IEnumerator MoveToDestination()
    {
        //Find direction
        Vector3 dir = (_destination.position - transform.position).normalized;
        //Orient the vehicle
        transform.rotation = Quaternion.LookRotation(-dir) * Quaternion.Euler(new Vector3(0, 90, 0));

        //Find distance
        float distance = (_destination.position - transform.position).magnitude;
        //Track distance travelled
        float travelled = 0.0f;

        //Make vehicle animations start
        SetAnimator_Engine(true);
        SetAnimator_Speed(3.0f);

        //Move until total distance is travelled
        while (travelled < distance)
        {
            transform.position += _velocity * Time.deltaTime * dir;
            travelled += _velocity * Time.deltaTime;

            yield return null;
        }

        //Make vehicle animations stop/idle
        SetAnimator_Engine(false);
        SetAnimator_Speed(0.0f);

        //Destination reached
        onDestinationReached?.Invoke(this);
        onDestinationReached = null;

        _moveRoutine = null;
    }

    /// <summary>
    /// Stop movement coroutine if it is running
    /// </summary>
    public void StopMovement()
    {
        if (_moveRoutine == null) return;

        //Stop movement coroutine
        StopCoroutine(_moveRoutine);
        _moveRoutine = null;

        //Make vehicle animations stop/idle
        SetAnimator_Engine(false);
        SetAnimator_Speed(0.0f);
    }

    /// <summary>
    /// Set the transform of the vehicle.
    /// Moves it instantly.
    /// </summary>
    /// <param name="position">Transform position</param>
    public void TranslateTo(Transform position)
    {
        if (position == null) return;

        transform.position = position.position;
        transform.rotation = position.rotation;
    }

    /// <summary>
    /// Set moving animation speed
    /// </summary>
    /// <param name="val">Animator speed value</param>
    public void SetAnimator_Speed(float val)
    {
        if (VehicleAnimator == null) return;

        VehicleAnimator.SetFloat("Blend_Speed", val);
    }

    /// <summary>
    /// Set engine animations on/off
    /// </summary>
    /// <param name="state">Engine on/off state</param>
    public void SetAnimator_Engine(bool state)
    {
        if(VehicleAnimator == null) return;

        VehicleAnimator.SetFloat("Engine", state ? 1.0f : 0.0f);
    }
}