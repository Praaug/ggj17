/// Credit NemoKrad (aka Charles Humphrey)
/// Sourced from - http://www.randomchaos.co.uk/SoftAlphaUIMask.aspx

namespace UnityEngine.UI.Extensions
{
    [ExecuteInEditMode]
    [AddComponentMenu( "UI/Effects/Extensions/SoftMaskScript" )]
    public class SoftMaskScript : MonoBehaviour
    {
        private Material mat;
        private Canvas canvas;

        [Tooltip( "The area that is to be used as the container." )]
        public RectTransform MaskArea;
        private RectTransform myRect;

        [Tooltip( "A Rect Transform that can be used to scale and move the mask - Does not apply to Text UI Components being masked" )]
        public RectTransform maskScalingRect;

        [Tooltip( "Texture to be used to do the soft alpha" )]
        public Texture AlphaMask;

        [Tooltip( "At what point to apply the alpha min range 0-1" )]
        [Range( 0, 1 )]
        public float CutOff = 0;

        [Tooltip( "Implement a hard blend based on the Cutoff" )]
        public bool HardBlend;

        [Tooltip( "Flip the masks alpha value" )]
        public bool FlipAlphaMask;

        [Tooltip( "If Mask Scaling Rect is given and this value is true, the area around the mask will not be clipped" )]
        public bool DontClipMaskScalingRect;

        [Tooltip( "If set to true, this mask is applied to all child Text and Graphic objects belonging to this object." )]
        public bool CascadeToALLChildren;

        public Material m_materialToUse = null;
        public Material m_materialToUseText = null;

        private Vector3[] worldCorners = new Vector3[ 4 ];
        private Vector2 maskOffset = Vector2.zero;
        private Vector2 maskScale = Vector2.one;

        private Vector2 AlphaUV;

        private Vector2 min;
        private Vector2 max = Vector2.one;
        private Vector2 p;
        private Vector2 siz;
        private Vector2 tp = new Vector2( .5f, .5f );


        bool MaterialNotSupported = false; // UI items like toggles, we can stil lcascade down to them though :)
        Rect maskRect;
        Rect contentRect;

        Vector2 centre;

        bool isText;

        // Use this for initialization
        void Start()
        {
            myRect = GetComponent<RectTransform>();

            if ( !MaskArea )
            {
                MaskArea = myRect;
            }

            Graphic _graphic = GetComponent<Graphic>();

            if ( _graphic != null )
            {
                mat = new Material( Shader.Find( "UI Extensions/SoftMaskShader" ) );
                _graphic.material = mat;
            }

            Text _text = GetComponent<Text>();

            if ( _text )
            {
                isText = true;
                mat = new Material( Shader.Find( "UI Extensions/SoftMaskShaderText" ) );
                GetComponent<Text>().material = mat;

                GetCanvas();

                Mask _mask = transform.parent.GetComponent<Mask>();

                // For some reason, having the mask control on the parent and disabled stops the mouse interacting
                // with the texture layer that is not visible.. Not needed for the Image.
                if ( transform.parent.GetComponent<Button>() == null && _mask == null )
                    _mask = transform.parent.gameObject.AddComponent<Mask>();

                if ( _mask != null )
                    _mask.enabled = false;
            }
            if ( CascadeToALLChildren )
            {
                for ( int c = 0; c < transform.childCount; c++ )
                {
                    SetSAM( transform.GetChild( c ) );
                }
            }

            MaterialNotSupported = mat == null;
        }

        void SetSAM( Transform t )
        {
            SoftMaskScript thisSam = t.gameObject.GetComponent<SoftMaskScript>();
            if ( thisSam == null )
            {
                thisSam = t.gameObject.AddComponent<SoftMaskScript>();

            }
            thisSam.MaskArea = MaskArea;
            thisSam.AlphaMask = AlphaMask;
            thisSam.CutOff = CutOff;
            thisSam.HardBlend = HardBlend;
            thisSam.FlipAlphaMask = FlipAlphaMask;
            thisSam.maskScalingRect = maskScalingRect;
            thisSam.DontClipMaskScalingRect = DontClipMaskScalingRect;
            thisSam.CascadeToALLChildren = CascadeToALLChildren;
            thisSam.m_materialToUse = m_materialToUse;
            thisSam.m_materialToUseText = m_materialToUseText;
        }

        void GetCanvas()
        {
            Transform t = transform;

            int lvlLimit = 100;
            int lvl = 0;

            while ( canvas == null && lvl < lvlLimit )
            {
                canvas = t.gameObject.GetComponent<Canvas>();
                if ( canvas == null )
                {
                    t = t.parent;
                }

                lvl++;
            }
        }

        void Update()
        {
            SetMask();
        }

        void SetMask()
        {
            if ( MaterialNotSupported || mat == null)
            {
                return;
            }

            // Get the two rectangle areas
            //maskRect = MaskArea.rect;
            //contentRect = myRect.rect;

            if ( isText ) // Need to do our calculations in world for Text
            {
                maskScalingRect = null;
                if ( canvas.renderMode == RenderMode.ScreenSpaceOverlay && Application.isPlaying )
                {
                    p = canvas.transform.InverseTransformPoint( MaskArea.transform.position );
                    siz = new Vector2( maskRect.width, maskRect.height );
                }
                else
                {
                    worldCorners = new Vector3[ 4 ];
                    MaskArea.GetWorldCorners( worldCorners );
                    siz = ( worldCorners[ 2 ] - worldCorners[ 0 ] );
                    p = MaskArea.transform.position;
                }

                min = p - ( new Vector2( siz.x, siz.y ) * .5f );
                max = p + ( new Vector2( siz.x, siz.y ) * .5f );
            }
            else // Need to do our calculations in tex space for Image.
            {
                if ( maskScalingRect != null )
                {
                    maskRect = maskScalingRect.rect;
                }

                // Get the centre offset
                if ( maskScalingRect != null )
                {
                    centre = myRect.transform.InverseTransformPoint( maskScalingRect.transform.TransformPoint( maskScalingRect.rect.center ) );
                }
                else
                {
                    centre = myRect.transform.InverseTransformPoint( MaskArea.transform.TransformPoint( MaskArea.rect.center ) );
                }
                centre += (Vector2)myRect.transform.InverseTransformPoint( myRect.transform.position ) - myRect.rect.center;

                // Set the scale for mapping texcoords mask
                AlphaUV = new Vector2( maskRect.width / contentRect.width, maskRect.height / contentRect.height );

                // set my min and max to the centre offest
                min = centre;
                max = min;

                siz = new Vector2( maskRect.width, maskRect.height ) * .5f;
                // Move them out to the min max extreams
                min -= siz;
                max += siz;

                // Now move these into texture space. 0 - 1
                min = new Vector2( min.x / contentRect.width, min.y / contentRect.height ) + tp;
                max = new Vector2( max.x / contentRect.width, max.y / contentRect.height ) + tp;
            }

            MaskArea.GetWorldCorners( worldCorners );
            var size = ( worldCorners[ 2 ] - worldCorners[ 0 ] );
            maskScale.Set( 1.0f / size.x, 1.0f / size.y );
            maskOffset = -worldCorners[ 0 ];
            maskOffset.Scale( maskScale );
            mat.SetTextureOffset( "_AlphaMask", maskOffset );
            mat.SetTextureScale( "_AlphaMask", maskScale );
            mat.SetTexture( "_AlphaMask", AlphaMask );


            mat.SetFloat( "_HardBlend", HardBlend ? 1 : 0 );

            // Pass the values to the shader
            mat.SetVector( "_Min", min );
            mat.SetVector( "_Max", max );

            mat.SetInt( "_FlipAlphaMask", FlipAlphaMask ? 1 : 0 );
            mat.SetTexture( "_AlphaMask", AlphaMask );

            mat.SetInt( "_NoOuterClip", DontClipMaskScalingRect && maskScalingRect != null ? 1 : 0 );

            if ( !isText ) // No mod needed for Text
            {
                mat.SetVector( "_AlphaUV", AlphaUV );
            }

            mat.SetFloat( "_CutOff", CutOff );
        }
    }
}