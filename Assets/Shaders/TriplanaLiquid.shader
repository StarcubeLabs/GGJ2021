// Triplanar Shader that takes into account local coordinates - Ben Golus

Shader "GGJ2021/BGolus/TriplanarLiquid" {
Properties {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Topside Texture", 2D) = "white" {}
        _Emissive("Emissive Power", Range(0,1)) = 0.5
        _ScrollSpeed("Scroll Speed", Range(-10,10)) = 1
        _TexScale ("Texture scale", Float) = 1
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

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float3 worldPos : TEXCOORD0;
                    half3 tspace0 : TEXCOORD1;
                    half3 tspace1 : TEXCOORD2;
                    half3 tspace2 : TEXCOORD3;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Emissive, _ScrollSpeed, _TexScale;
                fixed4 _Color;


                fixed4 _LightColor0;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;

                    half3 wNormal = UnityObjectToWorldNormal(v.normal);
                    half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
                    // compute bitangent from cross product of normal and tangent
                    half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
                    half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                    // output the tangent space matrix
                    o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                    o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                    o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    half3 vertexNormal = abs(normalize(half3(i.tspace0.z, i.tspace1.z, i.tspace2.z)));
                    half3 triblend = pow(vertexNormal, 4);
                    triblend /= max(dot(triblend, half3(1,1,1)), 0.0001);

                    // preview blend
                    // return fixed4(triblend.xyz, 1);

                    // calculate triplanar uvs
                    // applying texture scale and offset values ala TRANSFORM_TEX macro
                    float2 uvX = i.worldPos.zy * _MainTex_ST.xy + _MainTex_ST.zw;
                    float2 uvY = i.worldPos.xz * _MainTex_ST.xy + _MainTex_ST.zw;
                    float2 uvZ = i.worldPos.xy * _MainTex_ST.xy + _MainTex_ST.zw;

                    // sample the texture
                    fixed4 colX = tex2D(_MainTex, uvX + _Time.x * _ScrollSpeed);
                    fixed4 colY = tex2D(_MainTex, uvY + _Time.x * _ScrollSpeed);
                    fixed4 colZ = tex2D(_MainTex, uvZ + _Time.x * _ScrollSpeed);
                    fixed4 col = colX * triblend.x + colY * triblend.y + colZ * triblend.z;

                    // preview directional lighting
                    // return fixed4(ndotl.xxx, 1);

                    return fixed4(col.rgb * _Color, col.a);
                }
                ENDCG
            }
        }
    FallBack "Diffuse"
}