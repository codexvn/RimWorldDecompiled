using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D41 RID: 7489
	public class GameCondition_HateChantDrone : GameCondition
	{
		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x0600A7C4 RID: 42948 RVA: 0x00300ED4 File Offset: 0x002FF0D4
		public override string Label
		{
			get
			{
				return this.def.label + ": " + this.level.GetLabel().CapitalizeFirst();
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x0600A7C5 RID: 42949 RVA: 0x00300EFB File Offset: 0x002FF0FB
		public override string LetterText
		{
			get
			{
				return this.def.letterText.Formatted(this.level.GetLabel());
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x0600A7C6 RID: 42950 RVA: 0x00300EAA File Offset: 0x002FF0AA
		public override int TransitionTicks
		{
			get
			{
				return 180;
			}
		}

		// Token: 0x0600A7C7 RID: 42951 RVA: 0x00300F22 File Offset: 0x002FF122
		public override void PostMake()
		{
			base.PostMake();
			base.Permanent = true;
		}

		// Token: 0x0600A7C8 RID: 42952 RVA: 0x00300F31 File Offset: 0x002FF131
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Hate chant drone"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x0600A7C9 RID: 42953 RVA: 0x00300F4C File Offset: 0x002FF14C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<HateChantDroneLevel>(ref this.level, "level", HateChantDroneLevel.None, false);
		}

		// Token: 0x0600A7CA RID: 42954 RVA: 0x003000A3 File Offset: 0x002FE2A3
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A7CB RID: 42955 RVA: 0x00300F68 File Offset: 0x002FF168
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

		// Token: 0x0600A7CC RID: 42956 RVA: 0x00300FC8 File Offset: 0x002FF1C8
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x0600A7CD RID: 42957 RVA: 0x00300FFD File Offset: 0x002FF1FD
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x0400742B RID: 29739
		public HateChantDroneLevel level = HateChantDroneLevel.VeryLow;

		// Token: 0x0400742C RID: 29740
		private List<SkyOverlay> overlays = new List<SkyOverlay>();
	}
}
