using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182D RID: 6189
	public abstract class ThoughtWorker_Precept_ChildLabor : ThoughtWorker_Precept
	{
		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x06008F22 RID: 36642
		protected abstract TimeAssignmentDef AssignmentDef { get; }

		// Token: 0x06008F23 RID: 36643 RVA: 0x002A5004 File Offset: 0x002A3204
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			foreach (Pawn pawn in p.MapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.RaceProps.Humanlike && pawn.DevelopmentalStage.Child() && pawn.timetable != null)
				{
					for (int i = 0; i < 24; i++)
					{
						if (pawn.timetable.GetAssignment(i) == this.AssignmentDef)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
