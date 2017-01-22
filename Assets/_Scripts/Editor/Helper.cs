using UnityEngine;
using UnityEditor;
using System.Collections;

public class Helper : Editor
{
    [MenuItem( "GGJ/Set Mass" )]
    public static void SetMass()
    {
        foreach ( var _go in Selection.gameObjects )
        {
            var _rbs = _go.GetComponentsInChildren<Rigidbody>( true );

            foreach ( var _rb in _rbs )
            {
                _rb.mass = 100;
            }
        }
    }

}
