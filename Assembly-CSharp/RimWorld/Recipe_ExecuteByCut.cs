using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200230C RID: 8972
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x0600C2DC RID: 49884 RVA: 0x003881EB File Offset: 0x003863EB
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -100, null);
			}
			ExecutionUtility.DoExecutionByCut(billDoer, pawn, 8, true);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.GenericHumane);
		}
	}
}
