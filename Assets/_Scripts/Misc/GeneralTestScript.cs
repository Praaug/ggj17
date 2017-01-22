using UnityEngine;
using System.Collections;

public class GeneralTestScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
        Dbg.LogFast( "Button down: {0}", InputUtility.GetFixedButtonDown( InputButton.Attack, InputSource.Player1 ) );
    }
}
