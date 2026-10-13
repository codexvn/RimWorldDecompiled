using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C3 RID: 451
	public class JobDriver_CastAbilityWorld : JobDriver
	{
		// Token: 0x06000C68 RID: 3176 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0003A988 File Offset: 0x00038B88
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_General.Do(delegate
			{
				this.pawn.stances.SetStance(new Stance_WarmupAbilityWorld(this.job.ability.def.verbProperties.warmupTime.SecondsToTicks(), null, this.job.ability.verb));
			});
			yield return Toils_General.Do(delegate
			{
				this.job.ability.Activate(this.job.globalTarget);
			});
			yield break;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0003A998 File Offset: 0x00038B98
		public override string GetReport()
		{
			return "UsingVerb".Translate(this.job.ability.def.label, this.job.globalTarget.Label);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0003A9D8 File Offset: 0x00038BD8
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			Ability ability = this.job.ability;
			if (ability == null)
			{
				return;
			}
			ability.Notify_StartedCasting();
		}
	}
}
