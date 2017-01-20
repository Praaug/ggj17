using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[CustomEditor( typeof( MonoBehaviour ), true )]
[CanEditMultipleObjects]
public class MonoBehaviourEditor : Editor
{
    private const int CAT_NAME_HEIGHT = 18;
    private const int CONTENT_PIXEL_OFFSET = 2;
    private const int SORT_BUTTON_WIDTH = 16;

    private GUISkin m_skin;
    private Texture m_buttonUp;
    private Texture m_buttonDown;

    private float m_debugTimeStamp;

    private static Dictionary<System.Type, HashSet<EditorCategory>> s_categoryDict = new Dictionary<System.Type, HashSet<EditorCategory>>();
    private static HashSet<System.Type> s_ignoreList = new HashSet<System.Type>();
    private SortedList<int, KeyValuePair<EditorCategory, List<SerializedProperty>>> m_propertyList;
    private Dictionary<MethodInfo, ButtonAttribute> m_buttonDict = new Dictionary<MethodInfo, ButtonAttribute>();
    private List<MemberInfo> m_debugList = new List<MemberInfo>();

    protected virtual void OnEnable()
    {
        if ( m_skin == null )
            m_skin = Resources.Load<GUISkin>( EditorGUIUtility.isProSkin ? "EditorSkinDark" : "EditorSkinLight" );
        if ( m_buttonUp == null )
            m_buttonUp = Resources.Load<Texture>( "Button-up" );
        if ( m_buttonDown == null )
            m_buttonDown = Resources.Load<Texture>( "Button-down" );

        ExtractProperties();
        m_buttonDict = GetButtonMethods( serializedObject );

        if ( PrefabUtility.GetPrefabType( target ) == PrefabType.None )
            m_debugList = GetDebugFields( serializedObject );

        EditorApplication.update += Update;
    }

    public override void OnInspectorGUI()
    {
        if ( m_propertyList == null || s_ignoreList.Contains( target.GetType() ) )
        {
            base.OnInspectorGUI();
            return;
        }

        this.DrawScriptField();

        EditorGUILayout.BeginHorizontal();
        if ( GUILayout.Button( "Expand all", EditorStyles.miniButton ) )
        {
            foreach ( var _kvPair in m_propertyList.Values )
                _kvPair.Key.foldout = true;
        }
        if ( GUILayout.Button( "Collape all", EditorStyles.miniButton ) )
        {
            foreach ( var _kvPair in m_propertyList.Values )
                _kvPair.Key.foldout = false;
        }
        EditorGUILayout.EndHorizontal();


        for ( int i = 0; i < m_propertyList.Count; i++ )
        {
            KeyValuePair<EditorCategory, List<SerializedProperty>> _kvPair = m_propertyList[ i ];

            EditorCategory _cat = _kvPair.Key;
            List<SerializedProperty> _list = _kvPair.Value;

            float _height = CAT_NAME_HEIGHT;
            if ( _cat.foldout )
                foreach ( SerializedProperty _prop in _list )
                    _height += EditorGUI.GetPropertyHeight( _prop ) + CONTENT_PIXEL_OFFSET;
            Rect _rect = EditorGUILayout.GetControlRect( false, _height );

            DrawCategoryName( _cat, ref _rect );

            if ( _cat.foldout )
                DrawCategoryContents( _list, ref _rect );
            GUILayout.Space( 10 );
        }

        foreach ( var _kvPair in m_buttonDict )
        {
            MethodInfo _method = _kvPair.Key;
            ButtonAttribute _buttonAttribute = _kvPair.Value;

            if ( GUILayout.Button( _buttonAttribute.text ) )
            {
                _method.Invoke( serializedObject.targetObject, null );
                SceneView.RepaintAll();
            }
        }

        serializedObject.ApplyModifiedProperties();

        DrawDebugValues();
    }

    private void Update()
    {
        if ( m_debugList.Count == 0 )
            return;

        if ( EditorApplication.isPlaying && m_debugTimeStamp + 0.5f < Time.realtimeSinceStartup )
        {
            Repaint();
            m_debugTimeStamp = Time.realtimeSinceStartup;
        }
    }

    private void DrawDebugValues()
    {
        if ( EditorApplication.isPlaying && PrefabUtility.GetPrefabType( target ) == PrefabType.None && m_debugList.Count > 0 )
        {
            EditorGUILayout.LabelField( "Debug", EditorStyles.boldLabel );

            foreach ( MemberInfo _info in m_debugList )
            {
                object _value = null;
                if ( _info is FieldInfo )
                    _value = ( _info as FieldInfo ).GetValue( serializedObject.targetObject );
                else if ( _info is PropertyInfo )
                    _value = ( _info as PropertyInfo ).GetValue( serializedObject.targetObject, null );

                EditorGUILayout.LabelField( _info.Name, _value != null ? _value.ToString() : "null" );
            }

        }
    }

    private void DrawCategoryName( EditorCategory p_category, ref Rect p_rect )
    {
        Rect _lineRect = new Rect( p_rect.x, p_rect.y, p_rect.width, CAT_NAME_HEIGHT );

        Rect _foldoutRect = _lineRect;
        _foldoutRect.width -= SORT_BUTTON_WIDTH * 2;
        p_category.foldout = EditorGUI.Foldout( _foldoutRect, p_category.foldout, GUIContent.none );

        EditorGUI.LabelField( _lineRect, p_category.name, m_skin.FindStyle( "CategoryBG" ) );

        Rect _buttonRect = _lineRect;
        _buttonRect.x += _lineRect.width - SORT_BUTTON_WIDTH * 2;
        _buttonRect.width = SORT_BUTTON_WIDTH;

        if ( GUI.Button( _buttonRect, m_buttonUp, GUIStyle.none ) && p_category.order > 0 )
            SwapCategories( m_propertyList[ p_category.order ], m_propertyList[ p_category.order - 1 ] );

        _buttonRect.x += SORT_BUTTON_WIDTH;
        if ( GUI.Button( _buttonRect, m_buttonDown, GUIStyle.none ) && p_category.order < m_propertyList.Count - 1 )
            SwapCategories( m_propertyList[ p_category.order ], m_propertyList[ p_category.order + 1 ] );

        p_rect.y += CAT_NAME_HEIGHT;
        p_rect.height -= CAT_NAME_HEIGHT;
    }

    private void DrawCategoryContents( List<SerializedProperty> p_propertyList, ref Rect p_rect )
    {
        EditorGUI.LabelField( p_rect, "", m_skin.FindStyle( "ContentBG" ) );

        EditorGUI.indentLevel++;

        for ( int i = 0; i < p_propertyList.Count; i++ )
        {
            SerializedProperty _prop = p_propertyList[ i ];

            float _propHeight = EditorGUI.GetPropertyHeight( _prop );
            p_rect.height = _propHeight;

            if ( _prop.propertyType == SerializedPropertyType.ObjectReference && _prop.objectReferenceValue == null )
                EditorGUI.LabelField( p_rect, "", m_skin.FindStyle( "MissingReferenceBG" ) );

            EditorGUI.PropertyField( p_rect, _prop, true );

            p_rect.y += _propHeight + CONTENT_PIXEL_OFFSET;
        }

        EditorGUI.indentLevel--;
    }

    private void ExtractProperties()
    {
        System.Type _type = target.GetType();

        if ( System.Attribute.IsDefined( _type, typeof( StandardEditorAttribute ) ) )
        {
            if ( !s_ignoreList.Contains( _type ) )
                s_ignoreList.Add( _type );
            return;
        }

        m_propertyList = new SortedList<int, KeyValuePair<EditorCategory, List<SerializedProperty>>>();
        Dictionary<SerializedProperty, CategoryAttribute> _dict = GetSerializedProperties<CategoryAttribute>( serializedObject, typeof( ShowOnlyOnRuntimeAttribute ) );

        if ( _dict == null )
            return;

        // Create categories
        if ( !s_categoryDict.ContainsKey( _type ) )
        {
            HashSet<string> _nameSet = new HashSet<string>();
            string _tmpCatName;

            foreach ( var _kvPair in _dict )
            {
                _tmpCatName = _kvPair.Value != null ? _kvPair.Value.category : "Uncategorized";
                _nameSet.Add( _tmpCatName );
            }

            HashSet<EditorCategory> _categoryList = new HashSet<EditorCategory>();
            int _order = 0;
            foreach ( string _catName in _nameSet )
                _categoryList.Add( new EditorCategory( _catName, _order++ ) );

            s_categoryDict.Add( _type, _categoryList );
        }

        HashSet<EditorCategory> _catList = s_categoryDict[ _type ];
        foreach ( EditorCategory _category in _catList )
            m_propertyList.Add( _category.order, new KeyValuePair<EditorCategory, List<SerializedProperty>>( _category, new List<SerializedProperty>() ) );

        foreach ( var _kvPair in _dict )
        {
            string _categoryName = _kvPair.Value != null ? _kvPair.Value.category : "Uncategorized";
            m_propertyList.Values.First( kvPair => kvPair.Key.name == _categoryName ).Value.Add( _kvPair.Key );
        }
    }


    private HashSet<SerializedProperty> GetSerializedPropertyList<T>( SerializedObject p_object ) where T : System.Attribute
    {
        if ( p_object.targetObject == null )
            return null;

        HashSet<SerializedProperty> _result = new HashSet<SerializedProperty>();

        FieldInfo[] fields = p_object.targetObject.GetType().GetFields( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );

        foreach ( FieldInfo field in fields )
        {
            if ( ( field.Attributes & ( FieldAttributes.NotSerialized | FieldAttributes.Static ) ) != 0 )
                continue;

            if ( !field.IsPublic )
            {
                object[] serializable = field.GetCustomAttributes( typeof( SerializeField ), true );
                if ( serializable == null || serializable.Length <= 0 )
                    continue;
            }

            object[] hide = field.GetCustomAttributes( typeof( HideInInspector ), true );
            if ( hide != null && hide.Length > 0 )
                continue;

            SerializedProperty _prop = p_object.FindProperty( field.Name );
            if ( _prop == null )
                continue;

            object[] targetAttribute = field.GetCustomAttributes( typeof( T ), true );
            if ( targetAttribute != null && targetAttribute.Length > 0 )
                _result.Add( _prop );
        }

        return _result;
    }

    private void SwapCategories( KeyValuePair<EditorCategory, List<SerializedProperty>> p_kvPair1,
                                 KeyValuePair<EditorCategory, List<SerializedProperty>> p_kvPair2 )
    {
        m_propertyList[ p_kvPair1.Key.order ] = p_kvPair2;
        m_propertyList[ p_kvPair2.Key.order ] = p_kvPair1;

        int _tmpOrder = p_kvPair1.Key.order;
        p_kvPair1.Key.order = p_kvPair2.Key.order;
        p_kvPair2.Key.order = _tmpOrder;
    }

    private static Dictionary<SerializedProperty, T> GetSerializedProperties<T>( SerializedObject p_object ) where T : System.Attribute
    {
        return GetSerializedProperties<T>( p_object, null );
    }
    private static Dictionary<SerializedProperty, T> GetSerializedProperties<T>( SerializedObject p_object, System.Type ignoreAttributeType ) where T : System.Attribute
    {
        Dictionary<SerializedProperty, T> _result = new Dictionary<SerializedProperty, T>();

        if ( p_object.targetObject == null )
            return null;

        FieldInfo[] fields = GetAllFields( p_object.targetObject.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ).ToArray();
        foreach ( FieldInfo field in fields )
        {
            if ( ( field.Attributes & ( FieldAttributes.NotSerialized | FieldAttributes.Static ) ) != 0 )
                continue;

            if ( !field.IsPublic )
            {
                object[] serializable = field.GetCustomAttributes( typeof( SerializeField ), true );
                if ( serializable == null || serializable.Length <= 0 )
                    continue;
            }

            object[] hide = field.GetCustomAttributes( typeof( HideInInspector ), true );
            if ( hide != null && hide.Length > 0 )
                continue;

            if ( ignoreAttributeType != null )
            {
                object[] ignoreAttribute = field.GetCustomAttributes( ignoreAttributeType, true );
                if ( ignoreAttribute != null && ignoreAttribute.Length > 0 )
                    continue;
            }

            SerializedProperty _prop = p_object.FindProperty( field.Name );
            if ( _prop == null )
                continue;

            T[] targetAttribute = field.GetCustomAttributes( typeof( T ), true ) as T[];
            if ( targetAttribute == null || targetAttribute.Length <= 0 )
                _result.Add( _prop, default( T ) );
            else
                _result.Add( _prop, targetAttribute[ 0 ] );
        }

        return _result;
    }

    public static IEnumerable<FieldInfo> GetAllFields( System.Type t, BindingFlags p_bindingFlags )
    {
        if ( t == null )
            return Enumerable.Empty<FieldInfo>();

        BindingFlags flags = p_bindingFlags | BindingFlags.DeclaredOnly;
        return t.GetFields( flags ).Concat( GetAllFields( t.BaseType, p_bindingFlags ) );
    }

    private static Dictionary<MethodInfo, ButtonAttribute> GetButtonMethods( SerializedObject p_object )
    {
        if ( p_object == null || p_object.targetObject == null )
            return new Dictionary<MethodInfo, ButtonAttribute>();

        Dictionary<MethodInfo, ButtonAttribute> _result = new Dictionary<MethodInfo, ButtonAttribute>();

        MethodInfo[] _methodInfos = p_object.targetObject.GetType().GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );

        foreach ( MethodInfo _method in _methodInfos )
        {
            object[] _attributes = _method.GetCustomAttributes( typeof( ButtonAttribute ), true );
            if ( _attributes != null && _attributes.Length > 0 )
                _result.Add( _method, _attributes[ 0 ] as ButtonAttribute );
        }

        return _result;
    }

    private static List<MemberInfo> GetDebugFields( SerializedObject p_object )
    {
        List<MemberInfo> _result = new List<MemberInfo>();

        FieldInfo[] _fieldInfos = p_object.targetObject.GetType().GetFields( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );

        foreach ( FieldInfo _info in _fieldInfos )
        {
            object[] _attributes = _info.GetCustomAttributes( typeof( DebugAttribute ), true );
            if ( _attributes != null && _attributes.Length > 0 )
                _result.Add( _info );
        }

        PropertyInfo[] _propertyInfos = p_object.targetObject.GetType().GetProperties( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static );

        foreach ( PropertyInfo _info in _propertyInfos )
        {
            object[] _attributes = _info.GetCustomAttributes( typeof( DebugAttribute ), true );
            if ( _attributes != null && _attributes.Length > 0 )
                _result.Add( _info );
        }

        return _result;
    }
}

public class EditorCategory
{
    public string name;
    public bool foldout;
    public int order;

    public EditorCategory( string name, int order )
    {
        this.name = name;
        this.order = order;
        foldout = true;
    }

}