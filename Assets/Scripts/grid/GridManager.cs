using System.Collections.Generic;
using System.Text;
using AStar;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] grid;
    List<Vector3Int> roadPath = new();
    new Camera camera;
    BoundsInt bounds;
    
    public LayerMask raycastLayerMask;

    public Vector2Int start;

    void Start()
    {
        bounds = tilemap.cellBounds;
        
        CreateGrid();
    }

    private void CreateGrid()
    {
        var width = bounds.size.x;
        var height = bounds.size.y;
        grid = new Vector3Int[height, width];
        for (int x = bounds.xMin, i = 0; i < (width); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (height); y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    grid[j, i] = new Vector3Int(x, y, 0);
                }
                else
                {
                    grid[j, i] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    private void printArray<T>(T[,] arr)
    {
        var s = new StringBuilder();
        var row = new string[arr.GetLength(0)];
        for (int y = 0; y < arr.GetLength(1); y++)
        {
            for (int x = 0; x < arr.GetLength(0); x++)
            {
                if (typeof(T) == typeof(Vector3Int))
                    row[x] = (arr[x,y] as Vector3Int?)?.z.ToString() ?? ".";
                else if (typeof(T) == typeof(bool))
                    row[x] = (arr[x,y] as bool?)?.ToString() ?? ".";
                else
                    row[x] = ".";
            }

            s.AppendJoin(" ", row);
            s.AppendLine();
        }
        Debug.Log(s.ToString());
    }

    private void DrawRoad()
    {
        roadMap.ClearAllTiles();
        foreach (var t in roadPath)
        {
            roadMap.SetTile(t, roadTile);
        }
    }

    void Update()
    {
        if (camera == null)
            return;
            
        if (Input.GetMouseButtonDown(1))
        {
            var world = cameraToWorld(camera, Input.mousePosition, raycastLayerMask);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }
        if (Input.GetMouseButtonDown(2))
        {
            var world = cameraToWorld(camera, Input.mousePosition, raycastLayerMask);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = cameraToWorld(camera, Input.mousePosition, raycastLayerMask);
            Vector3Int end = tilemap.WorldToCell(mousePosition);
            
            if (roadPath != null && roadPath.Count > 0)
                roadPath.Clear();

            printArray(grid);
            bool[,] boolMap = gridToBoolMap(grid);
            printArray(boolMap);
            
            Debug.Log($"{start.x},{start.y} ===> {end.x},{end.y}");
            var boolMapStart = new Vector2Int(start.x - bounds.xMin, start.y - bounds.yMin);
            var boolMapEnd = new Vector2Int(end.x - bounds.xMin, end.y - bounds.yMin);
            Debug.Log($"{boolMapStart.x},{boolMapStart.y} ===> {boolMapEnd.x},{boolMapEnd.y}");
            (int, int)[] path = AStarPathfinding.GeneratePathSync(boolMapStart.x, boolMapStart.y, boolMapEnd.x, boolMapEnd.y, boolMap, false, true);
            var s = "";
            foreach (var tuple in path)
            {
                s += $"{tuple.Item1}, {tuple.Item2} , ";
            }
            Debug.Log(s);
            var boolMapPath = intTupleToVector3Ints(path);
            
            var stringBuilder = new StringBuilder();
            foreach (var vector3Int in boolMapPath)
            {
                stringBuilder.Append($" -> {vector3Int.x},{vector3Int.y}");
            }
            Debug.Log(stringBuilder.ToString());

            roadPath = boolMapToTileMap(boolMapPath);
            if (roadPath == null || roadPath.Count == 0)
                return;
            
            s = "";
            foreach (var tuple in roadPath)
            {
                s += $"{tuple.x}, {tuple.y} , ";
            }
            Debug.Log(s);
            
            DrawRoad();
        }
    }

    private List<Vector3Int> boolMapToTileMap(List<Vector3Int> boolMapPath)
    {
        var tileMapPath = new List<Vector3Int>();

        foreach (var b in boolMapPath)
        {
            tileMapPath.Add(new Vector3Int(b.x + bounds.xMin, b.y + bounds.yMin, b.z));
        }
        
        return tileMapPath;
    }

    private bool[,] gridToBoolMap(Vector3Int[,] grid)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        bool[,] boolMap = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                boolMap[x, y] = grid[x, y].z == 0;
            }
        }
        
        return boolMap;
    }

    private List<Vector3Int> intTupleToVector3Ints((int, int)[] xy)
    {
        var result = new List<Vector3Int>();
        foreach ((int, int) tuple in xy)
        {
            result.Add(new Vector3Int(tuple.Item1, tuple.Item2, 0));
        }
        return result;
    }

    private Vector3 cameraToWorld(Camera camera, Vector3 screenPoint, LayerMask raycastLayerMask, float raycastMaxDistance = 500f)
    {
        var ray = camera.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        bool raycast = Physics.Raycast(ray, out hit, raycastMaxDistance, raycastLayerMask);
        Vector3 hitPoint = raycast ? hit.point : ray.GetPoint(raycastMaxDistance);
        //Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), hitPoint, raycast ? Color.green : Color.yellow, 3f);
        return hitPoint;
    }

    void OnGUI()
    {
        if (camera == null)
            return;
        
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = camera.pixelHeight - currentEvent.mousePosition.y;

        point = cameraToWorld(camera, new Vector3(mousePos.x, mousePos.y, camera.nearClipPlane), raycastLayerMask);
        
        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + camera.pixelWidth + ":" + camera.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + point.ToString("F3"));
        GUILayout.Label("Tilemap grid position: " + tilemap.WorldToCell(point));
        GUILayout.Label("Roadmap grid position: " + roadMap.WorldToCell(point));
        GUILayout.EndArea();
    }
}
