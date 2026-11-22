using UnityEngine;
using System;

public class GameEvents 
{
    // interacion
    public static Action<float, float> interactionStarted;
    public static Action interactionCancelled;
    public static Action interactionSuccessful;
    public static Action<string> interactableHovered;

    // UI

    public static Action<string> onObjectiveChanged;

}
