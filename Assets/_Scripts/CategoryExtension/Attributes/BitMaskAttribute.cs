using UnityEngine;

public class BitMaskAttribute : PropertyAttribute
{
    public int count;
    public int separatorStep;
    public int disabledOptions;

    public BitMaskAttribute()
    {
        count = 32;
    }
    public BitMaskAttribute( int count, int separatorStep = 0, params int[] disabledOptions )
    {
        this.count = Mathf.Clamp( count, 1, 32 );
        this.separatorStep = separatorStep;

        for ( int i = 0; i < disabledOptions.Length; i++ )
            this.disabledOptions |= 1 << ( disabledOptions[ i ] - 1 );
    }
}