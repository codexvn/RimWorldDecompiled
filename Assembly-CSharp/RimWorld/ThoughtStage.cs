using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThoughtStage
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__19 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThoughtStage _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__19(int _003C_003E1__state)
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
			ThoughtStage thoughtStage = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!thoughtStage.labelSocial.NullOrEmpty() && thoughtStage.labelSocial == thoughtStage.label)
				{
					_003C_003E2__current = "labelSocial is the same as label. labelSocial is unnecessary in this case";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0064;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0064;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0064:
				if (thoughtStage.baseMoodEffect != 0f && thoughtStage.description.NullOrEmpty())
				{
					_003C_003E2__current = "affects mood but doesn't have a description";
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CConfigErrors_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[MustTranslate]
	public string label;

	[MustTranslate]
	public string labelSocial;

	[MustTranslate]
	public string labelAbstract;

	[MustTranslate]
	public string description;

	public float baseMoodEffect;

	public float baseOpinionOffset;

	public bool visible = true;

	[Unsaved(false)]
	private string cachedLabelCap;

	[Unsaved(false)]
	private string cachedLabelSocialCap;

	[Unsaved(false)]
	private string cachedLabelAbstractCap;

	[Unsaved(false)]
	[TranslationHandle(Priority = 100)]
	public string untranslatedLabel;

	[Unsaved(false)]
	[TranslationHandle]
	public string untranslatedLabelSocial;

	public string LabelCap
	{
		get
		{
			if (cachedLabelCap == null)
			{
				cachedLabelCap = label.CapitalizeFirst();
			}
			return cachedLabelCap;
		}
	}

	public string LabelSocialCap
	{
		get
		{
			if (cachedLabelSocialCap == null)
			{
				cachedLabelSocialCap = labelSocial.CapitalizeFirst();
			}
			return cachedLabelSocialCap;
		}
	}

	public string LabelAbstractCap
	{
		get
		{
			if (cachedLabelAbstractCap == null)
			{
				cachedLabelAbstractCap = ((labelAbstract != null) ? labelAbstract.CapitalizeFirst() : label.CapitalizeFirst());
			}
			return cachedLabelAbstractCap;
		}
	}

	public void PostLoad()
	{
		untranslatedLabel = label;
		untranslatedLabelSocial = labelSocial;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__19))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}
}
