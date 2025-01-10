using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static RequiredField;

[AttributeUsage(AttributeTargets.Field)]
public class RequiredField : PropertyAttribute
{
    public enum WarningType {Error,Warning}
    public WarningType warningType;

    public RequiredField(WarningType wt = WarningType.Error)
    {
        warningType = wt;
    }
}

[CustomPropertyDrawer(typeof(RequiredField))]
public class PropertyWarningEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
        if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
        {
            GUILayout.Space(30);
            Rect warrningRec = new Rect(position.x, position.y+ position.height*1.3f, position.width,position.height+1.3f);
            EditorGUI.HelpBox(warrningRec, "This Field is null",MessageType.Error);
        }

    }
}

public static class RequiredFieldValid
{
    static RequiredFieldValid()
    {
        EditorApplication.playModeStateChanged += (state) =>
        {
            if (state  == PlayModeStateChange.ExitingEditMode)
            {
                Debug.Log("dd");
                DebugWarning();
            }
        };
    }
    private static void DebugWarning()
    {
        bool canPuse  = false;
        MonoBehaviour[] mb = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (MonoBehaviour mb2 in mb)
        {
            FieldInfo[] fieldInfo = mb2.GetType().GetFields(BindingFlags.Instance |BindingFlags.NonPublic |BindingFlags.Public);
            foreach (FieldInfo fieldInfo2 in fieldInfo)
            {
                RequiredField requiredField = fieldInfo2.GetCustomAttribute<RequiredField>();
                if (requiredField !=null)
                {
                    object fliadValue = fieldInfo2.GetValue(mb2);
                    if(fliadValue.Equals(null) && requiredField.warningType == WarningType.Error) 
                    {
                        Debug.Log($"The field {fieldInfo2.Name} is required in {mb2.gameObject.name} ", mb2);
                        canPuse = true;
                    }   
                }
            }
        }

        if (canPuse)
        {
            EditorApplication.ExitPlaymode();
        }
    }
}

