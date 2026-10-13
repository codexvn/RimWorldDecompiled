using System;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A6A RID: 10858
	public class Bill_ProductionMech : Bill_Mech
	{
		// Token: 0x170025BF RID: 9663
		// (get) Token: 0x0600F19A RID: 61850 RVA: 0x0046170C File Offset: 0x0045F90C
		public override float BandwidthCost
		{
			get
			{
				return this.recipe.ProducedThingDef.GetStatValueAbstract(StatDefOf.BandwidthCost, null);
			}
		}

		// Token: 0x0600F19B RID: 61851 RVA: 0x00461724 File Offset: 0x0045F924
		public Bill_ProductionMech()
		{
		}

		// Token: 0x0600F19C RID: 61852 RVA: 0x0046172C File Offset: 0x0045F92C
		public Bill_ProductionMech(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600F19D RID: 61853 RVA: 0x00461738 File Offset: 0x0045F938
		public override Thing CreateProducts()
		{
			Pawn boundPawn = base.BoundPawn;
			PawnKindDef kind = (from pk in DefDatabase<PawnKindDef>.AllDefs
			where pk.race == this.recipe.ProducedThingDef
			select pk).First<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, boundPawn.Faction, PawnGenerationContext.NonPlayer, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false));
			boundPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, pawn);
			return pawn;
		}

		// Token: 0x0600F19E RID: 61854 RVA: 0x00461814 File Offset: 0x0045FA14
		public override void AppendInspectionData(StringBuilder sb)
		{
			switch (base.State)
			{
			case FormingState.Gathering:
				base.AppendCurrentIngredientCount(sb);
				break;
			case FormingState.Preparing:
				sb.AppendLine("GestatingMech".Translate(this.recipe.ProducedThingDef.LabelCap));
				sb.AppendLine("GestatingMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")));
				break;
			case FormingState.Forming:
				sb.AppendLine("GestatingMech".Translate(this.recipe.ProducedThingDef.LabelCap));
				break;
			case FormingState.Formed:
				if (base.BoundPawn != null)
				{
					sb.AppendLine("GestatedMech".Translate(this.recipe.ProducedThingDef.LabelCap, base.BoundPawn.Named("PAWN")) + " (" + "GestatedMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")) + ")");
				}
				break;
			}
			base.AppendInspectionData(sb);
		}
	}
}
