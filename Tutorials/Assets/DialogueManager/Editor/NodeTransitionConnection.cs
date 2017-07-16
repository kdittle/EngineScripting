using System.Collections;
using System;
using UnityEngine;
using UnityEditor;

public class NodeTransitionConnection : ScriptableObject
{
    public NodeTransition fromNode;
    public NodeTransition toNode;
    public Action<NodeTransitionConnection> OnClickRemoveConnection;

    public NodeTransitionConnection(NodeTransition fNode, NodeTransition tNode, Action<NodeTransitionConnection> OnClickRemove)
    {
        fromNode = fNode;
        toNode = tNode;
        OnClickRemoveConnection = OnClickRemove;
    }

    //Draw the actual line connecting things
    public void Draw()
    {
        Vector2 startPos = fromNode.node.nodeRect.center;
        Vector2 endPos = toNode.node.nodeRect.center;

        Handles.DrawLine(startPos, endPos);
    }  
}