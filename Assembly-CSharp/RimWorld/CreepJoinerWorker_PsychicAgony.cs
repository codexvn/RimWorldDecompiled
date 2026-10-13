using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020022D5 RID: 8917
	public class CreepJoinerWorker_PsychicAgony : BaseCreepJoinerWorker
	{
		// Token: 0x0600C1F8 RID: 49656 RVA: 0x003830B4 File Offset: 0x003812B4
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			this.ApplyOrRefreshHediff(base.Pawn);
			foreach (Pawn pawn in base.Pawn.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (pawn.Position.InHorDistOf(base.Pawn.Position, 14.9f))
				{
					this.ApplyOrRefreshHediff(pawn);
				}
			}
			SoundDefOf.PsychicBanshee.PlayOneShot(base.Pawn);
			MoteMaker.MakeAttachedOverlay(base.Pawn, ThingDefOf.Mote_PsychicBanshee, Vector3.zero, 1f, -1f);
		}

		// Token: 0x0600C1F9 RID: 49657 RVA: 0x00383174 File Offset: 0x00381374
		private void ApplyOrRefreshHediff(Pawn pawn)
		{
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.AgonyPulse, out hediff))
			{
				hediff.Severity = 0f;
			}
			else
			{
				hediff = pawn.health.AddHediff(HediffDefOf.AgonyPulse, null, null, null);
			}
			HediffWithComps hediffWithComps = hediff as HediffWithComps;
			HediffComp_Disappears hediffComp_Disappears = (hediffWithComps != null) ? hediffWithComps.GetComp<HediffComp_Disappears>() : null;
			if (hediffComp_Disappears != null)
			{
				hediffComp_Disappears.ticksToDisappear = CreepJoinerWorker_PsychicAgony.DisappearSeconds.RandomInRange * 60;
			}
		}

		// Token: 0x04008468 RID: 33896
		private const float Range = 14.9f;

		// Token: 0x04008469 RID: 33897
		private static readonly IntRange DisappearSeconds = new IntRange(2000, 4000);
	}
}
