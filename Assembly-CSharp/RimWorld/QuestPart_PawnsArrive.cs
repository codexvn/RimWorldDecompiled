using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_PawnsArrive : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__12 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_PawnsArrive _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private IEnumerator<Pawn> _003C_003E7__wrap2;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_PawnsArrive questPart_PawnsArrive = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_PawnsArrive).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00be;
				case 3:
					{
						_003C_003E1__state = -4;
						goto IL_0113;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_PawnsArrive.mapParent != null)
					{
						_003C_003E2__current = questPart_PawnsArrive.mapParent;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00be;
					IL_0113:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					break;
					IL_00be:
					if (!questPart_PawnsArrive.addPawnsToLookTargets)
					{
						break;
					}
					_003C_003E7__wrap2 = PawnsArriveQuestPartUtility.GetQuestLookTargets(questPart_PawnsArrive.pawns).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0113;
				}
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public string inSignal;

	public MapParent mapParent;

	public List<Pawn> pawns = new List<Pawn>();

	public PawnsArrivalModeDef arrivalMode;

	public IntVec3 spawnNear = IntVec3.Invalid;

	public bool joinPlayer;

	public bool addPawnsToLookTargets = true;

	public string customLetterText;

	public string customLetterLabel;

	public LetterDef customLetterDef;

	public bool sendStandardLetter = true;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__12))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__12(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool IncreasesPopulation => PawnsArriveQuestPartUtility.IncreasesPopulation(pawns, joinPlayer, makePrisoners: false);

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (signal.tag != inSignal)
		{
			return;
		}
		pawns.RemoveAll((Pawn x) => x.Destroyed);
		if (mapParent == null || !mapParent.HasMap || !quest.IsParentSuitableForQuest(mapParent))
		{
			mapParent = quest.TryFindNewSuitableMapParentForRetarget();
			if (spawnNear.IsValid)
			{
				spawnNear = IntVec3.Invalid;
			}
		}
		if (mapParent == null || !mapParent.HasMap || !pawns.Any())
		{
			return;
		}
		for (int i = 0; i < pawns.Count; i++)
		{
			if (joinPlayer && pawns[i].Faction != Faction.OfPlayer)
			{
				pawns[i].SetFaction(Faction.OfPlayer);
			}
		}
		IncidentParms incidentParms = new IncidentParms();
		incidentParms.target = mapParent.Map;
		incidentParms.spawnCenter = spawnNear;
		PawnsArrivalModeDef pawnsArrivalModeDef = arrivalMode ?? PawnsArrivalModeDefOf.EdgeWalkIn;
		if (!pawnsArrivalModeDef.Worker.CanUseOnMap(mapParent.Map))
		{
			foreach (PawnsArrivalModeDef item in DefDatabase<PawnsArrivalModeDef>.AllDefsListForReading.InRandomOrder())
			{
				if (item.canBeBackup && item.Worker.CanUseOnMap(mapParent.Map))
				{
					pawnsArrivalModeDef = item;
					break;
				}
			}
		}
		if (!pawnsArrivalModeDef.Worker.CanUseOnMap(mapParent.Map))
		{
			Log.Error($"Tried to do pawns arrive on map {mapParent.Map} but could not find a legal arrival mode, current method: {pawnsArrivalModeDef.defName}");
			return;
		}
		pawnsArrivalModeDef.Worker.TryResolveRaidSpawnCenter(incidentParms);
		pawnsArrivalModeDef.Worker.Arrive(pawns, incidentParms);
		if (!sendStandardLetter)
		{
			return;
		}
		TaggedString letterLabel;
		TaggedString letterText;
		if (joinPlayer && pawns.Count == 1 && pawns[0].RaceProps.Humanlike)
		{
			letterText = "LetterRefugeeJoins".Translate(pawns[0].Named("PAWN"));
			letterLabel = "LetterLabelRefugeeJoins".Translate(pawns[0].Named("PAWN"));
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref letterText, ref letterLabel, pawns[0]);
		}
		else
		{
			if (joinPlayer)
			{
				letterText = "LetterPawnsArriveAndJoin".Translate(GenLabel.ThingsLabel(pawns.Cast<Thing>()));
				letterLabel = "LetterLabelPawnsArriveAndJoin".Translate();
			}
			else
			{
				letterText = "LetterPawnsArrive".Translate(GenLabel.ThingsLabel(pawns.Cast<Thing>()));
				letterLabel = "LetterLabelPawnsArrive".Translate();
			}
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref letterLabel, ref letterText, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), informEvenIfSeenBefore: true);
		}
		letterLabel = (customLetterLabel.NullOrEmpty() ? letterLabel : customLetterLabel.Formatted(letterLabel.Named("BASELABEL")));
		letterText = (customLetterText.NullOrEmpty() ? letterText : customLetterText.Formatted(letterText.Named("BASETEXT")));
		Find.LetterStack.ReceiveLetter(letterLabel, letterText, customLetterDef ?? LetterDefOf.PositiveEvent, pawns, null, quest);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Defs.Look(ref arrivalMode, "arrivalMode");
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_Values.Look(ref spawnNear, "spawnNear");
		Scribe_Values.Look(ref joinPlayer, "joinPlayer", defaultValue: false);
		Scribe_Values.Look(ref addPawnsToLookTargets, "addPawnsToLookTargets", defaultValue: true);
		Scribe_Values.Look(ref customLetterLabel, "customLetterLabel");
		Scribe_Values.Look(ref customLetterText, "customLetterText");
		Scribe_Defs.Look(ref customLetterDef, "customLetterDef");
		Scribe_Values.Look(ref sendStandardLetter, "sendStandardLetter", defaultValue: true);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null));
			pawn.relations.everSeenByPlayer = true;
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn);
			}
			pawns.Add(pawn);
			arrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			mapParent = Find.RandomPlayerHomeMap.Parent;
			joinPlayer = true;
		}
	}

	public override bool QuestPartReserves(Pawn p)
	{
		return pawns.Contains(p);
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		pawns.Replace(replace, with);
	}
}
