using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D50 RID: 7504
	public class GameCondition_UnnaturalHeat : GameCondition
	{
		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x0600A82F RID: 43055 RVA: 0x00302558 File Offset: 0x00300758
		private int TargetTempOffset
		{
			get
			{
				int num = 0;
				foreach (Building thing in base.SingleMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.AtmosphericHeater))
				{
					num += thing.TryGetComp<Comp_AtmosphericHeater>().TempOffset;
				}
				return num;
			}
		}

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x0600A830 RID: 43056 RVA: 0x003010BF File Offset: 0x002FF2BF
		public override int TransitionTicks
		{
			get
			{
				return 2500;
			}
		}

		// Token: 0x0600A831 RID: 43057 RVA: 0x003025C4 File Offset: 0x003007C4
		public override void PostMake()
		{
			base.PostMake();
			this.tempOffset = 0f;
		}

		// Token: 0x0600A832 RID: 43058 RVA: 0x003025D7 File Offset: 0x003007D7
		public override float TemperatureOffset()
		{
			if (!base.Permanent)
			{
				return Mathf.Lerp(0f, this.tempOffset, Mathf.Min(1f, (float)base.TicksLeft / (float)this.TransitionTicks));
			}
			return this.tempOffset;
		}

		// Token: 0x0600A833 RID: 43059 RVA: 0x00302614 File Offset: 0x00300814
		public override void GameConditionTick()
		{
			this.tempOffset += Mathf.Sign((float)this.TargetTempOffset - this.tempOffset) * 0.001f;
			if (base.SingleMap.listerThings.ThingsOfDef(ThingDefOf.AtmosphericHeater).Count == 0)
			{
				base.Permanent = false;
			}
		}

		// Token: 0x0600A834 RID: 43060 RVA: 0x0030266A File Offset: 0x0030086A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.tempOffset, "tempOffset", 0f, false);
			Scribe_Values.Look<float>(ref this.heaterTargetTemp, "heaterTargetTemp", 20f, false);
		}

		// Token: 0x04007457 RID: 29783
		private const float TempChangePerTick = 0.001f;

		// Token: 0x04007458 RID: 29784
		private const float DefaultTargetTemp = 20f;

		// Token: 0x04007459 RID: 29785
		private float tempOffset;

		// Token: 0x0400745A RID: 29786
		public float heaterTargetTemp = 20f;
	}
}
