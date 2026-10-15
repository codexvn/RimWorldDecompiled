using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public struct SignalArgs
{
	[CompilerGenerated]
	private sealed class _003Cget_Args_003Ed__9 : IEnumerable<NamedArgument>, IEnumerable, IEnumerator<NamedArgument>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private NamedArgument _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SignalArgs _003C_003E4__this;

		public SignalArgs _003C_003E3___003C_003E4__this;

		private int _003Ci_003E5__2;

		NamedArgument IEnumerator<NamedArgument>.Current
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
		public _003Cget_Args_003Ed__9(int _003C_003E1__state)
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
				if (_003C_003E4__this.count == 0)
				{
					return false;
				}
				if (_003C_003E4__this.args != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_008f;
				}
				_003C_003E2__current = _003C_003E4__this.arg1;
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_008f;
			case 2:
				_003C_003E1__state = -1;
				if (_003C_003E4__this.count >= 2)
				{
					_003C_003E2__current = _003C_003E4__this.arg2;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00f9;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00f9;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0128;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00f9:
				if (_003C_003E4__this.count >= 3)
				{
					_003C_003E2__current = _003C_003E4__this.arg3;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0128;
				IL_0128:
				if (_003C_003E4__this.count >= 4)
				{
					_003C_003E2__current = _003C_003E4__this.arg4;
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_008f:
				if (_003Ci_003E5__2 < _003C_003E4__this.args.Length)
				{
					_003C_003E2__current = _003C_003E4__this.args[_003Ci_003E5__2];
					_003C_003E1__state = 1;
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
		IEnumerator<NamedArgument> IEnumerable<NamedArgument>.GetEnumerator()
		{
			_003Cget_Args_003Ed__9 _003Cget_Args_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_Args_003Ed__ = this;
			}
			else
			{
				_003Cget_Args_003Ed__ = new _003Cget_Args_003Ed__9(0);
			}
			_003Cget_Args_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_Args_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedArgument>)this).GetEnumerator();
		}
	}

	private int count;

	private NamedArgument arg1;

	private NamedArgument arg2;

	private NamedArgument arg3;

	private NamedArgument arg4;

	private NamedArgument[] args;

	public int Count => count;

	public IEnumerable<NamedArgument> Args
	{
		[IteratorStateMachine(typeof(_003Cget_Args_003Ed__9))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Args_003Ed__9(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public SignalArgs(SignalArgs args)
	{
		count = args.count;
		arg1 = args.arg1;
		arg2 = args.arg2;
		arg3 = args.arg3;
		arg4 = args.arg4;
		this.args = args.args;
	}

	public SignalArgs(NamedArgument arg1)
	{
		count = 1;
		this.arg1 = arg1;
		arg2 = default(NamedArgument);
		arg3 = default(NamedArgument);
		arg4 = default(NamedArgument);
		args = null;
	}

	public SignalArgs(NamedArgument arg1, NamedArgument arg2)
	{
		count = 2;
		this.arg1 = arg1;
		this.arg2 = arg2;
		arg3 = default(NamedArgument);
		arg4 = default(NamedArgument);
		args = null;
	}

	public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
	{
		count = 3;
		this.arg1 = arg1;
		this.arg2 = arg2;
		this.arg3 = arg3;
		arg4 = default(NamedArgument);
		args = null;
	}

	public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
	{
		count = 4;
		this.arg1 = arg1;
		this.arg2 = arg2;
		this.arg3 = arg3;
		this.arg4 = arg4;
		args = null;
	}

	public SignalArgs(params NamedArgument[] args)
	{
		count = args.Length;
		if (args.Length > 4)
		{
			arg1 = default(NamedArgument);
			arg2 = default(NamedArgument);
			arg3 = default(NamedArgument);
			arg4 = default(NamedArgument);
			this.args = new NamedArgument[args.Length];
			for (int i = 0; i < args.Length; i++)
			{
				this.args[i] = args[i];
			}
			return;
		}
		if (args.Length == 1)
		{
			arg1 = args[0];
			arg2 = default(NamedArgument);
			arg3 = default(NamedArgument);
			arg4 = default(NamedArgument);
		}
		else if (args.Length == 2)
		{
			arg1 = args[0];
			arg2 = args[1];
			arg3 = default(NamedArgument);
			arg4 = default(NamedArgument);
		}
		else if (args.Length == 3)
		{
			arg1 = args[0];
			arg2 = args[1];
			arg3 = args[2];
			arg4 = default(NamedArgument);
		}
		else if (args.Length == 4)
		{
			arg1 = args[0];
			arg2 = args[1];
			arg3 = args[2];
			arg4 = args[3];
		}
		else
		{
			arg1 = default(NamedArgument);
			arg2 = default(NamedArgument);
			arg3 = default(NamedArgument);
			arg4 = default(NamedArgument);
		}
		this.args = null;
	}

	public bool TryGetArg(int index, out NamedArgument arg)
	{
		if (index < 0 || index >= count)
		{
			arg = default(NamedArgument);
			return false;
		}
		if (args != null)
		{
			arg = args[index];
		}
		else
		{
			switch (index)
			{
			case 0:
				arg = arg1;
				break;
			case 1:
				arg = arg2;
				break;
			case 2:
				arg = arg3;
				break;
			default:
				arg = arg4;
				break;
			}
		}
		return true;
	}

	public bool TryGetArg(string name, out NamedArgument arg)
	{
		if (count == 0)
		{
			arg = default(NamedArgument);
			return false;
		}
		if (args != null)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].label == name)
				{
					arg = args[i];
					return true;
				}
			}
		}
		else
		{
			if (count >= 1 && arg1.label == name)
			{
				arg = arg1;
				return true;
			}
			if (count >= 2 && arg2.label == name)
			{
				arg = arg2;
				return true;
			}
			if (count >= 3 && arg3.label == name)
			{
				arg = arg3;
				return true;
			}
			if (count >= 4 && arg4.label == name)
			{
				arg = arg4;
				return true;
			}
		}
		arg = default(NamedArgument);
		return false;
	}

	public bool TryGetArg<T>(int index, out T arg)
	{
		if (!TryGetArg(index, out var arg2) || !(arg2.arg is T val))
		{
			arg = default(T);
			return false;
		}
		arg = val;
		return true;
	}

	public bool TryGetArg<T>(string name, out T arg)
	{
		if (!TryGetArg(name, out var arg2) || !(arg2.arg is T))
		{
			arg = default(T);
			return false;
		}
		arg = (T)arg2.arg;
		return true;
	}

	public NamedArgument GetArg(int index)
	{
		if (TryGetArg(index, out var arg))
		{
			return arg;
		}
		throw new ArgumentOutOfRangeException("index");
	}

	public NamedArgument GetArg(string name)
	{
		if (TryGetArg(name, out var arg))
		{
			return arg;
		}
		throw new ArgumentException("Could not find arg named " + name);
	}

	public T GetArg<T>(string name)
	{
		if (TryGetArg(name, out T arg))
		{
			return arg;
		}
		throw new ArgumentException("Could not find arg named " + name + " of type " + typeof(T).Name);
	}

	public TaggedString GetFormattedText(TaggedString text)
	{
		if (count == 0)
		{
			return text.Formatted();
		}
		if (args != null)
		{
			return text.Formatted(args);
		}
		if (count == 1)
		{
			return text.Formatted(arg1);
		}
		if (count == 2)
		{
			return text.Formatted(arg1, arg2);
		}
		if (count == 3)
		{
			return text.Formatted(arg1, arg2, arg3);
		}
		return text.Formatted(arg1, arg2, arg3, arg4);
	}

	public TaggedString GetTranslatedText(string textKey)
	{
		if (count == 0)
		{
			return textKey.Translate();
		}
		if (args != null)
		{
			return textKey.Translate(args);
		}
		if (count == 1)
		{
			return textKey.Translate(arg1);
		}
		if (count == 2)
		{
			return textKey.Translate(arg1, arg2);
		}
		if (count == 3)
		{
			return textKey.Translate(arg1, arg2, arg3);
		}
		return textKey.Translate(arg1, arg2, arg3, arg4);
	}

	public void Add(NamedArgument arg)
	{
		if (args != null)
		{
			NamedArgument[] array = new NamedArgument[args.Length + 1];
			for (int i = 0; i < args.Length; i++)
			{
				array[i] = args[i];
			}
			array[^1] = arg;
			args = array;
			count = args.Length;
			return;
		}
		if (count == 0)
		{
			arg1 = arg;
		}
		else if (count == 1)
		{
			arg2 = arg;
		}
		else if (count == 2)
		{
			arg3 = arg;
		}
		else if (count == 3)
		{
			arg4 = arg;
		}
		else
		{
			args = new NamedArgument[5];
			args[0] = arg1;
			args[1] = arg2;
			args[2] = arg3;
			args[3] = arg4;
			args[4] = arg;
			arg1 = default(NamedArgument);
			arg2 = default(NamedArgument);
			arg3 = default(NamedArgument);
			arg4 = default(NamedArgument);
		}
		count++;
	}

	public void Add(NamedArgument arg1, NamedArgument arg2)
	{
		Add(arg1);
		Add(arg2);
	}

	public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
	{
		Add(arg1);
		Add(arg2);
		Add(arg3);
	}

	public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
	{
		Add(arg1);
		Add(arg2);
		Add(arg3);
		Add(arg4);
	}

	public void Add(params NamedArgument[] args)
	{
		for (int i = 0; i < args.Length; i++)
		{
			Add(args[i]);
		}
	}

	public void Add(SignalArgs args)
	{
		if (args.count == 0)
		{
			return;
		}
		if (args.args != null)
		{
			for (int i = 0; i < args.args.Length; i++)
			{
				Add(args.args[i]);
			}
			return;
		}
		if (args.count >= 1)
		{
			Add(args.arg1);
		}
		if (args.count >= 2)
		{
			Add(args.arg2);
		}
		if (args.count >= 3)
		{
			Add(args.arg3);
		}
		if (args.count >= 4)
		{
			Add(args.arg4);
		}
	}
}
