using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001181 RID: 4481
	public class JobDriver_Replant : JobDriver_HaulToContainer
	{
		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06006D9D RID: 28061 RVA: 0x00224B8B File Offset: 0x00222D8B
		protected override int Duration
		{
			get
			{
				return (int)(((MinifiedTree)base.ThingToCarry).InnerTree.def.plant.harvestWork / JobDriver_PlantWork.WorkDonePerTick(this.pawn, ((MinifiedTree)base.ThingToCarry).InnerTree));
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x06006D9E RID: 28062 RVA: 0x00224BC9 File Offset: 0x00222DC9
		protected override EffecterDef WorkEffecter
		{
			get
			{
				return EffecterDefOf.Sow;
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06006D9F RID: 28063 RVA: 0x00224BD0 File Offset: 0x00222DD0
		protected override SoundDef WorkSustainer
		{
			get
			{
				return SoundDefOf.Interact_ConstructDirt;
			}
		}

		// Token: 0x06006DA0 RID: 28064 RVA: 0x00224BD7 File Offset: 0x00222DD7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.job.plantDefToSow.plant.interferesWithRoof && base.Container.Position.Roofed(base.Map));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006DA1 RID: 28065 RVA: 0x00224BE7 File Offset: 0x00222DE7
		protected override void ModifyPrepareToil(Toil toil)
		{
			toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta, false, false);
				}
			}));
		}
	}
}
