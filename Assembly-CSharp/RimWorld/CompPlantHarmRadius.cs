using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DA9 RID: 11689
	public class CompPlantHarmRadius : ThingComp
	{
		// Token: 0x170029E3 RID: 10723
		// (get) Token: 0x060106F6 RID: 67318 RVA: 0x004BE0F8 File Offset: 0x004BC2F8
		protected CompProperties_PlantHarmRadius Props
		{
			get
			{
				return (CompProperties_PlantHarmRadius)this.props;
			}
		}

		// Token: 0x170029E4 RID: 10724
		// (get) Token: 0x060106F7 RID: 67319 RVA: 0x004BE105 File Offset: 0x004BC305
		public float AgeDays
		{
			get
			{
				return (float)this.plantHarmAge / 60000f;
			}
		}

		// Token: 0x170029E5 RID: 10725
		// (get) Token: 0x060106F8 RID: 67320 RVA: 0x004BE114 File Offset: 0x004BC314
		public float CurrentRadius
		{
			get
			{
				return this.Props.radiusPerDayCurve.Evaluate(this.AgeDays);
			}
		}

		// Token: 0x060106F9 RID: 67321 RVA: 0x004BE12C File Offset: 0x004BC32C
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.plantHarmAge, "plantHarmAge", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x060106FA RID: 67322 RVA: 0x004BE152 File Offset: 0x004BC352
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostPostMake();
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x060106FB RID: 67323 RVA: 0x004BE16C File Offset: 0x004BC36C
		public override string CompInspectStringExtra()
		{
			return "FoliageKillRadius".Translate() + ": " + this.CurrentRadius.ToString("0.0") + " (+" + "PerDay".Translate((this.Props.radiusPerDayCurve.Evaluate(this.AgeDays + 1f) - this.Props.radiusPerDayCurve.Evaluate(this.AgeDays)).ToString("0.0")) + ")";
		}

		// Token: 0x060106FC RID: 67324 RVA: 0x004BE214 File Offset: 0x004BC414
		public override void CompTick()
		{
			if (!this.parent.Spawned || (this.initiatableComp != null && !this.initiatableComp.Initiated))
			{
				return;
			}
			this.plantHarmAge++;
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				float currentRadius = this.CurrentRadius;
				float num = 3.1415927f * currentRadius * currentRadius * this.Props.harmFrequencyPerArea;
				float num2 = 60f / num;
				int num3;
				if (num2 >= 1f)
				{
					this.ticksToPlantHarm = GenMath.RoundRandom(num2);
					num3 = 1;
				}
				else
				{
					this.ticksToPlantHarm = 1;
					num3 = GenMath.RoundRandom(1f / num2);
				}
				for (int i = 0; i < num3; i++)
				{
					this.HarmRandomPlantInRadius(currentRadius);
				}
			}
		}

		// Token: 0x060106FD RID: 67325 RVA: 0x004BE2D4 File Offset: 0x004BC4D4
		private void HarmRandomPlantInRadius(float radius)
		{
			IntVec3 c = this.parent.Position + (Rand.InsideUnitCircleVec3 * radius).ToIntVec3();
			if (!c.InBounds(this.parent.Map))
			{
				return;
			}
			Plant plant = c.GetPlant(this.parent.Map);
			if (plant != null)
			{
				if (this.Props.ignoreSpecialTrees && plant.def.plant.treeCategory == TreeCategory.Super)
				{
					return;
				}
				if (plant.LeaflessNow)
				{
					if (Rand.Value < this.Props.leaflessPlantKillChance)
					{
						plant.Kill(null, null);
						return;
					}
				}
				else
				{
					plant.MakeLeafless(Plant.LeaflessCause.Poison, this.Props.messageOnCropDeath);
				}
			}
		}

		// Token: 0x0400AB76 RID: 43894
		private int plantHarmAge;

		// Token: 0x0400AB77 RID: 43895
		private int ticksToPlantHarm;

		// Token: 0x0400AB78 RID: 43896
		protected CompInitiatable initiatableComp;
	}
}
