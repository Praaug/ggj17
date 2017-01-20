using UnityEngine;

/// <summary>
/// Attribute that is used to ensure a maximum value on a variable
/// </summary>
public class MaxAttribute : PropertyAttribute
{
    public float max;

    public MaxAttribute( float max )
    {
        this.max = max;
    }
}
