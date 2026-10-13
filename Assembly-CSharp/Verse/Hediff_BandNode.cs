using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000737 RID: 1847
	public class Hediff_BandNode : Hediff
	{
		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x000E85D4 File Offset: 0x000E67D4
		public int AdditionalBandwidth
		{
			get
			{
				return this.cachedTunedBandNodesCount;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x000E85DC File Offset: 0x000E67DC
		public override bool ShouldRemove
		{
			get
			{
				return this.cachedTunedBandNodesCount == 0;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x000E85E8 File Offset: 0x000E67E8
		public override HediffStage CurStage
		{
			get
			{
				if (this.curStage == null && this.cachedTunedBandNodesCount > 0)
				{
					StatModifier statModifier = new StatModifier();
					statModifier.stat = StatDefOf.MechBandwidth;
					statModifier.value = (float)this.cachedTunedBandNodesCount;
					this.curStage = new HediffStage();
					this.curStage.statOffsets = new List<StatModifier>
					{
						statModifier
					};
				}
				return this.curStage;
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x000E864C File Offset: 0x000E684C
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.pawn.IsHashIntervalTick(60, delta))
			{
				this.RecacheBandNodes();
			}
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x000E866B File Offset: 0x000E686B
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.RecacheBandNodes();
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x000E867C File Offset: 0x000E687C
		public void RecacheBandNodes()
		{
			int num = this.cachedTunedBandNodesCount;
			this.cachedTunedBandNodesCount = 0;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Building thing in maps[i].listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.BandNode))
				{
					if (thing.TryGetComp<CompBandNode>().tunedTo == this.pawn && thing.TryGetComp<CompPowerTrader>().PowerOn)
					{
						this.cachedTunedBandNodesCount++;
					}
				}
			}
			if (num != this.cachedTunedBandNodesCount)
			{
				this.curStage = null;
				Pawn_MechanitorTracker mechanitor = this.pawn.mechanitor;
				if (mechanitor == null)
				{
					return;
				}
				mechanitor.Notify_BandwidthChanged();
			}
		}

		// Token: 0x06002D03 RID: 11523 RVA: 0x000E8754 File Offset: 0x000E6954
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.cachedTunedBandNodesCount, "cachedTunedBandNodesCount", 0, false);
		}

		// Token: 0x040023CB RID: 9163
		private const int BandNodeCheckInterval = 60;

		// Token: 0x040023CC RID: 9164
		private int cachedTunedBandNodesCount;

		// Token: 0x040023CD RID: 9165
		private HediffStage curStage;
	}
}
