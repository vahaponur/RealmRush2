using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Vector2Int _gridSize;
    
    [Tooltip("Should match snap settings")]
    [SerializeField] private int _unityGridSize = 10;
    #endregion

    #region Private Fields

    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();

    #endregion

    #region Public Properties

    public Dictionary<Vector2Int,Node> Grid
    {
        get => _grid;
    }

    public int UnityGridSize
    {
        get => _unityGridSize;
    }

    #endregion
    #region MonoBehaveMethods

    private void Awake()
    {
        CreateGrid();
    }

    #endregion

    #region PrivateMethods
    /// <summary>
    /// Creates grid by given size 
    /// </summary>
    void CreateGrid()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                var coordinates = new Vector2Int(x, y);
                _grid.Add(coordinates, new Node(coordinates, true));
           
            }
        }
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Gets the node of a specifid grid
    /// </summary>
    /// <param name="coordinate">Node coordinate to find</param>
    /// <returns>Node of the given coordinate, null if not exist</returns>
    public Node GetNode(Vector2Int coordinate)
    {
        return !_grid.ContainsKey(coordinate) ? null : _grid[coordinate];
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if (_grid.ContainsKey(coordinates))
        {
            GetNode(coordinates).isWalkable = false;
        }
    }
    
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / _unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / _unityGridSize);
        return coordinates;
    }


    public void ResetNodes()
    {
        foreach (var enrty in _grid)
        {
            enrty.Value.connectedTo = null;
            enrty.Value.isExplored = false;
            enrty.Value.isInPath = false;
        }       
    }           
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = Vector3.zero;
        position.x = coordinates.x * _unityGridSize;
        position.z = coordinates.y * _unityGridSize;
        return position;
    }

    #endregion
}