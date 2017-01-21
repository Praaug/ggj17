using UnityEngine;
using System.Collections;

public static class Randomx
{
    public static float Bias( float p_bias )
    {
        return Random.Range( -p_bias, p_bias );
    }

    public static int Range0( int p_value )
    {
        return Random.Range( 0, p_value );
    }
    public static float Range0( float p_value )
    {
        return Random.Range( 0, p_value );
    }

    public static Vector3 Box( float p_sizeX, float p_sizeY, float p_sizeZ )
    {
        return new Vector3( Bias( p_sizeX * 0.5f ), Bias( p_sizeY * 0.5f ), Bias( p_sizeZ * 0.5f ) );
    }
    public static Vector3 Box( Vector3 p_boxSize )
    {
        return Box( p_boxSize.x, p_boxSize.y, p_boxSize.z );
    }
}
