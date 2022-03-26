using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
   /// <summary>
   /// Coordinates on grid of this node
   /// </summary>
   public Vector2Int _coordinates;

   /// <summary>
   /// Is the node can be added to tree or not
   /// </summary>
   public bool _isWalkable;

   /// <summary>
   /// Is the node on the tree
   /// </summary>
   public bool _isExplored;

   /// <summary>
   /// Is the node in the path
   /// </summary>
   public bool _isInPath;

   /// <summary>
   /// What node is the first parent
   /// </summary>
   public Node _connectedTo;

   public Node(Vector2Int coordinates,bool isWalkable)
   {
      _coordinates = coordinates;
      _isWalkable = isWalkable;
   }

}
