using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C88 RID: 7304
	public class QuestPart_RequirementsToAcceptBedroom : QuestPart_RequirementsToAccept
	{
		// Token: 0x0600A2C0 RID: 41664 RVA: 0x002EC85C File Offset: 0x002EAA5C
		public override AcceptanceReport CanAccept()
		{
			int num = this.CulpritsAre().Count<Pawn>();
			if (num > 0)
			{
				return ((num > 1) ? "QuestBedroomRequirementsUnsatisfied" : "QuestBedroomRequirementsUnsatisfiedSingle").Translate() + " " + (from p in this.CulpritsAre()
				select p.royalty.MainTitle().GetLabelFor(p).CapitalizeFirst() + " " + p.LabelShort).ToCommaList(true, false);
			}
			return true;
		}

		// Token: 0x0600A2C1 RID: 41665 RVA: 0x002EC8DC File Offset: 0x002EAADC
		private List<Pawn> CulpritsAre()
		{
			this.culpritsResult.Clear();
			if (this.targetPawns.Any<Pawn>())
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
				{
					Pawn_RoyaltyTracker royalty = pawn.royalty;
					if (((royalty != null) ? royalty.HighestTitleWithBedroomRequirements() : null) != null && !pawn.Suspended && (!pawn.royalty.HasPersonalBedroom() || pawn.royalty.AnyUnmetBedroomRequirements()))
					{
						this.culpritsResult.Add(pawn);
					}
				}
			}
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return this.culpritsResult;
			}
			this.tmpOccupiedBeds.Clear();
			List<Thing> list = this.mapParent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Bed);
			foreach (Pawn pawn2 in this.targetPawns)
			{
				RoyalTitle royalTitle = pawn2.royalty.HighestTitleWithBedroomRequirements();
				if (royalTitle != null)
				{
					Thing thing = null;
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing2 = list[i];
						if (thing2.Faction == Faction.OfPlayer && thing2.GetRoom(RegionType.Set_All) != null && !this.tmpOccupiedBeds.Contains(thing2))
						{
							CompAssignableToPawn compAssignableToPawn = thing2.TryGetComp<CompAssignableToPawn>();
							if (compAssignableToPawn != null && compAssignableToPawn.AssignedPawnsForReading.Count <= 0 && RoyalTitleUtility.BedroomSatisfiesRequirements(thing2.GetRoom(RegionType.Set_All), royalTitle))
							{
								thing = thing2;
								break;
							}
						}
					}
					if (thing != null)
					{
						this.tmpOccupiedBeds.Add(thing);
					}
					else
					{
						this.culpritsResult.Add(pawn2);
					}
				}
			}
			this.tmpOccupiedBeds.Clear();
			return this.culpritsResult;
		}

		// Token: 0x0600A2C2 RID: 41666 RVA: 0x002ECAC4 File Offset: 0x002EACC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.targetPawns, "targetPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.targetPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x17001901 RID: 6401
		// (get) Token: 0x0600A2C3 RID: 41667 RVA: 0x002ECB31 File Offset: 0x002EAD31
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				return this.CulpritsAre().Select(delegate(Pawn p)
				{
					RoyalTitle royalTitle = p.royalty.HighestTitleWithBedroomRequirements();
					return new Dialog_InfoCard.Hyperlink(royalTitle.def, royalTitle.faction, -1);
				});
			}
		}

		// Token: 0x0600A2C4 RID: 41668 RVA: 0x002ECB5D File Offset: 0x002EAD5D
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.targetPawns.Replace(replace, with);
		}

		// Token: 0x0400708F RID: 28815
		public List<Pawn> targetPawns = new List<Pawn>();

		// Token: 0x04007090 RID: 28816
		public MapParent mapParent;

		// Token: 0x04007091 RID: 28817
		private List<Thing> tmpOccupiedBeds = new List<Thing>();

		// Token: 0x04007092 RID: 28818
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
