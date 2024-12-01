using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

public enum NPCMode
{
    Idle,
    Loop,
    Patrol,
    PlayOnce,
    PatrolThenStop
}

[RequireComponent(typeof(CharacterController))]
public class NPCController : MonoBehaviour
{
    /// <summary>
    /// Character controller for the NPC
    /// </summary>
    private CharacterController CharacterController;

    /// <summary>
    /// Animator for the NPC
    /// </summary>
    private Animator NPCAnimator;

    /// <summary>
    /// NPC movement speed in m/s
    /// </summary>
    [Tooltip("1.5 = matching animation")]
    public float MoveSpeed;
    
    /// <summary>
    /// How much this NPC will move when shoved.
    /// </summary>
    [SerializeField] private float _shoveForce = 2.0f;

    /// <summary>
    /// Behaviour of the NPC on start
    /// </summary>
    [Tooltip("NPC will automatically play on start in this mode")]
    [SerializeField] private NPCMode _behaviourOnStart;

    /// <summary>
    /// Should the NPC be in dance mode on start
    /// </summary>
    [Tooltip("Should the NPC be in dance mode on start")]
    [SerializeField] private bool _danceOnStart;

    /// <summary>
    /// Should the NPC be visible on start
    /// </summary>
    [Tooltip("Should the NPC be visible on start. Only works if starting Idle")]
    [SerializeField] private bool _hideOnStart;

    /// <summary>
    /// List of waypoint transforms
    /// </summary>
    [Tooltip("Waypoints can be any transform")]
    public List<Transform> Waypoints;

    /// <summary>
    /// The Transform that the NPC will move to in MoveRoutine
    /// </summary>
    private Transform _destination;

    /// <summary>
    /// Keep track of where this NPC was oiginally placed
    /// </summary>
    private Vector3 _startPosition;

    /// <summary>
    /// Manage the movement coroutine
    /// </summary>
    Coroutine _moveRoutine;

    /// <summary>
    /// Manage the partol coroutine
    /// </summary>
    Coroutine _autoMoveRoutine;

    /// <summary>
    /// Cache components and set start behaviour
    /// </summary>
    private void Start()
    {
        CharacterController = GetComponent<CharacterController>();
        NPCAnimator = GetComponentInChildren<Animator>();
        _startPosition = transform.position;

        //Set starting behaviour
        SetDance(_danceOnStart);
        EnableModel(!_hideOnStart);
        switch (_behaviourOnStart)
        {
            case NPCMode.Loop:
                StartLoop();
                break;
            case NPCMode.Patrol:
                StartPatrol();
                break;
            case NPCMode.PlayOnce:
                PlayOnce();
                break;
            case NPCMode.PatrolThenStop:
                PatrolThenStop();
                break;
            default:
                //Idle
                break;
        }
    }

    /// <summary>
    /// Sets the NPC move speed
    /// </summary>
    /// <param name="speed"></param>
    public void SetMoveSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    /// <summary>
    /// Sets the animator's "Boogie" boolean
    /// </summary>
    /// <param name="state"></param>
    public void SetDance(bool state)
    {
        NPCAnimator?.SetBool("Boogie", state);
    }


    /// <summary>
    /// Get shoved by the player.
    /// </summary>
    public void Shove()
    {
        var pc = PlayerManager.Instance.PlayerController;
        if (!pc) return;
        
        Vector3 dir = transform.position - pc.transform.position;
        dir.y = 0.0f;
        dir.Normalize();
        CharacterController.Move(dir * _shoveForce);
    }

    /// <summary>
    /// Enable/Disable the NPC model and colliders
    /// </summary>
    /// <param name="state"></param>
    private void EnableModel(bool state)
    {
        NPCAnimator?.gameObject.SetActive(state);
        CharacterController.enabled = state;
    }

    /// <summary>
    /// Moves the NPC back to start position and stops movement
    /// </summary>
    public void ResetPosition()
    {
        StopMovement();

        //Move to the starting position
        CharacterController.enabled = false;
        transform.position = _startPosition;
        CharacterController.enabled = true;
    }

    /// <summary>
    /// Stops all movement related routines
    /// </summary>
    public void StopMovement()
    {
        if(_autoMoveRoutine != null)
        {
            StopCoroutine(_autoMoveRoutine);
            _autoMoveRoutine = null;
        }

        if(_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }

        //Stop walking animation
        NPCAnimator?.SetFloat("Blend_Move", 0.0f);
    }

    /// <summary>
    /// Start moving to any transform position
    /// </summary>
    /// <param name="position">Transform to move to</param>
    public void MoveToDestination(Transform position)
    {
        if (position == null) return;

        //Overwrite any previous movement commands
        StopMovement();

        _destination = position;
        _moveRoutine = StartCoroutine(MoveRoutine());
    }

    /// <summary>
    /// Start the PlayOnce routine
    /// Makes the NPC teleport back to Waypoint[0] when path is finished
    /// </summary>
    public void PlayOnce()
    {
        if (Waypoints == null || Waypoints.Count <= 0) return;

        //Overwrite any previous movement commands
        ResetPosition();

        //Make sure the model is enabled
        EnableModel(true);

        _autoMoveRoutine = StartCoroutine(PlayRoutine());
    }

    /// <summary>
    /// Start the looping routine
    /// Makes the NPC teleport back to Waypoint[0] when path is finished
    /// </summary>
    public void StartLoop()
    {
        if (Waypoints == null || Waypoints.Count <= 0) return;

        //Overwrite any previous movement commands
        ResetPosition();

        //Make sure the model is enabled
        EnableModel(true);

        _autoMoveRoutine = StartCoroutine(LoopRoutine());
    }

    public void PatrolThenStop()
    {
        if (Waypoints == null || Waypoints.Count <= 0) return;
        
        ResetPosition();
        
        EnableModel(true);

        _autoMoveRoutine = StartCoroutine(PatrolThenStopRoutine());
    }

    /// <summary>
    /// Start the partol coroutine
    /// Makes NPC move move through the waypoints and then return in reverse order
    /// </summary>
    public void StartPatrol()
    {
        if (Waypoints == null || Waypoints.Count <= 1)
        {
            Debug.LogError("At least 2 waypoints are required to patrol");
            return;
        }

        //Overwrite any previous movement commands
        ResetPosition();

        //Make sure the model is enabled
        EnableModel(true);

        _autoMoveRoutine = StartCoroutine(PatrolRoutine());
    }


    IEnumerator PatrolThenStopRoutine()
    {
        int _currentWayPoint = 0; // Current waypoint index

        while (_currentWayPoint < Waypoints.Count)
        {
            _destination = Waypoints[_currentWayPoint];

            yield return _moveRoutine = StartCoroutine(MoveRoutine());

            _currentWayPoint++;
        }

        if (_currentWayPoint >= Waypoints.Count)
        {
            NPCAnimator?.SetFloat("Blend_Move", 0.0f);
            _autoMoveRoutine = null;
        }
    }

    /// <summary>
    /// Cycle through waypoints one time and then stop
    /// </summary>
    /// <remarks>
    /// Teleports the NPC back to start but does not play again
    /// </remarks>
    IEnumerator PlayRoutine()
    {
        //Always start the patrol on Waypoints[0]
        int _currentWaypoint = 0; // The index of the current waypoint

        while (_currentWaypoint < Waypoints.Count)
        {
            //Set destination
            _destination = Waypoints[_currentWaypoint];

            //Wait for move routine to finish
            yield return _moveRoutine = StartCoroutine(MoveRoutine());

            //next waypoint
            _currentWaypoint++;

        }

        //Teleport to the starting position when there are no more waypoints
        CharacterController.enabled = false;
        transform.position = _startPosition;
        CharacterController.enabled = true;

        //Disable the model and collider
        EnableModel(false);

        _autoMoveRoutine = null;
    }

    /// <summary>
    /// Cycle through waypoint and move to them. Relies on MoveRoutine()
    /// </summary>
    /// <remarks>
    /// Teleports the NPC back to start position when final waypoint is reached
    /// </remarks>
    IEnumerator LoopRoutine()
    {
        //Always start the patrol on Waypoints[0]
        int _currentWaypoint = 0; // The index of the current waypoint

        //Set first destination
        _destination = Waypoints[0];

        while (true)
        {
            //Wait for move routine to finish
            yield return _moveRoutine = StartCoroutine(MoveRoutine());

            //next waypoint
            _currentWaypoint++;

            //Last waypoint reached
            if (_currentWaypoint >= Waypoints.Count)
            {
                //Teleport to the starting position
                CharacterController.enabled = false;
                transform.position = _startPosition;
                CharacterController.enabled = true;

                //Reset current waypoint
                _currentWaypoint = 0;
            }

            //Set next destination
            _destination = Waypoints[_currentWaypoint];
        }
    }

    /// <summary>
    /// Cycle through waypoints and move to them. Relies on MoveRoutine()
    /// </summary>
    /// <remarks>
    /// Moves through waypoints sequentially. When the end is reached Moves through waypoints in reverse order.
    /// </remarks>
    IEnumerator PatrolRoutine()
    {
        //Always start the patrol on Waypoints[0]
        int _currentWaypoint = 0; // The index of the current waypoint
        int _nextWaypointDirection = 1; // Direction to move the index. This should only be 1 or -1

        _destination = Waypoints[0];

        while (true)
        {
            //Wait for move routine to finish
            yield return _moveRoutine = StartCoroutine(MoveRoutine());

            //Get the next waypoint
            //Switch direction (+/-) if we reach max or min in the list
            if(_currentWaypoint + _nextWaypointDirection >= Waypoints.Count ||
               _currentWaypoint + _nextWaypointDirection < 0)
            {
                _nextWaypointDirection *= -1;
            }

            //Move the index
            _currentWaypoint += _nextWaypointDirection;

            //Set destination
            _destination = Waypoints[_currentWaypoint];
        }
    }

    /// <summary>
    /// Start moving towards _destination
    /// </summary>
    IEnumerator MoveRoutine()
    {
        //Find direction, ignoring y component
        Vector3 dir = _destination.position - transform.position;
        dir.y = 0.0f;

        //Orient the NPC with moving direction
        if(dir != Vector3.zero) // Quaternion.LookRotation(dir) complains
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }

        //Find distance
        float distance = dir.magnitude;
        //Track distance travelled
        float travelled = 0.0f;

        //Start walking animation
        NPCAnimator?.SetFloat("Blend_Move", 1.0f);

        //Move until total distance is travelled
        while (travelled < distance)
        {
            //Move in direction plus gravity
            CharacterController?.Move(MoveSpeed * Time.deltaTime * dir.normalized);
            CharacterController?.Move(Physics.gravity * Time.deltaTime);

            travelled += MoveSpeed * Time.deltaTime;

            yield return null;
        }

        //Stop walking animation
        NPCAnimator?.SetFloat("Blend_Move", 0.0f);

        _moveRoutine = null;
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        foreach(Transform waypoint in Waypoints)
        {
            if (!waypoint) continue;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(waypoint.position, 0.2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach(Transform waypoint in Waypoints)
        {
            if (!waypoint) continue;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(waypoint.position, 0.25f);
        }
    }


#endif
}
