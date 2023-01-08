using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    protected GameObject occupant;
    protected Vector2Int gridPos;


    protected void SetGridPos(Vector2Int pos) { gridPos = pos; }
    public Vector2Int GetGridPos() { return gridPos; }
    public GameObject GetOccupant() { return occupant; }
    public virtual void SetUpTile(Vector2Int pos, string team_color)
    {
        SetGridPos(pos);
    }
    public void SetOccupant(GameObject occ) { occupant = occ; }
}

