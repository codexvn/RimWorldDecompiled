using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse;

public class PlayLogEntry_InteractionSinglePawn : LogEntry
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__10 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PlayLogEntry_InteractionSinglePawn _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__10(int _003C_003E1__state)
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
			PlayLogEntry_InteractionSinglePawn playLogEntry_InteractionSinglePawn = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (playLogEntry_InteractionSinglePawn.initiator != null)
				{
					_003C_003E2__current = playLogEntry_InteractionSinglePawn.initiator;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetConcerns_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__10(0)
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

	protected InteractionDef intDef;

	protected Pawn initiator;

	protected List<RulePackDef> extraSentencePacks;

	public Faction initiatorFaction;

	public Ideo initiatorIdeo;

	protected string InitiatorName
	{
		get
		{
			if (initiator == null)
			{
				return "null";
			}
			return initiator.LabelShort;
		}
	}

	public PlayLogEntry_InteractionSinglePawn()
	{
	}

	public PlayLogEntry_InteractionSinglePawn(InteractionDef intDef, Pawn initiator, List<RulePackDef> extraSentencePacks)
	{
		this.intDef = intDef;
		this.initiator = initiator;
		this.extraSentencePacks = extraSentencePacks;
		initiatorFaction = initiator.Faction;
		initiatorIdeo = initiator.Ideo;
	}

	public override bool Concerns(Thing t)
	{
		return t == initiator;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__10))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (pov == initiator)
		{
			return CameraJumper.CanJump(initiator);
		}
		return false;
	}

	public override void ClickedFromPOV(Thing pov)
	{
		if (pov == initiator)
		{
			CameraJumper.TryJumpAndSelect(initiator);
			return;
		}
		throw new NotImplementedException();
	}

	public override Texture2D IconFromPOV(Thing pov)
	{
		return intDef.GetSymbol(initiatorFaction, initiatorIdeo);
	}

	public override Color? IconColorFromPOV(Thing pov)
	{
		return intDef.GetSymbolColor(initiatorFaction);
	}

	public override void Notify_FactionRemoved(Faction faction)
	{
		if (initiatorFaction == faction)
		{
			initiatorFaction = null;
		}
	}

	public override void Notify_IdeoRemoved(Ideo ideo)
	{
		if (initiatorIdeo == ideo)
		{
			initiatorIdeo = null;
		}
	}

	public override string GetTipString()
	{
		return intDef.LabelCap + "\n" + base.GetTipString();
	}

	protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
	{
		if (initiator == null)
		{
			Log.ErrorOnce("PlayLogEntry_InteractionSinglePawn has a null pawn reference.", 34422);
			return "[" + intDef.label + " error: null pawn reference]";
		}
		Rand.PushState();
		Rand.Seed = logID;
		GrammarRequest request = base.GenerateGrammarRequest();
		string text;
		if (pov == initiator)
		{
			request.IncludesBare.Add(intDef.logRulesInitiator);
			request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiator, request.Constants));
			text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog);
		}
		else
		{
			Log.ErrorOnce("Cannot display PlayLogEntry_InteractionSinglePawn from POV who isn't initiator.", 51251);
			text = ToString();
		}
		if (extraSentencePacks != null)
		{
			for (int i = 0; i < extraSentencePacks.Count; i++)
			{
				request.Clear();
				request.Includes.Add(extraSentencePacks[i]);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiator, request.Constants));
				text = text + " " + GrammarResolver.Resolve(extraSentencePacks[i].FirstRuleKeyword, request, "extraSentencePack", forceLog, extraSentencePacks[i].FirstUntranslatedRuleKeyword);
			}
		}
		Rand.PopState();
		return text;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref intDef, "intDef");
		Scribe_References.Look(ref initiator, "initiator", saveDestroyedThings: true);
		Scribe_Collections.Look(ref extraSentencePacks, "extras", LookMode.Undefined);
		Scribe_References.Look(ref initiatorFaction, "initiatorFaction");
		Scribe_References.Look(ref initiatorIdeo, "initiatorIdeo");
	}

	public override string ToString()
	{
		return intDef.label + ": " + InitiatorName;
	}
}
