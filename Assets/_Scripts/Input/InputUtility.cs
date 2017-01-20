using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    Horizontal,
    Vertical,
    MouseX,
    MouseY,
    MouseScrollwheel
}

public class InputUtility : MonoBehaviour
{
    #region Fields
    private static InputUtility instance
    {
        get
        {
            if ( s_instance == null )
                s_instance = new GameObject( "InputUtility" ).AddComponent<InputUtility>();

            return s_instance;
        }
    }
    private static InputUtility s_instance;
    #endregion

    #region Methods
    private void Update()
    {

    }

    public static float GetAxis( Axis p_axis )
    {
        switch ( p_axis )
        {
            case Axis.Horizontal:
                return Input.GetAxis( "Horizontal" );
            case Axis.Vertical:
                return Input.GetAxis( "Vertical" );
            case Axis.MouseX:
                return Input.GetAxis( "Mouse X" );
            case Axis.MouseY:
                return Input.GetAxis( "Mouse Y" );
            case Axis.MouseScrollwheel:
                return Input.GetAxis( "Mouse ScrollWheel" );
            default:
                throw new System.NotImplementedException( "This input axis is not implemented" );
        }
    }
    #endregion
}
