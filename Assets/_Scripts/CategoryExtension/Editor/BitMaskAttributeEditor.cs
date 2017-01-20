using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( BitMaskAttribute ) )]
public class BitMaskAttributeEditor : PropertyDrawer
{
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        BitMaskAttribute _bitMask = attribute as BitMaskAttribute;

        position = EditorGUI.PrefixLabel( position, label );

        int _oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        const float CHECKBOX_WIDTH = 16.0f;
        const float CHECKBOX_LABEL_WIDTH = 10.0f;

        for ( int i = 0; i < _bitMask.count; i++ )
        {
            //position.width = CHECKBOX_LABEL_WIDTH;

            //EditorGUI.LabelField( position, ( i + 1 ).ToString() );

            //position.x += CHECKBOX_LABEL_WIDTH;

            if ( _bitMask.separatorStep > 0 && i > 0 && ( i % _bitMask.separatorStep ) == 0 )
            {
                position.width = CHECKBOX_LABEL_WIDTH;
                EditorGUI.LabelField( position, "|" );
                position.x += CHECKBOX_LABEL_WIDTH;
            }

            position.width = CHECKBOX_WIDTH;

            EditorGUI.BeginDisabledGroup( ( _bitMask.disabledOptions & ( 1 << i ) ) == ( 1 << i ) );
            if ( EditorGUI.Toggle( position, ( property.intValue & ( 1 << i ) ) == ( 1 << i ) ) )
                property.intValue |= 1 << i;
            else
                property.intValue &= ~( 1 << i );

            EditorGUI.EndDisabledGroup();

            position.x += CHECKBOX_WIDTH;
        }

        EditorGUI.indentLevel = _oldIndent;
    }
}