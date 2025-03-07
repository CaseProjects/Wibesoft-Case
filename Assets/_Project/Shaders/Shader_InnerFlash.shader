Shader "Custom/InnerFlash"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Flash Color", Color) = (1,0,0,1) // Kırmızı yanıp sönme rengi
        _FlashSpeed ("Flash Speed", Float) = 2
        _InnerThreshold ("Inner Threshold", Range(0,1)) = 0.5
        _MinAlpha ("Minimum Alpha", Range(0,1)) = 0.2 // Alpha değeri en fazla buraya kadar düşer
        _MaxColorFactor ("Max Color Factor", Range(0,1)) = 0.5 // Verilen renk %50 kadar değişecek
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _FlashSpeed;
            float _InnerThreshold;
            float _MinAlpha;
            float _MaxColorFactor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Orijinal sprite rengini al
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Merkezden uzaklığı hesapla (kare etkisi)
                float2 centeredUV = abs(i.uv - 0.5) * 2;
                float distance = max(centeredUV.x, centeredUV.y);

                // 0 ile 1 arasında gidip gelen sinüs dalgası
                float flashEffect = (sin(_Time.y * _FlashSpeed) + 1) * 0.5; 

                // Alpha değerini 0 ile 1 yerine, MinAlpha ile 1 arasında tut
                float finalAlpha = _MinAlpha + (1.0 - _MinAlpha) * flashEffect;

                // İç bölge etkisi
                if (distance < _InnerThreshold)
                {
                    // Renk geçişi: Verilen rengin _MaxColorFactor kadarını geçiş yap
                    float colorBlendAmount = flashEffect * _MaxColorFactor;
                    texColor.rgb = lerp(texColor.rgb, _Color.rgb, colorBlendAmount); // Renk geçişi
                    texColor.a *= finalAlpha; // Alpha değeri 0 yerine _MinAlpha'ya düşecek
                }

                return fixed4(texColor.rgb, texColor.a);
            }
            ENDCG
        }
    }
}
