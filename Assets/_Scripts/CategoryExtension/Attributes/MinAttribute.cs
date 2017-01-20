using UnityEngine;
using System.Collections;

/// <summary>
/// Attribute that is used to ensure a minimum value on a variable
/// </summary>
public class MinAttribute : PropertyAttribute
{
    public float min;

    public MinAttribute( float min )
    {
        this.min = min;
    }
}
