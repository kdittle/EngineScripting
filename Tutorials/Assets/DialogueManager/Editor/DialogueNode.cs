using System.Collections;
using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class DialogueNode : ScriptableObject
{
    public Rect nodeRect;
    public GUIStyle style;

    public string DialogueText;
    public Texture TextBoxTexture;
    public Texture CharacterThumbnail;
    public AudioClip voClip;

    public bool isSelected;
    public bool isDragged;

    //Transition To
    public NodeTransition transitionFrom;
    public NodeTransition transitionTo;
    public bool isMakingTransition = false;

    public GUIStyle selectedStyle;
    public GUIStyle defaultStyle;

    public Action<DialogueNode> OnRemoveNode;
    //public Action<NodeTransition> OnCreateTransition;

    public DialogueNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        Action<DialogueNode> OnClickRemoveNode, Action<NodeTransition> OnClickMakeTransition)
    {
        nodeRect = new Rect(position.x, position.y, width, height);
        //Debug.Log(nodeRect);
        style = nodeStyle;
        transitionFrom = new NodeTransition(this, TransitionType.From, OnClickMakeTransition);
        transitionTo = new NodeTransition(null, TransitionType.To, OnClickMakeTransition);
        //OnCreateTransition = OnClickMakeTransition;
        this.selectedStyle = selectedStyle;
        this.defaultStyle = nodeStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    private void OnGUI()
    {

    }

    public void Drag(Vector2 delta)
    {
        nodeRect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(nodeRect, "", style);
    }

    public void DrawTransitions()
    {
        Handles.DrawLine(transitionFrom.node.nodeRect.position, transitionTo.node.nodeRect.position);
    }

    public bool ProcessEvent(Event e)
    {
        switch(e.type)
        {
            case EventType.mouseDown:
                {
                    if(e.button == 0)
                    {
                        if(nodeRect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedStyle;
                        }
                        else
                        {
                            GUI.changed = false;
                            isSelected = false;
                            style = defaultStyle;
                        }
                    }

                    if(e.button == 1 && isSelected && nodeRect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }

                    break;
                }
            case EventType.mouseUp:
                {
                    isDragged = false;
                    break;
                }
            case EventType.MouseDrag:
                {
                    if(e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
                }
        }
        return false;
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Make Transition"), false, OnMakeTransition);
        genericMenu.AddItem(new GUIContent("Remove Node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnMakeTransition()
    {
        isMakingTransition = true;
    }

    public void SetTransitionTo(DialogueNode toNode)
    {
        transitionTo.node = toNode;
        isMakingTransition = false;
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
            OnRemoveNode(this);
    }

    private void OnClickRemoveTransition(NodeTransition transitionToRemove)
    {

    }
}
