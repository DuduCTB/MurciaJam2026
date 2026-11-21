using UnityEngine;
using System;

public abstract class TriggerAction : MonoBehaviour
{
    /// <summary>
    /// Define la acción que hará el objeto al ser activado por un TriggerSwitch.
    /// </summary>
    /// <param name="instataneus"> Indica si la acción debe ser instantánea.</param>
    /// <returns>Si la acción se ha completado o no.</returns>
    /// 
    public bool started = false;

    public abstract bool ExecuteAction(bool instataneus = false, Action onActionFinished = null);
    public virtual bool UndoAction(bool instataneus = false, Action onActionFinished = null) { return false; }
}
