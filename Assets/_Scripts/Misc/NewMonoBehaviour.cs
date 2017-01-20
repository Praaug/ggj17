using UnityEngine;
using System.Collections;
using System.IO;

public static class GGJUtil
{
    public static bool IsFileLocked( FileInfo file )
    {
        FileStream stream = null;

        try
        {
            stream = file.Open( FileMode.Open, FileAccess.Read, FileShare.None );
        }
        catch ( IOException e )
        {
            Debug.Log( e );
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if ( stream != null )
                stream.Close();
        }

        //file is not locked
        return false;
    }
}
