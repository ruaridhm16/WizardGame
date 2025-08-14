Shader "Custom/MagicWaves"
{
    Properties
    {
        _Color     ("Tint (Purple)", Color) = (0.7, 0.3, 1.0, 1.0)
        _Intensity ("Intensity", Range(0,3)) = 1.0
        _Alpha     ("Global Alpha", Range(0,1)) = 1.0
        _Speed     ("Flow Speed", Range(0,5)) = 1.0
        _Scale     ("Noise Scale", Range(0.1,10)) = 2.0
        _Octaves   ("Octaves (1-6)", Range(1,6)) = 4
        _Threshold ("Alpha Threshold", Range(0,1)) = 0.35
        _Softness  ("Edge Softness", Range(0.001,0.5)) = 0.1
        _Pixelate  ("Pixel Grid (X,Y)", Vector) = (160, 90, 0, 0) // number of cells across (x) and down (y)
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

        Pass
        {
            Name "FORWARD"
            Blend SrcAlpha OneMinusSrcAlpha  // traditional alpha blending
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.5

            // URP core
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            // Minimal 2D unlit setup
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 posCS : SV_POSITION;
                float2 uv    : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float  _Intensity;
            float  _Alpha;
            float  _Speed;
            float  _Scale;
            float  _Octaves;
            float  _Threshold;
            float  _Softness;
            float4 _Pixelate;   // (x,y) grid counts
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.posCS = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                return o;
            }

            // --------- 2D Value noise + fBM (fast and compact) ----------
            float hash21(float2 p)
            {
                // simple hash to [0,1)
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 78.233);
                return frac(p.x * p.y);
            }

            float valueNoise2D(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                // corners
                float a = hash21(i);
                float b = hash21(i + float2(1,0));
                float c = hash21(i + float2(0,1));
                float d = hash21(i + float2(1,1));

                // smooth step for interpolation
                float2 u = f*f*(3.0 - 2.0*f);

                // bilerp
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            // fractal Brownian motion
            float fbm(float2 p, int octaves)
            {
                float sum = 0.0;
                float amp = 0.5;
                float2 pp = p;

                [unroll]
                for (int i = 0; i < 6; i++) // max 6
                {
                    if (i >= octaves) break;
                    sum += valueNoise2D(pp) * amp;
                    pp *= 2.02;
                    amp *= 0.5;
                }
                return sum;
            }

            // quantize UVs for pixelation (in UV space)
            float2 PixelateUV(float2 uv, float2 grid)
            {
                // Avoid divide-by-zero
                grid = max(grid, float2(1,1));
                return floor(uv * grid) / grid;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Pixelation step
                float2 grid = _Pixelate.xy;
                float2 uvPix = PixelateUV(i.uv, grid);

                // Animated domain
                float t = _Time.y * _Speed;
                // mild domain warp for ripple feel
                float2 warp = float2( fbm(uvPix * (_Scale*0.5) + t, 3), fbm(uvPix * (_Scale*0.5) - t, 3) );
                float2 p = uvPix * _Scale + warp * 0.75 + float2(t*0.35, -t*0.25);

                // Main fBM field
                int oct = (int)round(_Octaves);
                float n = fbm(p, clamp(oct,1,6));

                // Convert to a wavy look (ridged-ish)
                float waves = 1.0 - abs(2.0*n - 1.0); // ridge
                waves = pow(saturate(waves), 1.25);

                // Intensity and color
                float3 col = _Color.rgb * (waves * _Intensity);

                // Alpha from threshold/softness for transparent background
                // Anything below threshold fades out smoothly
                float a = smoothstep(_Threshold - _Softness, _Threshold + _Softness, waves) * _Alpha;

                return float4(col, a);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
