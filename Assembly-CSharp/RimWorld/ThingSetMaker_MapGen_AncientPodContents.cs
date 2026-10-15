using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThingSetMaker_MapGen_AncientPodContents : ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__15 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllGeneratableThingsDebugSub_003Ed__15(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = PawnKindDefOf.AncientSoldier.race;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = PawnKindDefOf.Slave.race;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = PawnKindDefOf.Megascarab.race;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Gold;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.Plasteel;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.ComponentIndustrial;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.ComponentSpacer;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllGeneratableThingsDebugSub_003Ed__15(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		PodContentsType podContentsType = parms.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(disallowFirstValue: true);
		switch (podContentsType)
		{
		case PodContentsType.AncientFriendly:
			outThings.Add(GenerateFriendlyAncient_NewTemp(parms));
			break;
		case PodContentsType.AncientIncapped:
			outThings.Add(GenerateIncappedAncient_NewTemp(parms));
			break;
		case PodContentsType.AncientHostile:
			outThings.Add(GenerateAngryAncient_NewTemp(parms));
			break;
		case PodContentsType.Slave:
			outThings.Add(GenerateSlave_NewTemp(parms));
			break;
		case PodContentsType.AncientHalfEaten:
			outThings.Add(GenerateHalfEatenAncient_NewTemp(parms));
			outThings.AddRange(GenerateScarabs_NewTemp(parms));
			break;
		default:
			Log.Error("Pod contents type not handled: " + podContentsType);
			break;
		case PodContentsType.Empty:
			break;
		}
	}

	private Pawn GenerateFriendlyAncient()
	{
		return GenerateFriendlyAncient_NewTemp(default(ThingSetMakerParams));
	}

	private Pawn GenerateFriendlyAncient_NewTemp(ThingSetMakerParams parms)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: true, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, forceRecruitable: true));
		GiveRandomLootInventoryForTombPawn(pawn);
		return pawn;
	}

	private Pawn GenerateIncappedAncient()
	{
		return GenerateIncappedAncient_NewTemp(default(ThingSetMakerParams));
	}

	private Pawn GenerateIncappedAncient_NewTemp(ThingSetMakerParams parms)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: true, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, forceRecruitable: true));
		HealthUtility.DamageUntilDowned(pawn);
		GiveRandomLootInventoryForTombPawn(pawn);
		return pawn;
	}

	private Pawn GenerateSlave()
	{
		return GenerateSlave_NewTemp(default(ThingSetMakerParams));
	}

	private Pawn GenerateSlave_NewTemp(ThingSetMakerParams parms)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: true, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, forceRecruitable: true));
		HealthUtility.DamageUntilDowned(pawn);
		GiveRandomLootInventoryForTombPawn(pawn);
		if (Rand.Value < 0.5f)
		{
			HealthUtility.DamageUntilDead(pawn);
		}
		return pawn;
	}

	private Pawn GenerateAngryAncient()
	{
		return GenerateAngryAncient_NewTemp(default(ThingSetMakerParams));
	}

	private Pawn GenerateAngryAncient_NewTemp(ThingSetMakerParams parms)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: true, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, forceRecruitable: true));
		GiveRandomLootInventoryForTombPawn(pawn);
		return pawn;
	}

	private Pawn GenerateHalfEatenAncient()
	{
		return GenerateHalfEatenAncient_NewTemp(default(ThingSetMakerParams));
	}

	private Pawn GenerateHalfEatenAncient_NewTemp(ThingSetMakerParams parms)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: true, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, forceRecruitable: true));
		int num = Rand.Range(6, 10);
		for (int i = 0; i < num; i++)
		{
			pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, Rand.Range(3, 8), 0f, -1f, pawn));
		}
		GiveRandomLootInventoryForTombPawn(pawn);
		return pawn;
	}

	private List<Thing> GenerateScarabs()
	{
		return GenerateScarabs_NewTemp(default(ThingSetMakerParams));
	}

	private List<Thing> GenerateScarabs_NewTemp(ThingSetMakerParams parms)
	{
		List<Thing> list = new List<Thing>();
		int num = Rand.Range(3, 6);
		for (int i = 0; i < num; i++)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megascarab, null, parms.tile);
			pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter);
			list.Add(pawn);
		}
		return list;
	}

	private void GiveRandomLootInventoryForTombPawn(Pawn p)
	{
		if (Rand.Value < 0.65f)
		{
			MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Gold, Rand.Range(10, 50));
		}
		else
		{
			MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Plasteel, Rand.Range(10, 50));
		}
		if (Rand.Value < 0.7f)
		{
			MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentIndustrial, Rand.Range(-2, 4));
		}
		else
		{
			MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentSpacer, Rand.Range(-2, 4));
		}
	}

	private void MakeIntoContainer(ThingOwner container, ThingDef def, int count)
	{
		if (count > 0)
		{
			Thing thing = ThingMaker.MakeThing(def);
			thing.stackCount = count;
			container.TryAdd(thing);
		}
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__15))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__15(-2);
	}
}
