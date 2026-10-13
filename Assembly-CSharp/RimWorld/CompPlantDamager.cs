using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DA6 RID: 11686
	public class CompPlantDamager : ThingComp
	{
		// Token: 0x170029E0 RID: 10720
		// (get) Token: 0x060106E3 RID: 67299 RVA: 0x004BDDAE File Offset: 0x004BBFAE
		public CompProperties_PlantDamager Props
		{
			get
			{
				return (CompProperties_PlantDamager)this.props;
			}
		}

		// Token: 0x060106E4 RID: 67300 RVA: 0x004BDDBB File Offset: 0x004BBFBB
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksToPlantHarm = this.Props.ticksBetweenDamage;
		}

		// Token: 0x060106E5 RID: 67301 RVA: 0x004BDDD5 File Offset: 0x004BBFD5
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.queuedCycles, "queuedCycles", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToPlantHarm, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x060106E6 RID: 67302 RVA: 0x004BDDFB File Offset: 0x004BBFFB
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			this.queuedCycles = this.Props.cycleCountOnSpawn;
		}

		// Token: 0x060106E7 RID: 67303 RVA: 0x004BDE14 File Offset: 0x004BC014
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			while (this.queuedCycles > 0)
			{
				this.DamageCycle();
				this.queuedCycles--;
			}
			this.ticksToPlantHarm--;
			if (this.ticksToPlantHarm <= 0)
			{
				this.ticksToPlantHarm = this.Props.ticksBetweenDamage;
				this.DamageCycle();
			}
		}

		// Token: 0x060106E8 RID: 67304 RVA: 0x004BDE7C File Offset: 0x004BC07C
		public void DamageCycle()
		{
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.Props.radius, true))
			{
				if (intVec.InBounds(this.parent.Map))
				{
					float distance = intVec.DistanceTo(this.parent.Position);
					float amount = this.ComputeDamage(distance);
					Plant plant = intVec.GetPlant(this.parent.Map);
					if (plant != null && (!this.Props.ignoreAnima || (plant.def != ThingDefOf.Plant_GrassAnima && plant.def != ThingDefOf.Plant_TreeAnima)))
					{
						plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, amount, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					}
				}
			}
		}

		// Token: 0x060106E9 RID: 67305 RVA: 0x004BDF78 File Offset: 0x004BC178
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Plant kill cycle",
					action = new Action(this.DamageCycle)
				};
			}
			yield break;
		}

		// Token: 0x060106EA RID: 67306 RVA: 0x004BDF88 File Offset: 0x004BC188
		private float ComputeDamage(float distance)
		{
			return this.Props.damagePerCycle * CompPlantDamager.DamageMultiplierOverDistance.Evaluate(distance / this.Props.radius);
		}

		// Token: 0x0400AB6A RID: 43882
		private int ticksToPlantHarm;

		// Token: 0x0400AB6B RID: 43883
		private int queuedCycles;

		// Token: 0x0400AB6C RID: 43884
		private static readonly SimpleCurve DamageMultiplierOverDistance = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.2f),
				true
			}
		};
	}
}
