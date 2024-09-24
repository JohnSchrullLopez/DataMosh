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
            sampler2D _Prev;
            int _Trigger;
            int _BlockSize;
 
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvr=round(i.uv*(_ScreenParams.xy/_BlockSize))/(_ScreenParams.xy/_BlockSize);
                //Get motion texture for current frame
                float4 mot = tex2D(_CameraMotionVectorsTexture,uvr);
                float4 depth = tex2D(_CameraDepthTexture, i.uv);

                //Fix coordinate differences between graphics APIs
                //Displace uv coordinates by intensity of Motion texture
                #if UNITY_UV_STARTS_AT_TOP
                float2 mvuv = float2(i.uv.x-mot.r, 1-i.uv.y+mot.g);
                #else
                float2 mvuv = float2(i.uv.x-mot.r, i.uv.y-mot.g);
                #endif

                //lerp switches between updating current frame normally or applying the datamosh effect to the previous frame
                //fixed4 col = lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), _Trigger);
                fixed4 col = lerp(tex2D(_MainTex,i.uv),tex2D(_Prev, mvuv), 1 - depth.r);
                return col;
            }
            ENDCG
        }
    }
}
