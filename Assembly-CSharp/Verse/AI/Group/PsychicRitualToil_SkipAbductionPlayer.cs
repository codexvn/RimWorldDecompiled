using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x02001081 RID: 4225
	public class PsychicRitualToil_SkipAbductionPlayer : PsychicRitualToil
	{
		// Token: 0x0600676D RID: 26477 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_SkipAbductionPlayer()
		{
		}

		// Token: 0x0600676E RID: 26478 RVA: 0x0020C280 File Offset: 0x0020A480
		public PsychicRitualToil_SkipAbductionPlayer(PsychicRitualRoleDef invokerRole)
		{
			this.invokerRole = invokerRole;
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x0020C290 File Offset: 0x0020A490
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn);
			}
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x0020C2BC File Offset: 0x0020A4BC
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
		{
			IntVec3 cell = psychicRitual.assignments.Target.Cell;
			bool flag = false;
			IEnumerable<Pawn> source = psychicRitual.Map.attackTargetsCache.TargetsHostileToColony.Where(delegate(IAttackTarget t)
			{
				Pawn pawn4 = t.Thing as Pawn;
				return pawn4 != null && pawn4.RaceProps.Humanlike && !pawn4.IsSubhuman && pawn4.Faction != Faction.OfPlayer && !t.ThreatDisabled(invoker) && !pawn4.IsOnHoldingPlatform;
			}).Cast<Pawn>().ToArray<Pawn>();
			Pawn pawn = null;
			Pawn pawn2;
			if (source.TryRandomElement(out pawn2))
			{
				pawn = pawn2;
				psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(pawn, pawn.Map, 1f), pawn.PositionHeld, 60);
				SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(pawn.PositionHeld, pawn.Map, false));
				SkipUtility.SkipTo(pawn, cell, psychicRitual.Map);
			}
			else
			{
				List<Pawn> list = Find.WorldPawns.AllPawnsAlive.Where(delegate(Pawn p)
				{
					if (p.RaceProps.Humanlike && p.HostileTo(invoker) && !p.IsSubhuman)
					{
						Faction faction2 = p.Faction;
						return ((faction2 != null) ? faction2.leader : null) == p;
					}
					return false;
				}).ToList<Pawn>();
				List<Pawn> list2 = Find.WorldPawns.AllPawnsAlive.Where(delegate(Pawn p)
				{
					if (p.RaceProps.Humanlike && p.HostileTo(invoker) && !p.IsSubhuman)
					{
						Faction faction2 = p.Faction;
						return ((faction2 != null) ? faction2.leader : null) != p;
					}
					return false;
				}).ToList<Pawn>();
				float chance = 0.4f * Mathf.Clamp01((float)list2.Count / 20f);
				Pawn pawn3 = null;
				if (Rand.Chance(0.08f) && !list.NullOrEmpty<Pawn>())
				{
					list.TryRandomElement(out pawn3);
				}
				else if (Rand.Chance(chance))
				{
					list2.TryRandomElement(out pawn3);
				}
				Faction faction;
				if (pawn3 != null)
				{
					pawn = (Pawn)GenSpawn.Spawn(pawn3, cell, psychicRitual.Map, WipeMode.Vanish);
					flag = true;
				}
				else if ((from f in Find.FactionManager.AllFactionsVisible
				where f.def.humanlikeFaction && f.HostileTo(Faction.OfPlayer)
				select f).TryRandomElement(out faction))
				{
					pawn = (Pawn)GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(faction.RandomPawnKind(), faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), cell, psychicRitual.Map, WipeMode.Vanish);
				}
			}
			if (pawn == null)
			{
				Log.Error("Could not find target pawn for player's skip abduction ritual.");
				return;
			}
			if (pawn.Dead)
			{
				Log.Error(string.Format("Skip abduction ritual abducted a dead pawn. World pawn abducted: {0}", flag));
			}
			if (pawn.IsSubhuman)
			{
				Log.Error(string.Format("Skip abduction ritual abducted a mutant. World pawn abducted: {0}", flag));
			}
			psychicRitual.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(cell, psychicRitual.Map, 1f), cell, 60);
			SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, psychicRitual.Map, false));
			int ticksToDisappear = Mathf.RoundToInt(((PsychicRitualDef_SkipAbductionPlayer)psychicRitual.def).comaDurationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent) * 60000f);
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, pawn, null);
			hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = ticksToDisappear;
			pawn.health.AddHediff(hediff, null, null, null);
			Pawn_NeedsTracker needs = pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.PsychicRitualVictim, null, null);
						}
					}
				}
			}
			TaggedString text = "SkipAbductionPlayerCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), pawn.Named("TARGET"), pawn.Faction.Named("FACTION"));
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, LetterDefOf.NeutralEvent, new LookTargets(pawn), null, null, null, null, 0, true);
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x0020C6C7 File Offset: 0x0020A8C7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
		}

		// Token: 0x040049B5 RID: 18869
		private const float ChanceForLeader = 0.08f;

		// Token: 0x040049B6 RID: 18870
		private const float ChanceForWorldPawn = 0.4f;

		// Token: 0x040049B7 RID: 18871
		public PsychicRitualRoleDef invokerRole;
	}
}
