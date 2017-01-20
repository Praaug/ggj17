using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( MaxAttribute ) )]
public class MaxAttributeEditor : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        // First get the attribute since it contains the range for the slider
        MaxAttribute max = attribute as MaxAttribute;

        // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
        if ( property.propertyType == SerializedPropertyType.Float )
            property.floatValue = Mathf.Min( max.max, EditorGUI.FloatField( position, label, property.floatValue ) );
        else if ( property.propertyType == SerializedPropertyType.Integer )
            property.intValue = (int)Mathf.Min( max.max, EditorGUI.IntField( position, label, property.intValue ) );
        else
            EditorGUI.LabelField( position, label.text, "Use Max with float or int." );
    }
}
