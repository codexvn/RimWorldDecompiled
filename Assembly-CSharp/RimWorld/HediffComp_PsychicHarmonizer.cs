using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003858 RID: 14424
	public class HediffComp_PsychicHarmonizer : HediffComp
	{
		// Token: 0x170032EC RID: 13036
		// (get) Token: 0x06013F18 RID: 81688 RVA: 0x005EACD8 File Offset: 0x005E8ED8
		public HediffCompProperties_PsychicHarmonizer Props
		{
			get
			{
				return (HediffCompProperties_PsychicHarmonizer)this.props;
			}
		}

		// Token: 0x06013F19 RID: 81689 RVA: 0x005EACE8 File Offset: 0x005E8EE8
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			Pawn pawn = this.parent.pawn;
			if (!pawn.IsHashIntervalTick(150, delta))
			{
				return;
			}
			if (pawn.needs == null || pawn.needs.mood == null || pawn.Faction == null)
			{
				return;
			}
			if (pawn.Spawned)
			{
				List<Pawn> pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
				this.AffectPawns(pawn, pawns);
				return;
			}
			Caravan caravan = pawn.GetCaravan();
			if (caravan != null)
			{
				this.AffectPawns(pawn, caravan.pawns.InnerListForReading);
			}
		}

		// Token: 0x06013F1A RID: 81690 RVA: 0x005EAD74 File Offset: 0x005E8F74
		private void AffectPawns(Pawn p, List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (p != pawn && p.RaceProps.Humanlike)
				{
					bool flag;
					if (pawn == null)
					{
						flag = (null != null);
					}
					else
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (needs == null)
						{
							flag = (null != null);
						}
						else
						{
							Need_Mood mood = needs.mood;
							flag = (((mood != null) ? mood.thoughts : null) != null);
						}
					}
					if (flag && pawn.Position.DistanceTo(p.Position) <= this.Props.range && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false))
					{
						bool flag2 = false;
						foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
						{
							Thought_PsychicHarmonizer thought_PsychicHarmonizer = thought_Memory as Thought_PsychicHarmonizer;
							if (thought_PsychicHarmonizer != null && thought_PsychicHarmonizer.harmonizer == this.parent)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							Thought_PsychicHarmonizer thought_PsychicHarmonizer2 = (Thought_PsychicHarmonizer)ThoughtMaker.MakeThought(this.Props.thought);
							thought_PsychicHarmonizer2.harmonizer = this.parent;
							thought_PsychicHarmonizer2.otherPawn = this.parent.pawn;
							pawn.needs.mood.thoughts.memories.TryGainMemory(thought_PsychicHarmonizer2, null);
						}
					}
				}
			}
		}
	}
}
