using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002915 RID: 10517
	public class TaleData_Pawn : TaleData
	{
		// Token: 0x0600E614 RID: 58900 RVA: 0x00431BBC File Offset: 0x0042FDBC
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Defs.Look<PawnKindDef>(ref this.kind, "kind");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<int>(ref this.chronologicalAge, "chronologicalAge", 0, false);
			Scribe_Values.Look<string>(ref this.relationInfo, "relationInfo", null, false);
			Scribe_Values.Look<bool>(ref this.everBeenColonistOrTameAnimal, "everBeenColonistOrTameAnimal", false, false);
			Scribe_Values.Look<bool>(ref this.everBeenQuestLodger, "everBeenQuestLodger", false, false);
			Scribe_Values.Look<bool>(ref this.isFactionLeader, "isFactionLeader", false, false);
			Scribe_Collections.Look<RoyalTitle>(ref this.royalTitles, "royalTitles", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<Name>(ref this.name, "name", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.primaryEquipment, "peq");
			Scribe_Defs.Look<ThingDef>(ref this.notableApparel, "app");
		}

		// Token: 0x0600E615 RID: 58901 RVA: 0x00431CD8 File Offset: 0x0042FED8
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			return GrammarUtility.RulesForPawn(prefix, this.name, this.title, this.kind, this.gender, this.faction, this.age, this.chronologicalAge, this.relationInfo, this.everBeenColonistOrTameAnimal, this.everBeenQuestLodger, this.isFactionLeader, this.royalTitles, false, string.Empty, constants, true);
		}

		// Token: 0x0600E616 RID: 58902 RVA: 0x00431D3C File Offset: 0x0042FF3C
		public static TaleData_Pawn GenerateFrom(Pawn pawn)
		{
			TaleData_Pawn taleData_Pawn = new TaleData_Pawn();
			taleData_Pawn.pawn = pawn;
			taleData_Pawn.kind = pawn.kindDef;
			taleData_Pawn.faction = pawn.Faction;
			taleData_Pawn.gender = (pawn.RaceProps.hasGenders ? pawn.gender : Gender.None);
			taleData_Pawn.age = pawn.ageTracker.AgeBiologicalYears;
			taleData_Pawn.chronologicalAge = pawn.ageTracker.AgeChronologicalYears;
			taleData_Pawn.everBeenColonistOrTameAnimal = PawnUtility.EverBeenColonistOrTameAnimal(pawn);
			taleData_Pawn.everBeenQuestLodger = PawnUtility.EverBeenQuestLodger(pawn);
			taleData_Pawn.isFactionLeader = (pawn.Faction != null && pawn.Faction.leader == pawn);
			if (pawn.royalty != null)
			{
				taleData_Pawn.royalTitles = new List<RoyalTitle>();
				foreach (RoyalTitle other in pawn.royalty.AllTitlesForReading)
				{
					taleData_Pawn.royalTitles.Add(new RoyalTitle(other));
				}
			}
			TaggedString taggedString = "";
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, pawn);
			taleData_Pawn.relationInfo = taggedString.Resolve();
			if (pawn.story != null)
			{
				taleData_Pawn.title = pawn.story.title;
			}
			if (pawn.RaceProps.Humanlike)
			{
				taleData_Pawn.name = pawn.Name;
				if (pawn.equipment.Primary != null)
				{
					taleData_Pawn.primaryEquipment = pawn.equipment.Primary.def;
				}
				Apparel apparel;
				if (pawn.apparel.WornApparel.TryRandomElement(out apparel))
				{
					taleData_Pawn.notableApparel = apparel.def;
				}
			}
			return taleData_Pawn;
		}

		// Token: 0x0600E617 RID: 58903 RVA: 0x00431EE4 File Offset: 0x004300E4
		public static TaleData_Pawn GenerateRandom(bool humanLike = false)
		{
			object obj;
			if (!humanLike)
			{
				obj = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
				where x.Isnt<CreepJoinerFormKindDef>()
				select x).RandomElement<PawnKindDef>();
			}
			else
			{
				obj = PawnKindDefOf.Drifter;
			}
			object obj2 = obj;
			Faction faction = FactionUtility.DefaultFactionFrom(obj2.defaultFactionDef);
			return TaleData_Pawn.GenerateFrom(PawnGenerator.GeneratePawn(obj2, faction, null));
		}

		// Token: 0x040099D4 RID: 39380
		public Pawn pawn;

		// Token: 0x040099D5 RID: 39381
		public PawnKindDef kind;

		// Token: 0x040099D6 RID: 39382
		public Faction faction;

		// Token: 0x040099D7 RID: 39383
		public Gender gender;

		// Token: 0x040099D8 RID: 39384
		public int age;

		// Token: 0x040099D9 RID: 39385
		public int chronologicalAge;

		// Token: 0x040099DA RID: 39386
		public string relationInfo;

		// Token: 0x040099DB RID: 39387
		public bool everBeenColonistOrTameAnimal;

		// Token: 0x040099DC RID: 39388
		public bool everBeenQuestLodger;

		// Token: 0x040099DD RID: 39389
		public bool isFactionLeader;

		// Token: 0x040099DE RID: 39390
		public List<RoyalTitle> royalTitles;

		// Token: 0x040099DF RID: 39391
		public Name name;

		// Token: 0x040099E0 RID: 39392
		public string title;

		// Token: 0x040099E1 RID: 39393
		public ThingDef primaryEquipment;

		// Token: 0x040099E2 RID: 39394
		public ThingDef notableApparel;

		// Token: 0x040099E3 RID: 39395
		private List<Faction> tmpFactions;

		// Token: 0x040099E4 RID: 39396
		private List<RoyalTitleDef> tmpRoyalTitles;
	}
}
