using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGO : MonoBehaviour
{
    public const float DEFAULT_TRAVEL_TIME = 0.5f;
    private Card card; // The card logic this game object is assigned to
    [SerializeField] bool isMoving;
    [SerializeField] bool isMovingnRotating;
    private Vector3 destination;
    private Quaternion rotation;
    private Vector3 startPoint;
    private Quaternion startRotate;
    private float timeCount;
    // Image
    // Card Text
    // Other visual card features

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving && destination != gameObject.transform.position)
        {
            IncrementPosition();
        }
        if(isMovingnRotating && destination != gameObject.transform.position)
        {
            IncrementRotation();
        }
    }

    public Card GetCard() { return card; }
    public Card SetCard(Card card) { this.card = card; return card; }

    public void Move(Vector3 coords)
    {
        destination = coords;
        isMoving = true;
    }

    public void Move(Vector3 coords, Quaternion rotation)
    {
        destination = coords;
        this.rotation = rotation;
        startPoint = transform.position;
        startRotate = transform.rotation;
        isMovingnRotating = true; // For performance, this should check whether this is needed
        //Debug.Log("Object: " + this + " Destination: " + destination + "Rotation: " + rotation);
        Debug.Log("Object: " + this + "Start: " + startPoint + " Destination: " + destination);
    }

    private void IncrementPosition()
    {
        // Calculate the next position
        float delta = Mathf.Max(DEFAULT_TRAVEL_TIME * Time.deltaTime,
            Vector3.Distance(gameObject.transform.position, destination) /
            DEFAULT_TRAVEL_TIME * Time.deltaTime);
        Vector3 currentPosition = gameObject.transform.position;
        Vector3 nextPosition = Vector3.MoveTowards(currentPosition, destination, delta);

        // Move the object to the next position
        gameObject.transform.position = nextPosition;
        if (isMoving && destination == gameObject.transform.position)
        {
            //SendMessageUpwards("OnMovementFinished", gameObject);
            isMoving = false;
        }
    }

    private void IncrementRotation()
    {
        transform.SetPositionAndRotation(
            Vector3.Lerp(startPoint, destination, timeCount),
            Quaternion.Lerp(startRotate, rotation, timeCount));
        //Debug.Log("Position: " + Vector3.Slerp(startPoint, destination, timeCount) +
         //   " Rotation: " + Quaternion.Slerp(startRotate, rotation, timeCount));
        if (isMovingnRotating && destination == gameObject.transform.position)
        {
            //SendMessageUpwards("OnMovementFinished", gameObject);
            isMovingnRotating = false;
            timeCount = 0;
        }
        timeCount += Time.deltaTime;
    }
}
