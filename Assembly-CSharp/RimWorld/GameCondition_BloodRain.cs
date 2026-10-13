using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D37 RID: 7479
	public class GameCondition_BloodRain : GameCondition_ForceWeather
	{
		// Token: 0x170019E3 RID: 6627
		// (get) Token: 0x0600A794 RID: 42900 RVA: 0x00222DEB File Offset: 0x00220FEB
		public override int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x0600A795 RID: 42901 RVA: 0x003000A3 File Offset: 0x002FE2A3
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A796 RID: 42902 RVA: 0x0030048D File Offset: 0x002FE68D
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Blood rain"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x0600A797 RID: 42903 RVA: 0x003004A8 File Offset: 0x002FE6A8
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			List<Map> affectedMaps = base.AffectedMaps;
			for (int i = 0; i < this.overlays.Count; i++)
			{
				for (int j = 0; j < affectedMaps.Count; j++)
				{
					this.overlays[i].TickOverlay(affectedMaps[j], 1f);
				}
			}
		}

		// Token: 0x0600A798 RID: 42904 RVA: 0x00300508 File Offset: 0x002FE708
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x0600A799 RID: 42905 RVA: 0x0030053D File Offset: 0x002FE73D
		public override void End()
		{
			base.End();
			base.SingleMap.weatherDecider.StartNextWeather();
		}

		// Token: 0x04007406 RID: 29702
		private List<SkyOverlay> overlays = new List<SkyOverlay>();
	}
}
