using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class DialogueInstance : EditorWindow
{
    private static DialogueInstance dialogueInstance;
    private GUIContent instanceInfo;

    //Information about the instance
    DialogueProperties properties;

    //List of nodes
    List<DialogueNode> DialogueNodes;
    List<NodeTransitionConnection> NodeTransitions;
    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private NodeTransition selectedFromNode;
    private NodeTransition selectedToNode;

    private bool isMakingTransition = false;
    private DialogueNode curSelectedNode;
    private int nodeIndex;

    private Vector2 offset;
    private Vector2 drag;

    public static DialogueInstance CreateNewInstance(string name)
    {
        dialogueInstance = (DialogueInstance)GetWindow(typeof(DialogueInstance));
        dialogueInstance.titleContent.text = name;
        return dialogueInstance;
    }

    private void OnEnable()
    {
        //DialogueNodes = new List<DialogueNode>();

        //Default node style
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        //selected node style
        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnDisable()
    {
        //Save some things?
    }

    public void SaveInstanceData()
    {
        //Save each instance's data here
    }

    public void LoadInstanceData()
    {
        //Load the instance data here
    }

    private void OnGUI()
    {
        DrawGrid(20.0f, 0.2f, Color.gray);
        DrawGrid(100.0f, 0.4f, Color.gray);

        DrawConnections();
        DrawNodes();

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed)
            Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridColor.a * gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0.0f);

        for(int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0.0f) + newOffset, new Vector3(gridSpacing * i, position.height, 0.0f) + newOffset);
        }

        for(int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0.0f) + newOffset, new Vector3(position.width, gridSpacing * j, 0.0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch(e.type)
        {
            case EventType.MouseDown:
                {
                    if (e.button == 1 && !isMakingTransition)
                        ProcessContextMenu(e.mousePosition);
                    else if(e.button == 0 && curSelectedNode != null && isMakingTransition)
                    {
                        for(int i = 0; i < DialogueNodes.Count; i++)
                        {
                            if(DialogueNodes[i] != DialogueNodes[nodeIndex])
                            {
                                DialogueNodes[nodeIndex].SetTransitionTo(DialogueNodes[i]);
                                CreateConnection();
                                isMakingTransition = false;
                                curSelectedNode = null;
                                break;
                            }
                        }
                    }

                    break;
                }
            case EventType.MouseDrag:
                {
                    if (e.button == 0)
                        OnDrag(e.delta);

                    break;
                }
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if(DialogueNodes != null)
        {
            //Debug.Log("Processing Events");
            for(int i = DialogueNodes.Count - 1; i >= 0; i--)
            {
                bool isGUIChanged = DialogueNodes[i].ProcessEvent(e);

                if(DialogueNodes[i].isSelected && curSelectedNode == null)
                {
                    //Debug.Log("Node selected");
                    curSelectedNode = DialogueNodes[i];
                    //Debug.Log(curSelectedNode.nodeRect.position);
                    nodeIndex = i;
                    isMakingTransition = curSelectedNode.isMakingTransition ? true : false;
                    break;
                }
                else if (curSelectedNode && DialogueNodes[i].Equals(curSelectedNode))
                {
                    if(curSelectedNode.isMakingTransition)
                    {
                        isMakingTransition = true;
                        nodeIndex = i;
                        break;
                    }
                }

                if (isGUIChanged)
                    GUI.changed = true;
            }
        }
    }

    private void ProcessContextMenu(Vector2 mousePos)
    {
        GenericMenu genMenu = new GenericMenu();
        genMenu.AddItem(new GUIContent("Add New Node"), false, () => OnClickAddNode(mousePos));
        genMenu.ShowAsContext();
    }


    private void OnClickAddNode(Vector2 pos)
    {
        if (DialogueNodes == null)
            DialogueNodes = new List<DialogueNode>();

        DialogueNodes.Add(new DialogueNode(pos, 200, 50, nodeStyle, selectedNodeStyle, OnClickRemoveNode, OnClickMakeTransition));
    }

    private void DrawNodes()
    {
        if(DialogueNodes != null)
        {
            for(int i = 0; i < DialogueNodes.Count; i++)
            {
                DialogueNodes[i].Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (NodeTransitions != null)
        {
            for (int i = 0; i < NodeTransitions.Count; i++)
            {
                NodeTransitions[i].Draw();
            }
        }
    }

    private void OnClickRemoveNode(DialogueNode nodeToRemove)
    {
        //if(DialogueNodes != null)
        //{
        //}

        if(NodeTransitions != null)
        {
            List<NodeTransitionConnection> connectionsToRemove = new List<NodeTransitionConnection>();

            for(int i = 0; i < NodeTransitions.Count; i++)
            {
                if (NodeTransitions[i].toNode == nodeToRemove || NodeTransitions[i].fromNode == nodeToRemove)
                    connectionsToRemove.Add(NodeTransitions[i]);
            }

            for(int i = 0; i < connectionsToRemove.Count; i++)
            {
                NodeTransitions.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;

            DialogueNodes.Remove(nodeToRemove);
        }
    }

    private void OnClickMakeTransition(NodeTransition transition)
    {
        for(int i = 0; i < DialogueNodes.Count; i++)
        {
            if(DialogueNodes[i].isSelected)
            {
                curSelectedNode = DialogueNodes[i];
                //Debug.Log("Selected Node: " + curSelectedNode.name);
            }
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if(DialogueNodes != null)
        {
            for(int i = 0; i < DialogueNodes.Count; i++)
            {
                DialogueNodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void CreateConnection()
    {
        if (NodeTransitions == null)
            NodeTransitions = new List<NodeTransitionConnection>();

        NodeTransitions.Add(new NodeTransitionConnection(DialogueNodes[nodeIndex].transitionFrom, DialogueNodes[nodeIndex].transitionTo, OnClickRemoveConnection));
    }

    private void OnClickRemoveConnection(NodeTransitionConnection trans)
    {
        NodeTransitions.Remove(trans);
    }
}
