using System;
using UnityEngine;

public static class CursorManager
{
    public static void SetCustomCursor( Texture2D _texture, Vector2 _hotSpot )
    {
        Cursor.SetCursor( _texture, _hotSpot, CursorMode.Auto );
    }

    private static bool s_cursorLocked;

    public static void LockMouse( bool isLocked ) { LockMouse( isLocked, true ); }
    public static void LockMouse( bool isLocked, bool showCursor )
    {
        s_cursorLocked = isLocked;
        if ( isLocked )
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = showCursor;
#if UNITY_EDITOR
            Cursor.lockState = CursorLockMode.None;
#else
            Cursor.lockState = CursorLockMode.Confined;
#endif
        }
    }


    public static void Toggle()
    {
        LockMouse( Cursor.visible );
    }

    public static void UpdateCursor()
    {
        LockMouse( s_cursorLocked );
    }
}
