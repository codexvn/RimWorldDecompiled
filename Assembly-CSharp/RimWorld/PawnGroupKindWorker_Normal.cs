using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
{
	[CompilerGenerated]
	private sealed class _003CGeneratePawnKindsExample_003Ed__3 : IEnumerable<PawnKindDef>, IEnumerable, IEnumerator<PawnKindDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PawnKindDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private PawnGroupMakerParms parms;

		public PawnGroupMakerParms _003C_003E3__parms;

		private PawnGroupMaker groupMaker;

		public PawnGroupMaker _003C_003E3__groupMaker;

		private IEnumerator<PawnGenOptionWithXenotype> _003C_003E7__wrap1;

		PawnKindDef IEnumerator<PawnKindDef>.Current
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
		public _003CGeneratePawnKindsExample_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003E2__current = _003C_003E7__wrap1.Current.Option.kind;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<PawnKindDef> IEnumerable<PawnKindDef>.GetEnumerator()
		{
			_003CGeneratePawnKindsExample_003Ed__3 _003CGeneratePawnKindsExample_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGeneratePawnKindsExample_003Ed__ = this;
			}
			else
			{
				_003CGeneratePawnKindsExample_003Ed__ = new _003CGeneratePawnKindsExample_003Ed__3(0);
			}
			_003CGeneratePawnKindsExample_003Ed__.parms = _003C_003E3__parms;
			_003CGeneratePawnKindsExample_003Ed__.groupMaker = _003C_003E3__groupMaker;
			return _003CGeneratePawnKindsExample_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PawnKindDef>)this).GetEnumerator();
		}
	}

	public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, FactionDef faction, PawnGroupMakerParms parms = null)
	{
		float num = ((parms != null && parms.points > 0f) ? parms.points : 100000f);
		float num2 = float.MaxValue;
		List<PawnGenOptionWithXenotype> options = PawnGroupMakerUtility.GetOptions(parms, faction, groupMaker.options, num, num, float.MaxValue);
		foreach (PawnGenOptionWithXenotype item in options)
		{
			if (item.Option.kind.isFighter && item.Cost < num2 && PawnGroupMakerUtility.PawnGenOptionValid(item.Option, parms))
			{
				num2 = item.Cost;
			}
		}
		if (num2 == float.MaxValue)
		{
			foreach (PawnGenOptionWithXenotype item2 in options)
			{
				if (item2.Cost < num2 && PawnGroupMakerUtility.PawnGenOptionValid(item2.Option, parms))
				{
					num2 = item2.Cost;
				}
			}
		}
		return num2;
	}

	public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
	{
		if (!base.CanGenerateFrom(parms, groupMaker))
		{
			return false;
		}
		if (!PawnGroupMakerUtility.AnyOptions(parms, parms.faction?.def, groupMaker.options, parms.points))
		{
			return false;
		}
		return true;
	}

	protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
	{
		if (!CanGenerateFrom(parms, groupMaker))
		{
			if (errorOnZeroResults)
			{
				Log.Error("Cannot generate pawns for " + parms.faction?.ToString() + " with " + parms.points + ". Defaulting to a single random cheap group.");
			}
			return;
		}
		bool flag = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
		Predicate<Pawn> predicate = ((parms.raidStrategy != null) ? ((Predicate<Pawn>)((Pawn p) => parms.raidStrategy.Worker.CanUsePawn(parms.points, p, outPawns))) : null);
		bool flag2 = false;
		foreach (PawnGenOptionWithXenotype item in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
		{
			PawnKindDef kind = item.Option.kind;
			Faction faction = parms.faction;
			Ideo ideo = parms.ideo;
			XenotypeDef xenotype = item.Xenotype;
			PlanetTile? tile = parms.tile;
			bool allowFood = flag;
			bool inhabitants = parms.inhabitants;
			Predicate<Pawn> validatorPostGear = predicate;
			PawnGenerationRequest request = new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, tile, forceGenerateNewPawn: false, allowDead: false, parms.faction.deactivated, canGeneratePawnRelations: true, mustBeCapableOfViolence: true, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: true, allowFood, allowAddictions: true, inhabitants, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, validatorPostGear, null, null, null, null, null, null, null, null, null, ideo, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, xenotype, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
			if (parms.raidAgeRestriction != null && parms.raidAgeRestriction.Worker.ShouldApplyToKind(item.Option.kind))
			{
				request.BiologicalAgeRange = parms.raidAgeRestriction.ageRange;
				request.AllowedDevelopmentalStages = parms.raidAgeRestriction.developmentStage;
			}
			if (item.Option.kind.pawnGroupDevelopmentStage.HasValue)
			{
				request.AllowedDevelopmentalStages = item.Option.kind.pawnGroupDevelopmentStage.Value;
			}
			if (!Find.Storyteller.difficulty.ChildRaidersAllowed && parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer))
			{
				request.AllowedDevelopmentalStages = DevelopmentalStage.Adult;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			if (parms.forceOneDowned && !flag2)
			{
				pawn.health.forceDowned = true;
				if (pawn.guest != null)
				{
					pawn.guest.Recruitable = true;
				}
				pawn.mindState.canFleeIndividual = false;
				flag2 = true;
			}
			outPawns.Add(pawn);
		}
	}

	[IteratorStateMachine(typeof(_003CGeneratePawnKindsExample_003Ed__3))]
	public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGeneratePawnKindsExample_003Ed__3(-2)
		{
			_003C_003E3__parms = parms,
			_003C_003E3__groupMaker = groupMaker
		};
	}
}
