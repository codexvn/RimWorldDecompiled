using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Verse;

public static class GenString
{
	[CompilerGenerated]
	private sealed class _003CSplitBy_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string str;

		public string _003C_003E3__str;

		private int chunkLength;

		public int _003C_003E3__chunkLength;

		private int _003Ci_003E5__2;

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
		public _003CSplitBy_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (str.NullOrEmpty())
				{
					return false;
				}
				if (chunkLength < 1)
				{
					throw new ArgumentException();
				}
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 += chunkLength;
				break;
			}
			if (_003Ci_003E5__2 < str.Length)
			{
				if (chunkLength > str.Length - _003Ci_003E5__2)
				{
					chunkLength = str.Length - _003Ci_003E5__2;
				}
				_003C_003E2__current = str.Substring(_003Ci_003E5__2, chunkLength);
				_003C_003E1__state = 1;
				return true;
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
			_003CSplitBy_003Ed__3 _003CSplitBy_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSplitBy_003Ed__ = this;
			}
			else
			{
				_003CSplitBy_003Ed__ = new _003CSplitBy_003Ed__3(0);
			}
			_003CSplitBy_003Ed__.str = _003C_003E3__str;
			_003CSplitBy_003Ed__.chunkLength = _003C_003E3__chunkLength;
			return _003CSplitBy_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private static string[] numberStrings;

	static GenString()
	{
		numberStrings = new string[10000];
		for (int i = 0; i < 10000; i++)
		{
			numberStrings[i] = (i - 5000).ToString();
		}
	}

	public static string ToStringCached(this int num)
	{
		if (num < -4999)
		{
			return num.ToString();
		}
		if (num > 4999)
		{
			return num.ToString();
		}
		return numberStrings[num + 5000];
	}

	[IteratorStateMachine(typeof(_003CSplitBy_003Ed__3))]
	public static IEnumerable<string> SplitBy(this string str, int chunkLength)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSplitBy_003Ed__3(-2)
		{
			_003C_003E3__str = str,
			_003C_003E3__chunkLength = chunkLength
		};
	}

	public static StringBuilder AppendLineIfNotEmpty(this StringBuilder sb)
	{
		if (sb.Length > 0)
		{
			sb.AppendLine();
		}
		return sb;
	}

	public static int GetHashCodeSafe(this string val)
	{
		return val?.GetHashCode() ?? 0;
	}
}
