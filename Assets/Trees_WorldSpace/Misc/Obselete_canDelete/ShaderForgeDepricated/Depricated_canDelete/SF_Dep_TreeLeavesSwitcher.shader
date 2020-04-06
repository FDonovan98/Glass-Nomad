// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:True,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:8433,x:35700,y:32885,varname:node_8433,prsc:2|diff-6416-RGB,diffpow-6416-RGB,spec-7470-RGB,gloss-5953-OUT,normal-555-RGB,emission-6802-OUT,transm-7194-OUT,clip-6416-A,voffset-3097-OUT;n:type:ShaderForge.SFN_Tex2d,id:6416,x:32659,y:32567,ptovrint:False,ptlb:Albedo_Mask(A),ptin:_Albedo_MaskA,varname:node_4147,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3232-OUT;n:type:ShaderForge.SFN_Tex2d,id:555,x:32634,y:33033,ptovrint:False,ptlb:NormalMap,ptin:_NormalMap,varname:node_2858,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True|UVIN-3232-OUT;n:type:ShaderForge.SFN_Color,id:7470,x:32785,y:32723,ptovrint:False,ptlb:Spec_color,ptin:_Spec_color,varname:node_7470,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:5953,x:32785,y:32889,ptovrint:False,ptlb:Gloss,ptin:_Gloss,varname:node_5953,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_SwitchProperty,id:9352,x:31786,y:33015,ptovrint:False,ptlb:SwitchRow,ptin:_SwitchRow,varname:node_9352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2604-OUT,B-9446-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:3739,x:31844,y:33191,ptovrint:False,ptlb:SwitchColumn,ptin:_SwitchColumn,varname:node_3739,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2604-OUT,B-9446-OUT;n:type:ShaderForge.SFN_Vector1,id:2604,x:31438,y:33054,varname:node_2604,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:9446,x:31450,y:33171,varname:node_9446,prsc:2,v1:0.5;n:type:ShaderForge.SFN_TexCoord,id:2256,x:31846,y:32767,varname:node_2256,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:7663,x:32102,y:32821,varname:node_7663,prsc:2|A-2256-U,B-9352-OUT,C-7840-R;n:type:ShaderForge.SFN_Add,id:4286,x:32102,y:32980,varname:node_4286,prsc:2|A-2256-V,B-3739-OUT,C-7840-G;n:type:ShaderForge.SFN_Append,id:3232,x:32278,y:32843,varname:node_3232,prsc:2|A-7663-OUT,B-4286-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:7726,x:31967,y:32533,ptovrint:False,ptlb:WorldSpaceOffset,ptin:_WorldSpaceOffset,varname:node_7726,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:True|A-6608-OUT,B-778-OUT;n:type:ShaderForge.SFN_Vector1,id:6608,x:31700,y:32411,varname:node_6608,prsc:2,v1:0;n:type:ShaderForge.SFN_If,id:9117,x:31523,y:32646,varname:node_9117,prsc:2|A-2610-OUT,B-7274-OUT,GT-3584-OUT,EQ-7274-OUT,LT-7274-OUT;n:type:ShaderForge.SFN_Vector1,id:7274,x:31221,y:32667,varname:node_7274,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Vector1,id:3584,x:31221,y:32775,varname:node_3584,prsc:2,v1:1;n:type:ShaderForge.SFN_If,id:3006,x:31498,y:32457,varname:node_3006,prsc:2|A-8020-OUT,B-7274-OUT,GT-3584-OUT,EQ-7274-OUT,LT-7274-OUT;n:type:ShaderForge.SFN_Frac,id:8020,x:31221,y:32390,varname:node_8020,prsc:2|IN-6460-X;n:type:ShaderForge.SFN_Frac,id:2610,x:31221,y:32537,varname:node_2610,prsc:2|IN-6460-Z;n:type:ShaderForge.SFN_ComponentMask,id:7840,x:32209,y:32513,varname:node_7840,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7726-OUT;n:type:ShaderForge.SFN_Append,id:778,x:31731,y:32533,varname:node_778,prsc:2|A-3006-OUT,B-9117-OUT;n:type:ShaderForge.SFN_ObjectPosition,id:6460,x:30888,y:32545,varname:node_6460,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:2025,x:33614,y:33024,ptovrint:False,ptlb:TransmissionBoost,ptin:_TransmissionBoost,varname:node_2025,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2.8;n:type:ShaderForge.SFN_ViewVector,id:7486,x:32634,y:33219,varname:node_7486,prsc:2;n:type:ShaderForge.SFN_LightVector,id:6491,x:32634,y:33354,varname:node_6491,prsc:2;n:type:ShaderForge.SFN_Dot,id:8686,x:32926,y:33280,varname:node_8686,prsc:2,dt:0|A-7486-OUT,B-6491-OUT;n:type:ShaderForge.SFN_OneMinus,id:8225,x:33399,y:33183,varname:node_8225,prsc:2|IN-8686-OUT;n:type:ShaderForge.SFN_Multiply,id:3636,x:33825,y:33034,varname:node_3636,prsc:2|A-2025-OUT,B-8225-OUT;n:type:ShaderForge.SFN_LightColor,id:18,x:33785,y:33329,varname:node_18,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3268,x:33993,y:33153,varname:node_3268,prsc:2|A-3636-OUT,B-18-RGB;n:type:ShaderForge.SFN_Multiply,id:7194,x:34264,y:32994,varname:node_7194,prsc:2|A-6416-RGB,B-3268-OUT;n:type:ShaderForge.SFN_Slider,id:3397,x:34079,y:33726,ptovrint:False,ptlb:Motion,ptin:_Motion,varname:node_3397,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-3,cur:0.1325256,max:3;n:type:ShaderForge.SFN_NormalVector,id:6320,x:34785,y:33222,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:3097,x:35202,y:33254,varname:node_3097,prsc:2|A-6320-OUT,B-6145-OUT;n:type:ShaderForge.SFN_Time,id:1387,x:33669,y:33492,varname:node_1387,prsc:2;n:type:ShaderForge.SFN_Sin,id:9417,x:34116,y:33512,varname:node_9417,prsc:2|IN-5114-OUT;n:type:ShaderForge.SFN_Multiply,id:8379,x:34591,y:33520,varname:node_8379,prsc:2|A-9417-OUT,B-3397-OUT;n:type:ShaderForge.SFN_Add,id:5114,x:34001,y:33548,varname:node_5114,prsc:2|A-2375-OUT,B-6649-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:8953,x:33589,y:33657,varname:node_8953,prsc:2;n:type:ShaderForge.SFN_Add,id:6649,x:33779,y:33657,varname:node_6649,prsc:2|A-8953-X,B-8953-Z;n:type:ShaderForge.SFN_Multiply,id:6802,x:34785,y:33015,varname:node_6802,prsc:2|A-7194-OUT,B-2800-OUT;n:type:ShaderForge.SFN_Multiply,id:2800,x:34455,y:33189,varname:node_2800,prsc:2|A-1588-OUT,B-1054-OUT;n:type:ShaderForge.SFN_Vector1,id:1054,x:34231,y:33332,varname:node_1054,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Tex2d,id:8557,x:33909,y:33983,ptovrint:False,ptlb:MotionPower,ptin:_MotionPower,varname:node_8557,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3232-OUT;n:type:ShaderForge.SFN_Multiply,id:6145,x:34963,y:33584,varname:node_6145,prsc:2|A-8379-OUT,B-8557-R,C-8557-G,D-8557-B;n:type:ShaderForge.SFN_Slider,id:93,x:33382,y:33434,ptovrint:False,ptlb:Motion_Speed,ptin:_Motion_Speed,varname:node_93,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:10;n:type:ShaderForge.SFN_Multiply,id:2375,x:33870,y:33451,varname:node_2375,prsc:2|A-93-OUT,B-1387-T;n:type:ShaderForge.SFN_LightAttenuation,id:1588,x:34244,y:33168,varname:node_1588,prsc:2;proporder:6416-555-7470-5953-9352-3739-7726-2025-3397-8557-93;pass:END;sub:END;*/

Shader "Custom/TreeLeavesSwitcher" {
    Properties {
        _Albedo_MaskA ("Albedo_Mask(A)", 2D) = "white" {}
        _NormalMap ("NormalMap", 2D) = "bump" {}
        _Spec_color ("Spec_color", Color) = (0.5,0.5,0.5,1)
        _Gloss ("Gloss", Float ) = 0
        [MaterialToggle] _SwitchRow ("SwitchRow", Float ) = 0
        [MaterialToggle] _SwitchColumn ("SwitchColumn", Float ) = 0
        [MaterialToggle] _WorldSpaceOffset ("WorldSpaceOffset", Float ) = 0.5
        _TransmissionBoost ("TransmissionBoost", Float ) = 2.8
        _Motion ("Motion", Range(-3, 3)) = 0.1325256
        _MotionPower ("MotionPower", 2D) = "white" {}
        _Motion_Speed ("Motion_Speed", Range(0, 10)) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Albedo_MaskA; uniform float4 _Albedo_MaskA_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float4 _Spec_color;
            uniform float _Gloss;
            uniform fixed _SwitchRow;
            uniform fixed _SwitchColumn;
            uniform fixed _WorldSpaceOffset;
            uniform float _TransmissionBoost;
            uniform float _Motion;
            uniform sampler2D _MotionPower; uniform float4 _MotionPower_ST;
            uniform float _Motion_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1387 = _Time;
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((o.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(o.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float4 _MotionPower_var = tex2Dlod(_MotionPower,float4(TRANSFORM_TEX(node_3232, _MotionPower),0.0,0));
                v.vertex.xyz += (v.normal*((sin(((_Motion_Speed*node_1387.g)+(mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)))*_Motion)*_MotionPower_var.r*_MotionPower_var.g*_MotionPower_var.b));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((i.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(i.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_3232, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 _Albedo_MaskA_var = tex2D(_Albedo_MaskA,TRANSFORM_TEX(node_3232, _Albedo_MaskA));
                clip(_Albedo_MaskA_var.a - 0.5);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Spec_color.rgb;
                float specularMonochrome;
                float3 diffuseColor = _Albedo_MaskA_var.rgb; // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = pow(max(0.0, NdotL ), _Albedo_MaskA_var.rgb);
                float3 node_7194 = (_Albedo_MaskA_var.rgb*((_TransmissionBoost*(1.0 - dot(viewDirection,lightDirection)))*_LightColor0.rgb));
                float3 backLight = pow(max(0.0, -NdotL ), _Albedo_MaskA_var.rgb) * node_7194;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 NdotLWrap = max(0,NdotL);
                float nlPow5 = Pow5(1-NdotLWrap);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (node_7194*(attenuation*0.2));
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Albedo_MaskA; uniform float4 _Albedo_MaskA_ST;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float4 _Spec_color;
            uniform float _Gloss;
            uniform fixed _SwitchRow;
            uniform fixed _SwitchColumn;
            uniform fixed _WorldSpaceOffset;
            uniform float _TransmissionBoost;
            uniform float _Motion;
            uniform sampler2D _MotionPower; uniform float4 _MotionPower_ST;
            uniform float _Motion_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1387 = _Time;
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((o.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(o.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float4 _MotionPower_var = tex2Dlod(_MotionPower,float4(TRANSFORM_TEX(node_3232, _MotionPower),0.0,0));
                v.vertex.xyz += (v.normal*((sin(((_Motion_Speed*node_1387.g)+(mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)))*_Motion)*_MotionPower_var.r*_MotionPower_var.g*_MotionPower_var.b));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((i.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(i.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float3 _NormalMap_var = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_3232, _NormalMap)));
                float3 normalLocal = _NormalMap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _Albedo_MaskA_var = tex2D(_Albedo_MaskA,TRANSFORM_TEX(node_3232, _Albedo_MaskA));
                clip(_Albedo_MaskA_var.a - 0.5);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Gloss;
                float perceptualRoughness = 1.0 - _Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = _Spec_color.rgb;
                float specularMonochrome;
                float3 diffuseColor = _Albedo_MaskA_var.rgb; // Need this for specular when using metallic
                diffuseColor = EnergyConservationBetweenDiffuseAndSpecular(diffuseColor, specularColor, specularMonochrome);
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = pow(max(0.0, NdotL ), _Albedo_MaskA_var.rgb);
                float3 node_7194 = (_Albedo_MaskA_var.rgb*((_TransmissionBoost*(1.0 - dot(viewDirection,lightDirection)))*_LightColor0.rgb));
                float3 backLight = pow(max(0.0, -NdotL ), _Albedo_MaskA_var.rgb) * node_7194;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 NdotLWrap = max(0,NdotL);
                float nlPow5 = Pow5(1-NdotLWrap);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                diffuseColor *= 1-specularMonochrome;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Albedo_MaskA; uniform float4 _Albedo_MaskA_ST;
            uniform fixed _SwitchRow;
            uniform fixed _SwitchColumn;
            uniform fixed _WorldSpaceOffset;
            uniform float _Motion;
            uniform sampler2D _MotionPower; uniform float4 _MotionPower_ST;
            uniform float _Motion_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 posWorld : TEXCOORD4;
                float3 normalDir : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1387 = _Time;
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((o.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(o.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float4 _MotionPower_var = tex2Dlod(_MotionPower,float4(TRANSFORM_TEX(node_3232, _MotionPower),0.0,0));
                v.vertex.xyz += (v.normal*((sin(((_Motion_Speed*node_1387.g)+(mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)))*_Motion)*_MotionPower_var.r*_MotionPower_var.g*_MotionPower_var.b));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((i.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(i.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float4 _Albedo_MaskA_var = tex2D(_Albedo_MaskA,TRANSFORM_TEX(node_3232, _Albedo_MaskA));
                clip(_Albedo_MaskA_var.a - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _Albedo_MaskA; uniform float4 _Albedo_MaskA_ST;
            uniform float4 _Spec_color;
            uniform float _Gloss;
            uniform fixed _SwitchRow;
            uniform fixed _SwitchColumn;
            uniform fixed _WorldSpaceOffset;
            uniform float _TransmissionBoost;
            uniform float _Motion;
            uniform sampler2D _MotionPower; uniform float4 _MotionPower_ST;
            uniform float _Motion_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                float4 node_1387 = _Time;
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((o.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(o.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float4 _MotionPower_var = tex2Dlod(_MotionPower,float4(TRANSFORM_TEX(node_3232, _MotionPower),0.0,0));
                v.vertex.xyz += (v.normal*((sin(((_Motion_Speed*node_1387.g)+(mul(unity_ObjectToWorld, v.vertex).r+mul(unity_ObjectToWorld, v.vertex).b)))*_Motion)*_MotionPower_var.r*_MotionPower_var.g*_MotionPower_var.b));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_2604 = 0.0;
                float node_9446 = 0.5;
                float node_7274 = 0.5;
                float node_3006_if_leA = step(frac(objPos.r),node_7274);
                float node_3006_if_leB = step(node_7274,frac(objPos.r));
                float node_3584 = 1.0;
                float node_9117_if_leA = step(frac(objPos.b),node_7274);
                float node_9117_if_leB = step(node_7274,frac(objPos.b));
                float2 node_7840 = lerp( 0.0, float2(lerp((node_3006_if_leA*node_7274)+(node_3006_if_leB*node_3584),node_7274,node_3006_if_leA*node_3006_if_leB),lerp((node_9117_if_leA*node_7274)+(node_9117_if_leB*node_3584),node_7274,node_9117_if_leA*node_9117_if_leB)), _WorldSpaceOffset ).rg;
                float2 node_3232 = float2((i.uv0.r+lerp( node_2604, node_9446, _SwitchRow )+node_7840.r),(i.uv0.g+lerp( node_2604, node_9446, _SwitchColumn )+node_7840.g));
                float4 _Albedo_MaskA_var = tex2D(_Albedo_MaskA,TRANSFORM_TEX(node_3232, _Albedo_MaskA));
                float3 node_7194 = (_Albedo_MaskA_var.rgb*((_TransmissionBoost*(1.0 - dot(viewDirection,lightDirection)))*_LightColor0.rgb));
                o.Emission = (node_7194*(attenuation*0.2));
                
                float3 diffColor = _Albedo_MaskA_var.rgb;
                float3 specColor = _Spec_color.rgb;
                float specularMonochrome = max(max(specColor.r, specColor.g),specColor.b);
                diffColor *= (1.0-specularMonochrome);
                float roughness = 1.0 - _Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
