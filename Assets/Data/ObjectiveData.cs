using UnityEngine;

[CreateAssetMenu(fileName = "NewObjectiveData", menuName = "ScriptableObjects/Objective Data", order = 0)]
public class ObjectiveData : ScriptableObject
{

    [TextArea(5, 10)]

    public string objectiveDesc;

}
