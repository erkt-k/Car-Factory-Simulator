using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Disable the GUI so the field is not editable
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true; // Re-enable the GUI
    }
}

public class Vehicles : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] private string currentStation = "inspection";

    public string GetCurrentStation() 
    { 
        return currentStation;
    }

    public void ToRepair()
    {
        currentStation = "repairment";
    }

}
