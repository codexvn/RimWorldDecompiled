using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200075C RID: 1884
	public class Hediff_PainField : HediffWithComps
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x000ED828 File Offset: 0x000EBA28
		public HediffComp_PainField PainComp
		{
			get
			{
				HediffComp_PainField result;
				if ((result = this.painComp) == null)
				{
					result = (this.painComp = base.GetComp<HediffComp_PainField>());
				}
				return result;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002E2D RID: 11821 RVA: 0x000ED850 File Offset: 0x000EBA50
		public HediffComp_Link LinkComp
		{
			get
			{
				HediffComp_Link result;
				if ((result = this.linkComp) == null)
				{
					result = (this.linkComp = base.GetComp<HediffComp_Link>());
				}
				return result;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000ED876 File Offset: 0x000EBA76
		public Pawn SourcePawn
		{
			get
			{
				return this.LinkComp.OtherPawn;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002E2F RID: 11823 RVA: 0x000ED883 File Offset: 0x000EBA83
		public override float PainOffset
		{
			get
			{
				return this.Severity;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002E30 RID: 11824 RVA: 0x000E4FF4 File Offset: 0x000E31F4
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent();
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002E31 RID: 11825 RVA: 0x000ED88C File Offset: 0x000EBA8C
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000ED8CD File Offset: 0x000EBACD
		public override float Severity
		{
			get
			{
				return this.painSeverity;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x000ED8D8 File Offset: 0x000EBAD8
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.def.Description);
				if (this.SourcePawn == null)
				{
					return stringBuilder.ToString();
				}
				CompActivity comp = this.SourcePawn.GetComp<CompActivity>();
				if (comp != null)
				{
					float f = this.PainComp.Props.activityMultiplier.Evaluate(comp.ActivityLevel);
					stringBuilder.Append(string.Format("\n{0}: x{1}", "ActivityLevelDesc".Translate().CapitalizeFirst(), f.ToStringPercent("0")));
				}
				if (this.PainComp.Props.disableWhenSuppressed && (this.SourcePawn.health.hediffSet.IsPsychicallySuppressed || this.pawn.health.hediffSet.IsPsychicallySuppressed))
				{
					stringBuilder.Append(string.Format("\n{0}: x0", "PsychicallySuppressedDesc".Translate().CapitalizeFirst()));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000ED9D8 File Offset: 0x000EBBD8
		public override void PostMake()
		{
			base.PostMake();
			if (this.PainComp == null)
			{
				Log.ErrorOnce("Attempted to create a pain field hediff but missing the HediffComp_PainField component", 94756930);
			}
			if (this.LinkComp == null)
			{
				Log.ErrorOnce("Attempted to create a pain field hediff but missing the Hediff_Link component", 47836289);
			}
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x000EDA0E File Offset: 0x000EBC0E
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Pain field"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000EDA38 File Offset: 0x000EBC38
		public override void PostTickInterval(int delta)
		{
			float num = this.painSeverity;
			this.goal = this.CalculateSeverity(delta);
			base.PostTickInterval(delta);
			this.painSeverity = Mathf.SmoothDamp(this.painSeverity, this.goal, ref this.vel, 0.25f, 10f, 0.016666668f);
			this.painSeverity = Mathf.Clamp01(this.painSeverity);
			if (Math.Abs(num - this.painSeverity) > Mathf.Epsilon)
			{
				this.pawn.health.hediffSet.DirtyPainCache();
				this.pawn.health.CheckForStateChange(null, this);
			}
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000EDAE0 File Offset: 0x000EBCE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.painSeverity, "painSeverity", 0f, false);
			Scribe_Values.Look<float>(ref this.goal, "goal", 0f, false);
			Scribe_Values.Look<float>(ref this.vel, "vel", 0f, false);
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000EDB38 File Offset: 0x000EBD38
		private float CalculateSeverity(int delta)
		{
			if (this.SourcePawn == null)
			{
				return 0f;
			}
			if (this.PainComp.Props.disableWhenSuppressed && (this.SourcePawn.health.hediffSet.IsPsychicallySuppressed || this.pawn.health.hediffSet.IsPsychicallySuppressed))
			{
				return 0f;
			}
			if (this.psychicSensitivityCached < 0f || this.pawn.IsHashIntervalTick(300, delta))
			{
				this.psychicSensitivityCached = this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
			}
			float num = this.pawn.PositionHeld.DistanceTo(this.SourcePawn.PositionHeld);
			float num2 = 0f;
			if (num < this.PainComp.Props.painDistance + 0.1f)
			{
				num2 = this.PainComp.Props.painInRange;
			}
			CompActivity compActivity;
			if (this.SourcePawn.TryGetComp(out compActivity))
			{
				if (compActivity.IsActive)
				{
					num2 = Mathf.Max(this.PainComp.Props.activatedMinimum, num2);
				}
				num2 *= this.PainComp.Props.activityMultiplier.Evaluate(compActivity.ActivityLevel);
			}
			num2 *= this.psychicSensitivityCached * this.PainComp.Props.psychicSensitivityMultiplier;
			return Mathf.Clamp01(num2);
		}

		// Token: 0x04002451 RID: 9297
		public float painSeverity;

		// Token: 0x04002452 RID: 9298
		public float goal;

		// Token: 0x04002453 RID: 9299
		public float vel;

		// Token: 0x04002454 RID: 9300
		private const float SeveritySmoothTime = 0.25f;

		// Token: 0x04002455 RID: 9301
		private HediffComp_PainField painComp;

		// Token: 0x04002456 RID: 9302
		private HediffComp_Link linkComp;

		// Token: 0x04002457 RID: 9303
		private float psychicSensitivityCached = -1f;
	}
}
