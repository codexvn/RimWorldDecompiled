using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Quest : IExposable, ILoadReferenceable, ISignalReceiver
{
	[CompilerGenerated]
	private sealed class _003Cget_Hyperlinks_003Ed__59 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Quest _003C_003E4__this;

		private IEnumerator<Quest> _003C_003E7__wrap1;

		private IEnumerator<Dialog_InfoCard.Hyperlink> _003C_003E7__wrap2;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003Cget_Hyperlinks_003Ed__59(int _003C_003E1__state)
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
			case 2:
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
				Quest quest = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (quest.parent != null)
					{
						_003C_003E2__current = new Dialog_InfoCard.Hyperlink(quest.parent);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0061;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0061;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00da;
				case 3:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_00da:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Quest current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = new Dialog_InfoCard.Hyperlink(current);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = quest.parts.SelectMany((QuestPart x) => x.Hyperlinks).Distinct().GetEnumerator();
					_003C_003E1__state = -4;
					break;
					IL_0061:
					_003C_003E7__wrap1 = (from q in quest.GetSubquests(null)
						orderby q.Historical
						select q).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00da;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Dialog_InfoCard.Hyperlink current2 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003Cget_Hyperlinks_003Ed__59 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Hyperlinks_003Ed__59(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	public int id;

	private List<QuestPart> parts = new List<QuestPart>();

	public string name;

	public TaggedString description;

	public float points;

	public int challengeRating = -1;

	public List<string> tags = new List<string>();

	public string lastSlateStateDebug;

	public QuestScriptDef root;

	public bool hidden;

	public Quest parent;

	public int appearanceTick = -1;

	public int acceptanceTick = -1;

	public int acceptanceExpireTick = -1;

	public bool initiallyAccepted;

	public bool dismissed;

	public bool hiddenInUI;

	private Pawn accepterPawn;

	private string accepterPawnLabel;

	public bool charity;

	public bool canGenerateInSpace;

	public List<string> signalsReceivedDebug;

	private bool ended;

	private QuestEndOutcome endOutcome;

	private bool cleanedUp;

	public int cleanupTick = -1;

	public const int MaxSignalsReceivedDebugCount = 25;

	private const int RemoveAllQuestPartsAfterTicksSinceCleanup = 1800000;

	public List<QuestPart> PartsListForReading => parts;

	public int TicksSinceAppeared => Find.TickManager.TicksGame - appearanceTick;

	public int TicksSinceAccepted
	{
		get
		{
			if (acceptanceTick >= 0)
			{
				return Find.TickManager.TicksGame - acceptanceTick;
			}
			return -1;
		}
	}

	public int TicksSinceCleanup
	{
		get
		{
			if (!cleanedUp)
			{
				return -1;
			}
			return Find.TickManager.TicksGame - cleanupTick;
		}
	}

	public int TicksUntilExpiry
	{
		get
		{
			if (acceptanceExpireTick == -1)
			{
				return -1;
			}
			return Mathf.Max(acceptanceExpireTick - Find.TickManager.TicksGame, 0);
		}
	}

	public string AccepterPawnLabelCap
	{
		get
		{
			if (accepterPawn == null)
			{
				return accepterPawnLabel;
			}
			return accepterPawn.LabelCap;
		}
	}

	public string AddedSignal => $"Quest{id}.Added";

	public string InitiateSignal => $"Quest{id}.Initiate";

	public bool EverAccepted
	{
		get
		{
			if (!initiallyAccepted)
			{
				return acceptanceTick >= 0;
			}
			return true;
		}
	}

	public Pawn AccepterPawn => accepterPawn;

	public bool RequiresAccepter
	{
		get
		{
			for (int i = 0; i < parts.Count; i++)
			{
				if (parts[i].RequiresAccepter)
				{
					return true;
				}
			}
			return false;
		}
	}

	public QuestState State
	{
		get
		{
			if (TicksUntilExpiry == 0 && acceptanceTick < 0)
			{
				return QuestState.EndedOfferExpired;
			}
			if (ended)
			{
				if (endOutcome == QuestEndOutcome.Success)
				{
					return QuestState.EndedSuccess;
				}
				if (endOutcome == QuestEndOutcome.Fail)
				{
					return QuestState.EndedFailed;
				}
				if (endOutcome == QuestEndOutcome.InvalidPreAcceptance)
				{
					return QuestState.EndedInvalid;
				}
				return QuestState.EndedUnknownOutcome;
			}
			if (acceptanceTick < 0)
			{
				return QuestState.NotYetAccepted;
			}
			return QuestState.Ongoing;
		}
	}

	public IEnumerable<GlobalTargetInfo> QuestLookTargets => parts.SelectMany((QuestPart x) => x.QuestLookTargets).Distinct();

	public IEnumerable<GlobalTargetInfo> QuestSelectTargets => parts.SelectMany((QuestPart x) => x.QuestSelectTargets).Distinct();

	public IEnumerable<Faction> InvolvedFactions => parts.SelectMany((QuestPart x) => x.InvolvedFactions).Distinct();

	public IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
	{
		[IteratorStateMachine(typeof(_003Cget_Hyperlinks_003Ed__59))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Hyperlinks_003Ed__59(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool Historical
	{
		get
		{
			if (State != 0)
			{
				return State != QuestState.Ongoing;
			}
			return false;
		}
	}

	public bool IncreasesPopulation
	{
		get
		{
			for (int i = 0; i < parts.Count; i++)
			{
				if (parts[i].IncreasesPopulation)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static Quest MakeRaw()
	{
		return new Quest
		{
			id = Find.UniqueIDsManager.GetNextQuestID(),
			appearanceTick = Find.TickManager.TicksGame,
			name = "Unnamed quest"
		};
	}

	public void QuestTick()
	{
		if (Historical)
		{
			if (!cleanedUp)
			{
				CleanupQuestParts();
			}
			if (TicksSinceCleanup >= 1800000)
			{
				parts.Clear();
			}
			return;
		}
		if (TicksUntilExpiry == 0 && State == QuestState.NotYetAccepted && !cleanedUp)
		{
			CleanupQuestParts();
		}
		if (Historical)
		{
			return;
		}
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i] is QuestPartActivable { State: QuestPartState.Enabled } questPartActivable)
			{
				try
				{
					questPartActivable.QuestPartTick();
				}
				catch (Exception value)
				{
					Log.Error($"Exception ticking QuestPart: {value}");
				}
				if (Historical)
				{
					break;
				}
			}
		}
	}

	public void AddPart(QuestPart part)
	{
		if (parts.Contains(part))
		{
			Log.Error("Tried to add the same QuestPart twice: " + part.ToStringSafe() + ", quest=" + this.ToStringSafe());
			return;
		}
		part.quest = this;
		parts.Add(part);
	}

	public T AddPart<T>() where T : QuestPart
	{
		T val = Activator.CreateInstance<T>();
		AddPart(val);
		return val;
	}

	public T GetFirstOrAddPart<T>() where T : QuestPart
	{
		return GetFirstPartOfType<T>() ?? AddPart<T>();
	}

	public T GetFirstPartOfType<T>() where T : QuestPart
	{
		foreach (QuestPart part in parts)
		{
			if (part is T result)
			{
				return result;
			}
		}
		return null;
	}

	public bool TryGetFirstPartOfType<T>(out T part) where T : QuestPart
	{
		part = GetFirstPartOfType<T>();
		return part != null;
	}

	public void RemovePart(QuestPart part)
	{
		if (!parts.Contains(part))
		{
			Log.Error("Tried to remove QuestPart which doesn't exist: " + part.ToStringSafe() + ", quest=" + this.ToStringSafe());
			return;
		}
		part.quest = null;
		parts.Remove(part);
	}

	public void Accept(Pawn by)
	{
		if (State == QuestState.NotYetAccepted)
		{
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].PreQuestAccept();
			}
			acceptanceTick = Find.TickManager.TicksGame;
			accepterPawn = by;
			dismissed = false;
			Initiate();
		}
	}

	public void End(QuestEndOutcome outcome, bool sendLetter = true, bool playSound = true)
	{
		if (Historical)
		{
			Log.Error($"Tried to resolve a historical quest. id={id}");
			return;
		}
		ended = true;
		endOutcome = outcome;
		CleanupQuestParts();
		if ((!EverAccepted && State == QuestState.EndedOfferExpired) || !sendLetter || hidden)
		{
			return;
		}
		string key = null;
		string key2 = null;
		LetterDef textLetterDef = null;
		switch (State)
		{
		case QuestState.EndedFailed:
			key2 = "LetterQuestFailedLabel";
			key = "LetterQuestCompletedFail";
			textLetterDef = LetterDefOf.NegativeEvent;
			if (playSound)
			{
				SoundDefOf.Quest_Failed.PlayOneShotOnCamera();
			}
			break;
		case QuestState.EndedSuccess:
			key2 = "LetterQuestCompletedLabel";
			key = "LetterQuestCompletedSuccess";
			textLetterDef = LetterDefOf.PositiveEvent;
			if (playSound)
			{
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera();
			}
			break;
		case QuestState.EndedUnknownOutcome:
			key2 = "LetterQuestConcludedLabel";
			key = "LetterQuestCompletedConcluded";
			textLetterDef = LetterDefOf.NeutralEvent;
			if (playSound)
			{
				SoundDefOf.Quest_Concluded.PlayOneShotOnCamera();
			}
			break;
		}
		Find.LetterStack.ReceiveLetter(key2.Translate(), key.Translate(name.CapitalizeFirst()), textLetterDef, null, null, this, null, null, 0, playSound);
	}

	public bool QuestReserves(Pawn p)
	{
		if (Historical)
		{
			return false;
		}
		int count = parts.Count;
		for (int i = 0; i < count; i++)
		{
			if (parts[i].QuestPartReserves(p))
			{
				return true;
			}
		}
		return false;
	}

	public bool QuestReserves(Faction f)
	{
		if (Historical)
		{
			return false;
		}
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].QuestPartReserves(f))
			{
				return true;
			}
		}
		return false;
	}

	public bool QuestReserves(TransportShip ship)
	{
		if (Historical)
		{
			return false;
		}
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].QuestPartReserves(ship))
			{
				return true;
			}
		}
		return false;
	}

	public void SetInitiallyAccepted()
	{
		acceptanceTick = Find.TickManager.TicksGame;
		acceptanceExpireTick = -1;
		initiallyAccepted = true;
	}

	public void SetNotYetAccepted()
	{
		acceptanceTick = -1;
	}

	public MapParent TryFindNewSuitableMapParentForRetarget()
	{
		return TryFindNewSuitablePlayerMapParentForRetarget();
	}

	public MapParent TryFindNewSuitablePlayerMapParentForRetarget(bool checkQuestScript = true)
	{
		if (checkQuestScript)
		{
			return root.TryFindNewSuitableMapParentForRetarget();
		}
		using (IEnumerator<Map> enumerator = Current.Game.PlayerHomeMaps.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current.Parent;
			}
		}
		foreach (Map map in Current.Game.Maps)
		{
			if (GravshipUtility.GetPlayerGravEngine_NewTemp(map) != null)
			{
				return map.Parent;
			}
		}
		return null;
	}

	public bool IsParentSuitableForQuest(MapParent mapParent)
	{
		return root.IsParentSuitableForQuest(mapParent);
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref id, "id", 0);
		Scribe_Values.Look(ref name, "name");
		Scribe_Values.Look(ref appearanceTick, "appearanceTick", -1);
		Scribe_Values.Look(ref acceptanceTick, "acceptanceTick", -1);
		Scribe_Values.Look(ref acceptanceExpireTick, "acceptanceExpireTick", -1);
		Scribe_References.Look(ref accepterPawn, "acceptedBy");
		Scribe_Values.Look(ref accepterPawnLabel, "acceptedByLabel");
		Scribe_Values.Look(ref ended, "ended", defaultValue: false);
		Scribe_Values.Look(ref endOutcome, "endOutcome", QuestEndOutcome.Unknown);
		Scribe_Values.Look(ref cleanedUp, "cleanedUp", defaultValue: false);
		Scribe_Values.Look(ref cleanupTick, "cleanupTick", -1);
		Scribe_Values.Look(ref initiallyAccepted, "initiallyAccepted", defaultValue: false);
		Scribe_Values.Look(ref dismissed, "dismissed", defaultValue: false);
		Scribe_Values.Look(ref hiddenInUI, "hiddenInUI", defaultValue: false);
		Scribe_Values.Look(ref challengeRating, "challengeRating", 0);
		Scribe_Values.Look(ref description, "description");
		Scribe_Values.Look(ref lastSlateStateDebug, "lastSlateStateDebug");
		Scribe_Defs.Look(ref root, "root");
		Scribe_Values.Look(ref hidden, "hidden", defaultValue: false);
		Scribe_Collections.Look(ref signalsReceivedDebug, "signalsReceivedDebug", LookMode.Undefined);
		Scribe_Collections.Look(ref parts, "parts", LookMode.Deep);
		Scribe_Collections.Look(ref tags, "tags", LookMode.Value);
		Scribe_Values.Look(ref charity, "charity", defaultValue: false);
		Scribe_References.Look(ref parent, "parent");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			if (parts.RemoveAll((QuestPart x) => x == null) != 0)
			{
				Log.Error("Some quest parts were null after loading.");
			}
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].quest = this;
			}
		}
		BackCompatibility.PostExposeData(this);
	}

	public void Notify_PawnDiscarded(Pawn pawn)
	{
		if (accepterPawn == pawn)
		{
			accepterPawn = null;
			accepterPawnLabel = pawn.LabelCap;
		}
		foreach (QuestPart item in PartsListForReading)
		{
			item.Notify_PawnDiscarded(pawn);
		}
	}

	public void Notify_SignalReceived(Signal signal)
	{
		if (!signal.global && !signal.tag.StartsWith($"Quest{id}."))
		{
			return;
		}
		for (int i = 0; i < parts.Count; i++)
		{
			try
			{
				if (parts[i].signalListenMode switch
				{
					QuestPart.SignalListenMode.OngoingOnly => State == QuestState.Ongoing, 
					QuestPart.SignalListenMode.NotYetAcceptedOnly => State == QuestState.NotYetAccepted, 
					QuestPart.SignalListenMode.OngoingOrNotYetAccepted => State == QuestState.Ongoing || State == QuestState.NotYetAccepted, 
					QuestPart.SignalListenMode.HistoricalOnly => Historical, 
					QuestPart.SignalListenMode.Always => true, 
					_ => false, 
				})
				{
					parts[i].Notify_QuestSignalReceived(signal);
				}
			}
			catch (Exception value)
			{
				Log.Error($"Error while processing a quest signal: {value}");
			}
		}
	}

	public void PostAdded()
	{
		Find.SignalManager.SendSignal(new Signal(AddedSignal));
	}

	public void Initiate()
	{
		Find.SignalManager.SendSignal(new Signal(InitiateSignal));
	}

	public void CleanupQuestParts()
	{
		if (cleanedUp)
		{
			return;
		}
		cleanupTick = Find.TickManager.TicksGame;
		for (int i = 0; i < parts.Count; i++)
		{
			try
			{
				parts[i].Notify_PreCleanup();
			}
			catch (Exception value)
			{
				Log.Error($"Error in QuestPart Notify_PreCleanup: {value}");
			}
		}
		for (int j = 0; j < parts.Count; j++)
		{
			try
			{
				parts[j].Cleanup();
			}
			catch (Exception value2)
			{
				Log.Error($"Error in QuestPart cleanup: {value2}");
			}
		}
		cleanedUp = true;
		Find.FactionManager.Notify_QuestCleanedUp(this);
		IdeoUtility.Notify_QuestCleanedUp(this, State);
		if (root.hideOnCleanup)
		{
			hiddenInUI = true;
		}
	}

	public void Notify_ThingsProduced(Pawn worker, List<Thing> things)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Notify_ThingsProduced(worker, things);
		}
	}

	public void Notify_PlantHarvested(Pawn worker, Thing harvested)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Notify_PlantHarvested(worker, harvested);
		}
	}

	public void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Notify_PawnKilled(pawn, dinfo);
		}
	}

	public void Notify_PawnBorn(Thing baby, Thing birther, Pawn mother, Pawn father)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Notify_PawnBorn(baby, birther, mother, father);
		}
	}

	public void Notify_FactionRemoved(Faction faction)
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].Notify_FactionRemoved(faction);
		}
	}

	public string GetUniqueLoadID()
	{
		return $"Quest_{id}";
	}
}
