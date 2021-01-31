Shader "GGJ2021/ParticleDissolve"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _BaseTransparency("Base Transparency", Range(0, 1)) = 0
        _Dissolve("Dissolve Texture", 2D) = "white" {}
        _DissolveAmount("Dissolve Amount", Range(0.01, 1.2)) = 0
    }
        SubShader
        {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
        Blend SrcAlpha One
        Cull Off Lighting Off ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    float4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                sampler2D _Dissolve;
                float4 _Dissolve_ST;
                float _DissolveAmount, _BaseTransparency;

                void Unity_Remap_float4(float4 In, float2 InMinMax, float2 OutMinMax, out float4 Out)
                {
                    Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                }

                float Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax)
                {
                    return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv);
                    fixed4 dissolveTex = tex2D(_Dissolve, i.uv);
                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);
                    float dissolve = i.color.a;
                    float2 InMinMax = float2(dissolve - .1, dissolve);
                    float2 OutMinMax = float2(0, col.a);
                    col.a = saturate(Unity_Remap_float(col.a, InMinMax, OutMinMax) - _BaseTransparency);
                    clip(dissolveTex.a - dissolve);
                    return col;
                }
                ENDCG
            }
        }
}