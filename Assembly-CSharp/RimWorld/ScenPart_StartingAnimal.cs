using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028DF RID: 10463
	public class ScenPart_StartingAnimal : ScenPart
	{
		// Token: 0x0600E4CA RID: 58570 RVA: 0x0042D2BB File Offset: 0x0042B4BB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animalKind, "animalKind");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<float>(ref this.bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f, false);
		}

		// Token: 0x0600E4CB RID: 58571 RVA: 0x0042D2FC File Offset: 0x0042B4FC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f + 1f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(scenPartRect.TopHalf());
			listing_Standard.ColumnWidth = scenPartRect.width;
			listing_Standard.TextFieldNumeric<int>(ref this.count, ref this.countBuf, 1f, 1E+09f);
			listing_Standard.End();
			if (Widgets.ButtonText(scenPartRect.BottomHalf(), this.CurrentAnimalLabel().CapitalizeFirst(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("RandomPet".Translate().CapitalizeFirst(), delegate()
				{
					this.animalKind = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				foreach (PawnKindDef localKind2 in this.PossibleAnimals(false))
				{
					PawnKindDef localKind = localKind2;
					list.Add(new FloatMenuOption(localKind.LabelCap, delegate()
					{
						this.animalKind = localKind;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E4CC RID: 58572 RVA: 0x0042D468 File Offset: 0x0042B668
		private IEnumerable<PawnKindDef> PossibleAnimals(bool checkForTamer = true)
		{
			return from td in DefDatabase<PawnKindDef>.AllDefs
			where td.RaceProps.Animal && (!checkForTamer || ScenPart_StartingAnimal.CanKeepPetTame(td))
			select td;
		}

		// Token: 0x0600E4CD RID: 58573 RVA: 0x0042D498 File Offset: 0x0042B698
		private static bool CanKeepPetTame(PawnKindDef def)
		{
			float level = (float)Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level).skills.GetSkill(SkillDefOf.Animals).Level;
			float statValueAbstract = def.race.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill, null);
			return level >= statValueAbstract;
		}

		// Token: 0x0600E4CE RID: 58574 RVA: 0x0042D510 File Offset: 0x0042B710
		private IEnumerable<PawnKindDef> RandomPets()
		{
			IEnumerable<PawnKindDef> enumerable = from td in this.PossibleAnimals(true)
			where td.RaceProps.petness > 0f
			select td;
			if (enumerable.EnumerableNullOrEmpty<PawnKindDef>())
			{
				enumerable = from td in this.PossibleAnimals(false)
				where td.RaceProps.petness > 0f
				select td;
			}
			return enumerable;
		}

		// Token: 0x0600E4CF RID: 58575 RVA: 0x0042D57E File Offset: 0x0042B77E
		private string CurrentAnimalLabel()
		{
			if (this.animalKind == null)
			{
				return "RandomPet".TranslateSimple();
			}
			return this.animalKind.label;
		}

		// Token: 0x0600E4D0 RID: 58576 RVA: 0x0042D59E File Offset: 0x0042B79E
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600E4D1 RID: 58577 RVA: 0x0042D5B0 File Offset: 0x0042B7B0
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return this.CurrentAnimalLabel().CapitalizeFirst() + " x" + this.count.ToString();
			}
			yield break;
		}

		// Token: 0x0600E4D2 RID: 58578 RVA: 0x0042D5C8 File Offset: 0x0042B7C8
		public override void Randomize()
		{
			if (Rand.Value < 0.5f)
			{
				this.animalKind = null;
			}
			else
			{
				this.animalKind = this.PossibleAnimals(false).RandomElement<PawnKindDef>();
			}
			this.count = ScenPart_StartingAnimal.PetCountChances.RandomElementByWeight((Pair<int, float> pa) => pa.Second).First;
			this.bondToRandomPlayerPawnChance = 0f;
		}

		// Token: 0x0600E4D3 RID: 58579 RVA: 0x0042D640 File Offset: 0x0042B840
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StartingAnimal scenPart_StartingAnimal = other as ScenPart_StartingAnimal;
			if (scenPart_StartingAnimal != null && scenPart_StartingAnimal.animalKind == this.animalKind)
			{
				this.count += scenPart_StartingAnimal.count;
				return true;
			}
			return false;
		}

		// Token: 0x0600E4D4 RID: 58580 RVA: 0x0042D67C File Offset: 0x0042B87C
		private float PetWeight(PawnKindDef animal)
		{
			FactionIdeosTracker ideos = Find.GameInitData.playerFaction.ideos;
			Ideo ideo = (ideos != null) ? ideos.PrimaryIdeo : null;
			if (ideo != null)
			{
				using (List<ThingDef>.Enumerator enumerator = ideo.VeneratedAnimals.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current == animal.race)
						{
							return 8f;
						}
					}
				}
			}
			return animal.RaceProps.petness;
		}

		// Token: 0x0600E4D5 RID: 58581 RVA: 0x0042D704 File Offset: 0x0042B904
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			int num;
			for (int i = 0; i < this.count; i = num + 1)
			{
				PawnKindDef kindDef;
				if (this.animalKind != null)
				{
					kindDef = this.animalKind;
				}
				else
				{
					kindDef = this.RandomPets().RandomElementByWeightWithFallback((PawnKindDef td) => this.PetWeight(td), null);
				}
				Pawn animal = PawnGenerator.GeneratePawn(kindDef, Faction.OfPlayer, null);
				if (animal.Name == null || animal.Name.Numerical)
				{
					animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal, NameStyle.Full, null, false, null);
				}
				if (Rand.Value < this.bondToRandomPlayerPawnChance && animal.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted)
				{
					Pawn pawn = (from p in Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount)
					where TrainableUtility.CanBeMaster(p, animal, false) && !p.story.traits.HasTrait(TraitDefOf.Psychopath) && !p.Inhumanized()
					select p).RandomElementWithFallback(null);
					if (pawn != null)
					{
						animal.training.Train(TrainableDefOf.Obedience, null, true);
						animal.training.SetWantedRecursive(TrainableDefOf.Obedience, true);
						animal.playerSettings.Master = pawn;
						if (pawn.Ideo == null || pawn.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.Bonded, pawn.Named(HistoryEventArgsNames.Doer))))
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
						}
					}
				}
				yield return animal;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600E4D6 RID: 58582 RVA: 0x0042D714 File Offset: 0x0042B914
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.animalKind != null) ? this.animalKind.GetHashCode() : 0) ^ this.count ^ this.bondToRandomPlayerPawnChance.GetHashCode();
		}

		// Token: 0x04009926 RID: 39206
		private PawnKindDef animalKind;

		// Token: 0x04009927 RID: 39207
		private int count = 1;

		// Token: 0x04009928 RID: 39208
		private float bondToRandomPlayerPawnChance = 0.5f;

		// Token: 0x04009929 RID: 39209
		private string countBuf;

		// Token: 0x0400992A RID: 39210
		private static readonly List<Pair<int, float>> PetCountChances = new List<Pair<int, float>>
		{
			new Pair<int, float>(1, 20f),
			new Pair<int, float>(2, 10f),
			new Pair<int, float>(3, 5f),
			new Pair<int, float>(4, 3f),
			new Pair<int, float>(5, 1f),
			new Pair<int, float>(6, 1f),
			new Pair<int, float>(7, 1f),
			new Pair<int, float>(8, 1f),
			new Pair<int, float>(9, 1f),
			new Pair<int, float>(10, 0.1f),
			new Pair<int, float>(11, 0.1f),
			new Pair<int, float>(12, 0.1f),
			new Pair<int, float>(13, 0.1f),
			new Pair<int, float>(14, 0.1f)
		};

		// Token: 0x0400992B RID: 39211
		public const float VeneratedAnimalWeight = 8f;
	}
}
