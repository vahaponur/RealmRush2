using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Represents Enemy Movement Behaviour
/// </summary>
[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] [Range(0f, 4f)] private float _speed = 1f;

    #endregion

    #region Private Fields

    /// <summary>
    /// Path of enemy
    /// </summary>
    private List<Node> _path = new List<Node>();

    private Enemy _enemyMain;
    private GridManager _gridManager;
    private Pathfinder _pathfinder;

    #endregion

    #region Public Properties

    #endregion

    #region MonoBehaveMethods

    private void Awake()
    {
        _enemyMain = GetComponent<Enemy>();
        _gridManager = FindObjectOfType<GridManager>();
        _pathfinder = FindObjectOfType<Pathfinder>();
        
        
    }

    void OnEnable()
    {
        ReturnToStart();
        RecalculatePath(true);
        
    }

    #endregion

    #region PublicMethods

    #endregion

    #region PrivateMethods

    /// <summary>
    /// Enemy follows path by moving with given seconds between tiles
    /// </summary>
    /// <returns>Null</returns>
    IEnumerator FollowPath()
    {
      

        for (int i = 1; i < _path.Count; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = _gridManager.GetPositionFromCoordinates(_path[i].coordinates);
            float travelPercent = 0f;
            transform.LookAt(endPos);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * _speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        ProcessFinishPath();
    }

    /// <summary>
    /// Finds the path for enemy on the current level
    /// </summary>
    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = resetPath ? _pathfinder.StartCoordinates : _gridManager.GetCoordinatesFromPosition(transform.position);
        StopAllCoroutines();
      _path.Clear();
      _path = _pathfinder.GetNewPath(coordinates);
      StartCoroutine(FollowPath());
      
    }

    /// <summary>
    /// Set position of the enemy to first point of the path
    /// </summary>
    void ReturnToStart()
    {
        transform.position = _gridManager.GetPositionFromCoordinates(_pathfinder.StartCoordinates);
    }

    /// <summary>
    /// Calls the events if enemy successfully finishes the path
    /// </summary>
    void ProcessFinishPath()
    {
        _enemyMain.GivePenalty();
        gameObject.SetActive(false);
    }

    #endregion
}