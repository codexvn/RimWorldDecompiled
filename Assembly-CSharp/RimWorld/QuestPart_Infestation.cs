using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_Infestation : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__11 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Infestation _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

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
		public _003Cget_QuestLookTargets_003Ed__11(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				QuestPart_Infestation questPart_Infestation = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Infestation).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00bb;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
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
					if (questPart_Infestation.mapParent != null)
					{
						_003C_003E2__current = questPart_Infestation.mapParent;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00bb;
					IL_00bb:
					if (questPart_Infestation.mapParent != null && questPart_Infestation.mapParent.HasMap && questPart_Infestation.loc.IsValid)
					{
						_003C_003E2__current = new TargetInfo(questPart_Infestation.loc, questPart_Infestation.mapParent.Map);
						_003C_003E1__state = 3;
						return true;
					}
					break;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__11(0)
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

	[CompilerGenerated]
	private sealed class _003Cget_QuestSelectTargets_003Ed__15 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Infestation _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private List<Thing> _003Chives_003E5__3;

		private int _003Ci_003E5__4;

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
		public _003Cget_QuestSelectTargets_003Ed__15(int _003C_003E1__state)
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
			_003Chives_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_Infestation questPart_Infestation = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Infestation).QuestSelectTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_012d;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_Infestation.mapParent == null || !questPart_Infestation.mapParent.HasMap)
					{
						break;
					}
					_003Chives_003E5__3 = questPart_Infestation.mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.Hive);
					_003Ci_003E5__4 = 0;
					goto IL_013f;
					IL_013f:
					if (_003Ci_003E5__4 < _003Chives_003E5__3.Count)
					{
						if (_003Chives_003E5__3[_003Ci_003E5__4] is Hive hive && !hive.questTags.NullOrEmpty() && hive.questTags.Contains(questPart_Infestation.tag))
						{
							_003C_003E2__current = hive;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_012d;
					}
					_003Chives_003E5__3 = null;
					break;
					IL_012d:
					_003Ci_003E5__4++;
					goto IL_013f;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestSelectTargets_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestSelectTargets_003Ed__15(0)
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

	public int hivesCount;

	public MapParent mapParent;

	public string tag;

	public IntVec3? overrideLoc;

	public string customLetterText;

	public string customLetterLabel;

	public LetterDef customLetterDef;

	public bool sendStandardLetter = true;

	private IntVec3 loc = IntVec3.Invalid;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__11))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__11(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override string QuestSelectTargetsLabel => "SelectHiveTargets".Translate();

	public override IEnumerable<GlobalTargetInfo> QuestSelectTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestSelectTargets_003Ed__15))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestSelectTargets_003Ed__15(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignal))
		{
			return;
		}
		loc = IntVec3.Invalid;
		if (mapParent == null || !mapParent.HasMap || !quest.IsParentSuitableForQuest(mapParent))
		{
			mapParent = quest.TryFindNewSuitableMapParentForRetarget() ?? Find.AnyPlayerHomeMap?.Parent;
		}
		if (mapParent == null || !mapParent.HasMap)
		{
			return;
		}
		Thing thing = InfestationUtility.SpawnTunnels(hivesCount, mapParent.Map, spawnAnywhereIfNoGoodCell: true, ignoreRoofedRequirement: true, tag, overrideLoc, null);
		if (thing != null)
		{
			loc = overrideLoc ?? thing.Position;
			if (sendStandardLetter)
			{
				TaggedString label = (customLetterLabel.NullOrEmpty() ? ((TaggedString)IncidentDefOf.Infestation.letterLabel) : customLetterLabel.Formatted(IncidentDefOf.Infestation.letterLabel.Named("BASELABEL")));
				TaggedString text = (customLetterText.NullOrEmpty() ? ((TaggedString)IncidentDefOf.Infestation.letterText) : customLetterText.Formatted(IncidentDefOf.Infestation.letterText.Named("BASETEXT")));
				Find.LetterStack.ReceiveLetter(label, text, customLetterDef ?? IncidentDefOf.Infestation.letterDef, new TargetInfo(loc, mapParent.Map), null, quest);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref hivesCount, "hivesCount", 0);
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_Values.Look(ref customLetterLabel, "customLetterLabel");
		Scribe_Values.Look(ref customLetterText, "customLetterText");
		Scribe_Defs.Look(ref customLetterDef, "customLetterDef");
		Scribe_Values.Look(ref sendStandardLetter, "sendStandardLetter", defaultValue: true);
		Scribe_Values.Look(ref loc, "loc");
		Scribe_Values.Look(ref tag, "tag");
		Scribe_Values.Look(ref overrideLoc, "overrideLoc", null);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && !quest.Historical && (mapParent == null || !mapParent.HasMap))
		{
			Log.Warning($"Quest {quest} loaded with quest part {this} with invalid map and no fallback map. Ending quest.");
			quest.End(QuestEndOutcome.Unknown, sendLetter: false);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			mapParent = Find.RandomPlayerHomeMap.Parent;
			hivesCount = 5;
		}
	}
}
