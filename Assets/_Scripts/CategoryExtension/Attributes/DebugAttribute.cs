using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Fields or properties marked with this attribute will be displayed in the inspector at runtime
/// </summary>
[AttributeUsage( AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
public class DebugAttribute : Attribute
{

}
