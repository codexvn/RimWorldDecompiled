using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_Dialog : QuestPart
{
	public class Option : IExposable
	{
		public string text;

		public string outSignal;

		public void ExposeData()
		{
			Scribe_Values.Look(ref text, "text");
			Scribe_Values.Look(ref outSignal, "outSignal");
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_InvolvedFactions_003Ed__14 : IEnumerable<Faction>, IEnumerable, IEnumerator<Faction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Faction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Dialog _003C_003E4__this;

		private IEnumerator<Faction> _003C_003E7__wrap1;

		Faction IEnumerator<Faction>.Current
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
		public _003Cget_InvolvedFactions_003Ed__14(int _003C_003E1__state)
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
				QuestPart_Dialog questPart_Dialog = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Dialog).InvolvedFactions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Faction current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_Dialog.relatedFaction != null)
					{
						_003C_003E2__current = questPart_Dialog.relatedFaction;
						_003C_003E1__state = 2;
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
		IEnumerator<Faction> IEnumerable<Faction>.GetEnumerator()
		{
			_003Cget_InvolvedFactions_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InvolvedFactions_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Faction>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__12 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Dialog _003C_003E4__this;

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
		public _003Cget_QuestLookTargets_003Ed__12(int _003C_003E1__state)
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
				QuestPart_Dialog questPart_Dialog = _003C_003E4__this;
				GlobalTargetInfo globalTargetInfo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Dialog).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0071;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0071;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0071:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					globalTargetInfo = questPart_Dialog.lookTargets.TryGetPrimaryTarget();
					if (globalTargetInfo.IsValid)
					{
						_003C_003E2__current = globalTargetInfo;
						_003C_003E1__state = 2;
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

	public string text;

	public string title;

	public List<Option> options = new List<Option>();

	public Faction relatedFaction;

	public bool addToArchive = true;

	public bool radioMode;

	public bool getLookTargetsFromSignal;

	public LookTargets lookTargets;

	public QuestPartDialogCloseAction.CloseActionKey closeAction;

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

	public override IEnumerable<Faction> InvolvedFactions
	{
		[IteratorStateMachine(typeof(_003Cget_InvolvedFactions_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_InvolvedFactions_003Ed__14(-2)
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
		DiaNode diaNode = new DiaNode(signal.args.GetFormattedText(text));
		LookTargets resolvedLookTargets = lookTargets;
		if (getLookTargetsFromSignal && !resolvedLookTargets.IsValid())
		{
			SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out resolvedLookTargets);
		}
		if (resolvedLookTargets.IsValid())
		{
			DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
			diaOption.action = delegate
			{
				CameraJumper.TryJumpAndSelect(resolvedLookTargets.TryGetPrimaryTarget());
			};
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
		}
		if (options.Any())
		{
			for (int i = 0; i < options.Count; i++)
			{
				int localIndex = i;
				DiaOption diaOption2 = new DiaOption(signal.args.GetFormattedText(options[i].text));
				diaOption2.action = delegate
				{
					Find.SignalManager.SendSignal(new Signal(options[localIndex].outSignal));
				};
				diaOption2.resolveTree = true;
				diaNode.options.Add(diaOption2);
			}
		}
		else
		{
			DiaOption diaOption3 = new DiaOption("OK".Translate());
			diaOption3.resolveTree = true;
			diaNode.options.Add(diaOption3);
		}
		TaggedString formattedText = signal.args.GetFormattedText(title);
		Dialog_NodeTreeWithFactionInfo dialog_NodeTreeWithFactionInfo = new Dialog_NodeTreeWithFactionInfo(diaNode, relatedFaction, delayInteractivity: true, radioMode, formattedText);
		QuestPartDialogCloseAction questPartDialogCloseAction = QuestPartDialogCloseActionRegistry.CloseActionOf(closeAction);
		if (questPartDialogCloseAction != null)
		{
			dialog_NodeTreeWithFactionInfo.soundClose = questPartDialogCloseAction.dialogCloseSound;
			dialog_NodeTreeWithFactionInfo.closeAction = questPartDialogCloseAction.dialogCloseAction;
		}
		Find.WindowStack.Add(dialog_NodeTreeWithFactionInfo);
		if (addToArchive)
		{
			Find.Archive.Add(new ArchivedDialog(diaNode.text, formattedText, relatedFaction));
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref text, "text");
		Scribe_Values.Look(ref title, "title");
		Scribe_Collections.Look(ref options, "options", LookMode.Deep);
		Scribe_References.Look(ref relatedFaction, "relatedFaction");
		Scribe_Values.Look(ref addToArchive, "addToArchive", defaultValue: true);
		Scribe_Values.Look(ref radioMode, "radioMode", defaultValue: false);
		Scribe_Values.Look(ref getLookTargetsFromSignal, "getLookTargetsFromSignal", defaultValue: false);
		Scribe_Deep.Look(ref lookTargets, "lookTargets");
		Scribe_Values.Look(ref closeAction, "closeAction", QuestPartDialogCloseAction.CloseActionKey.None);
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		title = "Title";
		text = "DEV: Test";
		relatedFaction = Faction.OfMechanoids;
		addToArchive = false;
		Option option = new Option();
		option.text = "Option 1";
		option.outSignal = "DebugSignal" + Rand.Int;
		options.Add(option);
		Option option2 = new Option();
		option2.text = "Option 2";
		option2.outSignal = "DebugSignal" + Rand.Int;
		options.Add(option2);
	}
}
