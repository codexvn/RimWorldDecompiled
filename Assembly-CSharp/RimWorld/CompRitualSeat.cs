using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompRitualSeat : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__4 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompRitualSeat _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__4(int _003C_003E1__state)
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
			CompRitualSeat compRitualSeat = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				tmpIdeos.Clear();
				string ideosString = compRitualSeat.GetIdeosString(tmpIdeos);
				if (string.IsNullOrEmpty(ideosString))
				{
					return false;
				}
				Dialog_InfoCard.Hyperlink[] array = new Dialog_InfoCard.Hyperlink[tmpIdeos.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Dialog_InfoCard.Hyperlink(tmpIdeos[i]);
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelatedToIdeos_Name".Translate(), ideosString, "Stat_Thing_RelatedToIdeos_Desc".Translate(), 1110, null, array);
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private static StringBuilder tmpStringBuilder = new StringBuilder();

	private static List<Ideo> tmpIdeos = new List<Ideo>();

	public override string CompInspectStringExtra()
	{
		string ideosString = GetIdeosString();
		if (string.IsNullOrEmpty(ideosString))
		{
			return null;
		}
		return "RitualSeatOf".Translate(ideosString.Named("IDEOS")).Resolve();
	}

	private string GetIdeosString(List<Ideo> outIdeos = null)
	{
		tmpStringBuilder.Clear();
		foreach (Ideo item in Find.IdeoManager.IdeosInViewOrder)
		{
			bool flag = false;
			foreach (Precept item2 in item.PreceptsListForReading)
			{
				if (item2 is Precept_RitualSeat precept_RitualSeat && precept_RitualSeat.ThingDef == parent.def)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				outIdeos?.Add(item);
				if (tmpStringBuilder.Length > 0)
				{
					tmpStringBuilder.Append(", ");
				}
				tmpStringBuilder.Append(item.name.ApplyTag(item).Resolve());
			}
		}
		return tmpStringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__4))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
