using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002A6C RID: 10860
	public class Bill_ResurrectMech : Bill_Mech
	{
		// Token: 0x170025C3 RID: 9667
		// (get) Token: 0x0600F1AA RID: 61866 RVA: 0x00461B6C File Offset: 0x0045FD6C
		public override float BandwidthCost
		{
			get
			{
				if (base.Gestator.ActiveBill == this)
				{
					Pawn gestatingMech = base.Gestator.GestatingMech;
					if (gestatingMech != null)
					{
						return gestatingMech.GetStatValue(StatDefOf.BandwidthCost, true, -1);
					}
					Corpse resurrectingMechCorpse = base.Gestator.ResurrectingMechCorpse;
					if (resurrectingMechCorpse != null)
					{
						return resurrectingMechCorpse.InnerPawn.GetStatValue(StatDefOf.BandwidthCost, true, -1);
					}
				}
				return 0f;
			}
		}

		// Token: 0x0600F1AB RID: 61867 RVA: 0x00461724 File Offset: 0x0045F924
		public Bill_ResurrectMech()
		{
		}

		// Token: 0x0600F1AC RID: 61868 RVA: 0x0046172C File Offset: 0x0045F92C
		public Bill_ResurrectMech(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600F1AD RID: 61869 RVA: 0x00461BCC File Offset: 0x0045FDCC
		public override bool PawnAllowedToStartAnew(Pawn p)
		{
			if (!base.PawnAllowedToStartAnew(p))
			{
				return false;
			}
			if (base.State == FormingState.Gathering && base.Gestator.ResurrectingMechCorpse == null)
			{
				IEnumerable<Corpse> source = this.AllAvailableResurrectionCorpses(p);
				if (source.Any<Corpse>() && !source.Any((Corpse c) => p.mechanitor.HasBandwidthToResurrect(c)))
				{
					JobFailReason.Is("NotEnoughBandwidth".Translate(), null);
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F1AE RID: 61870 RVA: 0x00461C4C File Offset: 0x0045FE4C
		public override Thing CreateProducts()
		{
			Pawn innerPawn = base.Gestator.ResurrectingMechCorpse.InnerPawn;
			ResurrectionUtility.TryResurrect(innerPawn, new ResurrectionParams
			{
				dontSpawn = true
			});
			innerPawn.needs.energy.CurLevel = innerPawn.needs.energy.MaxLevel * 0.5f;
			innerPawn.health.RemoveAllHediffs();
			base.BoundPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, innerPawn);
			if (innerPawn.IsWorldPawn())
			{
				Find.WorldPawns.RemovePawn(innerPawn);
			}
			return innerPawn;
		}

		// Token: 0x0600F1AF RID: 61871 RVA: 0x00461CD8 File Offset: 0x0045FED8
		public override bool IsFixedOrAllowedIngredient(Thing thing)
		{
			if (!base.IsFixedOrAllowedIngredient(thing))
			{
				return false;
			}
			Corpse corpse = thing as Corpse;
			return corpse == null || corpse.InnerPawn.Faction == Faction.OfPlayer;
		}

		// Token: 0x0600F1B0 RID: 61872 RVA: 0x00461D0F File Offset: 0x0045FF0F
		private IEnumerable<Corpse> AllAvailableResurrectionCorpses(Pawn pawn)
		{
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
			foreach (Thing thing in list)
			{
				Corpse corpse = (Corpse)thing;
				if (this.recipe.fixedIngredientFilter.Allows(corpse) && !corpse.IsForbidden(pawn) && pawn.CanReach(corpse, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					yield return corpse;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600F1B1 RID: 61873 RVA: 0x00461D28 File Offset: 0x0045FF28
		public override void AppendInspectionData(StringBuilder sb)
		{
			switch (base.State)
			{
			case FormingState.Gathering:
				base.AppendCurrentIngredientCount(sb);
				break;
			case FormingState.Forming:
			{
				Building_MechGestator gestator = base.Gestator;
				if (((gestator != null) ? gestator.ResurrectingMechCorpse : null) != null)
				{
					sb.AppendLine("ResurrectingMech".Translate(base.Gestator.ResurrectingMechCorpse.InnerPawn.LabelCap));
				}
				break;
			}
			case FormingState.Formed:
			{
				Building_MechGestator gestator2 = base.Gestator;
				if (((gestator2 != null) ? gestator2.GestatingMech : null) != null && base.BoundPawn != null)
				{
					sb.AppendLine("ResurrectedMech".Translate(base.Gestator.GestatingMech.LabelCap, base.BoundPawn.Named("PAWN")) + " (" + "GestatedMechRequiresMechanitor".Translate(base.BoundPawn.Named("PAWN")) + ")");
				}
				break;
			}
			}
			base.AppendInspectionData(sb);
		}
	}
}
