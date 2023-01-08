using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Pathfinder
{
    private static Dictionary<Vector2Int, Waypoint> field;
    private static Queue<Waypoint> queue;

    // For making ARC easier to code, these had to be rearranged,
    // It shouldn't break anything but it will change the
    // Priority of movement from Right, Left, Up, Down, to the order shown below
    private static Vector2Int[] adjacentOddTiles = new Vector2Int[]
    {
        new Vector2Int(-1, 0), // Up & Right
        new Vector2Int(0, 1), // Right
        new Vector2Int(1, 0), // Down & Right
        new Vector2Int(1, -1), // Down & Left
        new Vector2Int(0, -1), //Left
        new Vector2Int(-1, -1) // Up & Left
    };
    private static Vector2Int[] adjacentEvenTiles = new Vector2Int[]
    {
        new Vector2Int(-1, 1), // Up & Right
        new Vector2Int(0, 1), // Right
        new Vector2Int(1, 1), // Down & Right
        new Vector2Int(1, 0), // Down & Left
        new Vector2Int(0, -1), //Left
        new Vector2Int(-1, 0) // Up & Left
    };

    // Called by BattleController after grid has been created
    public static void Initialise(IList<IList<Tile>> battleGrid)
    {
        field = new Dictionary<Vector2Int, Waypoint>();
        foreach (IList<Tile> list in battleGrid)
        {
            foreach (Tile tile in list)
            {
                field.Add(tile.GetGridPos(), new Waypoint(tile));
                //Debug.Log("Grid Pos: " + tile.GetGridPos() + "WayPoint : " + field[tile.GetGridPos()]);
            }
        }
        queue = new Queue<Waypoint>();
    }

    public static void ResetWaypoints()
    {
        foreach (Waypoint waypoint in field.Values)
        {
            waypoint.SetIsExplored(false);
            waypoint.SetExploredFrom(null);
        }
    }

    // Methods For Exploring Neighbours

    private static void ExploreNeighbours(Tile searchCenter, bool occupantsMatter)
    {
        Vector2Int[] directions = GetDirections(searchCenter);
        foreach (Vector2Int direction in directions)
        {
            //Debug.Log(direction);
            Vector2Int neighbourCoords = searchCenter.GetGridPos() + direction;
            //Debug.Log("Neighour Coords: " + neighbourCoords);
            try
            {
                QueueNewNeighbours(searchCenter, neighbourCoords, occupantsMatter);
            }
            catch (Exception e)
            {

            }
        }
    }

    private static void QueueNewNeighbours(Tile searchCenter, Vector2Int neighbourCoords, bool occupantsMatter)
    {
        Waypoint neighbour = field[neighbourCoords];
        if (occupantsMatter)
        {
            if (!neighbour.GetIsExplored() && !queue.Contains(neighbour) && neighbour.GetTile().GetOccupant() == null)
            {
                queue.Enqueue(neighbour);
                neighbour.SetExploredFrom(field[searchCenter.GetGridPos()]);
            }
        }
        else
        {
            if (!neighbour.GetIsExplored() && !queue.Contains(neighbour))
            {
                queue.Enqueue(neighbour);
                neighbour.SetExploredFrom(field[searchCenter.GetGridPos()]);
            }
        }
    }

    public static List<Tile> CreatePath(Tile startPoint, Tile endPoint, List<Tile> path)
    {
        // Create The Path
        path.Add(endPoint);
        try
        {
            Waypoint previous = field[endPoint.GetGridPos()].GetExploredFrom();
            while (previous.GetTile() != startPoint)
            {
                path.Add(previous.GetTile());
                previous = previous.GetExploredFrom();
            }

            path.Add(startPoint);
            path.Reverse();
        }
        catch (NullReferenceException)
        {
            Debug.LogWarning("--------------------NULL ALERT---------------");
            Debug.Log(field[endPoint.GetGridPos()]);
            Debug.Log(endPoint.transform.position);
            Debug.Log(field[endPoint.GetGridPos()].GetExploredFrom());
            Debug.Log("End point: " + endPoint.GetGridPos());
        }
        PrintPath(path);
        queue.Clear(); // Reset queue for next caller
        return path;
    }


    // Method for finding a path

    public static List<Tile> CalcPath(Tile origin, Tile destination, bool occupantsMatter)
    {
        // If the destination is the same, just return the list with the destination tile
        List<Tile> path = new List<Tile>();
        if (origin == destination) { path.Add(destination); Debug.Log("Same spot"); return path; }
        Tile startPoint = origin;
        Tile endPoint = destination;

        //Debug.Log("Start: " + startPoint.GetGridPos());
        //Debug.Log("End: " + endPoint.GetGridPos());
        ResetWaypoints();

        // Breadth First Search
        bool endPointFound = false;
        Tile searchCenter;
        queue.Enqueue(field[startPoint.GetGridPos()]);
        //Debug.Log("Queue Count: " + queue.Count);
        //Debug.Log("End Point Found: " + endPointFound);
        while (queue.Count > 0 && !endPointFound)
        {
            searchCenter = queue.Dequeue().GetTile();
            //Debug.Log("Exploring tiles around: " + searchCenter.GetGridPos() + " " + searchCenter.transform.position);
            if (searchCenter == endPoint) { endPointFound = true; }
            else
            {
                ExploreNeighbours(searchCenter, occupantsMatter);
                field[searchCenter.GetGridPos()].SetIsExplored(true);
            }

            //Debug.Log("Queue Count: " + queue.Count);
            //Debug.Log("End Point Found: " + endPointFound);
        }


        //Debug.Log("attempting to create path");
        return CreatePath(startPoint, endPoint, path);
    }

    public static List<Tile> GetAllTilesinRadius(Tile epicentre, int radius)
    {
        ResetWaypoints();
        List<Tile> tiles = new List<Tile>();
        if (radius == 0) { tiles.Add(epicentre); return tiles; }
        // Get all the neighbours of the epicentre, no need to distance check epicentre.
        Tile searchCenter;
        queue.Enqueue(field[epicentre.GetGridPos()]);
        searchCenter = queue.Dequeue().GetTile();
        ExploreNeighbours(searchCenter, false);
        field[searchCenter.GetGridPos()].SetIsExplored(true);
        // Then the while loop is for all other tiles
        int currentRadius = 0;
        while (queue.Count > 0)
        {
            searchCenter = queue.Dequeue().GetTile();
            currentRadius = DistanceCheck(searchCenter, epicentre, 1);
            if (currentRadius == radius) { tiles.Add(searchCenter); }
            if (currentRadius > radius) { break; }
            ExploreNeighbours(searchCenter, false);
            field[searchCenter.GetGridPos()].SetIsExplored(true);
        }
        queue.Clear(); // Reset queue for next caller
        PrintPath(tiles);
        return tiles;

    }

    public static List<Tile> GetTilesinArc3(Tile epicentre, Tile target, int range)
    //throws NullPointException
    {
        List<Tile> targets = new List<Tile>();
        if (range > 0)
        {
            Vector2Int[] directions = GetDirections(epicentre);
            Debug.Log(directions);
            Vector2Int[] opposites = GetOppositeDirections(epicentre);
            Debug.Log(opposites);
            Vector2Int direction = target.GetGridPos() - epicentre.GetGridPos();
            Debug.Log("Target: " + target.GetGridPos());
            Debug.Log("Epicentre: " + epicentre.GetGridPos());
            Debug.Log("i " + direction);
            Vector2Int adj1 = new Vector2Int(); Vector2Int adj2 = new Vector2Int();
            int i = 0; bool found = false;
            while (i < directions.Length && !found)
            {
                if (direction.Equals(directions[i]))
                {
                    found = true; int j = i; int k = i;
                    j = i - 1;
                    if (j < 0) { j += 6; }
                    adj1 = directions[j];
                    k = i + 1;
                    if (k > 5) { k -= 6; }
                    adj2 = directions[k];
                    break;
                }
                i++;
            }
            Debug.Log("j " + adj1);
            Debug.Log("k " + adj2);
            int focus = range - 1; // The tile to calculate directions, is one before the affected
            Vector2Int pos = epicentre.GetGridPos() +
                (direction * (focus - (focus / 2))) +
                (opposites[i] * (focus / 2));
            Debug.Log("Focus: " + focus + " pos: " + pos);
            Tile target1; Tile target2; Tile target3;
            Waypoint w1 = field[pos + direction]; if (w1 == null) { target1 = null; } else { target1 = w1.GetTile(); }
            Waypoint w2 = field[pos + adj1]; if (w2 == null) { target2 = null; } else { target2 = w2.GetTile(); }
            Waypoint w3 = field[pos + adj2]; if (w3 == null) { target3 = null; } else { target3 = w3.GetTile(); }
            Debug.Log(pos + direction);
            Debug.Log(pos + adj1);
            Debug.Log(pos + adj2);
            targets.Add(target1); targets.Add(target2); targets.Add(target3);
        }
        return targets;
    }

    public static List<Tile> GetTilesinArc5(Tile epicentre, Tile target, int range)
    //throws NullPointException
    {
        List<Tile> targets = new List<Tile>();
        if (range > 0)
        {
            Vector2Int[] directions = GetDirections(epicentre);
            Vector2Int[] opposites = GetOppositeDirections(epicentre);
            Vector2Int direction = target.GetGridPos() - epicentre.GetGridPos();

            int i = 0; bool found = false; int j = 0;
            while (i < directions.Length && !found)
            {
                if (direction.Equals(directions[i]))
                {
                    found = true;
                    j = i - 3;
                    if (j < 0) { j += 6; }
                    break;
                }
                i++;
            }
            int focus = range - 1; // The tile to calculate directions, is one before the affected
            Vector2Int pos = epicentre.GetGridPos() +
                (direction * (focus - (focus / 2))) +
                (opposites[i] * (focus / 2));
            Waypoint w0 = field[pos];
            if (w0 == null) { return targets; }
            List<Tile> options = GetAllTilesinRadius(w0.GetTile(), 1);
            foreach (Tile tile in options)
            {
                if (tile.GetGridPos().Equals(pos + GetDirections(field[pos].GetTile())[j]))
                {// Skip Tile
                }
                else
                {
                    Debug.Log("Arc-5 Tile Added Vector: " + tile.GetGridPos());
                    targets.Add(tile);
                }
            }
        }
        return targets;
    }

    public static Tile GetTileInLine(Tile epicentre, Tile target, int range)
    {
        Tile tile = null;
        Vector2Int[] directions = GetDirections(epicentre);
        Vector2Int[] opposites = GetOppositeDirections(epicentre);
        Vector2Int direction = target.GetGridPos() - epicentre.GetGridPos();
        int i = 0; bool found = false;
        while (i < directions.Length && !found)
        {
            if (direction.Equals(directions[i]))
            {
                found = true;
                break;
            }
            i++;
        }
        Vector2Int pos = epicentre.GetGridPos() +
                (directions[i] * (range - (range / 2))) +
                (opposites[i] * (range / 2));
        Waypoint w = field[pos];
        if (w != null)
        {
            tile = w.GetTile();
        }
        return tile;
    }

    public static List<Tile> GetTilesInCone(Tile epicentre, Tile target, int range)
    {
        List<Tile> targets = new List<Tile>();
        if (range > 0)
        {
            Vector2Int[] directions = GetDirections(target);
            Vector2Int[] opposites = GetOppositeDirections(target);
            Vector2Int direction = target.GetGridPos() - epicentre.GetGridPos();
            int adj1 = 0; int adj2 = 0;
            int i = 0; bool found = false;
            while (i < directions.Length && !found)
            {
                if (direction.Equals(directions[i]))
                {
                    found = true; int j = i; int k = i;
                    j = i - 2;
                    if (j < 0) { j += 6; }
                    adj1 = j;
                    k = i + 2;
                    if (k > 5) { k -= 6; }
                    adj2 = k;
                    break;
                }
                i++;
            }
            Waypoint w1 = field[target.GetGridPos()]; if (w1 == null) { targets.Add(null); } else { targets.Add(w1.GetTile()); }
            for (int x = range - 1; x > 0; x--)
            {
                Waypoint w2 = field[target.GetGridPos() + (directions[adj1] * (x - (x / 2))) +
                (opposites[adj1] * (x / 2))]; if (w2 == null) { targets.Add(null); } else { targets.Add(w2.GetTile()); }
                Waypoint w3 = field[target.GetGridPos() + (directions[adj2] * (x - (x / 2))) +
                (opposites[adj2] * (x / 2))]; if (w2 == null) { targets.Add(null); } else { targets.Add(w2.GetTile()); }
            }
        }
        return targets;
    }

    // Starts at 1 as if they previous tile is the epicentre, it's 1 away
    public static int DistanceCheck(Tile search, Tile epicentre, int counter)
    {
        Waypoint way = field[search.GetGridPos()].GetExploredFrom();
        while (way != null && way.GetTile() != epicentre)
        {
            way = way.GetExploredFrom();
            counter++;
        }
        return counter;
    }

    public static void PrintPath(List<Tile> path)
    {
        Debug.Log("Print Path");
        foreach (Tile t in path)
        {
            Debug.Log(t.GetGridPos());
        }
    }
    /*
    public static Tile GetTileInDirection(Tile origin, Tile target, int radius)
    {
        Tile tile = null;
        if(radius == 0) { tile = origin; return tile; }
        Vector2Int[] directions = GetDirections(origin);
        Vector2Int direction = target.GetGridPos() - origin.GetGridPos();
        
    }
    */
    public static Vector2Int[] GetAdjacentOddTiles() { return adjacentOddTiles; }
    public static Vector2Int[] GetAdjacentEvenTiles() { return adjacentEvenTiles; }
    public static Vector2Int[] GetDirections(Tile tile)
    {
        Vector2Int[] directions = GetAdjacentOddTiles();
        if (tile.GetGridPos().x % 2 == 0) { directions = GetAdjacentEvenTiles(); }
        return directions;
    }
    public static Vector2Int[] GetOppositeDirections(Tile tile)
    {
        Vector2Int[] directions = GetAdjacentEvenTiles();
        if (tile.GetGridPos().x % 2 == 0) { directions = GetAdjacentOddTiles(); }
        return directions;
    }
}
