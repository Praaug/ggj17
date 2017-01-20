using UnityEngine;

public class CategoryAttribute : PropertyAttribute
{
    public string category;

    public CategoryAttribute( string category )
    {
        this.category = category;
    }
}
