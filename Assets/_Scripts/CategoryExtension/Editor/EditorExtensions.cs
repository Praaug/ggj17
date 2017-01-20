using UnityEngine;
using UnityEditor;

public static class EditorExtensions
{
    public static void DrawScriptField( this Editor editor )
    {
        EditorGUI.BeginDisabledGroup( true );
        EditorGUILayout.ObjectField( "Script", MonoScript.FromMonoBehaviour( editor.target as MonoBehaviour ), typeof( MonoScript ), false );
        EditorGUI.EndDisabledGroup();
    }

}