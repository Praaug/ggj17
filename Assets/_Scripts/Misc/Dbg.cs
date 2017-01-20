using UnityEngine;
using System.IO;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

public enum DebugType
{
    Log = 0,
    Warning = 1,
    Error = 2,
    Exception = 3
}

public partial class Dbg : MonoBehaviour
{
    private const bool ENABLED_OVERRIDE = true;
#if !FINAL
    private const StackTraceLogType LOG_TYPE = StackTraceLogType.None;
#else
    private const StackTraceLogType LOG_TYPE = StackTraceLogType.None;
#endif

    #region Static Fields
    private const string LOG_FILE = "LKLog.txt";
    private static bool s_isInitialized;
    private static Dbg s_instance = null;
    private static Dbg Instance
    {
        get
        {
            if ( s_instance == null && !s_isInitialized )
            {
                Debug.Log( "Dbg: Tried to spawn new instance" );
                s_instance = FindObjectOfType<Dbg>() ?? new GameObject( "DebugManager" ).AddComponent<Dbg>();
            }
            return s_instance;
        }
    }
    private static int frameCount = 0;
    private static int skipFrameDepth = 0;

    public static bool enableLogging = true;

    public static string errorString = "";
    #endregion

    #region Static Methods
    private static void Log( Object p_object, string message, StackTraceLogType p_type )
    {
        skipFrameDepth++;
        if ( Instance != null )
            Instance.Write( message, p_object, DebugType.Log, p_type );
        else
            Debug.Log( message, p_object );
    }

    public static void Log( object p_object )
    {
        skipFrameDepth++;
        Log( p_object as Object, p_object != null ? p_object.ToString() : "null", StackTraceLogType.ScriptOnly );
    }
    public static void Log( string message )
    {
        skipFrameDepth++;
        Log( null, message, StackTraceLogType.ScriptOnly );
    }
    public static void Log( Object p_object, string message )
    {
        skipFrameDepth++;
        Log( p_object, message, StackTraceLogType.ScriptOnly );
    }
    public static void Log( string message, params object[] args )
    {
        skipFrameDepth++;
        Log( null, string.Format( message, args ), StackTraceLogType.ScriptOnly );
    }
    public static void Log( Object p_object, string message, params object[] args )
    {
        skipFrameDepth++;
        Log( p_object, string.Format( message, args ), StackTraceLogType.ScriptOnly );
    }

    public static void LogFast( object p_object )
    {
        skipFrameDepth++;
        Log( p_object as Object, p_object != null ? p_object.ToString() : "null", StackTraceLogType.None );
    }
    public static void LogFast( string message )
    {
        skipFrameDepth++;
        Log( null, message, StackTraceLogType.None );
    }
    public static void LogFast( Object p_object, string message )
    {
        skipFrameDepth++;
        Log( p_object, message, StackTraceLogType.None );
    }
    public static void LogFast( string message, params object[] args )
    {
        skipFrameDepth++;
        Log( null, string.Format( message, args ), StackTraceLogType.None );
    }
    public static void LogFast( Object p_object, string message, params object[] args )
    {
        skipFrameDepth++;
        Log( p_object, string.Format( message, args ), StackTraceLogType.None );
    }

    public static void LogWarning( object p_object )
    {
        skipFrameDepth++;
        LogWarning( p_object as Object, p_object != null ? p_object.ToString() : "null" );
    }
    public static void LogWarning( string message, params object[] args )
    {
        skipFrameDepth++;
        LogWarning( null as Object, string.Format( message, args ) );
    }
    public static void LogWarning( Object p_object, string message, params object[] args )
    {
        skipFrameDepth++;
        LogWarning( p_object, string.Format( message, args ) );
    }
    public static void LogWarning( Object p_object, string message )
    {
        skipFrameDepth++;
        if ( Instance != null )
            Instance.Write( message, p_object, DebugType.Warning, StackTraceLogType.ScriptOnly );
        else
            Debug.LogWarning( message, p_object );
    }

    public static void LogError( object p_object )
    {
        skipFrameDepth++;
        LogError( p_object as Object, p_object != null ? p_object.ToString() : "null" );
    }
    public static void LogError( string message, params object[] args )
    {
        skipFrameDepth++;
        LogError( null as Object, string.Format( message, args ) );
    }
    public static void LogError( Object p_object, string message, params object[] args )
    {
        skipFrameDepth++;
        LogError( p_object, string.Format( message, args ) );
    }
    public static void LogError( Object p_object, string message )
    {
        skipFrameDepth++;

        errorString += string.Format( "Error: {0}\n{1}\n\n\n", message, new StackTrace( skipFrameDepth, true ) );

        if ( Instance != null )
            Instance.Write( message, p_object, DebugType.Error, StackTraceLogType.ScriptOnly );
        else
            Debug.LogError( message, p_object );

    }

    public static void LogException( object p_object )
    {
        skipFrameDepth++;
        LogException( p_object as Object, p_object != null ? p_object.ToString() : "null" );
    }
    public static void LogException( string message, params object[] args )
    {
        skipFrameDepth++;
        LogException( null as Object, string.Format( message, args ) );
    }
    public static void LogException( Object p_object, string message, params object[] args )
    {
        skipFrameDepth++;
        LogException( p_object, string.Format( message, args ) );
    }
    public static void LogException( Object p_object, string message )
    {
        skipFrameDepth++;

        errorString += string.Format( "Exception: {0}\n{1}\n\n\n", message, new StackTrace( skipFrameDepth, true ) );

        if ( Instance != null )
            Instance.Write( string.Format( "Exception: {0}", message ), p_object, DebugType.Exception, StackTraceLogType.ScriptOnly );
        else
            Debug.LogError( message );
    }
    #endregion
}

public partial class Dbg : MonoBehaviour
{
    #region Fields
    private StreamWriter OutputStream;
    #endregion

    #region Methods
    private void Awake()
    {
        if ( s_instance != null )
        {
            Destroy( gameObject );
            return;
        }

        s_instance = this;
        s_isInitialized = true;
        DontDestroyOnLoad( gameObject );

#pragma warning disable
        Application.stackTraceLogType = LOG_TYPE;
#pragma warning restore
        string LogFile = LOG_FILE;
        LogFile = LogFile.Replace( ".txt", "" );

        if ( File.Exists( LogFile + ".txt" ) && GGJUtil.IsFileLocked( new FileInfo( LogFile + ".txt" ) ) )
        {
            int i = 1;
            LogFile += "_";
            while ( File.Exists( LogFile + i + ".txt" ) && GGJUtil.IsFileLocked( new FileInfo( LogFile + i + ".txt" ) ) )
            {
                i++;
            }
            LogFile += i;
        }

        LogFile += ".txt";

        // Open the log file to append the new log to it.
        OutputStream = new StreamWriter( LogFile, false );

        Application.logMessageReceived += Application_logMessageReceived;

        Log( "LOG STARTED!" );
    }

    private void Application_logMessageReceived( string condition, string stackTrace, LogType type )
    {
        if ( !( type == LogType.Error || type == LogType.Exception ) )
            return;

        errorString += string.Format( "UnityLog: {0}\n{1}\n\n\n", condition, stackTrace );
    }

    private void Update()
    {
        frameCount = Time.frameCount;
    }

    private void OnDestroy()
    {
        if ( OutputStream != null )
        {
            OutputStream.Close();
            OutputStream = null;
        }

        if ( s_instance == this )
            s_instance = null;

        Application.logMessageReceived -= Application_logMessageReceived;
    }

    private void Write( string message, Object p_object, DebugType p_type, StackTraceLogType p_stackTraceType )
    {
        if ( !enableLogging || !ENABLED_OVERRIDE )
            return;

        if ( p_stackTraceType != StackTraceLogType.None )
            message = string.Format( "{0}\n{1}", message, new StackTrace( ++skipFrameDepth, true ) );
        skipFrameDepth = 0;

        if ( OutputStream != null )
        {
            OutputStream.WriteLine( string.Format( "[{0:H:mm:ss}; {2:00000}] {3}\n", System.DateTime.Now, frameCount, message ) );
            OutputStream.Flush();
        }

        if ( p_type == DebugType.Exception )
        {
            Debug.LogException( new System.Exception( message ) );
        }
        else
        {
            System.Action<string, Object> LogMethod;
            switch ( p_type )
            {
                default:
                case DebugType.Log:
                    LogMethod = Debug.Log;
                    break;
                case DebugType.Warning:
                    LogMethod = Debug.LogWarning;
                    break;
                case DebugType.Error:
                    LogMethod = Debug.LogError;
                    break;
            }

            LogMethod( message, p_object );
        }
    }

    #endregion
}