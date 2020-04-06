// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WorldSpaceTrees/BarkSwitcherAS"
{
	Properties
	{
		_Base_Color("Base_Color", Color) = (1,1,1,0)
		_Albedo("Albedo", 2D) = "white" {}
		[Toggle]_ToggleSwitch0("Toggle Switch0", Float) = 0
		_NormalMap("NormalMap", 2D) = "bump" {}
		_DarknessOnSundown("DarknessOnSundown", Range( 0 , 1)) = 0.75
		_Gloss("Gloss", Range( 0 , 1)) = 0
		_AO_power("AO_power", Range( 0 , 1)) = 0.8
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _NormalMap;
		uniform float _ToggleSwitch0;
		uniform float _DarknessOnSundown;
		uniform float4 _Base_Color;
		uniform sampler2D _Albedo;
		uniform float _Gloss;
		uniform float _AO_power;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_output_12_0 = ( i.uv_texcoord + lerp(0.0,0.5,_ToggleSwitch0) );
			float3 tex2DNode17 = UnpackNormal( tex2D( _NormalMap, temp_output_12_0 ) );
			o.Normal = tex2DNode17;
			float3 ase_worldPos = i.worldPos;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = Unity_SafeNormalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float dotResult4 = dot( float3(0,1,0) , ase_worldlightDir );
			float clampResult6 = clamp( dotResult4 , 0.0 , 1.0 );
			float3 temp_cast_0 = (clampResult6).xxx;
			float temp_output_2_0_g3 = _DarknessOnSundown;
			float temp_output_3_0_g3 = ( 1.0 - temp_output_2_0_g3 );
			float3 appendResult7_g3 = (float3(temp_output_3_0_g3 , temp_output_3_0_g3 , temp_output_3_0_g3));
			o.Albedo = ( float4( ( ( temp_cast_0 * temp_output_2_0_g3 ) + appendResult7_g3 ) , 0.0 ) * ( _Base_Color * tex2D( _Albedo, temp_output_12_0 ) ) ).rgb;
			o.Smoothness = _Gloss;
			float lerpResult20 = lerp( tex2DNode17.g , tex2DNode17.b , 0.4);
			o.Occlusion = ( lerpResult20 * _AO_power );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16200
318;399;1906;1014;2073.093;555.8198;1.648085;True;False
Node;AmplifyShaderEditor.RangedFloatNode;14;-2808.011,260.4688;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-2812.011,336.4688;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;1;-1891.364,-575.9447;Float;False;1509.034;413.6017;SundownDarkness;6;7;6;5;4;3;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ToggleSwitchNode;13;-2522.885,272.5485;Float;False;Property;_ToggleSwitch0;Toggle Switch0;2;0;Create;True;0;0;False;0;0;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-2513.892,71.9126;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;2;-1841.364,-341.343;Float;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;3;-1770.815,-518.0313;Float;False;Constant;_Vector0;Vector 0;15;0;Create;True;0;0;False;0;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;4;-1573.718,-417.943;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;-2014.152,124.9843;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;17;-1728,317.299;Float;True;Property;_NormalMap;NormalMap;3;0;Create;True;0;0;False;0;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;8;-1683.867,-75.08652;Float;False;Property;_Base_Color;Base_Color;0;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-1730,101.4229;Float;True;Property;_Albedo;Albedo;1;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;6;-1031.42,-450.3517;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1212.661,-249.2265;Float;False;Property;_DarknessOnSundown;DarknessOnSundown;4;0;Create;True;0;0;False;0;0.75;0.74;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1230.56,31.22041;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;7;-593.3298,-399.5216;Float;False;Lerp White To;-1;;3;047d7c189c36a62438973bad9d37b1c2;0;2;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1252.23,574.6376;Float;False;Property;_AO_power;AO_power;6;0;Create;True;0;0;False;0;0.8;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;20;-1210.489,426.9857;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-306.3452,-199.8334;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-821.2452,298.4671;Float;False;Property;_Gloss;Gloss;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-752.6954,460.8367;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;WorldSpaceTrees/BarkSwitcherAS;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;14;0
WireConnection;13;1;15;0
WireConnection;4;0;3;0
WireConnection;4;1;2;0
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;17;1;12;0
WireConnection;9;1;12;0
WireConnection;6;0;4;0
WireConnection;10;0;8;0
WireConnection;10;1;9;0
WireConnection;7;1;6;0
WireConnection;7;2;5;0
WireConnection;20;0;17;2
WireConnection;20;1;17;3
WireConnection;23;0;7;0
WireConnection;23;1;10;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;0;0;23;0
WireConnection;0;1;17;0
WireConnection;0;4;19;0
WireConnection;0;5;21;0
ASEEND*/
//CHKSM=7FEA33E68B83151AA189C63FE1596DEFB96989E3