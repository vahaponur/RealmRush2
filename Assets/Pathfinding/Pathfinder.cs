using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private Vector2Int _startCoordinates;
    [SerializeField] private Vector2Int _destCoordinates;
    #endregion

    #region Private Fields

    private Vector2Int[] _searchDirections = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    private GridManager _gridManager;
    private Dictionary<Vector2Int, Node> _grid;

    private Queue<Node> _frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> _reached = new Dictionary<Vector2Int, Node>();
    
    private Node _startNode;
    private Node _destNode;
    private Node _currentSearchNode;

    #endregion

    #region Public Properties
    public  Vector2Int StartCoordinates
    {
        get => _startCoordinates;
    }

    public Vector2Int DestinationCoordinates
    {
        get => _destCoordinates;
    }
    #endregion

    #region MonoBehaveMethods
    void Awake()
    {
        _gridManager = FindObjectOfType<GridManager>();
        _grid = _gridManager.Grid;
        _startNode = _gridManager.GetNode(_startCoordinates);
        _destNode = _gridManager.GetNode(_destCoordinates);
       
    }

    void Start()
    {

        GetNewPath();
    }

   
    void Update()
    {
        
    }
    #endregion
    
    #region PublicMethods

    public List<Node> GetNewPath()
    {
        return GetNewPath(_startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        _gridManager.ResetNodes();
        BreadthFirstSearch(coordinates);
        return BuildPath();
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath",false,SendMessageOptions.DontRequireReceiver);
    }
    #endregion
    
    #region PrivateMethods

    void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();
        for (int i = 0; i < _searchDirections.Length; i++)
        {
            var nodeCoordinate = _currentSearchNode.coordinates + _searchDirections[i];
            if (_grid.ContainsKey(nodeCoordinate))
            {
                neighbors.Add(_grid[nodeCoordinate]);
            }
        }

        foreach (var neighbor in neighbors)
        {
            if (neighbor.isWalkable && !_reached.ContainsKey(neighbor.coordinates))
            {
                neighbor.connectedTo = _currentSearchNode;
                _reached.Add(neighbor.coordinates,neighbor);
                _frontier.Enqueue(neighbor);
            }
        }

  
        
        
    }

 
    void BreadthFirstSearch(Vector2Int coordinates)
    {
        _startNode.isWalkable = true;
        _destNode.isWalkable = true;
        _frontier.Clear();
        _reached.Clear();
        bool isRunning = true;
        Node node = _grid[coordinates];
        _frontier.Enqueue(node);
        _reached.Add(coordinates,node);

        while (_frontier.Count > 0 && isRunning)
        {
            _currentSearchNode = _frontier.Dequeue();
            _currentSearchNode.isExplored = true;
            ExploreNeighbors();
            if (_currentSearchNode.coordinates ==_destCoordinates)
            {
                isRunning = false;
            }
        }
        
    }

    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = _destNode;

        path.Add(currentNode);
        currentNode.isInPath = true;
        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isInPath = true;
        }
        path.Reverse();
        return path;
    }
    public bool WillBlockPath(Vector2Int coordinates)
    {
        if(_grid.ContainsKey(coordinates))
        {
            bool previousState = _grid[coordinates].isWalkable;

            _grid[coordinates].isWalkable = false;
            List<Node> newPath = GetNewPath();
            _grid[coordinates].isWalkable = previousState;

            if(newPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false; 
    }

    

    #endregion
}
