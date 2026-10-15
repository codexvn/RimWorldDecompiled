using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ScenPart_StartingAnimal : ScenPart
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass17_0
	{
		public Pawn animal;

		internal bool _003CPlayerStartingThings_003Eb__1(Pawn p)
		{
			if (TrainableUtility.CanBeMaster(p, animal, checkSpawned: false) && !p.story.traits.HasTrait(TraitDefOf.Psychopath))
			{
				return !p.Inhumanized();
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__13 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_StartingAnimal _003C_003E4__this;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetSummaryListEntries_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ScenPart_StartingAnimal scenPart_StartingAnimal = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == "PlayerStartsWith")
				{
					_003C_003E2__current = scenPart_StartingAnimal.CurrentAnimalLabel().CapitalizeFirst() + " x" + scenPart_StartingAnimal.count;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetSummaryListEntries_003Ed__13 _003CGetSummaryListEntries_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSummaryListEntries_003Ed__ = this;
			}
			else
			{
				_003CGetSummaryListEntries_003Ed__ = new _003CGetSummaryListEntries_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetSummaryListEntries_003Ed__.tag = _003C_003E3__tag;
			return _003CGetSummaryListEntries_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPlayerStartingThings_003Ed__17 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ScenPart_StartingAnimal _003C_003E4__this;

		private int _003Ci_003E5__2;

		Thing IEnumerator<Thing>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CPlayerStartingThings_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ScenPart_StartingAnimal CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < CS_0024_003C_003E8__locals12.count)
			{
				_003C_003Ec__DisplayClass17_0 CS_0024_003C_003E8__locals19 = new _003C_003Ec__DisplayClass17_0();
				PawnKindDef kindDef = ((CS_0024_003C_003E8__locals12.animalKind == null) ? CS_0024_003C_003E8__locals12.RandomPets().RandomElementByWeightWithFallback((PawnKindDef td) => CS_0024_003C_003E8__locals12.PetWeight(td)) : CS_0024_003C_003E8__locals12.animalKind);
				CS_0024_003C_003E8__locals19.animal = PawnGenerator.GeneratePawn(kindDef, Faction.OfPlayer, null);
				if (CS_0024_003C_003E8__locals19.animal.Name == null || CS_0024_003C_003E8__locals19.animal.Name.Numerical)
				{
					CS_0024_003C_003E8__locals19.animal.Name = PawnBioAndNameGenerator.GeneratePawnName(CS_0024_003C_003E8__locals19.animal);
				}
				if (Rand.Value < CS_0024_003C_003E8__locals12.bondToRandomPlayerPawnChance && CS_0024_003C_003E8__locals19.animal.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted)
				{
					Pawn pawn = (from p in Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount)
						where TrainableUtility.CanBeMaster(p, CS_0024_003C_003E8__locals19.animal, checkSpawned: false) && !p.story.traits.HasTrait(TraitDefOf.Psychopath) && !p.Inhumanized()
						select p).RandomElementWithFallback();
					if (pawn != null)
					{
						CS_0024_003C_003E8__locals19.animal.training.Train(TrainableDefOf.Obedience, null, complete: true);
						CS_0024_003C_003E8__locals19.animal.training.SetWantedRecursive(TrainableDefOf.Obedience, checkOn: true);
						CS_0024_003C_003E8__locals19.animal.playerSettings.Master = pawn;
						if (pawn.Ideo == null || pawn.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.Bonded, pawn.Named(HistoryEventArgsNames.Doer))))
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Bond, CS_0024_003C_003E8__locals19.animal);
						}
					}
				}
				_003C_003E2__current = CS_0024_003C_003E8__locals19.animal;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPlayerStartingThings_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPlayerStartingThings_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private PawnKindDef animalKind;

	private int count = 1;

	private float bondToRandomPlayerPawnChance = 0.5f;

	private string countBuf;

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

	public const float VeneratedAnimalWeight = 8f;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref animalKind, "animalKind");
		Scribe_Values.Look(ref count, "count", 0);
		Scribe_Values.Look(ref bondToRandomPlayerPawnChance, "bondToRandomPlayerPawnChance", 0f);
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f + 1f);
		Listing_Standard listing_Standard = new Listing_Standard();
		listing_Standard.Begin(scenPartRect.TopHalf());
		listing_Standard.ColumnWidth = scenPartRect.width;
		listing_Standard.TextFieldNumeric(ref count, ref countBuf, 1f);
		listing_Standard.End();
		if (!Widgets.ButtonText(scenPartRect.BottomHalf(), CurrentAnimalLabel().CapitalizeFirst(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			return;
		}
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		list.Add(new FloatMenuOption("RandomPet".Translate().CapitalizeFirst(), delegate
		{
			animalKind = null;
		}));
		foreach (PawnKindDef item in PossibleAnimals(checkForTamer: false))
		{
			PawnKindDef localKind = item;
			list.Add(new FloatMenuOption(localKind.LabelCap, delegate
			{
				animalKind = localKind;
			}));
		}
		Find.WindowStack.Add(new FloatMenu(list));
	}

	private IEnumerable<PawnKindDef> PossibleAnimals(bool checkForTamer = true)
	{
		return DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef td) => td.RaceProps.Animal && (!checkForTamer || CanKeepPetTame(td)));
	}

	private static bool CanKeepPetTame(PawnKindDef def)
	{
		int level = Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount).MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level).skills.GetSkill(SkillDefOf.Animals).Level;
		float statValueAbstract = def.race.GetStatValueAbstract(StatDefOf.MinimumHandlingSkill);
		return (float)level >= statValueAbstract;
	}

	private IEnumerable<PawnKindDef> RandomPets()
	{
		IEnumerable<PawnKindDef> enumerable = from td in PossibleAnimals()
			where td.RaceProps.petness > 0f
			select td;
		if (enumerable.EnumerableNullOrEmpty())
		{
			enumerable = from td in PossibleAnimals(checkForTamer: false)
				where td.RaceProps.petness > 0f
				select td;
		}
		return enumerable;
	}

	private string CurrentAnimalLabel()
	{
		if (animalKind == null)
		{
			return "RandomPet".TranslateSimple();
		}
		return animalKind.label;
	}

	public override string Summary(Scenario scen)
	{
		return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
	}

	[IteratorStateMachine(typeof(_003CGetSummaryListEntries_003Ed__13))]
	public override IEnumerable<string> GetSummaryListEntries(string tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSummaryListEntries_003Ed__13(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}

	public override void Randomize()
	{
		if (Rand.Value < 0.5f)
		{
			animalKind = null;
		}
		else
		{
			animalKind = PossibleAnimals(checkForTamer: false).RandomElement();
		}
		count = PetCountChances.RandomElementByWeight((Pair<int, float> pa) => pa.Second).First;
		bondToRandomPlayerPawnChance = 0f;
	}

	public override bool TryMerge(ScenPart other)
	{
		if (other is ScenPart_StartingAnimal scenPart_StartingAnimal && scenPart_StartingAnimal.animalKind == animalKind)
		{
			count += scenPart_StartingAnimal.count;
			return true;
		}
		return false;
	}

	private float PetWeight(PawnKindDef animal)
	{
		Ideo ideo = Find.GameInitData.playerFaction.ideos?.PrimaryIdeo;
		if (ideo != null)
		{
			foreach (ThingDef veneratedAnimal in ideo.VeneratedAnimals)
			{
				if (veneratedAnimal == animal.race)
				{
					return 8f;
				}
			}
		}
		return animal.RaceProps.petness;
	}

	[IteratorStateMachine(typeof(_003CPlayerStartingThings_003Ed__17))]
	public override IEnumerable<Thing> PlayerStartingThings()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlayerStartingThings_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ ((animalKind != null) ? animalKind.GetHashCode() : 0) ^ count ^ bondToRandomPlayerPawnChance.GetHashCode();
	}
}
