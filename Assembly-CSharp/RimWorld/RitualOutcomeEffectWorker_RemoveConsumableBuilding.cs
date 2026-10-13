using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002765 RID: 10085
	public class RitualOutcomeEffectWorker_RemoveConsumableBuilding : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021DC RID: 8668
		// (get) Token: 0x0600DD3A RID: 56634 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ApplyOnFailure
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021DD RID: 8669
		// (get) Token: 0x0600DD3B RID: 56635 RVA: 0x004114F4 File Offset: 0x0040F6F4
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return this.def.allowAttachableOutcome;
			}
		}

		// Token: 0x0600DD3C RID: 56636 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_RemoveConsumableBuilding()
		{
		}

		// Token: 0x0600DD3D RID: 56637 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_RemoveConsumableBuilding(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD3E RID: 56638 RVA: 0x00411514 File Offset: 0x0040F714
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (!jobRitual.cancelled)
			{
				base.Apply(progress, totalPresence, jobRitual);
			}
			if (jobRitual.selectedTarget.HasThing)
			{
				Thing thing = jobRitual.selectedTarget.Thing;
				if (this.def.effecter != null)
				{
					this.def.effecter.Spawn(thing, jobRitual.selectedTarget.Map, 1f).Cleanup();
				}
				if (this.def.fleckDef != null)
				{
					CellRect cellRect = thing.OccupiedRect();
					for (int i = 0; i < cellRect.Area * this.def.flecksPerCell; i++)
					{
						FleckCreationData dataStatic = FleckMaker.GetDataStatic(cellRect.RandomVector3, thing.Map, this.def.fleckDef, this.def.fleckScaleRange.RandomInRange);
						dataStatic.rotation = this.def.fleckRotationRange.RandomInRange;
						dataStatic.velocityAngle = this.def.fleckVelocityAngle.RandomInRange;
						dataStatic.velocitySpeed = this.def.fleckVelocitySpeed.RandomInRange;
						thing.Map.flecks.CreateFleck(dataStatic);
					}
				}
				if (this.def.filthDefToSpawn != null)
				{
					foreach (IntVec3 c in thing.OccupiedRect())
					{
						FilthMaker.TryMakeFilth(c, thing.Map, this.def.filthDefToSpawn, this.def.filthCountToSpawn.RandomInRange, FilthSourceFlags.None, true);
					}
				}
				thing.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
