using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B47 RID: 6983
	public sealed class OutfitDatabase : IExposable
	{
		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x06009BEF RID: 39919 RVA: 0x002D38C5 File Offset: 0x002D1AC5
		public List<ApparelPolicy> AllOutfits
		{
			get
			{
				return this.outfits;
			}
		}

		// Token: 0x06009BF0 RID: 39920 RVA: 0x002D38CD File Offset: 0x002D1ACD
		public OutfitDatabase()
		{
			this.GenerateStartingOutfits();
		}

		// Token: 0x06009BF1 RID: 39921 RVA: 0x002D38E6 File Offset: 0x002D1AE6
		public void ExposeData()
		{
			Scribe_Collections.Look<ApparelPolicy>(ref this.outfits, "outfits", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06009BF2 RID: 39922 RVA: 0x002D38FE File Offset: 0x002D1AFE
		public ApparelPolicy DefaultOutfit()
		{
			if (this.outfits.Count == 0)
			{
				this.MakeNewOutfit();
			}
			return this.outfits[0];
		}

		// Token: 0x06009BF3 RID: 39923 RVA: 0x002D3920 File Offset: 0x002D1B20
		public void SetDefault(ApparelPolicy policy)
		{
			int index = this.outfits.IndexOf(policy);
			ApparelPolicy value = this.outfits[0];
			this.outfits[0] = policy;
			this.outfits[index] = value;
		}

		// Token: 0x06009BF4 RID: 39924 RVA: 0x002D3964 File Offset: 0x002D1B64
		public AcceptanceReport TryDelete(ApparelPolicy apparelPolicy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.outfits != null && pawn.outfits.CurrentApparelPolicy == apparelPolicy)
				{
					return new AcceptanceReport("OutfitInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.outfits != null && pawn2.outfits.CurrentApparelPolicy == apparelPolicy)
				{
					pawn2.outfits.CurrentApparelPolicy = null;
				}
			}
			this.outfits.Remove(apparelPolicy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009BF5 RID: 39925 RVA: 0x002D3A54 File Offset: 0x002D1C54
		public ApparelPolicy MakeNewOutfit()
		{
			int num;
			if (!this.outfits.Any<ApparelPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.outfits.Max((ApparelPolicy o) => o.id) + 1;
			}
			int id = num;
			ApparelPolicy apparelPolicy = new ApparelPolicy(id, "ApparelPolicy".Translate() + " " + id.ToString());
			apparelPolicy.filter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			this.outfits.Add(apparelPolicy);
			return apparelPolicy;
		}

		// Token: 0x06009BF6 RID: 39926 RVA: 0x002D3AEC File Offset: 0x002D1CEC
		private void GenerateStartingOutfits()
		{
			this.MakeNewOutfit().label = "OutfitAnything".Translate();
			ApparelPolicy apparelPolicy = this.MakeNewOutfit();
			apparelPolicy.label = "OutfitWorker".Translate();
			apparelPolicy.filter.SetDisallowAll(null, null);
			apparelPolicy.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.apparel != null && ((thingDef.apparel.defaultOutfitTags != null && thingDef.apparel.defaultOutfitTags.Contains("Worker")) || thingDef.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
				{
					apparelPolicy.filter.SetAllow(thingDef, true);
				}
			}
			ApparelPolicy apparelPolicy2 = this.MakeNewOutfit();
			apparelPolicy2.label = "OutfitSoldier".Translate();
			apparelPolicy2.filter.SetDisallowAll(null, null);
			apparelPolicy2.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef2 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef2.apparel != null && ((thingDef2.apparel.defaultOutfitTags != null && thingDef2.apparel.defaultOutfitTags.Contains("Soldier")) || thingDef2.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
				{
					apparelPolicy2.filter.SetAllow(thingDef2, true);
				}
			}
			ApparelPolicy apparelPolicy3 = this.MakeNewOutfit();
			apparelPolicy3.label = "OutfitNudist".Translate();
			apparelPolicy3.filter.SetDisallowAll(null, null);
			apparelPolicy3.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
			foreach (ThingDef thingDef3 in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef3.apparel != null && (thingDef3.apparel.defaultOutfitTags.NotNullAndContains("Nudist") || (!thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs) && !thingDef3.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))))
				{
					apparelPolicy3.filter.SetAllow(thingDef3, true);
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				ApparelPolicy apparelPolicy4 = this.MakeNewOutfit();
				apparelPolicy4.label = "OutfitSlave".Translate();
				apparelPolicy4.filter.SetDisallowAll(null, null);
				apparelPolicy4.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
				foreach (ThingDef thingDef4 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef4.apparel != null && thingDef4.apparel.defaultOutfitTags != null && thingDef4.apparel.defaultOutfitTags.Contains("Slave"))
					{
						apparelPolicy4.filter.SetAllow(thingDef4, true);
					}
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				ApparelPolicy apparelPolicy5 = this.MakeNewOutfit();
				apparelPolicy5.label = "OutfitSpacefarer".Translate();
				apparelPolicy5.filter.SetDisallowAll(null, null);
				apparelPolicy5.filter.SetAllow(SpecialThingFilterDefOf.AllowDeadmansApparel, false);
				foreach (ThingDef thingDef5 in DefDatabase<ThingDef>.AllDefs)
				{
					if (thingDef5.apparel != null && ((thingDef5.apparel.defaultOutfitTags != null && thingDef5.apparel.defaultOutfitTags.Contains("Spacefarer")) || thingDef5.thingCategories.NotNullAndContains(ThingCategoryDefOf.ApparelUtility)))
					{
						apparelPolicy5.filter.SetAllow(thingDef5, true);
					}
				}
			}
		}

		// Token: 0x04006C39 RID: 27705
		private List<ApparelPolicy> outfits = new List<ApparelPolicy>();
	}
}
