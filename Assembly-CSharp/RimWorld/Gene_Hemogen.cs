using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200383F RID: 14399
	public class Gene_Hemogen : Gene_Resource, IGeneResourceDrain
	{
		// Token: 0x170032C3 RID: 12995
		// (get) Token: 0x06013E88 RID: 81544 RVA: 0x000FD030 File Offset: 0x000FB230
		public Gene_Resource Resource
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170032C4 RID: 12996
		// (get) Token: 0x06013E89 RID: 81545 RVA: 0x005E8C53 File Offset: 0x005E6E53
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x170032C5 RID: 12997
		// (get) Token: 0x06013E8A RID: 81546 RVA: 0x005E8C5B File Offset: 0x005E6E5B
		public bool CanOffset
		{
			get
			{
				return this.Active && !this.pawn.Deathresting;
			}
		}

		// Token: 0x170032C6 RID: 12998
		// (get) Token: 0x06013E8B RID: 81547 RVA: 0x005E8C75 File Offset: 0x005E6E75
		public string DisplayLabel
		{
			get
			{
				return this.Label + " (" + "Gene".Translate() + ")";
			}
		}

		// Token: 0x170032C7 RID: 12999
		// (get) Token: 0x06013E8C RID: 81548 RVA: 0x005E8CA5 File Offset: 0x005E6EA5
		public float ResourceLossPerDay
		{
			get
			{
				return this.def.resourceLossPerDay;
			}
		}

		// Token: 0x170032C8 RID: 13000
		// (get) Token: 0x06013E8D RID: 81549 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public override float InitialResourceMax
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170032C9 RID: 13001
		// (get) Token: 0x06013E8E RID: 81550 RVA: 0x0012950E File Offset: 0x0012770E
		public override float MinLevelForAlert
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x170032CA RID: 13002
		// (get) Token: 0x06013E8F RID: 81551 RVA: 0x0031CBE4 File Offset: 0x0031ADE4
		public override float MaxLevelOffset
		{
			get
			{
				return 0.1f;
			}
		}

		// Token: 0x170032CB RID: 13003
		// (get) Token: 0x06013E90 RID: 81552 RVA: 0x005E8CB4 File Offset: 0x005E6EB4
		protected override Color BarColor
		{
			get
			{
				return new ColorInt(138, 3, 3).ToColor;
			}
		}

		// Token: 0x170032CC RID: 13004
		// (get) Token: 0x06013E91 RID: 81553 RVA: 0x005E8CD8 File Offset: 0x005E6ED8
		protected override Color BarHighlightColor
		{
			get
			{
				return new ColorInt(145, 42, 42).ToColor;
			}
		}

		// Token: 0x06013E92 RID: 81554 RVA: 0x005E8CFB File Offset: 0x005E6EFB
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Hemogen"))
			{
				return;
			}
			base.PostAdd();
			this.Reset();
		}

		// Token: 0x06013E93 RID: 81555 RVA: 0x005E8D18 File Offset: 0x005E6F18
		public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (thing.def.IsMeat)
			{
				IngestibleProperties ingestible = thing.def.ingestible;
				bool flag;
				if (ingestible == null)
				{
					flag = false;
				}
				else
				{
					ThingDef sourceDef = ingestible.sourceDef;
					bool? flag2;
					if (sourceDef == null)
					{
						flag2 = null;
					}
					else
					{
						RaceProperties race = sourceDef.race;
						flag2 = ((race != null) ? new bool?(race.Humanlike) : null);
					}
					bool? flag3 = flag2;
					bool flag4 = true;
					flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
				}
				if (flag)
				{
					GeneUtility.OffsetHemogen(this.pawn, 0.0375f * thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)numTaken, true);
				}
			}
		}

		// Token: 0x06013E94 RID: 81556 RVA: 0x005E8DAF File Offset: 0x005E6FAF
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			GeneResourceDrainUtility.TickResourceDrainInterval(this, delta);
		}

		// Token: 0x06013E95 RID: 81557 RVA: 0x005E8DBF File Offset: 0x005E6FBF
		public override void SetTargetValuePct(float val)
		{
			this.targetValue = Mathf.Clamp(val * this.Max, 0f, this.Max - this.MaxLevelOffset);
		}

		// Token: 0x06013E96 RID: 81558 RVA: 0x005E8DE6 File Offset: 0x005E6FE6
		public bool ShouldConsumeHemogenNow()
		{
			return this.Value < this.targetValue;
		}

		// Token: 0x06013E97 RID: 81559 RVA: 0x005E8DF6 File Offset: 0x005E6FF6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!this.Active)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06013E98 RID: 81560 RVA: 0x005E8E06 File Offset: 0x005E7006
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.hemogenPacksAllowed, "hemogenPacksAllowed", true, false);
		}

		// Token: 0x0400DC03 RID: 56323
		public bool hemogenPacksAllowed = true;
	}
}
