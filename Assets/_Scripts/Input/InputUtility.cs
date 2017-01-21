﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    Horizontal,
    Vertical,
    //MouseX,
    //MouseY,
    //MouseScrollwheel
}

public enum InputButton
{
    Attack
}

public enum InputSource
{
    Player1,
    Player2
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

    [SerializeField, Category( "KeyBindings - Player 1" )]
    private string m_horAxis1 = "Horizontal";
    [SerializeField, Category( "KeyBindings - Player 1" )]
    private string m_verAxis1 = "Vertical";
    [SerializeField, Category( "KeyBindings - Player 1" )]
    private KeyCode m_attackKey1 = KeyCode.Space;

    [SerializeField, Category( "KeyBindings - Player 2" )]
    private string m_horAxis2 = "Horizontal";
    [SerializeField, Category( "KeyBindings - Player 2" )]
    private string m_verAxis2 = "Vertical";
    [SerializeField, Category( "KeyBindings - Player 2" )]
    private KeyCode m_attackKey2 = KeyCode.Space;

    #endregion

    #region Methods
    private void Awake()
    {
        if ( s_instance != null && s_instance != this )
        {
            Destroy( this );
            return;
        }

        s_instance = this;
    }

    private void Update()
    {

    }

    public static bool GetButtonDown( InputButton p_button, InputSource p_inputSource )
    {
        switch ( p_button )
        {
            case InputButton.Attack:
                if ( p_inputSource == InputSource.Player1 )
                    return Input.GetKeyDown( s_instance.m_attackKey1 );
                else
                    return Input.GetKeyDown( s_instance.m_attackKey2 );
            default:
                throw new System.NotImplementedException( "This button is not yet implemented in the InputUtility" );
        }
    }

    public static float GetAxis( Axis p_axis, InputSource p_inputSource )
    {
        switch ( p_axis )
        {
            case Axis.Horizontal:
                if ( p_inputSource == InputSource.Player1 )
                    return Input.GetAxis( s_instance.m_horAxis1 );
                else
                    return Input.GetAxis( s_instance.m_horAxis2 );
            case Axis.Vertical:
                if ( p_inputSource == InputSource.Player1 )
                    return Input.GetAxis( s_instance.m_verAxis1 );
                else
                    return Input.GetAxis( s_instance.m_verAxis2 );
            //case Axis.MouseX:
            //    return Input.GetAxis( "Mouse X" );
            //case Axis.MouseY:
            //    return Input.GetAxis( "Mouse Y" );
            //case Axis.MouseScrollwheel:
            //    return Input.GetAxis( "Mouse ScrollWheel" );
            default:
                throw new System.NotImplementedException( "This input axis is not implemented" );
        }
    }
    #endregion
}
