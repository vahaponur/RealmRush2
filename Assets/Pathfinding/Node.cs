using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Node
{
   /// <summary>
   /// Coordinates on grid of this node
   /// </summary>
   [FormerlySerializedAs("_coordinates")] public Vector2Int coordinates;

   /// <summary>
   /// Is the node can be added to tree or not
   /// </summary>
   [FormerlySerializedAs("_isWalkable")] public bool isWalkable;

   /// <summary>
   /// Is the node on the tree
   /// </summary>
   [FormerlySerializedAs("_isExplored")] public bool isExplored;

   /// <summary>
   /// Is the node in the path
   /// </summary>
   [FormerlySerializedAs("_isInPath")] public bool isInPath;

   /// <summary>
   /// What node is the first parent
   /// </summary>
   [FormerlySerializedAs("_connectedTo")] public Node connectedTo;

   public Node(Vector2Int coordinates,bool isWalkable)
   {
      this.coordinates = coordinates;
      this.isWalkable = isWalkable;
   }

}
