using System.Collections;
using System;
using UnityEngine;
using UnityEditor;

public enum TransitionType { From, To};

public class NodeTransition : ScriptableObject
{
    public DialogueNode node;
    public TransitionType transitionType;

    public Action<NodeTransition> OnClickRemoveTransition;

    public NodeTransition(DialogueNode node, TransitionType type, Action<NodeTransition> OnClickRemoveTransition)
    {
        this.node = node;
        transitionType = type;
        this.OnClickRemoveTransition = OnClickRemoveTransition;
    }

    //Nothing to really draw here to to speak
    public void Draw()
    {
        if (OnClickRemoveTransition != null)
            OnClickRemoveTransition(this);
    }

}
