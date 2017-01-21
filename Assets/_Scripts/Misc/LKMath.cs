#region Class Information
/*----------------------------------------------------------------------------------
// File:           LKMath.cs
// Project:          $projectName$
// Solution:           $solutionName$
// Description:        $description$
//
// Change History:
// Name                            
// Date                                          
// Version  
// Description                    
// ----------------------------------------------------------------------------------
// $programmer$  $date$             1.0     Created
// ----------------------------------------------------------------------------------*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Random = UnityEngine.Random;

/// <summary>
/// Static class that provides a lot of useful math functions
/// </summary>
/// <remarks>A lot of functions were created in context to some repeating calculations in Looterkings</remarks>
public static class GGJMath
{
    /// <summary>
    /// The square root of 2 as a constant
    /// </summary>
    public const float SQRT2 = 1.4142135623730950488016887242097f;

    public delegate float InterpolationMethod( float start, float end, float value );

    #region Methods
    /// <summary>
    /// Returns a random seed for randomization processes
    /// </summary>
    public static int GetRandomSeed()
    {
        return Random.Range( int.MinValue, int.MaxValue );
    }

    /// <summary>
    /// Returns 0 if value is close to 0
    /// </summary>
    /// <param name="p_deadZone">The margin in which value is considered 0</param>
    public static float DeadZone( float p_value, float p_deadZone )
    {
        return ( p_value < p_deadZone && p_value > -p_deadZone ) ? 0 : p_value;
    }

    /// <summary>
    /// Alternative Clamp function that takes a reference value and a max delta that must not be exceeded by value
    /// </summary>
    /// <param name="p_value">The value that needs clamping</param>
    /// <param name="p_refValue">The reference value</param>
    /// <param name="p_maxDelta">The maximum allowed delta between p_value and p_refValue</param>
    public static float Clamp( float p_value, float p_refValue, float p_maxDelta )
    {
        return Mathf.Clamp( p_value, p_refValue - p_maxDelta, p_refValue + p_maxDelta );
    }

    public static float ToEuler( float p_value )
    {
        return p_value < -360.0f ? ToEuler( p_value + 360 ) : ( p_value + 360 ) % 360.0f;
    }

    /// <summary>
    /// Returns true when x lies between the lower and upper bounds
    /// </summary>
    /// <param name="x">The value that should be checked</param>
    /// <param name="lower">The lower bound to check</param>
    /// <param name="upper">The upper bound to check</param>
    public static bool InRange( float x, float lower, float upper )
    {
        if ( lower > upper )
        {
            float tmp = lower;
            lower = upper;
            upper = tmp;
        }

        return x >= lower && x <= upper;
    }

    /// <summary>
    /// Returns true when x lies between the lower and upper bounds
    /// </summary>
    /// <param name="x">The value that should be checked</param>
    /// <param name="lower">The lower bound to check</param>
    /// <param name="upper">The upper bound to check</param>
    public static bool InRange( int x, int lower, int upper )
    {
        if ( lower > upper )
        {
            int tmp = lower;
            lower = upper;
            upper = tmp;
        }

        return x >= lower && x <= upper;
    }

    /// <summary>
    /// Returns true if the difference (delta) between a and b is smaller than maxDelta
    /// </summary>
    /// <param name="a">The first value</param>
    /// <param name="b">The second value</param>
    public static bool DeltaCheck( float a, float b, float p_maxDelta )
    {
        return Mathf.Abs( a - b ) < p_maxDelta;
    }

    /// <summary>
    /// Method designed for time depended checks. Returns true when the time has not yet passed
    /// </summary>
    /// <param name="p_beginTimestamp">The initial timestamp</param>
    /// <param name="p_currentTimestamp">The current timestamp</param>
    /// <param name="p_maxDeltaTime">The maximum difference allowed between the initial and current timestamp (e.g. the duration)</param>
    /// <returns>Returns true when the current timestamp is not greater than begintimestamp plus maxdeltatime</returns>
    public static bool TimeCheck( float p_beginTimestamp, float p_currentTimestamp, float p_maxDeltaTime )
    {
        return p_beginTimestamp + p_maxDeltaTime > p_currentTimestamp;
    }

    /// <summary>
    /// Indicates if the two float are equal with a certain delta threshold
    /// </summary>
    /// <param name="a">The original value</param>
    /// <param name="b">The value to compare</param>
    /// <param name="p_threshold">The maximum delta between a and b</param>
    /// <returns>True if the delta of a and b is less than the threshold</returns>
    public static bool RoughlyEqual( float a, float b, float p_threshold )
    {
        return Mathf.Abs( a - b ) < p_threshold;
    }

    /// <summary>
    /// Indicates whether two vectors are equal. This is not precise and tolerates a 0.01f difference in spacial distance
    /// </summary>
    /// <param name="a">The first vector</param>
    /// <param name="b">The second vector</param>
    public static bool RoughlyEqual( Vector3 a, Vector3 b )
    {
        return ( a - b ).sqrMagnitude < 0.000001f;
    }
    /// <summary>
    /// Indicates whether two vectors are equal. This is not precise and tolerates a 0.001f difference in spacial distance
    /// </summary>
    /// <param name="a">The first vector</param>
    /// <param name="b">The second vector</param>
    /// <param name="p_epsilon">The tolerated threshold</param>
    public static bool RoughlyEqual( Vector3 a, Vector3 b, float p_epsilon )
    {
        return ( a - b ).sqrMagnitude < p_epsilon * p_epsilon;
    }

    /// <summary>
    /// Indicates whether two quaternions are equal. This is not precise and tolerates a 0.01f difference in angular delta
    /// </summary>
    /// <param name="a">The first quaternion</param>
    /// <param name="b">The second quaternion</param>
    public static bool RoughlyEqual( Quaternion a, Quaternion b )
    {
        return Mathf.Abs( Quaternion.Dot( a, b ) - 1.0f ) < 0.01f;
    }
    /// <summary>
    /// Returns the center point of a given set of points
    /// </summary>
    public static Vector3 GetCenterOfPoints( params Vector3[] points )
    {
        return GetCenterOfPoints( points as IEnumerable<Vector3> );
    }
    /// <summary>
    /// Returns the center point of a given set of points
    /// </summary>
    public static Vector3 GetCenterOfPoints( IEnumerable<Vector3> points )
    {
        Vector3 _center = Vector3.zero;

        foreach ( Vector3 _point in points )
            _center += _point;

        return _center / points.Count();
    }

    /// <summary>
    /// Rotates a given point around another
    /// </summary>
    /// <param name="p_point">The point that should be rotated</param>
    /// <param name="p_origin">The point around which should be rotated</param>
    /// <param name="axis">The axis on which should be rotated</param>
    /// <param name="angle">The angle that should be rotated</param>
    public static Vector3 RotateAround( Vector3 p_point, Vector3 p_origin, Vector3 axis, float angle )
    {
        return RotateAround( p_point, p_origin, Quaternion.AngleAxis( angle, axis ) );
    }
    /// <summary>
    /// Rotates a given point around another
    /// </summary>
    /// <param name="p_point">The point that should be rotated</param>
    /// <param name="p_origin">The point around which should be rotated</param>
    /// <param name="p_rotation">The rotation that should be applied</param>
    public static Vector3 RotateAround( Vector3 p_point, Vector3 p_origin, Quaternion p_rotation )
    {
        return Matrix4x4.TRS( p_origin, p_rotation, Vector3.one ).MultiplyPoint3x4( p_origin.To( p_point ) );
    }

    /// <summary>
    /// Returns vector inside a sphere with given offset to the center
    /// </summary>
    /// <param name="p_radius">Maximum distance to the origin</param>
    /// <param name="p_offset">Minimum distance to the origin</param>
    public static Vector3 RandomSpherePoint( float p_radius, float p_offset )
    {
        Vector3 _randomSpherePoint = Random.insideUnitSphere;
        return ( _randomSpherePoint.normalized * p_offset ) + ( _randomSpherePoint * ( p_radius - p_offset ) );
    }

    /// <summary>
    /// Returns vector inside a circle with given offset to the center
    /// </summary>
    /// <param name="p_radius">Maximum distance to the origin</param>
    /// <param name="p_offset">Minimum distance to the origin</param>
    public static Vector2 RandomCirclePoint( float p_radius, float p_offset )
    {
        Vector2 _randomCirclePos = Random.insideUnitCircle;
        return ( _randomCirclePos.normalized * p_offset ) + ( _randomCirclePos * ( p_radius - p_offset ) );
    }

    /// <summary>
    /// Returns a random point inside an arc within given max angle and a minimum and maximum distance
    /// </summary>
    /// <param name="p_direction">The original direction</param>
    /// <param name="p_maxDeltaAngle">The maximum delta angle</param>
    /// <param name="p_minRange">The minimum range</param>
    /// <param name="p_maxRange">The maxmium range</param>
    public static Vector3 RandomArcPoint( Vector3 p_direction, float p_maxDeltaAngle, float p_minRange, float p_maxRange )
    {
        return RandomDirectionArc( p_direction, p_maxDeltaAngle ).normalized * Random.Range( p_minRange, p_maxRange );
    }

    public static Vector3 RandomArcPoint( Vector3 p_direction, float p_maxDeltaAngle, float p_angleOffset, float p_minRange, float p_maxRange )
    {
        return RandomDirectionArc( p_direction, p_maxDeltaAngle, p_angleOffset ).normalized * Random.Range( p_minRange, p_maxRange );
    }

    /// <summary>
    /// Calculates a random flee point in range with given offset and forward vector
    /// </summary>
    /// <param name="p_radius">The radius to search in</param>
    /// <param name="p_offset">A minimum offset of the resulting point to the origin</param>
    /// <param name="forward">The forward vector what the flee point should face</param>
    /// <param name="maxAngle">The maximum angle to serach a flee point in</param>
    /// <returns>Returns a new flee point</returns>
    public static Vector3 RandomFleePoint( float p_radius, float p_offset, Vector3 forward, float maxAngle )
    {
        Quaternion rotation = Quaternion.LookRotation( forward, Vector3.up );

        float _deltaAngle = Random.Range( -maxAngle, maxAngle );

        return ( ( rotation * Quaternion.AngleAxis( _deltaAngle, Vector3.up ) ) * Vector3.forward ) * Mathf.Lerp( p_offset, p_radius, Random.value );

    }

    /// <summary>
    /// Calculates a random direction vector inside an arc
    /// </summary>
    /// <param name="p_direction">The original direction</param>
    /// <param name="p_maxDeltaAngle">The maximum delta angle</param>
    /// <returns>a random direction vector inside an arc</returns>
    public static Vector3 RandomDirectionArc( Vector3 p_direction, float p_maxDeltaAngle )
    {
        return Quaternion.AngleAxis( Random.Range( -p_maxDeltaAngle, p_maxDeltaAngle ), Vector3.up ) * p_direction;
    }

    /// <summary>
    /// Calculates a random direction vector inside an arc
    /// </summary>
    /// <param name="p_direction">The original direction</param>
    /// <param name="p_maxDeltaAngle">The maximum delta angle</param>
    /// <returns>a random direction vector inside an arc</returns>
    public static Vector3 RandomDirectionArc( Vector3 p_direction, float p_maxDeltaAngle, float p_angleOffset )
    {
        return Quaternion.AngleAxis( Randomx.sign * Random.Range( p_angleOffset, p_maxDeltaAngle ), Vector3.up ) * p_direction;
    }

    /// <summary>
    /// Returns a random point inside a sphere with radius equal to 1
    /// </summary>
    public static Vector3 insideUnitSphere
    {
        get
        {
            Quaternion _rotation = Quaternion.AngleAxis( Random.Range( 0, 360 ), Vector3.right );
            return _rotation * insideUnitCircle;
        }
    }
    /// <summary>
    /// Returns a random point inside a circle with radius equal to 1
    /// </summary>
    public static Vector2 insideUnitCircle
    {
        get
        {
            float _radiant = Random.Range( 0, 2 * Mathf.PI );
            float _radius = Random.value;
            return new Vector2( Mathf.Cos( _radiant ), Mathf.Sin( _radiant ) ) * _radius;
        }
    }
    /// <summary>
    /// Returns a random point inside a circle with radius equal to 1 and a given random Index for network sync
    /// </summary>
    /// <param name="randIndex">The index inside the random array</param>
    /// <seealso cref="LKRandom"/>
    public static Vector2 InsideUnitCircle( int randIndex )
    {
        float _radiant = Random.Range( randIndex, 2 * Mathf.PI );
        float _radius = Random.value;
        return new Vector2( Mathf.Cos( _radiant ), Mathf.Sin( _radiant ) ) * _radius;
    }

    /// <summary>
    /// Calculates a 3D point of a circle lying on the XZ-axis plane
    /// </summary>
    /// <param name="p_radiant">The radiant of the angle</param>
    public static Vector3 CirclePos3D( float p_radiant )
    {
        return new Vector3( Mathf.Cos( p_radiant ), 0.0f, Mathf.Sin( p_radiant ) ).normalized;
    }

    /// <summary>
    /// Clamps the vector to have a minimum magnitude 
    /// </summary>
    /// <param name="p_vector">The vector that should be clamped</param>
    /// <param name="p_minMagnitude">The minimum magnitude the vector should have</param>
    /// <returns>Returns the vector with a minimum length of p_minMagnitude</returns>
    public static Vector3 ClampMagnitude( Vector3 p_vector, float p_minMagnitude )
    {
        return p_vector.magnitude >= p_minMagnitude ? p_vector : p_vector.normalized * p_minMagnitude;
    }

    /// <summary>
    /// Clamps the vector to have a minimum magnitude of 1
    /// </summary>
    /// <param name="p_vector">The vector that should be clamped</param>
    /// <returns>Returns the vector with a minimum length of p_minMagnitude</returns>
    public static Vector3 ClampMagnitudeNormalized( Vector3 p_vector )
    {
        return p_vector.sqrMagnitude >= 1.0f ? p_vector : p_vector.normalized;
    }

    /// <summary>
    /// Rolls a certain amount of dice with given sides and returns the sum of all throws
    /// </summary>
    /// <param name="p_amount">The amount of dice rolled</param>
    /// <param name="p_sides">Sides per die</param>
    /// <returns>Returns the sum of all throws</returns>
    public static int RollDices( int p_amount, int p_sides )
    {
        int result = 0;
        for ( int i = 0; i < p_amount; i++ )
        {
            result += Random.Range( 1, p_sides + 1 );
        }

        return result;
    }

    /// <summary>
    /// Calculates the square distance between two points in 3D space
    /// </summary>
    public static float SqrDistance( Vector3 a, Vector3 b )
    {
        return ( a - b ).sqrMagnitude;
    }

    /// <summary>
    /// Calculates the square distance between two points in 2D space
    /// </summary>
    public static float SqrDistance( Vector2 a, Vector2 b )
    {
        return ( a - b ).sqrMagnitude;
    }

    /// <summary>
    /// Indicates two points are closer or equal to each other than a certain maximum distance
    /// </summary>
    /// <param name="p_maxDistance">The maximum distance the two points should have to each other</param>
    /// <returns>Return true if closer or equal</returns>
    public static bool DistanceCheck( Vector3 a, Vector3 b, float p_maxDistance )
    {
        return SqrDistance( a, b ) <= p_maxDistance * p_maxDistance;
    }
    /// <summary>
    /// Indicates two points are closer or equal to each other than a certain maximum distance
    /// </summary>
    /// <param name="p_maxDistance">The maximum distance the two points should have to each other</param>
    /// <returns>Return true if closer or equal</returns>
    public static bool DistanceCheck( Vector2 a, Vector2 b, float p_maxDistance )
    {
        return SqrDistance( a, b ) <= p_maxDistance * p_maxDistance;
    }

    /// <summary>
    /// Indicates two points are closer or equal to each other than a certain maximum distance and at least a certain minimum distance apart
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="p_minDistance">The minimum distance the two points should have to each other</param>
    /// <param name="p_maxDistance">The maximum distance the two points should have to each other</param>
    /// <returns></returns>
    public static bool DistanceCheck( Vector3 a, Vector3 b, float p_minDistance, float p_maxDistance )
    {
        return InRange( SqrDistance( a, b ), p_minDistance * p_minDistance, p_maxDistance * p_maxDistance );
    }

    /// <summary>
    /// Indicates two points are closer or equal to each other than a certain maximum distance and at least a certain minimum distance apart
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="p_minDistance">The minimum distance the two points should have to each other</param>
    /// <param name="p_maxDistance">The maximum distance the two points should have to each other</param>
    /// <returns></returns>
    public static bool DistanceCheck( Vector2 a, Vector2 b, float p_minDistance, float p_maxDistance )
    {
        return InRange( SqrDistance( a, b ), p_minDistance * p_minDistance, p_maxDistance * p_maxDistance );
    }

    /// <summary>
    /// Indicates if the angle between two given vectors is below a certain threshold
    /// </summary>
    /// <param name="a">The first vector</param>
    /// <param name="b">The second vector</param>
    /// <param name="p_maxAngle">The maximum allowed angle between a and b</param>
    public static bool AngleCheck( Vector3 a, Vector3 b, float p_maxAngle )
    {
        return Vector3.Angle( a, b ) < p_maxAngle;
    }

    /// <summary>
    /// Returns the distance of a point to a line
    /// </summary>
    /// <param name="p_lineStart">The start point of the line</param>
    /// <param name="p_lineEnd">The end point of the line</param>
    /// <param name="p_point">The point that should be checked</param>
    public static float DistanceToLine( Vector3 p_lineStart, Vector3 p_lineEnd, Vector3 p_point )
    {
        return Vector3.Distance( ProjectToLine( p_lineStart, p_lineEnd, p_point ), p_point );
    }
    /// <summary>
    /// Returns the squared distance of a point to a line
    /// </summary>
    /// <param name="p_lineStart">The start point of the line</param>
    /// <param name="p_lineEnd">The end point of the line</param>
    /// <param name="p_point">The point that should be checked</param>
    public static float SqrDistanceToLine( Vector3 p_lineStart, Vector3 p_lineEnd, Vector3 p_point )
    {
        return SqrDistance( ProjectToLine( p_lineStart, p_lineEnd, p_point ), p_point );
    }

    /// <summary>
    /// Returns the closest distance to the given ray from a point.
    /// </summary>
    /// <param name="ray">The ray.</param>
    /// <param name="point">The point to check distance from the ray.</param>
    /// <returns>Distance from the point to the closest point of the ray.</returns>
    public static float DistanceToRay( Ray ray, Vector3 point )
    {
        return Vector3.Cross( ray.direction, point - ray.origin ).magnitude;
    }
    /// <summary>
    /// Returns the closest square distance to the given ray from a point
    /// </summary>
    /// <param name="ray">The ray to do calculation on</param>
    /// <param name="point">The point to check distance from the ray</param>
    /// <returns>Returns square distance from the point to the closet point of the ray.</returns>
    public static float SqrDistanceToRay( Ray ray, Vector3 point )
    {
        return Vector3.Cross( ray.direction, point - ray.origin ).sqrMagnitude;
    }

    /// <summary>
    /// Returns a value that represents the height of a projectile at an arc
    /// </summary>
    /// <param name="x">The distance travelled by the projectile</param>
    /// <param name="d">The maximum distance on which the projectile hits the ground</param>
    /// <param name="alpha">The angle in radiant at the beginning of the shot</param>
    public static float ProjectileArc( float x, float d, float alpha )
    {
        return alpha == 0 ? 0 : -x * ( x - d ) * ( Mathf.Tan( alpha ) / d );
    }

    public static Quaternion ConeRotation( Quaternion rotation, float degreeX, float degreeY )
    {
        return ConeRotationInternal( rotation, degreeX, degreeY, Random.insideUnitCircle );
    }
    private static Quaternion ConeRotationInternal( Quaternion rotation, float degreeX, float degreeY, Vector2 circlePos )
    {
        Quaternion _leftRightRot = Quaternion.AngleAxis( degreeX * circlePos.x, Vector3.up );
        Quaternion _upDownRot = Quaternion.AngleAxis( degreeY * circlePos.y, Vector3.right );

        return rotation * _leftRightRot * _upDownRot;
    }

    /// <summary>
    /// Clamps a rotation to be inside a given maximum angle from its reference rotation
    /// </summary>
    /// <param name="p_value">The rotation that should not exceed the max angle</param>
    /// <param name="p_refRot">The referenced rotation to use as "rotation origin"</param>
    /// <param name="p_axis">The axis to rotate around</param>
    /// <param name="p_maxAngle">THe max angle that should not be exceeded</param>
    /// <returns>Returns either the original or clamped value of <see cref="p_value"/></returns>
    public static Quaternion ClampRotation( Quaternion p_value, Quaternion p_refRot, Vector3 p_axis, float p_maxAngle )
    {
        float _angle = Quaternion.Angle( p_value, p_refRot );

        Quaternion _result = p_value;

        if ( _angle > p_maxAngle )
        {
            float _dot = Vector3.Dot( p_value * Vector3.forward, ( p_refRot * Quaternion.AngleAxis( 90.0f, p_axis ) ) * Vector3.forward );
            _result = p_refRot * Quaternion.AngleAxis( Mathf.Sign( _dot ) * p_maxAngle, p_axis );
        }

        return _result;
    }

    public static bool IsColinear( Vector3 a, Vector3 b, Vector3 c )
    {
        float v = ( b.x - a.x ) * ( c.z - a.z ) - ( c.x - a.x ) * ( b.z - a.z );
        return v <= 0.000001f && v >= -0.000001f;
    }

    public static Vector3 AdjustToPlane( Vector3 p_vector, Vector3 p_normal )
    {
        if ( p_vector == Vector3.zero || p_normal == Vector3.up )
            return p_vector;

        Vector3 horTangent = Vector3.Cross( p_normal, Vector3.up ).normalized;
        Vector3 verTangent = Vector3.Cross( p_normal, horTangent ).normalized;

        // horizontal
        Vector3 _horPart = Vector3.Dot( p_vector, horTangent ) * horTangent;
        Vector3 _verPart = p_vector - _horPart;

        Vector3 _heightAdjust = Vector3.up * ( Mathf.Tan( Mathf.Acos( p_normal.y ) ) ) * _verPart.magnitude;
        _heightAdjust *= Mathf.Sign( Vector3.Dot( -verTangent, _verPart ) );

        return ( _horPart + _verPart + _heightAdjust ).normalized * p_vector.magnitude;
    }
    #endregion

    #region Mathfx Interpolation functions
    public static float Hermite( float start, float end, float value )
    {
        return Mathf.Lerp( start, end, value * value * ( 3.0f - 2.0f * value ) );
    }

    /// <summary>
    /// Ease out to the end
    /// </summary>
    public static float Sinerp( float start, float end, float value )
    {
        return Mathf.Lerp( start, end, Mathf.Sin( value * Mathf.PI * 0.5f ) );
    }

    /// <summary>
    /// Ease in to the end
    /// </summary>
    public static float Coserp( float start, float end, float value )
    {
        return Mathf.Lerp( start, end, 1.0f - Mathf.Cos( value * Mathf.PI * 0.5f ) );
    }

    public static float Berp( float start, float end, float value )
    {
        value = Mathf.Clamp01( value );
        value = ( Mathf.Sin( value * Mathf.PI * ( 0.2f + 2.5f * value * value * value ) ) * Mathf.Pow( 1f - value, 2.2f ) + value ) * ( 1f + ( 1.2f * ( 1f - value ) ) );
        return start + ( end - start ) * value;
    }

    public static float SmoothStep( float start, float end, float value )
    {
        value = Mathf.Clamp( value, start, end );
        float v1 = ( value - start ) / ( end - start );
        float v2 = ( value - start ) / ( end - start );
        return -2 * v1 * v1 * v1 + 3 * v2 * v2;
    }

    public static float Lerp( float start, float end, float value )
    {
        return ( ( 1.0f - value ) * start ) + ( value * end );
    }

    /// <summary>
    /// Projects a vector to a line. The result will be at the point on the line with the smallest distance to the given point
    /// </summary>
    /// <param name="p_lineStart">The start point of the line</param>
    /// <param name="p_lineEnd">The end point of the line</param>
    /// <param name="p_point">The point that should be projected to the line</param>
    public static Vector3 ProjectToLine( Vector3 p_lineStart, Vector3 p_lineEnd, Vector3 p_point )
    {
        Vector3 _fullDir = p_lineEnd - p_lineStart;
        Vector3 _lineDir = _fullDir.normalized;
        float _closestPoint = Mathf.Clamp( Vector3.Dot( ( p_point - p_lineStart ), _lineDir ), 0.0f, _fullDir.magnitude );
        return p_lineStart + ( _closestPoint * _lineDir );
    }

    public static float Bounce( float x )
    {
        return Mathf.Abs( Mathf.Sin( 6.28f * ( x + 1f ) * ( x + 1f ) ) * ( 1f - x ) );
    }

    /*
      * CLerp - Circular Lerp - is like lerp but handles the wraparound from 0 to 360.
      * This is useful when interpolating eulerAngles and the object
      * crosses the 0/360 boundary.  The standard Lerp function causes the object
      * to rotate in the wrong direction and looks stupid. Clerp fixes that.
      */
    public static float Clerp( float start, float end, float value )
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs( ( max - min ) / 2.0f );//half the distance between min and max
        float retval = 0.0f;
        float diff = 0.0f;

        if ( ( end - start ) < -half )
        {
            diff = ( ( max - start ) + end ) * value;
            retval = start + diff;
        }
        else if ( ( end - start ) > half )
        {
            diff = -( ( max - end ) + start ) * value;
            retval = start + diff;
        }
        else retval = start + ( end - start ) * value;

        // Debug.Log("Start: "  + start + "   End: " + end + "  Value: " + value + "  Half: " + half + "  Diff: " + diff + "  Retval: " + retval);
        return retval;
    }

    public static Vector3 Interpolate( Vector3 start, Vector3 end, float value, InterpolationMethod p_Interpolation )
    {
        if ( p_Interpolation != null )
            return new Vector3( p_Interpolation( start.x, end.x, value ),
                                p_Interpolation( start.y, end.y, value ),
                                p_Interpolation( start.z, end.z, value ) );
        else
            return Vector3.Lerp( start, end, value );
    }

    #endregion

    public static int Fibonnacci( int index )
    {
        if ( index <= 2 )
            return 1;

        return Fibonnacci( index - 1 ) + Fibonnacci( index - 2 );
    }

    public static int GetMax<T>( T[] p_items, Func<T, int> CompareFunction )
    {
        int _result = 0;
        for ( int i = 0; i < p_items.Length; i++ )
            _result = Mathf.Max( _result, CompareFunction( p_items[ i ] ) );

        return _result;
    }

    /// <summary>
    /// Returns the next available ID for a set of items with given ID return function
    /// </summary>
    /// <typeparam name="T">The type of the set (has to be a class)</typeparam>
    /// <param name="p_items">The set of item</param>
    /// <param name="predicate">Function that returns an ID of an item</param>
    public static int NextAvailableID<T>( IEnumerable<T> p_items, Func<T, int> predicate ) where T : class
    {
        return NextAvailableID( p_items, predicate, 0, null );
    }

    /// <summary>
    /// Returns the next available ID for a set of items with given ID return function
    /// </summary>
    /// <typeparam name="T">The type of the set (has to be a class)</typeparam>
    /// <param name="p_items">The set of item</param>
    /// <param name="predicate">Function that returns an ID of an item</param>
    public static int NextAvailableID<T>( IEnumerable<T> p_items, Func<T, int> predicate, int startValue ) where T : class
    {
        return NextAvailableID( p_items, predicate, startValue, null );
    }

    /// <summary>
    /// Returns the next available ID for a set of items with given ID return function
    /// </summary>
    /// <typeparam name="T">The type of the set (has to be a class)</typeparam>
    /// <param name="p_items">The set of item</param>
    /// <param name="predicate">Function that returns an ID of an item</param>
    public static int NextAvailableID<T>( IEnumerable<T> p_items, Func<T, int> predicate, T ignoreItem ) where T : class
    {
        return NextAvailableID( p_items, predicate, 0, ignoreItem );
    }

    /// <summary>
    /// Returns the next available ID for a set of items with given ID return function
    /// </summary>
    /// <typeparam name="T">The type of the set (has to be a class)</typeparam>
    /// <param name="p_items">The set of item</param>
    /// <param name="predicate">Function that returns an ID of an item</param>
    /// 
    public static int NextAvailableID<T>( IEnumerable<T> p_items, Func<T, int> predicate, int startValue, T ignoreItem ) where T : class
    {
        int _tmp = startValue;

        while ( p_items.FirstOrDefault( item => item != ignoreItem && predicate( item ) == _tmp ) != default( T ) )
            _tmp++;

        return _tmp;
    }

    //public static U RandomEnumEntry<T, U>( IEnumerable<T> p_items, Func<T, U> predicate, U ignoreValue ) where U : struct, IConvertible, IFormattable, IComparable
    //{
    //    HashSet<U> _test = new HashSet<U>( EnumUtility.GetValues<U>() );

    //    foreach ( T item in p_items )
    //        _test.Remove( predicate( item ) );

    //    _test.Remove( ignoreValue );

    //    return _test.RandomItem();
    //}

    public static int CalcGUIWidth( string p_string )
    {
        return (int)GUI.skin.label.CalcSize( new GUIContent( p_string ) ).x;
    }
}

//public enum InterpolationType
//{
//    /// <summary>
//    /// Basic interpolation. No easy in or easy out
//    /// </summary>
//    Lerp,
//    /// <summary>
//    /// Ease out to the end
//    /// </summary>
//    Sinerp,
//    /// <summary>
//    /// Ease in when close to 0
//    /// </summary>
//    Coserp,
//    /// <summary>
//    /// Ease in and ease out at the limits
//    /// </summary>
//    Hermite
//}
