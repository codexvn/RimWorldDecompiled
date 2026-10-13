using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D3C RID: 3388
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x04003E55 RID: 15957
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x04003E56 RID: 15958
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x04003E57 RID: 15959
		private static readonly string PlanetOriginName = "_PlanetOrigin";

		// Token: 0x04003E58 RID: 15960
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x04003E59 RID: 15961
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04003E5A RID: 15962
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04003E5B RID: 15963
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04003E5C RID: 15964
		private static readonly string ColorName = "_Color";

		// Token: 0x04003E5D RID: 15965
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04003E5E RID: 15966
		private static readonly string SecondaryTexName = "_SecondaryTex";

		// Token: 0x04003E5F RID: 15967
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04003E60 RID: 15968
		private static readonly string MacroTexName = "_MacroTex";

		// Token: 0x04003E61 RID: 15969
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04003E62 RID: 15970
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04003E63 RID: 15971
		private static readonly string AgeSecsName = "_AgeSecs";

		// Token: 0x04003E64 RID: 15972
		private static readonly string RandomPerObjectName = "_RandomPerObject";

		// Token: 0x04003E65 RID: 15973
		private static readonly string RandomPerObjectOffsetRandomName = "_RandomPerObjectOffsetRandom";

		// Token: 0x04003E66 RID: 15974
		private static readonly string RotationName = "_Rotation";

		// Token: 0x04003E67 RID: 15975
		private static readonly string OverlayOpacityName = "_OverlayOpacity";

		// Token: 0x04003E68 RID: 15976
		private static readonly string OverlayColorName = "_OverlayColor";

		// Token: 0x04003E69 RID: 15977
		private static readonly string AgeSecsPausableName = "_AgeSecsPausable";

		// Token: 0x04003E6A RID: 15978
		private static readonly string MainTextureOffsetName = "_Main_TexOffset";

		// Token: 0x04003E6B RID: 15979
		private static readonly string MainTextureScaleName = "_Main_TexScale";

		// Token: 0x04003E6C RID: 15980
		private static readonly string MaskTextureOffsetName = "_Mask_TexOffset";

		// Token: 0x04003E6D RID: 15981
		private static readonly string MaskTextureScaleName = "_Mask_TexScale";

		// Token: 0x04003E6E RID: 15982
		private static readonly string ShockwaveColorName = "_ShockwaveColor";

		// Token: 0x04003E6F RID: 15983
		private static readonly string WaterCastVectSunName = "_WaterCastVectSun";

		// Token: 0x04003E70 RID: 15984
		private static readonly string WaterCastVectMoonName = "_WaterCastVectMoon";

		// Token: 0x04003E71 RID: 15985
		private static readonly string WaterOutputTexName = "_WaterOutputTex";

		// Token: 0x04003E72 RID: 15986
		private static readonly string WaterOffsetTexName = "_WaterOffsetTex";

		// Token: 0x04003E73 RID: 15987
		private static readonly string WaterReflectionTexName = "_WaterReflectionTex";

		// Token: 0x04003E74 RID: 15988
		private static readonly string ShadowCompositeTexName = "_ShadowCompositeTex";

		// Token: 0x04003E75 RID: 15989
		private static readonly string FallIntensityName = "_FallIntensity";

		// Token: 0x04003E76 RID: 15990
		private static readonly string MainTex_STName = "_MainTex_ST";

		// Token: 0x04003E77 RID: 15991
		private static readonly string SquashNStretchName = "_SquashNStretch";

		// Token: 0x04003E78 RID: 15992
		private static readonly string WorkingName = "_Working";

		// Token: 0x04003E79 RID: 15993
		private static readonly string FadeTexName = "_FadeTex";

		// Token: 0x04003E7A RID: 15994
		private static readonly string FadeTexScrollSpeedName = "_FadeTexScrollSpeed";

		// Token: 0x04003E7B RID: 15995
		private static readonly string ScrollSpeedName = "_ScrollSpeed";

		// Token: 0x04003E7C RID: 15996
		private static readonly string FadeTexScaleName = "_FadeTexScale";

		// Token: 0x04003E7D RID: 15997
		private static readonly string PollutedTexName = "_PollutedTex";

		// Token: 0x04003E7E RID: 15998
		private static readonly string IsPollutedName = "_IsPolluted";

		// Token: 0x04003E7F RID: 15999
		private static readonly string ToxGasTexName = "_ToxGasTex";

		// Token: 0x04003E80 RID: 16000
		private static readonly string RotGasTexName = "_RotGasTex";

		// Token: 0x04003E81 RID: 16001
		private static readonly string DeadlifeDustTexName = "_DeadlifeDustTex";

		// Token: 0x04003E82 RID: 16002
		private static readonly string DestructionStartAgeName = "_DestructionStartAgeSecs";

		// Token: 0x04003E83 RID: 16003
		private static readonly string PawnCenterWorldName = "_pawnCenterWorld";

		// Token: 0x04003E84 RID: 16004
		private static readonly string PawnDrawSizeWorldName = "_pawnDrawSizeWorld";

		// Token: 0x04003E85 RID: 16005
		private static readonly string AlphaAddTexName = "_AlphaAddTex";

		// Token: 0x04003E86 RID: 16006
		private static readonly string BurnTexName = "_BurnTex";

		// Token: 0x04003E87 RID: 16007
		private static readonly string BurnColorName = "_BurnColor";

		// Token: 0x04003E88 RID: 16008
		private static readonly string BurnScaleName = "_BurnScale";

		// Token: 0x04003E89 RID: 16009
		private static readonly string PollutionTintColorName = "_PollutionTintColor";

		// Token: 0x04003E8A RID: 16010
		private static readonly string MapSizeName = "_MapSize";

		// Token: 0x04003E8B RID: 16011
		private static readonly string NoiseTexName = "_NoiseTex";

		// Token: 0x04003E8C RID: 16012
		private static readonly string SunsetColorRampName = "_SunsetColorRamp";

		// Token: 0x04003E8D RID: 16013
		private static readonly string CloudMapName = "_CloudTex";

		// Token: 0x04003E8E RID: 16014
		private static readonly string CloudShaderOpacityName = "_FinalOpacityMult";

		// Token: 0x04003E8F RID: 16015
		private static readonly string GameTimeName = "_GameTime";

		// Token: 0x04003E90 RID: 16016
		private static readonly string BackgroundModeEnabledName = "_BackgroundModeEnabled";

		// Token: 0x04003E91 RID: 16017
		private static readonly string ProgressName = "_Progress";

		// Token: 0x04003E92 RID: 16018
		private static readonly string DrawPosName = "_DrawPos";

		// Token: 0x04003E93 RID: 16019
		private static readonly string SeedName = "_Seed";

		// Token: 0x04003E94 RID: 16020
		private static readonly string MainCameraScreenParamsName = "_MainCameraScreenParams";

		// Token: 0x04003E95 RID: 16021
		private static readonly string MainCameraVPName = "_MainCameraVP";

		// Token: 0x04003E96 RID: 16022
		public static readonly int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x04003E97 RID: 16023
		public static readonly int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x04003E98 RID: 16024
		public static readonly int PlanetOrigin = Shader.PropertyToID(ShaderPropertyIDs.PlanetOriginName);

		// Token: 0x04003E99 RID: 16025
		public static readonly int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x04003E9A RID: 16026
		public static readonly int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x04003E9B RID: 16027
		public static readonly int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x04003E9C RID: 16028
		public static readonly int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04003E9D RID: 16029
		public static readonly int AgeSecs = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsName);

		// Token: 0x04003E9E RID: 16030
		public static readonly int AgeSecsPausable = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsPausableName);

		// Token: 0x04003E9F RID: 16031
		public static readonly int RandomPerObject = Shader.PropertyToID(ShaderPropertyIDs.RandomPerObjectName);

		// Token: 0x04003EA0 RID: 16032
		public static readonly int RandomPerObjectOffsetRandom = Shader.PropertyToID(ShaderPropertyIDs.RandomPerObjectOffsetRandomName);

		// Token: 0x04003EA1 RID: 16033
		public static readonly int Rotation = Shader.PropertyToID(ShaderPropertyIDs.RotationName);

		// Token: 0x04003EA2 RID: 16034
		public static readonly int OverlayOpacity = Shader.PropertyToID(ShaderPropertyIDs.OverlayOpacityName);

		// Token: 0x04003EA3 RID: 16035
		public static readonly int OverlayColor = Shader.PropertyToID(ShaderPropertyIDs.OverlayColorName);

		// Token: 0x04003EA4 RID: 16036
		public static readonly int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04003EA5 RID: 16037
		public static readonly int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04003EA6 RID: 16038
		public static readonly int SecondaryTex = Shader.PropertyToID(ShaderPropertyIDs.SecondaryTexName);

		// Token: 0x04003EA7 RID: 16039
		public static readonly int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x04003EA8 RID: 16040
		public static readonly int MacroTex = Shader.PropertyToID(ShaderPropertyIDs.MacroTexName);

		// Token: 0x04003EA9 RID: 16041
		public static readonly int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x04003EAA RID: 16042
		public static readonly int ShockwaveColor = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveColorName);

		// Token: 0x04003EAB RID: 16043
		public static readonly int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x04003EAC RID: 16044
		public static readonly int WaterCastVectSun = Shader.PropertyToID(ShaderPropertyIDs.WaterCastVectSunName);

		// Token: 0x04003EAD RID: 16045
		public static readonly int WaterCastVectMoon = Shader.PropertyToID(ShaderPropertyIDs.WaterCastVectMoonName);

		// Token: 0x04003EAE RID: 16046
		public static readonly int WaterOutputTex = Shader.PropertyToID(ShaderPropertyIDs.WaterOutputTexName);

		// Token: 0x04003EAF RID: 16047
		public static readonly int WaterOffsetTex = Shader.PropertyToID(ShaderPropertyIDs.WaterOffsetTexName);

		// Token: 0x04003EB0 RID: 16048
		public static readonly int WaterReflectionTex = Shader.PropertyToID(ShaderPropertyIDs.WaterReflectionTexName);

		// Token: 0x04003EB1 RID: 16049
		public static readonly int ShadowCompositeTex = Shader.PropertyToID(ShaderPropertyIDs.ShadowCompositeTexName);

		// Token: 0x04003EB2 RID: 16050
		public static readonly int FallIntensity = Shader.PropertyToID(ShaderPropertyIDs.FallIntensityName);

		// Token: 0x04003EB3 RID: 16051
		public static readonly int MainTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MainTextureOffsetName);

		// Token: 0x04003EB4 RID: 16052
		public static readonly int MainTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MainTextureScaleName);

		// Token: 0x04003EB5 RID: 16053
		public static readonly int MaskTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureOffsetName);

		// Token: 0x04003EB6 RID: 16054
		public static readonly int MaskTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureScaleName);

		// Token: 0x04003EB7 RID: 16055
		public static readonly int Tiling = Shader.PropertyToID(ShaderPropertyIDs.MainTex_STName);

		// Token: 0x04003EB8 RID: 16056
		public static readonly int SquashNStretch = Shader.PropertyToID(ShaderPropertyIDs.SquashNStretchName);

		// Token: 0x04003EB9 RID: 16057
		public static readonly int Working = Shader.PropertyToID(ShaderPropertyIDs.WorkingName);

		// Token: 0x04003EBA RID: 16058
		public static readonly int FadeTex = Shader.PropertyToID(ShaderPropertyIDs.FadeTexName);

		// Token: 0x04003EBB RID: 16059
		public static readonly int TexScrollSpeed = Shader.PropertyToID(ShaderPropertyIDs.FadeTexScrollSpeedName);

		// Token: 0x04003EBC RID: 16060
		public static readonly int ScrollSpeed = Shader.PropertyToID(ShaderPropertyIDs.ScrollSpeedName);

		// Token: 0x04003EBD RID: 16061
		public static readonly int TexScale = Shader.PropertyToID(ShaderPropertyIDs.FadeTexScaleName);

		// Token: 0x04003EBE RID: 16062
		public static readonly int PollutedTex = Shader.PropertyToID(ShaderPropertyIDs.PollutedTexName);

		// Token: 0x04003EBF RID: 16063
		public static readonly int IsPolluted = Shader.PropertyToID(ShaderPropertyIDs.IsPollutedName);

		// Token: 0x04003EC0 RID: 16064
		public static readonly int ToxGasTex = Shader.PropertyToID(ShaderPropertyIDs.ToxGasTexName);

		// Token: 0x04003EC1 RID: 16065
		public static readonly int RotGasTex = Shader.PropertyToID(ShaderPropertyIDs.RotGasTexName);

		// Token: 0x04003EC2 RID: 16066
		public static readonly int DeadlifeDustTex = Shader.PropertyToID(ShaderPropertyIDs.DeadlifeDustTexName);

		// Token: 0x04003EC3 RID: 16067
		public static readonly int VoidNodeDestructionStartAge = Shader.PropertyToID(ShaderPropertyIDs.DestructionStartAgeName);

		// Token: 0x04003EC4 RID: 16068
		public static readonly int PawnCenterWorld = Shader.PropertyToID(ShaderPropertyIDs.PawnCenterWorldName);

		// Token: 0x04003EC5 RID: 16069
		public static readonly int PawnDrawSizeWorld = Shader.PropertyToID(ShaderPropertyIDs.PawnDrawSizeWorldName);

		// Token: 0x04003EC6 RID: 16070
		public static readonly int AlphaAddTex = Shader.PropertyToID(ShaderPropertyIDs.AlphaAddTexName);

		// Token: 0x04003EC7 RID: 16071
		public static readonly int BurnTex = Shader.PropertyToID(ShaderPropertyIDs.BurnTexName);

		// Token: 0x04003EC8 RID: 16072
		public static readonly int BurnColor = Shader.PropertyToID(ShaderPropertyIDs.BurnColorName);

		// Token: 0x04003EC9 RID: 16073
		public static readonly int BurnScale = Shader.PropertyToID(ShaderPropertyIDs.BurnScaleName);

		// Token: 0x04003ECA RID: 16074
		public static readonly int PollutionTintColor = Shader.PropertyToID(ShaderPropertyIDs.PollutionTintColorName);

		// Token: 0x04003ECB RID: 16075
		public static readonly int MapSize = Shader.PropertyToID(ShaderPropertyIDs.MapSizeName);

		// Token: 0x04003ECC RID: 16076
		public static readonly int NoiseTex = Shader.PropertyToID(ShaderPropertyIDs.NoiseTexName);

		// Token: 0x04003ECD RID: 16077
		public static readonly int SunsetColorRamp = Shader.PropertyToID(ShaderPropertyIDs.SunsetColorRampName);

		// Token: 0x04003ECE RID: 16078
		public static readonly int CloudMap = Shader.PropertyToID(ShaderPropertyIDs.CloudMapName);

		// Token: 0x04003ECF RID: 16079
		public static readonly int CloudShaderOpacity = Shader.PropertyToID(ShaderPropertyIDs.CloudShaderOpacityName);

		// Token: 0x04003ED0 RID: 16080
		public static readonly int GameTime = Shader.PropertyToID(ShaderPropertyIDs.GameTimeName);

		// Token: 0x04003ED1 RID: 16081
		public static readonly int BackgroundModeEnabled = Shader.PropertyToID(ShaderPropertyIDs.BackgroundModeEnabledName);

		// Token: 0x04003ED2 RID: 16082
		public static readonly int Progress = Shader.PropertyToID(ShaderPropertyIDs.ProgressName);

		// Token: 0x04003ED3 RID: 16083
		public static readonly int DrawPos = Shader.PropertyToID(ShaderPropertyIDs.DrawPosName);

		// Token: 0x04003ED4 RID: 16084
		public static readonly int Seed = Shader.PropertyToID(ShaderPropertyIDs.SeedName);

		// Token: 0x04003ED5 RID: 16085
		public static readonly int MainCameraScreenParams = Shader.PropertyToID(ShaderPropertyIDs.MainCameraScreenParamsName);

		// Token: 0x04003ED6 RID: 16086
		public static readonly int MainCameraVP = Shader.PropertyToID(ShaderPropertyIDs.MainCameraVPName);
	}
}
