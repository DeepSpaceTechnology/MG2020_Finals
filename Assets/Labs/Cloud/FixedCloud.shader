Shader "Cloud/FixedCloud"
{
 Properties
    {        
        [Header(Cloud Setting)]
         _FurTex ("Cloud Pattern", 2D) = "white" { }
         _Cutoff ("Cutoff", Range(0.01, 1.0)) = 0.5
            }
    
    Category
    {

        Tags { "Queue"="AlphaTest" "RenderType"="TransparentCutout" "IgnoreProjector"="True"}
		LOD 200
        Cull Off
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        
        SubShader
        { 
        Pass 
        {
            Tags 
            {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _FurTex;
            half4 _FurTex_ST;
            half _Cutoff;

            struct v2f 
            {
                half4 uv: TEXCOORD0;
                V2F_SHADOW_CASTER;
            };
            v2f vert (appdata_base v) 
            {
                v2f o;
                o.uv.zw = TRANSFORM_TEX(v.texcoord, _FurTex);
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(v2f i) : COLOR 
            {
                fixed3 noise = tex2D(_FurTex, i.uv.zw).rgb;
                fixed alpha = clamp(noise, 0, 1);
                clip(alpha-_Cutoff);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        } 
        
//        Fallback "Transparent/Cutout/VertexLit" 
   }
}       