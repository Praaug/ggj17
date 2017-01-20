using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( MinAttribute ) )]
public class MinAttributeEditor : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        // First get the attribute since it contains the range for the slider
        MinAttribute min = attribute as MinAttribute;

        // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
        if ( property.propertyType == SerializedPropertyType.Float )
            property.floatValue = Mathf.Max( min.min, EditorGUI.FloatField( position, label, property.floatValue ) );
        else if ( property.propertyType == SerializedPropertyType.Integer )
            property.intValue = (int)Mathf.Max( min.min, EditorGUI.IntField( position, label, property.intValue ) );
        else
            EditorGUI.LabelField( position, label.text, "Use Max with float or int." );
    }
}