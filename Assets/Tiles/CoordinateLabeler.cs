using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Labels tile coordinates as 2D Cartesian Coordinate System
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class CoordinateLabeler : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _blockedColor = Color.gray;
    [SerializeField] private Color _exploredColor = Color.yellow;
    [SerializeField] private Color _pathColor = new Color(1F, 0.6F, 0);

    #endregion

    #region Private Fields

    TMP_Text _label;
    Vector2 _parentLocation = new Vector2Int();
    private GridManager _gridManager;
    private string _locationStr;

    #endregion

    #region Public Properties

    #endregion

    #region MonoBehaveMethods

    void Awake()
    {
        GetSelfDependencies();
        SetSelfFields();
    }

    void Start()
    {
        GetExteriorDependencies();
    }


    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateParentName();
            return;
        }

        DisplayCoordinates();
        SetBlockedColor();
        ToggleLabels(KeyCode.L);
    }

    #endregion

    #region PublicMethods

    #endregion

    #region PrivateMethods

    /// <summary>
    /// Displays the coordinates on tile.
    /// </summary>
    void DisplayCoordinates()
    {
        _parentLocation.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        _parentLocation.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);
        _locationStr = $"({_parentLocation.x}, {_parentLocation.y})";
        _label.text = _locationStr;
    }

    /// <summary>
    /// Updates parent name according to coordinate it's standing
    /// </summary>
    void UpdateParentName()
    {
        transform.parent.gameObject.name = _parentLocation.ToString();
    }

    /// <summary>
    /// Gets necessary references from attached gameobject on awake
    /// </summary>
    void GetSelfDependencies()
    {
        _label = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Gets necessary EXTERIOR references on start
    /// </summary>
    void GetExteriorDependencies()
    {
        //Get Waypoint from parent
        _gridManager = FindObjectOfType<GridManager>();
    }

    /// <summary>
    /// Sets the wanted fields on start of attached gameobject or its componenets fields 
    /// </summary>
    void SetSelfFields()
    {
        //Set TMP Color
        _label.color = _defaultColor;
        //CloseLabels by default
        _label.enabled = false;
    }

    /// <summary>
    /// Changes the text color of label to blocked color, only called if parent is not placable
    /// </summary>
    void SetBlockedColor()
    {
        Node node = _gridManager.GetNode(Vector2Int.RoundToInt(_parentLocation));
        if (node is null) return;

        if (node._isWalkable) return;
        _label.color = _blockedColor;

        if (!node._isInPath) return;
        _label.color = _pathColor;
        if (!node._isExplored) return;
        _label.color = _exploredColor;
    }

    /// <summary>
    /// Opens or closes the labels 
    /// </summary>
    /// <param name="keyCode">Change state of label key</param>
    void ToggleLabels(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            _label.enabled = !_label.IsActive();
        }
    }

    #endregion
}