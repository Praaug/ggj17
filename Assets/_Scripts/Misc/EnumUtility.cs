using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*!
 *  Delete tags you wont need. Required tags: author, brief, date 
 *  \details   This class is used to demonstrate a number of section commands.
 *  \author    Thorsten Schmiedel
 *  \author    Last edited: never
 *  \date      26-11-2015
 *  \date      Last Edited: never
 */
/// <summary>
/// Utilityclass that provides some functionality for Enums
/// </summary>
public static class EnumUtility
{
    #region Variables
    /// <summary>
    /// Dictionary to store enum values in. It caches the values to increase performance
    /// </summary>
    private static Dictionary<Type, Array> m_enumValueDict = new Dictionary<Type, Array>();
    private static Dictionary<Type, string[]> m_enumNamesDict = new Dictionary<Type, string[]>();
    #endregion

    #region Methods
    /// <summary>
    /// Returns all names of a specific Enum
    /// </summary>
    /// <typeparam name="T">The Enum from which the values should be returned</typeparam>
    public static string[] GetNames<T>() where T : struct, IConvertible, IFormattable, IComparable
    {
        if ( !m_enumNamesDict.ContainsKey( typeof( T ) ) )
            m_enumNamesDict.Add( typeof( T ), Enum.GetNames( typeof( T ) ) );

        return m_enumNamesDict[ typeof( T ) ];
    }

    /// <summary>
    /// Returns all values of a specific Enum
    /// </summary>
    /// <typeparam name="T">The Enum from which the values should be returned</typeparam>
    public static T[] GetValues<T>() where T : struct, IConvertible, IFormattable, IComparable
    {
        if ( !m_enumValueDict.ContainsKey( typeof( T ) ) )
            m_enumValueDict.Add( typeof( T ), Enum.GetValues( typeof( T ) ) );

        return m_enumValueDict[ typeof( T ) ] as T[];
    }

    /// <summary>
    /// Parses a string to an enum value
    /// </summary>
    /// <typeparam name="T">The enum type of the resulting value</typeparam>
    /// <param name="enumValue">The string to parse to an enum value</param>
    public static T Parse<T>( string enumValue )
    {
        return (T)Enum.Parse( typeof( T ), enumValue );
    }
    #endregion
}
