// Triplanar Shader that takes into account local coordinates - Ben Golus

Shader "GGJ2021/BGolus/Colormapped VertexShaded Triplanar" {
Properties {
        _ColorR("Color R", Color) = (1,1,1,1)
        _ColorG("Color G", Color) = (1,1,1,1)
        _ColorB("Color B", Color) = (1,1,1,1)
        _MainTex ("Topside Texture", 2D) = "white" {}
        _SideTex("Side Texture", 2D) = "white" {}
        _Emissive("Emissive Power", Range(0,1)) = 0.5
        _TexScale ("Texture scale", Float) = 1
        _TexScale2 ("Secondary Normal Map scale", Float) = 1
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" "DisableBatching"="True" }
        LOD 200
   
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        sampler2D _MainTex;
        sampler2D _SideTex;
        float _TexScale;
        float _TexScale2;
        struct Input {
            float3 worldPos;
            float3 worldNormal;
            float3 color : COLOR;
            INTERNAL_DATA
        };
        half _Emissive;
        half _Glossiness;
        half _Metallic;
        fixed4 _ColorR;
        fixed4 _ColorG;
        fixed4 _ColorB;
        void surf (Input IN, inout SurfaceOutputStandard o) {
 
            // get scale from matrix
            float3 scale = float3(
                length(unity_WorldToObject._m00_m01_m02),
                length(unity_WorldToObject._m10_m11_m12),
                length(unity_WorldToObject._m20_m21_m22)
                );
 
            // get translation from matrix
            float3 pos = unity_WorldToObject._m03_m13_m23 / scale;
 
            // get unscaled rotation from matrix
            float3x3 rot = float3x3(
                normalize(unity_WorldToObject._m00_m01_m02),
                normalize(unity_WorldToObject._m10_m11_m12),
                normalize(unity_WorldToObject._m20_m21_m22)
                );
            // make box mapping with rotation preserved
            float3 map = mul(rot, IN.worldPos) + pos;
            float3 norm = mul(rot, IN.worldNormal);
 
            float3 blend = abs(norm) / dot(abs(norm), float3(1,1,1));
            float2 uv;
            fixed4 c;
            if (blend.x > max(blend.y, blend.z)) {
                uv = map.yz;
                c = tex2D(_MainTex, uv * (1 / _TexScale));
            } else if (blend.z > blend.y) {
                uv = map.xy;
                c = tex2D(_SideTex, uv * (1 / _TexScale));
            } else {
                uv = map.xz;
                c = tex2D(_MainTex, uv * (1 / _TexScale));

            }
            fixed4 redCol = c.r * _ColorR;
            fixed4 greenCol = c.g * _ColorG;
            fixed4 blueCol = c.b * _ColorB;
            fixed4 mixCol = max(redCol, max(greenCol, blueCol));
            o.Albedo = mixCol;
 
            // Metallic and smoothness come from slider variables
            o.Metallic = mixCol * _Metallic * IN.color;
            o.Smoothness = mixCol * _Glossiness * IN.color;
            o.Emission = mixCol * _Emissive * IN.color;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}