using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Vector2Int _gridSize;

    #endregion

    #region Private Fields

    private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();

    #endregion

    #region MonoBehaveMethods

    private void Awake()
    {
        CreateGrid();
    }

    #endregion

    #region PrivateMethods

    void CreateGrid()
    {
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                var coordinates = new Vector2Int(x, y);
                _grid.Add(coordinates, new Node(coordinates, true));
                Debug.Log(_grid[coordinates]._coordinates);
            }
        }
    }

    #endregion

    #region Public Methods

    public Node GetNode(Vector2Int coordinate)
    {
        return !_grid.ContainsKey(coordinate) ? null : _grid[coordinate];
    }

    #endregion
}