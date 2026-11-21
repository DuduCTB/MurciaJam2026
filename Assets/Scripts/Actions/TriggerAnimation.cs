using System;
using UnityEngine;

public class TriggerAnimation : TriggerAction
{

    [SerializeField] private String animParameter;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public override bool ExecuteAction(bool instataneus = false, Action onActionFinished = null)
    {
        anim.SetBool(animParameter, true);
        return true;
    }

    public override bool UndoAction(bool instataneus = false, Action onActionFinished = null)
    {
        anim.SetBool(animParameter, false);
        return true;
    }


}
