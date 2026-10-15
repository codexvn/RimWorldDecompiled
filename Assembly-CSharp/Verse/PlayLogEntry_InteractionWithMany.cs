using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.Grammar;

namespace Verse;

public class PlayLogEntry_InteractionWithMany : PlayLogEntry_Interaction
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__4 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PlayLogEntry_InteractionWithMany _003C_003E4__this;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetConcerns_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PlayLogEntry_InteractionWithMany playLogEntry_InteractionWithMany = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = playLogEntry_InteractionWithMany.initiator;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = playLogEntry_InteractionWithMany.recipients.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetConcerns_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__4(0)
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

	private List<Pawn> recipients;

	public PlayLogEntry_InteractionWithMany()
	{
	}

	public PlayLogEntry_InteractionWithMany(InteractionDef intDef, Pawn initiator, List<Pawn> recipients, List<RulePackDef> extraSentencePacks)
	{
		base.intDef = intDef;
		base.initiator = initiator;
		this.recipients = recipients;
		base.extraSentencePacks = extraSentencePacks;
		initiatorFaction = initiator.Faction;
		initiatorIdeo = initiator.Ideo;
	}

	public override bool Concerns(Thing t)
	{
		if (t is Pawn item)
		{
			if (t != initiator)
			{
				return recipients.Contains(item);
			}
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__4))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
	{
		if (initiator == null || recipients == null || recipients.Contains(null))
		{
			Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
			return "[" + intDef.label + " error: null pawn reference]";
		}
		Rand.PushState();
		Rand.Seed = logID;
		GrammarRequest request = base.GenerateGrammarRequest();
		string result;
		if (pov == initiator)
		{
			request.IncludesBare.Add(intDef.logRulesInitiator);
			request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiator, request.Constants));
			result = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog);
		}
		else if (pov is Pawn pawn && recipients.Contains(pawn))
		{
			if (intDef.logRulesRecipient != null)
			{
				request.IncludesBare.Add(intDef.logRulesRecipient);
			}
			else
			{
				request.IncludesBare.Add(intDef.logRulesInitiator);
			}
			request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiator, request.Constants));
			request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", pawn, request.Constants));
			result = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog);
		}
		else
		{
			Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
			result = ToString();
		}
		Rand.PopState();
		return result;
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (recipients.Contains(pov as Pawn))
		{
			return CameraJumper.CanJump(initiator);
		}
		return false;
	}

	public override void ClickedFromPOV(Thing pov)
	{
		if (pov == initiator)
		{
			CameraJumper.TryJumpAndSelect(recipients.RandomElement());
			return;
		}
		if (recipients.Contains(pov as Pawn))
		{
			CameraJumper.TryJumpAndSelect(initiator);
			return;
		}
		throw new NotImplementedException();
	}

	public override string ToString()
	{
		return intDef.label + ": " + base.InitiatorName + "-> Many";
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref recipients, "recipients", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			recipients.RemoveAll((Pawn x) => x == null);
		}
	}
}
