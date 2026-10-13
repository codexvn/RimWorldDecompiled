using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200269D RID: 9885
	public class StageEndTrigger_RoleArrivedAndSpectatorsOnSubstructure : StageEndTrigger_RolesArrived
	{
		// Token: 0x0600D906 RID: 55558 RVA: 0x00404BB0 File Offset: 0x00402DB0
		protected override bool ArrivedCheck(string r, LordJob_Ritual ritual)
		{
			if (!base.ArrivedCheck(r, ritual))
			{
				return false;
			}
			Thing thing = ritual.selectedTarget.Thing;
			Building_GravEngine building_GravEngine;
			if (thing == null)
			{
				building_GravEngine = null;
			}
			else
			{
				CompPilotConsole compPilotConsole = thing.TryGetComp<CompPilotConsole>();
				building_GravEngine = ((compPilotConsole != null) ? compPilotConsole.engine : null);
			}
			Building_GravEngine building_GravEngine2 = building_GravEngine;
			if (building_GravEngine2 == null)
			{
				Log.Error("Engine could not be found in ritual end trigger");
				return true;
			}
			foreach (Pawn pawn in ritual.assignments.SpectatorsForReading)
			{
				if (!building_GravEngine2.ValidSubstructure.Contains(pawn.Position))
				{
					return false;
				}
			}
			return true;
		}
	}
}
