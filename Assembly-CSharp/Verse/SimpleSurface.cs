using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__5 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SimpleSurface _003C_003E4__this;

		private string prefix;

		public string _003C_003E3__prefix;

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
		public _003CConfigErrors_003Ed__5(int _003C_003E1__state)
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
			SimpleSurface simpleSurface = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				for (int i = 0; i < simpleSurface.columns.Count - 1; i++)
				{
					if (simpleSurface.columns[i + 1].x < simpleSurface.columns[i].x)
					{
						_003C_003E2__current = prefix + ": columns are out of order";
						_003C_003E1__state = 1;
						return true;
					}
				}
				break;
			}
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__5 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.prefix = _003C_003E3__prefix;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__4 : IEnumerator<SurfaceColumn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private SurfaceColumn _003C_003E2__current;

		public SimpleSurface _003C_003E4__this;

		private List<SurfaceColumn>.Enumerator _003C_003E7__wrap1;

		SurfaceColumn IEnumerator<SurfaceColumn>.Current
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
		public _003CGetEnumerator_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
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
			_003C_003E7__wrap1 = default(List<SurfaceColumn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SimpleSurface simpleSurface = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = simpleSurface.columns.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					SurfaceColumn current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<SurfaceColumn>.Enumerator);
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
	}

	private List<SurfaceColumn> columns = new List<SurfaceColumn>();

	public float Evaluate(float x, float y)
	{
		if (columns.Count == 0)
		{
			Log.Error("Evaluating a SimpleCurve2D with no columns.");
			return 0f;
		}
		if (x <= columns[0].x)
		{
			return columns[0].y.Evaluate(y);
		}
		if (x >= columns[columns.Count - 1].x)
		{
			return columns[columns.Count - 1].y.Evaluate(y);
		}
		SurfaceColumn surfaceColumn = columns[0];
		SurfaceColumn surfaceColumn2 = columns[columns.Count - 1];
		for (int i = 0; i < columns.Count; i++)
		{
			if (x <= columns[i].x)
			{
				surfaceColumn2 = columns[i];
				if (i > 0)
				{
					surfaceColumn = columns[i - 1];
				}
				break;
			}
		}
		float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
		return Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
	}

	public void Add(SurfaceColumn newColumn)
	{
		columns.Add(newColumn);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	[IteratorStateMachine(typeof(_003CGetEnumerator_003Ed__4))]
	public IEnumerator<SurfaceColumn> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetEnumerator_003Ed__4(0)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__5))]
	public IEnumerable<string> ConfigErrors(string prefix)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__prefix = prefix
		};
	}
}
