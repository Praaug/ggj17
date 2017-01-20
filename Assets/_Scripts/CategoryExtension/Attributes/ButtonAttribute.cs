[System.AttributeUsage( System.AttributeTargets.Method, AllowMultiple = false, Inherited = true )]
public class ButtonAttribute : System.Attribute
{
    public string text;

    public ButtonAttribute( string text )
    {
        this.text = text;
    }
}