Shader "Custom/Laser2D"
{
    Properties
    {
        _Color ("Laser Color", Color) = (1,0,0,1)
        _Intensity ("Intensity", Float) = 5
        _Width ("Beam Width", Range(0,1)) = 0.25
        _Softness ("Edge Softness", Range(0.001,0.5)) = 0.1
        _ScrollSpeed ("Scroll Speed", Float) = 4
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Flicker ("Flicker Strength", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend One One
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _NoiseTex;
            float4 _Color;
            float _Intensity;
            float _Width;
            float _Softness;
            float _ScrollSpeed;
            float _Flicker;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // Centered UV (0 = center of beam)
                float dist = abs(i.uv.y - 0.5);

                // Beam shape
                float beam = smoothstep(_Width, _Width - _Softness, dist);

                // Scrolling noise
                float2 noiseUV = float2(i.uv.x + _Time.y * _ScrollSpeed, i.uv.y);
                float noise = tex2D(_NoiseTex, noiseUV).r;

                // Flicker
                float flicker = lerp(1, noise, _Flicker);

                float final = beam * flicker;

                return half4(_Color.rgb * final * _Intensity, final);
            }
            ENDHLSL
        }
    }
}
