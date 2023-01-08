using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    private bool isExplored = false;
    private Waypoint exploredFrom;
    private Tile tile;

    public Waypoint(Tile tile)
    {
        this.tile = tile;
    }
    public Tile GetTile() { return tile; }

    public bool GetIsExplored() { return isExplored; }

    public void SetIsExplored(bool b) { isExplored = b; }

    public Waypoint GetExploredFrom() { return exploredFrom; }

    public void SetExploredFrom(Waypoint waypoint) { exploredFrom = waypoint; }
}
