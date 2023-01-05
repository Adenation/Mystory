using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Energy : MonoBehaviour, ISelectable
{
    public const int ENERGY_SWORD = 0;
    public const int ENERGY_SHIELD = 1;
    public const int ENERGY_SPIRIT = 2;
    public const int ENERGY_EARTH = 3;
    public const int ENERGY_AIR = 4;
    public const int ENERGY_FIRE = 5;
    public const int ENERGY_WATER = 6;
    public const int ENERGY_LIGHT = 7;
    public const int ENERGY_SHADOW = 8;

    private readonly float DEFAULT_PATROL_SPEED = 3f;
    private const float SHRINK_SIZE = 0.01f;
    private const float EXPAND_SIZE = 0.5f;
    private int element;
    private bool isMoving;
    private bool isLooping;
    private bool isOrbiting;
    private bool isScaling;
    private bool arrivedAtOrbit;
    private List<Vector3> patrolPoints;
    private Vector3 nextPatrolPoint;
    private Vector3 nextSize;

    public static readonly string HALO_NAME = "Selected Spawn Rate";

    [SerializeField] VisualEffect halo; // Shows selected.
    private const float HALO_SELECTED = 500f;
    private const float HALO_HOVER = 250f;
    private const float HALO_OFF = 0f;

    [SerializeField] bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        patrolPoints = new List<Vector3>() { Vector3.zero };
        nextPatrolPoint = Vector3.zero;
        arrivedAtOrbit = false;
        // Default to avoid nulls
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) { Move(); }
        if (isScaling) { Scale(); }
    }

    public bool SetElement(int element)
    {
        if (element < ENERGY_SWORD || element > ENERGY_SHADOW)
            return false;
        return true;
    }

    public void IsOrbiting() { isOrbiting = !isOrbiting; }
    private void IsScaling() { isScaling = !isScaling; }
    public void SetPatrolPoints(List<Vector3> patrolPoints, bool isLooping,
        bool isOrbiting)
    {
        arrivedAtOrbit = false;
        // If isOrbiting is true, isLooping will not work and it will
        // Always orbit the second patrol point in the list
        this.patrolPoints = patrolPoints;
        nextPatrolPoint = patrolPoints[0];
        // When Patrolpoints are set, it auto triggers movement
        isMoving = true; this.isLooping = isLooping;
        this.isOrbiting = isOrbiting;
        if (isOrbiting) {
            Debug.Log("Orbit Point: " + patrolPoints[1] +
            " Distance to Orbit Point: " + Vector3.Distance(patrolPoints[0],
            patrolPoints[1]));
        }
    }

    private void Move()
    {
        if (isOrbiting)
        {
            if (!arrivedAtOrbit)
            {
                MoveToNext();
                if (transform.position.Equals(nextPatrolPoint))
                {
                    arrivedAtOrbit = true;
                }

            }
            else
            {
                transform.RotateAround(patrolPoints[1], transform.forward, 180 * Time.deltaTime);
            }
               
        }
        else
        {
            MoveToNext();
            CheckPatrol();
        }
    }

    private void MoveToNext()
    {
        float step = DEFAULT_PATROL_SPEED * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nextPatrolPoint, step);
        
    }

    private void CheckPatrol()
    {
        if (transform.position.Equals(nextPatrolPoint))
        {
            int index = patrolPoints.IndexOf(nextPatrolPoint);
            //Debug.Log("Index" + index);
            if (index + 1 < patrolPoints.Count)
            {
                nextPatrolPoint = patrolPoints[index + 1];
            }
            else if (isLooping) // energy only loops in hand
            {
                nextPatrolPoint = patrolPoints[0];
            }
            else
            {
                isMoving = false;// destination reached
            }
        }
    }

    public void Shrink()
    {
        isScaling = true;
        nextSize = new Vector3(SHRINK_SIZE, SHRINK_SIZE, SHRINK_SIZE);
    }

    public void Expand()
    {
        isScaling = true;
        nextSize = new Vector3(EXPAND_SIZE, EXPAND_SIZE, EXPAND_SIZE);
    }

    private void Scale()
    {
        float step = DEFAULT_PATROL_SPEED * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, nextSize, step);
        if(transform.localScale == nextSize)
        {
            isScaling = false;
        }
    }

    private void OnMouseDown()
    {
        GetComponentInParent<EnergyManager>().OnEnergySelected(this);
        Debug.Log("Energy has been clicked ");
    }
    public void Selected()
    {
        // Turns halos/particle effects etc. on and off
        isSelected = true;
        halo.SetFloat(HALO_NAME, HALO_SELECTED);
    }
    public void Deselected()
    {
        isSelected = false;
        halo.SetFloat(HALO_NAME, HALO_OFF);
    }
    private void OnMouseEnter()
    {
        if (!isSelected)
        {
            halo.SetFloat(HALO_NAME, HALO_HOVER);
        }
    }
    private void OnMouseExit()
    {
        if (!isSelected)
        {
            halo.SetFloat(HALO_NAME, HALO_OFF);
        }
    }
}
