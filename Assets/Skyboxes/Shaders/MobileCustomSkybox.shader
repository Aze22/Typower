Shader "Mobile/CustomSkybox" {
Properties {
	_MobText ("Mobile Skybox", 2D) = "white" {}
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" }
	Cull Off ZWrite Off Fog { Mode Off }
	Pass {
		SetTexture [_MobText] { combine texture }
	}
}
}
