using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class NameBank
{
	[CompilerGenerated]
	private sealed class _003Cget_AllNameLists_003Ed__5 : IEnumerable<List<string>>, IEnumerable, IEnumerator<List<string>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private List<string> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public NameBank _003C_003E4__this;

		private int _003Ci_003E5__2;

		private int _003Cj_003E5__3;

		List<string> IEnumerator<List<string>>.Current
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
		public _003Cget_AllNameLists_003Ed__5(int _003C_003E1__state)
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
			NameBank nameBank = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cj_003E5__3++;
				goto IL_006d;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_008a;
			IL_006d:
			if (_003Cj_003E5__3 < numSlots)
			{
				_003C_003E2__current = nameBank.names[_003Ci_003E5__2, _003Cj_003E5__3];
				_003C_003E1__state = 1;
				return true;
			}
			_003Ci_003E5__2++;
			goto IL_008a;
			IL_008a:
			if (_003Ci_003E5__2 < numGenders)
			{
				_003Cj_003E5__3 = 0;
				goto IL_006d;
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
		IEnumerator<List<string>> IEnumerable<List<string>>.GetEnumerator()
		{
			_003Cget_AllNameLists_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllNameLists_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<List<string>>)this).GetEnumerator();
		}
	}

	public PawnNameCategory nameType;

	private List<string>[,] names;

	private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

	private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;

	private IEnumerable<List<string>> AllNameLists
	{
		[IteratorStateMachine(typeof(_003Cget_AllNameLists_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllNameLists_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public NameBank(PawnNameCategory ID)
	{
		nameType = ID;
		names = new List<string>[numGenders, numSlots];
		for (int i = 0; i < numGenders; i++)
		{
			for (int j = 0; j < numSlots; j++)
			{
				names[i, j] = new List<string>();
			}
		}
	}

	public void ErrorCheck()
	{
		foreach (List<string> allNameList in AllNameLists)
		{
			foreach (string item in (from x in allNameList
				group x by x into g
				where g.Count() > 1
				select g.Key).ToList())
			{
				Log.Error("Duplicated name: " + item);
			}
			foreach (string item2 in allNameList)
			{
				if (item2.Trim() != item2)
				{
					Log.Error("Trimmable whitespace on name: [" + item2 + "]");
				}
			}
		}
	}

	private List<string> NamesFor(PawnNameSlot slot, Gender gender)
	{
		return names[(uint)gender, (uint)slot];
	}

	public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
	{
		foreach (string item in namesToAdd)
		{
			NamesFor(slot, gender).Add(item);
		}
	}

	public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
	{
		AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
	}

	public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
	{
		List<string> list = NamesFor(slot, gender);
		int num = 0;
		if (list.Count == 0)
		{
			Log.Error("Name list for gender=" + gender.ToString() + " slot=" + slot.ToString() + " is empty.");
			return "Errorname";
		}
		string text;
		do
		{
			text = list.RandomElement();
			if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
			{
				return text;
			}
			num++;
		}
		while (num <= 50);
		return text;
	}
}
