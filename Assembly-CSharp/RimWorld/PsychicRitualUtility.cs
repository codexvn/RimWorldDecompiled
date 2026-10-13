using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B7 RID: 5815
	public static class PsychicRitualUtility
	{
		// Token: 0x0600890E RID: 35086 RVA: 0x0028DF06 File Offset: 0x0028C106
		public static IEnumerable<Thing> GetPsychicRitualSpotsAffectedByThing(Map map, CellRect thingRect)
		{
			foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PsychicRitualSpot)))
			{
				CompPsychicRitualSpot compPsychicRitualSpot = thing.TryGetComp<CompPsychicRitualSpot>();
				if (compPsychicRitualSpot != null && compPsychicRitualSpot.OccupiedCells.Overlaps(thingRect.Cells))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600890F RID: 35087 RVA: 0x0028DF20 File Offset: 0x0028C120
		public static void DrawPsychicRitualSpotsAffectedByThingOverlay(Map map, ThingDef def, IntVec3 pos, Rot4 rotation)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			int num = 0;
			CellRect thingRect = GenAdj.OccupiedRect(pos, rotation, def.size);
			foreach (Thing t in PsychicRitualUtility.GetPsychicRitualSpotsAffectedByThing(map, thingRect))
			{
				if (num++ > 10)
				{
					break;
				}
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}

		// Token: 0x06008910 RID: 35088 RVA: 0x0028DFB0 File Offset: 0x0028C1B0
		public static SkillDef GetPhilophagySkillAndXpTransfer(Pawn invoker, Pawn target, float xpTransferPercent, out float xpTransfer)
		{
			IOrderedEnumerable<SkillRecord> orderedEnumerable = from s in target.skills.skills
			where !s.TotallyDisabled && !invoker.skills.GetSkill(s.def).TotallyDisabled
			orderby s.XpTotalEarned descending
			select s;
			if (!orderedEnumerable.Any<SkillRecord>())
			{
				xpTransfer = 0f;
				return null;
			}
			PsychicRitualUtility.tmpCandidateSkills.Clear();
			SkillRecord skillRecord = orderedEnumerable.First<SkillRecord>();
			foreach (SkillRecord skillRecord2 in orderedEnumerable)
			{
				if (skillRecord2 == skillRecord)
				{
					PsychicRitualUtility.tmpCandidateSkills.Add(skillRecord2);
				}
				else if (Mathf.Abs(skillRecord2.XpTotalEarned - skillRecord.XpTotalEarned) < 0.01f)
				{
					PsychicRitualUtility.tmpCandidateSkills.Add(skillRecord2);
				}
			}
			SkillDef def = PsychicRitualUtility.tmpCandidateSkills[0].def;
			foreach (SkillRecord skillRecord3 in PsychicRitualUtility.tmpCandidateSkills)
			{
				if (invoker.skills.GetSkill(def).XpTotalEarned < invoker.skills.GetSkill(skillRecord3.def).XpTotalEarned)
				{
					def = skillRecord3.def;
				}
			}
			float xpTotalEarned = target.skills.GetSkill(def).XpTotalEarned;
			xpTransfer = xpTotalEarned * xpTransferPercent;
			return def;
		}

		// Token: 0x06008911 RID: 35089 RVA: 0x0028E144 File Offset: 0x0028C344
		public static void RegisterAsExecutionIfPrisoner(Pawn target, Pawn invoker)
		{
			if (!target.IsPrisonerOfColony)
			{
				return;
			}
			ThoughtUtility.GiveThoughtsForPawnExecuted(target, invoker, PawnExecutionKind.GenericBrutal);
			TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
			{
				invoker,
				target
			});
		}

		// Token: 0x06008912 RID: 35090 RVA: 0x0028E170 File Offset: 0x0028C370
		public static void AddPsychicRitualGuiltToPawns(PsychicRitualDef ritualDef, IEnumerable<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = (Thought_PsychicRitualGuilt)ThoughtMaker.MakeThought(ThoughtDefOf.PsychicRitualGuilt);
				thought_PsychicRitualGuilt.ritualDef = ritualDef;
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
								memories.TryGainMemory(thought_PsychicRitualGuilt, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x04005995 RID: 22933
		private static List<SkillRecord> tmpCandidateSkills = new List<SkillRecord>();
	}
}
