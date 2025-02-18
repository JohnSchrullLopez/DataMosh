Shader "Custom/DMEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            /*
             * Random number generation taken from Spatial in this stack overflow question:
             * https://stackoverflow.com/questions/4200224/random-noise-functions-for-glsl
            */
            
            // A single iteration of Bob Jenkins' One-At-A-Time hashing algorithm.
            uint hash( uint x )
            {
                x += ( x << 10u );
                x ^= ( x >>  6u );
                x += ( x <<  3u );
                x ^= ( x >> 11u );
                x += ( x << 15u );
                return x;
            }
            
            // Construct a float with half-open range [0:1] using low 23 bits.
            // All zeroes yields 0.0, all ones yields the next smallest representable value below 1.0.
            float floatConstruct( uint m )
            {
                const uint ieeeMantissa = 0x007FFFFFu; // binary32 mantissa bitmask
                const uint ieeeOne      = 0x3F800000u; // 1.0 in IEEE binary32

                m &= ieeeMantissa;                     // Keep only mantissa bits (fractional part)
                m |= ieeeOne;                          // Add fractional part to 1.0

                float  f = asfloat( m );       // Range [1:2]
                return f - 1.0;                        // Range [0:1]
            }

            float random(float x)
            {
                return floatConstruct(hash(asint(x)));
            }
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraMotionVectorsTexture;
            sampler2D _CameraDepthTexture;
            sampler2D _Mask;
            sampler2D _Top;
            sampler2D _Prev;
            
            float _DMIntensity;
            int _BlockSize;
            float _PerBlockNoise;
            float _BlockDecay;

            fixed4 frag (v2f i) : SV_Target
            {
                //create blocks by rounding uv
                float2 uvr=round(i.uv*(_ScreenParams.xy/_BlockSize))/(_ScreenParams.xy/_BlockSize);

                //Get motion texture for current frame and use rounded uvs to sample texture
                float4 mot = tex2D(_CameraMotionVectorsTexture,uvr);
                float4 depth = tex2D(_CameraDepthTexture, i.uv);
                float4 mask = tex2D(_Mask, i.uv);
                float4 top = tex2D(_Top, i.uv);

                //add noise to individual blocks
                float n = random(_Time.x * (uvr.x+uvr.y*_ScreenParams.x));
                mot=max(abs(mot)-round(n/_PerBlockNoise),0)*sign(mot); 

                //Render selected objects on top of masked dm objects
                mask.a = lerp(mask.a, 0, top.a);
                
                //Fix coordinate differences between graphics APIs
                //Displace uv coordinates by intensity of Motion texture
                #if UNITY_UV_STARTS_AT_TOP
                float2 mvuv = float2(i.uv.x-mot.r, 1-i.uv.y+mot.g);
                #else
                float2 mvuv = float2(i.uv.x-mot.r, i.uv.y-mot.g);
                #endif

                
                //FULLSCREENex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), _DMIntensity);

                //fixed4 col = lerp(t
                //OBJECT MASKED
                //fixed4 col = lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), tex2D(_Mask, i.uv).a);
                //fixed4 col = lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), lerp(round(1-(n)/1.4),1, tex2D(_Mask, i.uv).a));

                //DOF BASED
                //fixed4 col = lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), 1 - depth.r);

                fixed4 col = lerp(lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), _DMIntensity), tex2D(_Prev, mvuv), mask.a);
                //fixed4 col = lerp(lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), _DMIntensity), tex2D(_Prev, mvuv), lerp(round(1-(n)/1.4),1, mask.a));
                return col;
            }
            ENDCG
        }
    }
}