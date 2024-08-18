using UnityEngine;
using UnityEngine.Tilemaps;

public class GetAllTiles : MonoBehaviour
{
    public Grid m_Grid;
    public Tilemap map;
    public TileBase tile;
    
    public LayerMask raycastLayerMask;
    
    void Update()
    {
        if (m_Grid && Input.GetMouseButton(0))
        {
            var hitPoint = cameraToWorld(Camera.main, Input.mousePosition, raycastLayerMask);
            Vector3Int gridPos = m_Grid.WorldToCell(hitPoint);
            map.SetTile(gridPos, tile);
        }
    }

    private Vector3 cameraToWorld(Camera camera, Vector3 screenPoint, LayerMask raycastLayerMask, float raycastMaxDistance = 500f)
    {
        var ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        bool raycast = Physics.Raycast(ray, out hit, raycastMaxDistance, raycastLayerMask);
        Vector3 hitPoint = raycast ? hit.point : ray.GetPoint(raycastMaxDistance);
        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), hitPoint, raycast ? Color.green : Color.yellow, 3f);
        return hitPoint;
    }
}