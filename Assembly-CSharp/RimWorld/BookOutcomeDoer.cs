using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public abstract class BookOutcomeDoer : ReadingOutcomeDoer
{
	[CompilerGenerated]
	private sealed class _003CGetHyperlinks_003Ed__13 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CGetHyperlinks_003Ed__13(int _003C_003E1__state)
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
			if (_003C_003E1__state != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetHyperlinks_003Ed__13(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	private CompQuality compQualityCached;

	public new BookOutcomeProperties Props => (BookOutcomeProperties)props;

	public Book Book => (Book)base.Parent;

	private CompQuality CompQuality => compQualityCached ?? (compQualityCached = base.Parent.GetComp<CompQuality>());

	public QualityCategory Quality => CompQuality.Quality;

	public abstract bool DoesProvidesOutcome(Pawn reader);

	public virtual void OnBookGenerated(Pawn author = null)
	{
	}

	public virtual string GetBenefitsString(Pawn reader = null)
	{
		return "";
	}

	public virtual bool BenefitDetailsCanChange(Pawn reader = null)
	{
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetHyperlinks_003Ed__13))]
	public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetHyperlinks_003Ed__13(-2);
	}

	public virtual IEnumerable<RulePack> GetTopicRulePacks()
	{
		return null;
	}

	public virtual IEnumerable<Rule_String> GetTopicRuleStrings()
	{
		return null;
	}

	public virtual IEnumerable<Gizmo> GetGizmos()
	{
		return Enumerable.Empty<Gizmo>();
	}

	public virtual void Reset()
	{
	}
}
