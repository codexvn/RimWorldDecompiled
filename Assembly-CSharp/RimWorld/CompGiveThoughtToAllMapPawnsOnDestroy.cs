using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CB9 RID: 11449
	public class CompGiveThoughtToAllMapPawnsOnDestroy : ThingComp
	{
		// Token: 0x170028A3 RID: 10403
		// (get) Token: 0x06010119 RID: 65817 RVA: 0x004A65AC File Offset: 0x004A47AC
		private CompProperties_GiveThoughtToAllMapPawnsOnDestroy Props
		{
			get
			{
				return (CompProperties_GiveThoughtToAllMapPawnsOnDestroy)this.props;
			}
		}

		// Token: 0x0601011A RID: 65818 RVA: 0x004A65BC File Offset: 0x004A47BC
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Vanish && this.Props.ignoreOnVanish)
			{
				return;
			}
			if (this.Props.onlyWhenKilled && mode != DestroyMode.KillFinalize && mode != DestroyMode.KillFinalizeLeavingsOnly)
			{
				return;
			}
			if (previousMap != null)
			{
				if (!this.Props.message.NullOrEmpty())
				{
					Messages.Message(this.Props.message, new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NegativeEvent, true);
				}
				foreach (Pawn pawn in previousMap.mapPawns.AllPawnsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
						}
					}
				}
			}
		}
	}
}
