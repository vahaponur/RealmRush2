using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents attached gameobject as Waypoint
/// </summary>
public class Tile : MonoBehaviour
{
    #region Serialized Fields

    [Tooltip("Can player put a turrent on this tile?")] [SerializeField]
    private bool isPlaceable;

    [SerializeField] private Tower _turretPrefab;

    #endregion

    #region Private Fields

    private GridManager _gridManager;
    private Vector2Int _coordinates = new Vector2Int();
    private Pathfinder _pathfinder;
    #endregion

    #region Public Properties
    public bool IsPlaceable
    {
        get => isPlaceable;
    }
    #endregion

    #region MonoBehaveMethods

    private void Start()
    {
        GetExteriorDependencies();
        if (!(_gridManager is  null))
        {
            _coordinates = _gridManager.GetCoordinatesFromPosition(transform.position);
            if (!isPlaceable)
            {
                //We change the ex situation, now turrets can be placed on players way, tiles are placeble if and only if that node is walkable
              _gridManager.BlockNode(_coordinates);  
            }

        }
    }

    private void OnMouseDown()
    {
        if (_gridManager.GetNode(_coordinates).isWalkable && !_pathfinder.WillBlockPath(_coordinates))
        {
            PlaceTurret();
            if (!isPlaceable)
            {
                _gridManager.BlockNode(_coordinates);
                _pathfinder.NotifyReceivers();
            }
        }
    }

    #endregion

    #region PublicMethods

    #endregion

    #region PrivateMethods
    /// <summary>
    /// Place tower on this tile if not already placed
    /// </summary>
    void PlaceTurret()
    {
        bool isPlaced = _turretPrefab.CreateTower(_turretPrefab,transform.position);
       
        isPlaceable = !isPlaced;
    }


    void GetExteriorDependencies()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();

    }
    #endregion
}