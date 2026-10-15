using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ScenPart_StartingMech : ScenPart
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public Pawn mech;

		internal bool _003CPlayerStartingThings_003Eb__0(Pawn p)
		{
			if (MechanitorUtility.IsMechanitor(p))
			{
				return p.mechanitor.CanOverseeSubject(mech);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__8 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_StartingMech _003C_003E4__this;

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
		public _003CGetSummaryListEntries_003Ed__8(int _003C_003E1__state)
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
			ScenPart_StartingMech scenPart_StartingMech = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == "PlayerStartsWith")
				{
					_003C_003E2__current = "Mechanoid".Translate().CapitalizeFirst() + ": " + scenPart_StartingMech.mechKind.LabelCap;
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
			_003CGetSummaryListEntries_003Ed__8 _003CGetSummaryListEntries_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSummaryListEntries_003Ed__ = this;
			}
			else
			{
				_003CGetSummaryListEntries_003Ed__ = new _003CGetSummaryListEntries_003Ed__8(0)
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
	private sealed class _003CPlayerStartingThings_003Ed__9 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ScenPart_StartingMech _003C_003E4__this;

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
		public _003CPlayerStartingThings_003Ed__9(int _003C_003E1__state)
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
			ScenPart_StartingMech scenPart_StartingMech = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass9_0();
				if (scenPart_StartingMech.mechKind == null)
				{
					scenPart_StartingMech.mechKind = scenPart_StartingMech.PossibleMechs.RandomElement();
				}
				PawnGenerationRequest request = new PawnGenerationRequest(scenPart_StartingMech.mechKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null);
				CS_0024_003C_003E8__locals5.mech = PawnGenerator.GeneratePawn(request);
				if (Rand.Chance(scenPart_StartingMech.overseenByPlayerPawnChance) && CS_0024_003C_003E8__locals5.mech.OverseerSubject != null)
				{
					(from p in Find.GameInitData.startingAndOptionalPawns.Take(Find.GameInitData.startingPawnCount)
						where MechanitorUtility.IsMechanitor(p) && p.mechanitor.CanOverseeSubject(CS_0024_003C_003E8__locals5.mech)
						orderby p.mechanitor.OverseenPawns.Count
						select p).RandomElementWithFallback()?.relations.AddDirectRelation(PawnRelationDefOf.Overseer, CS_0024_003C_003E8__locals5.mech);
				}
				_003C_003E2__current = CS_0024_003C_003E8__locals5.mech;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPlayerStartingThings_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPlayerStartingThings_003Ed__9(0)
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

	private PawnKindDef mechKind;

	private float overseenByPlayerPawnChance = 0.5f;

	private IEnumerable<PawnKindDef> PossibleMechs => DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef td) => td.RaceProps.IsMechanoid && td.race.GetCompProperties<CompProperties_OverseerSubject>() != null);

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref mechKind, "mechKind");
		Scribe_Values.Look(ref overseenByPlayerPawnChance, "overseenByPlayerPawnChance", 0f);
	}

	public override void DoEditInterface(Listing_ScenEdit listing)
	{
		Rect scenPartRect = listing.GetScenPartRect(this, 2f * ScenPart.RowHeight + 4f);
		if (Widgets.ButtonText(new Rect(scenPartRect.xMin, scenPartRect.yMin, scenPartRect.width, ScenPart.RowHeight), (mechKind != null) ? mechKind.LabelCap : "RandomMech".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("RandomMech".Translate().CapitalizeFirst(), delegate
			{
				mechKind = null;
			}));
			foreach (PawnKindDef possibleMech in PossibleMechs)
			{
				PawnKindDef localKind = possibleMech;
				list.Add(new FloatMenuOption(localKind.LabelCap, delegate
				{
					mechKind = localKind;
				}));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}
		Rect rect = new Rect(scenPartRect.xMin, scenPartRect.yMin + ScenPart.RowHeight + 4f, scenPartRect.width, ScenPart.RowHeight);
		TaggedString taggedString = "StartOverseenChance".Translate();
		float x = Text.CalcSize(taggedString).x;
		Widgets.Label(new Rect(rect.xMin, rect.yMin, x, ScenPart.RowHeight), taggedString);
		Rect rect2 = new Rect(rect.xMin + x + 4f, rect.yMin, rect.width - x - 4f, ScenPart.RowHeight);
		overseenByPlayerPawnChance = Widgets.HorizontalSlider(rect2, overseenByPlayerPawnChance, 0f, 1f, middleAlignment: false, overseenByPlayerPawnChance.ToStringPercent(), null, null, 0.01f);
	}

	public override void Randomize()
	{
		mechKind = PossibleMechs.RandomElement();
		overseenByPlayerPawnChance = Rand.Range(0f, 1f);
	}

	public override string Summary(Scenario scen)
	{
		return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
	}

	[IteratorStateMachine(typeof(_003CGetSummaryListEntries_003Ed__8))]
	public override IEnumerable<string> GetSummaryListEntries(string tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSummaryListEntries_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}

	[IteratorStateMachine(typeof(_003CPlayerStartingThings_003Ed__9))]
	public override IEnumerable<Thing> PlayerStartingThings()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlayerStartingThings_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ ((mechKind != null) ? mechKind.GetHashCode() : 0);
	}
}
