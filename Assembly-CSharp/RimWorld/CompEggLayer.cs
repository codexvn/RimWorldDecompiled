using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C6F RID: 11375
	public class CompEggLayer : ThingComp
	{
		// Token: 0x17002832 RID: 10290
		// (get) Token: 0x0600FF12 RID: 65298 RVA: 0x0049F008 File Offset: 0x0049D208
		private bool Active
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.eggLayFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable) && !pawn.Sterile() && (!ModsConfig.AnomalyActive || !pawn.IsShambler);
			}
		}

		// Token: 0x17002833 RID: 10291
		// (get) Token: 0x0600FF13 RID: 65299 RVA: 0x0049F06F File Offset: 0x0049D26F
		public bool CanLayNow
		{
			get
			{
				return this.Active && this.eggProgress >= 1f;
			}
		}

		// Token: 0x17002834 RID: 10292
		// (get) Token: 0x0600FF14 RID: 65300 RVA: 0x0049F08B File Offset: 0x0049D28B
		public bool FullyFertilized
		{
			get
			{
				return this.fertilizationCount >= this.Props.eggFertilizationCountMax;
			}
		}

		// Token: 0x17002835 RID: 10293
		// (get) Token: 0x0600FF15 RID: 65301 RVA: 0x0049F0A3 File Offset: 0x0049D2A3
		private bool ProgressStoppedBecauseUnfertilized
		{
			get
			{
				return this.Props.eggProgressUnfertilizedMax < 1f && this.fertilizationCount == 0 && this.eggProgress >= this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x17002836 RID: 10294
		// (get) Token: 0x0600FF16 RID: 65302 RVA: 0x0049F0D7 File Offset: 0x0049D2D7
		public CompProperties_EggLayer Props
		{
			get
			{
				return (CompProperties_EggLayer)this.props;
			}
		}

		// Token: 0x0600FF17 RID: 65303 RVA: 0x0049F0E4 File Offset: 0x0049D2E4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.eggProgress, "eggProgress", 0f, false);
			Scribe_Values.Look<int>(ref this.fertilizationCount, "fertilizationCount", 0, false);
			Scribe_References.Look<Pawn>(ref this.fertilizedBy, "fertilizedBy", false);
		}

		// Token: 0x0600FF18 RID: 65304 RVA: 0x0049F130 File Offset: 0x0049D330
		public override void CompTick()
		{
			if (!this.parent.IsHashIntervalTick(2500))
			{
				return;
			}
			if (!this.Active)
			{
				return;
			}
			float num = 2500f / (this.Props.eggLayIntervalDays * 60000f);
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
			}
			this.eggProgress += num;
			if (this.eggProgress > 1f)
			{
				this.eggProgress = 1f;
			}
			if (this.ProgressStoppedBecauseUnfertilized)
			{
				this.eggProgress = this.Props.eggProgressUnfertilizedMax;
			}
		}

		// Token: 0x0600FF19 RID: 65305 RVA: 0x0049F1C8 File Offset: 0x0049D3C8
		public void Fertilize(Pawn male)
		{
			this.fertilizationCount = this.Props.eggFertilizationCountMax;
			this.fertilizedBy = male;
		}

		// Token: 0x0600FF1A RID: 65306 RVA: 0x0049F1E2 File Offset: 0x0049D3E2
		public ThingDef NextEggType()
		{
			if (this.fertilizationCount > 0)
			{
				return this.Props.eggFertilizedDef;
			}
			return this.Props.eggUnfertilizedDef;
		}

		// Token: 0x0600FF1B RID: 65307 RVA: 0x0049F204 File Offset: 0x0049D404
		public virtual Thing ProduceEgg()
		{
			if (!this.Active)
			{
				string str = "LayEgg while not Active: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + ((parent != null) ? parent.ToString() : null));
			}
			this.eggProgress = 0f;
			int randomInRange = this.Props.eggCountRange.RandomInRange;
			if (randomInRange == 0)
			{
				return null;
			}
			Thing thing;
			if (this.fertilizationCount > 0)
			{
				thing = ThingMaker.MakeThing(this.Props.eggFertilizedDef, null);
				this.fertilizationCount = Mathf.Max(0, this.fertilizationCount - randomInRange);
			}
			else
			{
				thing = ThingMaker.MakeThing(this.Props.eggUnfertilizedDef, null);
			}
			thing.stackCount = randomInRange;
			CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
			if (compHatcher != null)
			{
				compHatcher.hatcheeFaction = this.parent.Faction;
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					compHatcher.hatcheeParent = pawn;
				}
				if (this.fertilizedBy != null)
				{
					compHatcher.otherParent = this.fertilizedBy;
				}
			}
			return thing;
		}

		// Token: 0x0600FF1C RID: 65308 RVA: 0x0049F2F0 File Offset: 0x0049D4F0
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			string text = "EggProgress".Translate() + ": " + this.eggProgress.ToStringPercent();
			if (this.fertilizationCount > 0)
			{
				text += "\n" + "Fertilized".Translate();
			}
			else if (this.ProgressStoppedBecauseUnfertilized)
			{
				text += "\n" + "ProgressStoppedUntilFertilized".Translate();
			}
			return text;
		}

		// Token: 0x0600FF1D RID: 65309 RVA: 0x0049F385 File Offset: 0x0049D585
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: LayEgg",
					action = delegate()
					{
						this.eggProgress = 1f;
					}
				};
			}
			yield break;
		}

		// Token: 0x0400A702 RID: 42754
		private const int EggTickInterval = 2500;

		// Token: 0x0400A703 RID: 42755
		private float eggProgress;

		// Token: 0x0400A704 RID: 42756
		private int fertilizationCount;

		// Token: 0x0400A705 RID: 42757
		private Pawn fertilizedBy;
	}
}
