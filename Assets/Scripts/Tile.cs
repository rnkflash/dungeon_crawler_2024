using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile north;
    public Tile south;
    public Tile east;
    public Tile west;
    public Transform posichon;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (posichon != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(posichon.position, 0.5f);    
        }

        if (north != null)
            DrawLineToTile(north, Color.red);
        
        if (south != null)
            DrawLineToTile(south, Color.yellow);
        
        if (east != null)
            DrawLineToTile(east, Color.cyan);
        
        if (west != null)
            DrawLineToTile(west, Color.blue);
    }

    private void DrawLineToTile(Tile tile, Color color)
    {
        Gizmos.color = color;
        var direction = (tile.posichon.position - posichon.position).normalized; 
        Gizmos.DrawLine(posichon.position + Vector3.up * 0.1f, (posichon.position  + Vector3.up * 0.1f) + direction * 2f);
    }

#endif
    
}
