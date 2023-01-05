using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PatrolGenerator
{
    public const int PATROL_RANDOM = 0;
    public const int PATROL_SPHERE = 1;
    public const int PATROL_LINE = 2;
    public const int PATROL_RECT = 3;
    public const int PATROL_SEMI_RANDOM = 4;

    public static List<Vector3> GenerateScreenPatrolPoints(
        Camera cam, Vector3 startPoint, float left, float right,
        float down, float up, int option)
    {
        // Have more options later beyond pure random based on energy type
        // And affiliation
        if (cam == null) { cam = Camera.main; }
        List<Vector3> patrolPoints = new List<Vector3>()
        {
            startPoint
        };

        Debug.Log("Start: " + startPoint);

        switch (option)
        {
            case PATROL_RANDOM:
                for (int i = 0; i < Random.Range(4, 8); i++)
                {
                    float x = Random.Range(left, right);
                    float y = Random.Range(down, up);
                    Vector3 v = cam.ViewportToWorldPoint(new Vector3(x, y, cam.nearClipPlane));
                    Debug.Log("x: " + x + " y: " + " v: " + v);
                    v.z = startPoint.z;
                    patrolPoints.Add(v);
                    Debug.Log("Patrol Point(" + i + "): " + patrolPoints[i + 1]);
                }
                break;
            default: //PATROL_SPHERE
                //add point of orbit
                patrolPoints.Add(startPoint +
                    new Vector3(Random.Range(0.15f, 0.25f),
                    Random.Range(0f, 0.1f), Random.Range(0f, 0.01f)));
                break;
        }
        return patrolPoints;
    }
}
