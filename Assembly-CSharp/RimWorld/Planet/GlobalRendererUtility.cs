using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C6E RID: 15470
	public static class GlobalRendererUtility
	{
		// Token: 0x0601502A RID: 86058 RVA: 0x0064A864 File Offset: 0x00648A64
		public static void UpdateGlobalShadersParams()
		{
			int gameTime = ShaderPropertyIDs.GameTime;
			TickManager tickManager = Find.TickManager;
			int? num = (tickManager != null) ? new int?(tickManager.TicksGame) : null;
			Shader.SetGlobalFloat(gameTime, ((num != null) ? new float?((float)num.GetValueOrDefault() / 60f) : null).GetValueOrDefault());
		}
	}
}
