using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

using Random = UnityEngine.Random;
using System.Reflection;
using System.Globalization;

public enum RotationConstrains
{
    None,
    IgnoreY
}

[Flags]
public enum DebugLevel
{
    Everything = -1,
    None = 0,
    Logs = 1 << 1,
    Warnings = 1 << 2,
    Error = 1 << 3
}

public static partial class ExtensionMethods
{
    #region Methods
    #region C# Extensions
    public static bool Contains( this string p_string, string p_value, bool ignoreCase )
    {
        if ( string.IsNullOrEmpty( p_value ) )
            return true;

        if ( !ignoreCase )
            return p_string.Contains( p_value );

        return p_string.ToLower().Contains( p_value.ToLower() );
    }
    public static string ToString0000( this int p_number )
    {
        int _abs = Mathf.Abs( p_number );
        if ( _abs >= 1000 )
            return p_number.ToString();
        else if ( _abs >= 100 )
            return "0" + p_number;
        else if ( _abs >= 10 )
            return "00" + p_number;
        else
            return "000" + p_number;
    }

    public static string ToString000( this int p_number )
    {
        int _abs = Mathf.Abs( p_number );
        if ( _abs >= 100 )
            return p_number.ToString();
        else if ( _abs >= 10 )
            return "0" + p_number;
        else
            return "00" + p_number;
    }
    /// <summary>
    /// Returns the reziprocal 1/x of the number
    /// </summary>
    public static float Reziprocal( this float value ) { return 1.0f / value; }
    /// <summary>
    /// Returns true if the given float value only differs only a certain amount from a certain value. This can be used to compare floats without float imprecision issues
    /// </summary>
    /// <param name="value">The value that should be compared</param>
    /// <param name="other">The value that the given value should be compared to</param>
    /// <param name="p_threshold">The allowed threshold that both values are considered equal</param>
    public static bool RoughlyEqual( this float value, float other, float p_threshold )
    {
        return Mathf.Abs( value - other ) < p_threshold;
    }

    ///<summary> 
    /// Returns the last entry of the array
    /// </summary>
    /// <exception cref="InvalidOperationException">The array has length 0</exception>
    public static T Last<T>( this T[] array )
    {
        if ( array.Length == 0 )
            throw new InvalidOperationException( "You are trying to retrieve the last element of the array, but the array is empty" );

        return array[ array.Length - 1 ];
    }

    /// <summary>The square of the value</summary>
    public static float Sqr( this float value ) { return value * value; }
    public static float Cubed( this float value ) { return value * value * value; }
    public static float ToEuler( this float p_value ) { return p_value < -360.0f ? ToEuler( p_value + 360 ) : ( p_value + 360 ) % 360.0f; }
    public static bool Ranges( this float value, float min, float max ) { return value >= min && value <= max; }
    public static bool Ranges( this int value, int min, int max ) { return value >= min && value <= max; }
    /// <summary>Clamps the value to a minimum of min</summary>
    public static float Min( this float value, float min ) { return Mathf.Max( value, min ); }
    /// <summary>Clamps the value to a minimum of min</summary>
    public static int Min( this int value, int min ) { return Mathf.Max( value, min ); }
    /// <summary>Clamps the value to a maximum of max</summary>
    public static float Max( this float value, float max ) { return Mathf.Min( value, max ); }
    /// <summary>Clamps the value to a maximum of max</summary>
    public static int Max( this int value, int max ) { return Mathf.Min( value, max ); }
    /// <summary>Clamps the value between min and max</summary>
    public static float Clamp( this float value, float min, float max ) { return Mathf.Clamp( value, min, max ); }
    /// <summary>Clamps the value between min and max</summary>
    public static float Clamp( this int value, int min, int max ) { return Mathf.Clamp( value, min, max ); }
    public static string ToReadableString( this int p_number )
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ",";
        nfi.NumberGroupSeparator = ".";

        if ( Math.Abs( p_number ) >= 1000000000 )
            return string.Format( "{0}M", ( p_number / 1000000 ).ToString( "#,##0", nfi ) );

        if ( Mathf.Abs( p_number ) >= 100000 )
            return string.Format( "{0}K", ( p_number / 1000 ).ToString( "#,##0", nfi ) );

        return p_number.ToString( "#,##0", nfi );
    }
    public static int LowestBitSetSlow( this int x )
    {
        if ( x == 0 )
            return 0;

        int _result = 1;
        while ( ( x & 1 ) == 0 )
        {
            x >>= 1;
            _result++;
        }

        return _result;
    }
    public static int HighestBitSetSlow( this int x )
    {
        if ( x == 0 )
            return 0;

        int _result = 0;
        while ( ( x & ( 1 << 31 ) ) == 0 )
        {
            x <<= 1;
            _result++;
        }

        return 32 - _result;
    }
    #endregion
    #region Collection releated
    /// <summary>Finds the index of the first item matching an expression in an enumerable.</summary>
    /// <param name="items">The enumerable to search.</param>
    /// <param name="predicate">The expression to test the items against.</param>
    /// <returns>The index of the first matching item, or -1 if no items match.</returns>
    public static int FindIndex<T>( this IEnumerable<T> items, Func<T, bool> predicate )
    {
        if ( items == null ) throw new ArgumentNullException( "items" );
        if ( predicate == null ) throw new ArgumentNullException( "predicate" );

        int retVal = 0;
        foreach ( var item in items )
        {
            if ( predicate( item ) ) return retVal;
            retVal++;
        }
        return -1;
    }
    ///<summary>Finds the index of the first occurence of an item in an enumerable.</summary>
    ///<param name="items">The enumerable to search.</param>
    ///<param name="item">The item to find.</param>
    ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
    public static int IndexOf<T>( this IEnumerable<T> items, T item ) { return items.FindIndex( i => EqualityComparer<T>.Default.Equals( item, i ) ); }
    public static bool IsEmpty<T>( this IEnumerable<T> items )
    {
        return items.Count() == 0;
    }
    public static void Shuffle<T>( this IList<T> list )
    {
        int n = list.Count;
        while ( n > 1 )
        {
            n--;
            int k = Random.Range( 0, n + 1 );
            T value = list[ k ];
            list[ k ] = list[ n ];
            list[ n ] = value;
        }
    }
    public static bool IsEmpty<T>( this IList<T> list )
    {
        return list.Count == 0;
    }
    public static List<T> Copy<T>( this IList<T> list )
    {
        List<T> _result = new List<T>( list.Count );
        _result.AddRange( list );

        return _result;
    }
    /// <summary>
    /// Returns the object with the maximum value of a certain <see cref="IComparable"/> value
    /// </summary>
    /// <typeparam name="T">The type of the maximum object</typeparam>
    /// <param name="items">The list of items that should be investigated</param>
    /// <param name="predicate">The predicate that returns the value that should be compared to determine the maximum</param>
    //public static T Max<T>( this IEnumerable<T> items, Func<T, IComparable> predicate )
    //{
    //    return items.Aggregate( ( i1, i2 ) => predicate( i1 ).CompareTo( predicate( i2 ) ) > 0 ? i1 : i2 );
    //}
    //public static T Max<T>( this IEnumerable<T> items, Func<T, IComparable> predicate )
    //{
    //    T _currentMax = default( T );
    //    foreach ( T item in items )
    //    {
    //        if ( ReferenceEquals( _currentMax, null ) || predicate( item ).CompareTo( predicate( _currentMax ) ) > 0 )
    //            _currentMax = item;
    //    }

    //    return _currentMax;
    //}
    /// <summary>
    /// Returns the object with the minimum value of a certain <see cref="IComparable"/> value
    /// </summary>
    /// <typeparam name="T">The type of the minimum object</typeparam>
    /// <param name="items">The list of items that should be investigated</param>
    /// <param name="predicate">The predicate that returns the value that should be compared to determine the minimum</param>
    public static T Min<T>( this IEnumerable<T> items, Func<T, IComparable> predicate )
    {
        return items.Aggregate( ( i1, i2 ) => predicate( i1 ).CompareTo( predicate( i2 ) ) < 0 ? i1 : i2 );
    }
    /// <summary>
    /// Returns the object with the maximum value of a certain <see cref="IComparable"/> value
    /// </summary>
    /// <typeparam name="T">The type of the maximum object</typeparam>
    /// <param name="items">The list of items that should be investigated</param>
    /// <param name="predicate">The predicate that returns the value that should be compared to determine the maximum</param>
    public static T Max<T>( this IEnumerable<T> items, Func<T, IComparable> predicate )
    {
        return items.Aggregate( ( i1, i2 ) => predicate( i1 ).CompareTo( predicate( i2 ) ) > 0 ? i1 : i2 );
    }

    public static T RandomItem<T>( this List<T> list )
    {
        int _count = list.Count;
        if ( _count > 0 )
            return list[ Random.Range( 0, _count ) ];
        else
            return default( T );
    }
    public static T RandomItem<T>( this List<T> list, Func<T, bool> predicate )
    {
        var _filteredList = list.Where( predicate );
        if ( _filteredList.Count() == 0 )
            return default( T );

        return _filteredList.RandomItem();
    }

    public static T RandomItem<T>( this T[] array )
    {
        return array[ Random.Range( 0, array.Length ) ];
    }

    public static T RandomItem<T>( this IEnumerable<T> collection )
    {
        int _count = collection.Count();
        if ( _count > 0 )
            return collection.ElementAt( Random.Range( 0, _count ) );
        else
            return default( T );
    }
    public static T RandomItem<T>( this IEnumerable<T> collection, Func<T, bool> predicate )
    {
        var _filteredCollection = collection.Where( predicate );
        if ( _filteredCollection.Count() == 0 )
            return default( T );

        return _filteredCollection.RandomItem();
    }
    #endregion
    #region Unity Extensions

    /// <summary>
    /// Returns a random value between 0 and its value
    /// </summary>
    public static float Rand( this float p_value )
    {
        return Random.Range( 0.0f, p_value );
    }
    public static void EnableComponents( this GameObject gameObject, bool enabled, params Type[] types )
    {
        Component c;
        foreach ( System.Type type in types )
        {
            c = gameObject.GetComponent( type );
            if ( c != null )
            {
                var infos = c.GetType().GetProperties().Where( i => i.Name.Contains( "enable" ) );

                foreach ( var info in infos )
                    info.SetValue( c, enabled, null );
            }
        }
    }

    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponent\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on the object or its parent or children</returns>
    public static void AssignComponent<T>( this Component c, out T p_component, DebugLevel p_debugLevel = DebugLevel.Everything ) where T : Component
    {
        p_component = c.GetComponent<T>();
        if ( p_component == null )
            p_component = c.GetComponentInParent<T>();
        if ( p_component == null )
            p_component = c.GetComponentInChildren<T>();
    }

    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponent\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on the object or its parent or children</returns>
    public static void AssignComponentInParent<T>( this Component c, out T p_component, DebugLevel p_debugLevel = DebugLevel.Everything ) where T : Component
    {
        p_component = c.GetComponentInParent<T>();
    }
    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponent\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on the object or its parent or children</returns>
    public static void AssignComponentInChildren<T>( this Component c, out T p_component, DebugLevel p_debugLevel = DebugLevel.Everything ) where T : Component
    {
        p_component = c.GetComponentInChildren<T>();
    }


    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponent\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on the object or its parent or children</returns>
    public static bool TryAssignComponent<T>( this Component c, out T p_component, DebugLevel p_debugLevel = DebugLevel.Everything )
    {
        return TryAssignComponent( c, out p_component, true, p_debugLevel );
    }
    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponent\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_checkParentAndChildren">Indicates whether the parents and children should also be checked</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on the object or its parent or children</returns>
    public static bool TryAssignComponent<T>( this Component c, out T p_component, bool p_checkParentAndChildren, DebugLevel p_debugLevel = DebugLevel.Everything )
    {
        p_component = c.GetComponent<T>();
        if ( p_component != null )
            return true;

        if ( p_checkParentAndChildren )
        {
            p_component = c.GetComponentInParent<T>();
            if ( p_component != null )
                return true;

            p_component = c.GetComponentInChildren<T>();
            if ( p_component != null )
                return true;
        }

        return false;
    }
    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponentInParent\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on its parent</returns>
    public static bool TryAssignComponentInParent<T>( this Component c, out T p_component, DebugLevel p_debugLevel = DebugLevel.Everything )
    {
        p_component = c.GetComponentInParent<T>();
        if ( p_component != null )
            return true;

        return false;
    }
    /// <summary>Used to assign a component from the gameobject</summary>
    /// <remarks>Use this instead of <c>T item = GetComponentInChrildren\{T}();</c></remarks>
    /// <typeparam name="T">Type that inherites from Component</typeparam>
    /// <param name="c">The component the methods is executed on</param>
    /// <param name="p_component">The component that should be assigned</param>
    /// <param name="p_debugLevel">The debug level that should be logged to receive further infomation about unexpected behaviour</param>
    /// <returns>True if component could be found and assigned. False if no component was found on its children</returns>
    public static bool TryAssignComponentInChildren<T>( this Component c, out T p_component, DebugLevel p_debugLevel = DebugLevel.Everything )
    {
        p_component = c.GetComponentInChildren<T>();
        if ( p_component != null )
            return true;

        return false;
    }

    public static IEnumerable<Material> GetMaterialsInChildren( this Component c )
    {
        return GetMaterialsInChildren( c, false );
    }
    public static IEnumerable<Material> GetMaterialsInChildren( this Component c, bool p_ignoreParticleRenderer )
    {
        if ( p_ignoreParticleRenderer )
            return c.GetComponentsInChildren<Renderer>().Where( r => !( r is ParticleSystemRenderer ) ).SelectMany( r => r.materials );
        else
            return c.GetComponentsInChildren<Renderer>().SelectMany( r => r.materials );
    }

    public static void Reset( this Transform transform )
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    public static void Reset( this Transform transform, Transform target )
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
        transform.localScale = target.localScale;
    }
    public static void Reset( this Transform transform, Vector3 position, Quaternion rotation )
    {
        transform.position = position;
        transform.rotation = rotation;
    }
    public static void Reset( this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale )
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }

    /// <summary>
    /// Indicates whether this transform is a child of the given transform
    /// </summary>
    public static bool IsChildOf( this Transform transform, Transform target, bool p_includeSelf )
    {
        Transform _tmp = transform;
        while ( _tmp != target )
        {
            _tmp = _tmp.parent;

            if ( _tmp == null )
                return false;
        }

        return true;
    }

    /// <summary>Resets the position of the transform to its local origin</summary>
    public static void ResetPos( this Transform transform ) { transform.localPosition = Vector3.zero; }
    /// <summary>
    /// Get all children from the transform
    /// </summary>
    /// <param name="transform">the root transform</param>
    public static Transform[] GetChildren( this Transform transform )
    {
        Transform[] result = new Transform[ transform.childCount ];
        for ( int i = 0; i < result.Length; i++ )
        {
            result[ i ] = transform.GetChild( i );
        }
        return result;
    }

    public static void GetAllChildren( this Transform p_transform, ref List<Transform> p_children, bool p_iterateChildren )
    {
        for ( int i = 0; i < p_transform.childCount; i++ )
        {
            var _child = p_transform.GetChild( i );
            p_children.Add( _child );
            if ( p_iterateChildren )
                _child.GetAllChildren( ref p_children, true );
        }
    }
    /// <summary>
    /// Returns a certain child in the transform recursively
    /// </summary>
    /// <param name="p_name">The name of the child</param>
    public static Transform FindChildRecursive( this Transform p_transform, string p_name )
    {
        List<Transform> _list = new List<Transform>();

        p_transform.GetAllChildren( ref _list, true );

        for ( int i = 0; i < _list.Count; i++ )
        {
            if ( _list[ i ].name == p_name )
            {
                Transform _result = _list[ i ];
                return _result;
            }
        }
        return null;
    }

    public static Vector3 GetHeadPos( this Transform transform )
    {
        Renderer _r = transform.GetComponent<Renderer>();
        if ( _r == null )
            _r = transform.GetComponentInChildren<Renderer>();
        if ( _r == null )
            return transform.position;
        return transform.position + transform.up * _r.bounds.size.y * 0.5f;
    }

    /// <summary>
    /// Moves the transform in a straight line towards the target. Does not overshoot the target
    /// </summary>
    /// <param name="transform">The transform that should be moved</param>
    /// <param name="p_target">The target point of the movement</param>
    /// <param name="p_maxDistanceDelta">The maximum distance it is allowed to travel</param>
    public static void MoveTowards( this Transform transform, Vector3 p_target, float p_maxDistanceDelta )
    {
        transform.position = Vector3.MoveTowards( transform.position, p_target, p_maxDistanceDelta );
    }

    /// <summary>
    /// Moves the transform in the direction it is facing
    /// </summary>
    /// <param name="transform">The transform that should be moved</param>
    /// <param name="p_distance">The distance the transform should be moved</param>
    public static void MoveForward( this Transform transform, float p_distance )
    {
        transform.position += transform.forward * p_distance;
    }

    /// <summary>Unparents the current transform</summary>
    public static void Unparent( this Transform transform )
    {
        transform.SetParent( null );
    }
    /// <summary>
    /// Determines if a transform is parented
    /// </summary>
    /// <returns>Returns true if transform has a parent</returns>
    public static bool IsParented( this Transform transform ) { return transform.parent != null; }

    public static Ray ForwardRay( this Transform transform ) { return ForwardRay( transform, 0.0f ); }
    public static Ray ForwardRay( this Transform transform, float yOffset )
    {
        return new Ray( transform.position + Vector3.up * yOffset, transform.forward );
    }

    public static Vector3 XZ( this Vector3 v, int x, int z )
    {
        return new Vector3( x, v.y, z );
    }
    public static Vector3 XZ( this Vector3 v, Vector3 target )
    {
        return new Vector3( target.x, v.y, target.z );
    }
    /// <summary>
    /// Sets the x component of a vector and returns the result
    /// </summary>
    /// <param name="v">The vector manipulate</param>
    /// <param name="newX">The new x value</param>
    public static Vector3 X( this Vector3 v, float newX )
    {
        return new Vector3( newX, v.y, v.z );
    }
    /// <summary>
    /// Sets the y component of a vector and returns the result
    /// </summary>
    /// <param name="v">The vector manipulate</param>
    /// <param name="newY">The new y value</param>
    public static Vector3 Y( this Vector3 v, float newY )
    {
        return new Vector3( v.x, newY, v.z );
    }
    /// <summary>
    /// Sets the z component of a vector and returns the result
    /// </summary>
    /// <param name="v">The vector manipulate</param>
    /// <param name="newZ">The new z value</param>
    public static Vector3 Z( this Vector3 v, float newZ )
    {
        return new Vector3( v.x, v.y, newZ );
    }

    /// <summary>
    /// Returns a the vector with y = 0. If x and z = 0 a random direction on the XZ-plane is returned
    /// </summary>
    public static Vector3 CancelY( this Vector3 v )
    {
        Vector3 _result = v.Y( 0 );
        return _result != Vector3.zero ? _result : Random.insideUnitCircle.ToVec3().normalized;
    }
    /// <summary>
    /// Maps the x and z component of the <see cref="Vector3"/> to a <see cref="Vector2"/>
    /// </summary>
    public static Vector2 GetXZ( this Vector3 v )
    {
        return new Vector2( v.x, v.z );
    }/// <summary>
     /// Returns a vector that points from this vector to target
     /// </summary>
     /// <param name="v">The vector from</param>
     /// <param name="target">The vector to</param>
    public static Vector2 To( this Vector2 v, Vector2 target )
    {
        return target - v;
    }
    /// <summary>
    /// Returns a vector that points from this vector to target
    /// </summary>
    /// <param name="v">The vector from</param>
    /// <param name="target">The vector to</param>
    public static Vector3 To( this Vector3 v, Vector3 target )
    {
        return target - v;
    }
    ///<summary>
    /// Increases y value of the vector
    /// </summary>
    public static Vector3 Up( this Vector3 v, float p_value )
    {
        return v + Vector3.up * p_value;
    }
    public static Vector3 Right( this Vector3 v, float p_value )
    {
        return v + Vector3.right * p_value;
    }
    public static Vector3 Forward( this Vector3 v, float p_value )
    {
        return v + Vector3.forward * p_value;
    }

    /// <summary>
    /// Maps the vector2 to the 3D XZ-plane (x, y) => (x, 0, y)
    /// </summary>
    public static Vector3 ToVec3( this Vector2 v )
    {
        return new Vector3( v.x, 0, v.y );
    }

    /// <summary>
    /// Rotates a vector about a certain degree
    /// </summary>
    /// <param name="v">The vector that should be rotated</param>
    /// <param name="p_degrees">Rotation degrees in angle</param>
    public static Vector2 Rotate( this Vector2 v, float p_degrees )
    {
        float sin = Mathf.Sin( p_degrees * Mathf.Deg2Rad );
        float cos = Mathf.Cos( p_degrees * Mathf.Deg2Rad );
        float tx = v.x;
        float ty = v.y;
        return new Vector2( ( cos * tx ) - ( sin * ty ), ( sin * tx ) + ( cos * ty ) );
    }

    public static Color Solid( this Color c ) { return new Color( c.r, c.g, c.b, 1.0f ); }
    /// <summary>
    /// Returns the same color, but with a specified alpha value
    /// </summary>
    /// <param name="c">The color that should be manipulated</param>
    /// <param name="alpha">The new alpha value for this color</param>
    /// <returns>Returns the same color, but with a specified alpha value</returns>
    public static Color Fade( this Color c, float alpha )
    {
        return new Color( c.r, c.g, c.b, alpha );
    }

    /// <summary>
    /// Returns the hexadecimal value of the Color
    /// </summary>
    /// <param name="c">The Color</param>
    /// <returns>The Hex value of the color</returns>
    public static string ToHex( this Color c )
    {
        Color32 c32 = c;
        return c32.ToHex();
    }

    /// <summary>
    /// Returns the hexadecimal value of the Color
    /// </summary>
    /// <param name="c">The Color</param>
    /// <returns>The Hex value of the color</returns>
    public static string ToHex( this Color32 c )
    {
        string _hex = c.r.ToString( "X2" ) + c.g.ToString( "X2" ) + c.b.ToString( "X2" ) + c.a.ToString( "X2" );
        return _hex;
    }

    public static float GetPercentage( this Slider slider )
    {
        return slider.value / slider.maxValue;
    }

    public static void Set( this CapsuleCollider p_collider, CharacterController p_cc )
    {
        p_collider.height = p_cc.height;
        p_collider.radius = p_cc.radius;
        p_collider.center = p_cc.center;
    }

    public static Vector3 ToDirection( this Quaternion p_quaternion )
    {
        return p_quaternion * Vector3.forward;
    }


    private static void EnableEmissionInternal( GameObject go, bool enabled )
    {
        var particleSystem = go.GetComponent<ParticleSystem>();
        if ( particleSystem == null )
            return;

        var emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    public static float GetEmissionRate( this ParticleSystem particleSystem )
    {
        return particleSystem.emission.rate.constantMax;
    }

    public static void SetEmissionRate( this ParticleSystem particleSystem, float emissionRate )
    {
        var emission = particleSystem.emission;
        var rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }

    /// <summary>
    /// Updates a button's interactivity settings and modifies text color in subobjects
    /// </summary>
    /// <param name="p_button">The button to update</param>
    /// <param name="p_interactive">Whether button shall be interactive or not</param>
    public static void SetActive( this Button p_button, bool p_interactive )
    {
        if ( p_button )
        {
            p_button.interactable = p_interactive;
            Text _buttonText = p_button.GetComponentInChildren<Text>();
            if ( _buttonText )
                _buttonText.color = p_interactive ? Color.white : Color.grey;
        }
    }

    #endregion

    #region Interface Extensions

    #endregion

    #region Serialization related

    //! Returns a random item in the list
    public static byte[] ReadAllBytes( this BinaryReader reader )
    {
        const int bufferSize = 4096;
        using ( var ms = new MemoryStream() )
        {
            byte[] buffer = new byte[ bufferSize ];
            int count;
            while ( ( count = reader.Read( buffer, 0, buffer.Length ) ) != 0 )
                ms.Write( buffer, 0, count );
            return ms.ToArray();
        }
    }

    #endregion

    #region Layer related
    public static bool IsLayer( this GameObject gameObject, int layerMask ) { return ( layerMask & ( 1 << gameObject.layer ) ) != 0; }
    #endregion
    #endregion
}

public static class FlagsHelper
{
    public static bool IsSet<T>( T flags, T flag ) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        return ( flagsValue & flagValue ) != 0;
    }

    public static void Set<T>( ref T flags, T flag ) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)( flagsValue | flagValue );
    }

    public static void Unset<T>( ref T flags, T flag ) where T : struct
    {
        int flagsValue = (int)(object)flags;
        int flagValue = (int)(object)flag;

        flags = (T)(object)( flagsValue & ( ~flagValue ) );
    }
}