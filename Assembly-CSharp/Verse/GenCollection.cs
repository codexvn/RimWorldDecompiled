using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public static class GenCollection
{
	private static class SortStableTempList<T>
	{
		public static List<Pair<T, int>> list = new List<Pair<T, int>>();

		public static bool working;
	}

	private class ComparerChain<T> : IComparer<T>
	{
		private readonly IComparer<T> first;

		private readonly IComparer<T> second;

		public ComparerChain(IComparer<T> first, IComparer<T> second)
		{
			this.first = first;
			this.second = second;
		}

		public int Compare(T x, T y)
		{
			int num = first.Compare(x, y);
			if (num != 0)
			{
				return num;
			}
			return second.Compare(x, y);
		}
	}

	private class DescendingComparer<T> : IComparer<T>
	{
		private readonly IComparer<T> cmp;

		public DescendingComparer(IComparer<T> cmp)
		{
			this.cmp = cmp;
		}

		public int Compare(T x, T y)
		{
			return -cmp.Compare(x, y);
		}
	}

	public class DictionarySlice<T, V> : IReadOnlyDictionary<T, V>, IEnumerable<KeyValuePair<T, V>>, IEnumerable, IReadOnlyCollection<KeyValuePair<T, V>>
	{
		[CompilerGenerated]
		private sealed class _003CGetEnumerator_003Ed__11 : IEnumerator<KeyValuePair<T, V>>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private KeyValuePair<T, V> _003C_003E2__current;

			public DictionarySlice<T, V> _003C_003E4__this;

			private HashSet<T>.Enumerator _003C_003E7__wrap1;

			KeyValuePair<T, V> IEnumerator<KeyValuePair<T, V>>.Current
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
			public _003CGetEnumerator_003Ed__11(int _003C_003E1__state)
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
				_003C_003E7__wrap1 = default(HashSet<T>.Enumerator);
				_003C_003E1__state = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = _003C_003E1__state;
					DictionarySlice<T, V> dictionarySlice = _003C_003E4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						_003C_003E1__state = -1;
						_003C_003E7__wrap1 = dictionarySlice.keys.GetEnumerator();
						_003C_003E1__state = -3;
						break;
					case 1:
						_003C_003E1__state = -3;
						break;
					}
					while (_003C_003E7__wrap1.MoveNext())
					{
						T current = _003C_003E7__wrap1.Current;
						if (current != null && dictionarySlice.underlying.TryGetValue(current, out var value))
						{
							_003C_003E2__current = new KeyValuePair<T, V>(current, value);
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(HashSet<T>.Enumerator);
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

		[CompilerGenerated]
		private sealed class _003Cget_Values_003Ed__3 : IEnumerable<V>, IEnumerable, IEnumerator<V>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private V _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			public DictionarySlice<T, V> _003C_003E4__this;

			private HashSet<T>.Enumerator _003C_003E7__wrap1;

			V IEnumerator<V>.Current
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
			public _003Cget_Values_003Ed__3(int _003C_003E1__state)
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
				_003C_003E7__wrap1 = default(HashSet<T>.Enumerator);
				_003C_003E1__state = -2;
			}

			private bool MoveNext()
			{
				try
				{
					int num = _003C_003E1__state;
					DictionarySlice<T, V> dictionarySlice = _003C_003E4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						_003C_003E1__state = -1;
						_003C_003E7__wrap1 = dictionarySlice.keys.GetEnumerator();
						_003C_003E1__state = -3;
						break;
					case 1:
						_003C_003E1__state = -3;
						break;
					}
					while (_003C_003E7__wrap1.MoveNext())
					{
						T current = _003C_003E7__wrap1.Current;
						if (current != null && dictionarySlice.underlying.TryGetValue(current, out var value))
						{
							_003C_003E2__current = value;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(HashSet<T>.Enumerator);
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
			IEnumerator<V> IEnumerable<V>.GetEnumerator()
			{
				_003Cget_Values_003Ed__3 result;
				if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
				{
					_003C_003E1__state = 0;
					result = this;
				}
				else
				{
					result = new _003Cget_Values_003Ed__3(0)
					{
						_003C_003E4__this = _003C_003E4__this
					};
				}
				return result;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<V>)this).GetEnumerator();
			}
		}

		private HashSet<T> keys;

		private IReadOnlyDictionary<T, V> underlying;

		public V this[T key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException();
				}
				if (!keys.Contains(key))
				{
					throw new KeyNotFoundException(key.ToStringSafe());
				}
				return underlying[key];
			}
		}

		public IEnumerable<V> Values
		{
			[IteratorStateMachine(typeof(DictionarySlice<, >._003Cget_Values_003Ed__3))]
			get
			{
				//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
				return new _003Cget_Values_003Ed__3(-2)
				{
					_003C_003E4__this = this
				};
			}
		}

		public IEnumerable<T> Keys => keys;

		public int Count => keys.Count;

		public bool ContainsKey(T key)
		{
			if (key != null)
			{
				return keys.Contains(key);
			}
			return false;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public DictionarySlice(IReadOnlyDictionary<T, V> dict, IEnumerable<T> keys)
		{
			underlying = dict;
			this.keys = new HashSet<T>(keys);
		}

		[IteratorStateMachine(typeof(DictionarySlice<, >._003CGetEnumerator_003Ed__11))]
		public IEnumerator<KeyValuePair<T, V>> GetEnumerator()
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003CGetEnumerator_003Ed__11(0)
			{
				_003C_003E4__this = this
			};
		}

		public bool TryGetValue(T key, out V value)
		{
			if (!keys.Contains(key))
			{
				value = default(V);
				return false;
			}
			return underlying.TryGetValue(key, out value);
		}
	}

	[CompilerGenerated]
	private sealed class _003CConcat_003Ed__76<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<T> lhs;

		public IEnumerable<T> _003C_003E3__lhs;

		private T rhs;

		public T _003C_003E3__rhs;

		private IEnumerator<T> _003C_003E7__wrap1;

		T IEnumerator<T>.Current
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
		public _003CConcat_003Ed__76(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = lhs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0068;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0068;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0068:
					if (_003C_003E7__wrap1.MoveNext())
					{
						T current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = rhs;
					_003C_003E1__state = 2;
					return true;
				}
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CConcat_003Ed__76<T> _003CConcat_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConcat_003Ed__ = this;
			}
			else
			{
				_003CConcat_003Ed__ = new _003CConcat_003Ed__76<T>(0);
			}
			_003CConcat_003Ed__.lhs = _003C_003E3__lhs;
			_003CConcat_003Ed__.rhs = _003C_003E3__rhs;
			return _003CConcat_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCross_003Ed__75<T, V> : IEnumerable<Pair<T, V>>, IEnumerable, IEnumerator<Pair<T, V>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pair<T, V> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<T> lhs;

		public IEnumerable<T> _003C_003E3__lhs;

		private IEnumerable<V> rhs;

		public IEnumerable<V> _003C_003E3__rhs;

		private T[] _003Clhsv_003E5__2;

		private V[] _003Crhsv_003E5__3;

		private int _003Ci_003E5__4;

		private int _003Cj_003E5__5;

		Pair<T, V> IEnumerator<Pair<T, V>>.Current
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
		public _003CCross_003Ed__75(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clhsv_003E5__2 = null;
			_003Crhsv_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			int num2;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				num2 = _003Cj_003E5__5 + 1;
				_003Cj_003E5__5 = num2;
				goto IL_0098;
			}
			_003C_003E1__state = -1;
			_003Clhsv_003E5__2 = lhs.ToArray();
			_003Crhsv_003E5__3 = rhs.ToArray();
			_003Ci_003E5__4 = 0;
			goto IL_00b8;
			IL_0098:
			if (_003Cj_003E5__5 < _003Crhsv_003E5__3.Length)
			{
				_003C_003E2__current = new Pair<T, V>(_003Clhsv_003E5__2[_003Ci_003E5__4], _003Crhsv_003E5__3[_003Cj_003E5__5]);
				_003C_003E1__state = 1;
				return true;
			}
			num2 = _003Ci_003E5__4 + 1;
			_003Ci_003E5__4 = num2;
			goto IL_00b8;
			IL_00b8:
			if (_003Ci_003E5__4 < _003Clhsv_003E5__2.Length)
			{
				_003Cj_003E5__5 = 0;
				goto IL_0098;
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
		IEnumerator<Pair<T, V>> IEnumerable<Pair<T, V>>.GetEnumerator()
		{
			_003CCross_003Ed__75<T, V> _003CCross_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCross_003Ed__ = this;
			}
			else
			{
				_003CCross_003Ed__ = new _003CCross_003Ed__75<T, V>(0);
			}
			_003CCross_003Ed__.lhs = _003C_003E3__lhs;
			_003CCross_003Ed__.rhs = _003C_003E3__rhs;
			return _003CCross_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pair<T, V>>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CExcept_003Ed__81<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : class
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<T> lhs;

		public IEnumerable<T> _003C_003E3__lhs;

		private T rhs;

		public T _003C_003E3__rhs;

		private IEnumerator<T> _003C_003E7__wrap1;

		T IEnumerator<T>.Current
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
		public _003CExcept_003Ed__81(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = lhs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					T current = _003C_003E7__wrap1.Current;
					if (current != rhs)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CExcept_003Ed__81<T> _003CExcept_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CExcept_003Ed__ = this;
			}
			else
			{
				_003CExcept_003Ed__ = new _003CExcept_003Ed__81<T>(0);
			}
			_003CExcept_003Ed__.lhs = _003C_003E3__lhs;
			_003CExcept_003Ed__.rhs = _003C_003E3__rhs;
			return _003CExcept_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CInRandomOrder_003Ed__9<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<T> source;

		public IEnumerable<T> _003C_003E3__source;

		private IList<T> workingList;

		public IList<T> _003C_003E3__workingList;

		private int _003CcountUnChosen_003E5__2;

		private int _003Crand_003E5__3;

		T IEnumerator<T>.Current
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
		public _003CInRandomOrder_003Ed__9(int _003C_003E1__state)
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
				if (source == null)
				{
					throw new ArgumentNullException("source");
				}
				if (workingList == null)
				{
					workingList = source.ToList();
				}
				else
				{
					workingList.Clear();
					foreach (T item in source)
					{
						workingList.Add(item);
					}
				}
				_003CcountUnChosen_003E5__2 = workingList.Count;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				IList<T> list = workingList;
				int index = _003Crand_003E5__3;
				IList<T> list2 = workingList;
				int index2 = _003CcountUnChosen_003E5__2 - 1;
				T value = workingList[_003CcountUnChosen_003E5__2 - 1];
				T value2 = workingList[_003Crand_003E5__3];
				list[index] = value;
				list2[index2] = value2;
				_003CcountUnChosen_003E5__2--;
				break;
			}
			}
			if (_003CcountUnChosen_003E5__2 > 0)
			{
				_003Crand_003E5__3 = Rand.Range(0, _003CcountUnChosen_003E5__2);
				_003C_003E2__current = workingList[_003Crand_003E5__3];
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CInRandomOrder_003Ed__9<T> _003CInRandomOrder_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CInRandomOrder_003Ed__ = this;
			}
			else
			{
				_003CInRandomOrder_003Ed__ = new _003CInRandomOrder_003Ed__9<T>(0);
			}
			_003CInRandomOrder_003Ed__.source = _003C_003E3__source;
			_003CInRandomOrder_003Ed__.workingList = _003C_003E3__workingList;
			return _003CInRandomOrder_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPairwise_003Ed__82<T> : IEnumerable<(T, T)>, IEnumerable, IEnumerator<(T, T)>, IEnumerator, IDisposable where T : class
	{
		private int _003C_003E1__state;

		private (T, T) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<T> enumerable;

		public IEnumerable<T> _003C_003E3__enumerable;

		private IEnumerator<T> _003Citerator_003E5__2;

		(T, T) IEnumerator<(T, T)>.Current
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
		public _003CPairwise_003Ed__82(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Citerator_003E5__2 = null;
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
				_003Citerator_003E5__2 = enumerable.GetEnumerator();
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Citerator_003E5__2.MoveNext())
			{
				T current = _003Citerator_003E5__2.Current;
				if (!_003Citerator_003E5__2.MoveNext())
				{
					_003C_003E2__current = ValueTuple.Create<T, T>(current, null);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = ValueTuple.Create(current, _003Citerator_003E5__2.Current);
				_003C_003E1__state = 2;
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
		IEnumerator<(T, T)> IEnumerable<(T, T)>.GetEnumerator()
		{
			_003CPairwise_003Ed__82<T> _003CPairwise_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPairwise_003Ed__ = this;
			}
			else
			{
				_003CPairwise_003Ed__ = new _003CPairwise_003Ed__82<T>(0);
			}
			_003CPairwise_003Ed__.enumerable = _003C_003E3__enumerable;
			return _003CPairwise_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(T, T)>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CTakeRandom_003Ed__88<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<T> list;

		public List<T> _003C_003E3__list;

		private int count;

		public int _003C_003E3__count;

		private int _003Ci_003E5__2;

		T IEnumerator<T>.Current
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
		public _003CTakeRandom_003Ed__88(int _003C_003E1__state)
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
				if (list.NullOrEmpty())
				{
					return false;
				}
				_003Ci_003E5__2 = 0;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				int num = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num;
				break;
			}
			}
			if (_003Ci_003E5__2 < count)
			{
				_003C_003E2__current = list[Rand.Range(0, list.Count)];
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CTakeRandom_003Ed__88<T> _003CTakeRandom_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CTakeRandom_003Ed__ = this;
			}
			else
			{
				_003CTakeRandom_003Ed__ = new _003CTakeRandom_003Ed__88<T>(0);
			}
			_003CTakeRandom_003Ed__.list = _003C_003E3__list;
			_003CTakeRandom_003Ed__.count = _003C_003E3__count;
			return _003CTakeRandom_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
	{
		foreach (T item in enumerable)
		{
			hashSet.Add(item);
		}
	}

	public static bool NullOrEmpty<T>(this HashSet<T> hashSet)
	{
		if (hashSet != null)
		{
			return !hashSet.Any();
		}
		return true;
	}

	public static bool Empty<T>(this Queue<T> queue)
	{
		return queue.Count == 0;
	}

	public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dest, IDictionary<TKey, TValue> source)
	{
		foreach (KeyValuePair<TKey, TValue> item in source)
		{
			dest.Add(item.Key, item.Value);
		}
	}

	public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, ICollection<TKey> remove)
	{
		foreach (TKey item in remove)
		{
			if (dict.ContainsKey(item))
			{
				dict.Remove(item);
			}
		}
	}

	public static void SetOrAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
	{
		dict[key] = value;
	}

	public static void AddDistinct<K, V>(this Dictionary<K, V> dict, K key, V value)
	{
		dict.TryAdd(key, value);
	}

	public static void Increment<K>(this Dictionary<K, int> dict, K key)
	{
		if (!dict.TryAdd(key, 1))
		{
			dict[key]++;
		}
	}

	public static bool SharesElementWith<T>(this IEnumerable<T> source, IEnumerable<T> other)
	{
		if (source is IList<T> list && other is IList<T> list2)
		{
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					if (EqualityComparer<T>.Default.Equals(list[i], list2[j]))
					{
						return true;
					}
				}
			}
			return false;
		}
		return source.Any(other.Contains<T>);
	}

	[IteratorStateMachine(typeof(_003CInRandomOrder_003Ed__9<>))]
	public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source, IList<T> workingList = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CInRandomOrder_003Ed__9<T>(-2)
		{
			_003C_003E3__source = source,
			_003C_003E3__workingList = workingList
		};
	}

	public static T RandomElement<T>(this HashSet<T> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.Count == 0)
		{
			Log.Warning("Getting random element from empty hashset.");
			return default(T);
		}
		HashSet<T>.Enumerator enumerator = source.GetEnumerator();
		int num = Rand.Range(0, source.Count);
		T result = default(T);
		while (enumerator.MoveNext())
		{
			if (num == 0)
			{
				result = enumerator.Current;
				break;
			}
			num--;
		}
		enumerator.Dispose();
		return result;
	}

	public static T RandomElement<T>(this IEnumerable<T> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source is ICollection<T> collection)
		{
			if (collection.Count == 0)
			{
				Log.Warning("Getting random element from empty collection.");
				return default(T);
			}
			return source.ElementAt(Rand.Range(0, collection.Count));
		}
		List<T> list = source.ToList();
		if (list.Count == 0)
		{
			Log.Warning("Getting random element from empty collection.");
			return default(T);
		}
		return list[Rand.Range(0, list.Count)];
	}

	public static T RandomElementWithFallback<T>(this IEnumerable<T> source, T fallback = default(T))
	{
		if (source.TryRandomElement(out var result))
		{
			return result;
		}
		return fallback;
	}

	public static bool TryRandomElement<T>(this IEnumerable<T> source, Predicate<T> predicate, out T result)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		IList<T> list = source as IList<T>;
		if (list != null)
		{
			if (list.Count == 0)
			{
				result = default(T);
				return false;
			}
		}
		else
		{
			list = source.ToList();
			if (!list.Any())
			{
				result = default(T);
				return false;
			}
		}
		if (!list.Any((T x) => predicate(x)))
		{
			result = default(T);
			return false;
		}
		result = list.Where((T x) => predicate(x)).RandomElement();
		return true;
	}

	public static bool TryRandomElement<T>(this HashSet<T> source, out T result)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (source.Count == 0)
		{
			result = default(T);
			return false;
		}
		HashSet<T>.Enumerator enumerator = source.GetEnumerator();
		int num = Rand.Range(0, source.Count);
		result = default(T);
		while (enumerator.MoveNext())
		{
			if (num == 0)
			{
				result = enumerator.Current;
				break;
			}
			num--;
		}
		enumerator.Dispose();
		return true;
	}

	public static bool TryRandomElement<T>(this IEnumerable<T> source, out T result)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		ICollection<T> collection = source as ICollection<T>;
		if (collection != null)
		{
			if (collection.Count == 0)
			{
				result = default(T);
				return false;
			}
		}
		else
		{
			collection = source.ToList();
			if (!collection.Any())
			{
				result = default(T);
				return false;
			}
		}
		result = collection.RandomElement();
		return true;
	}

	public static T RandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector)
	{
		float num = 0f;
		IList<T> list = source as IList<T>;
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				float num2 = weightSelector(list[i]);
				if (num2 < 0f)
				{
					Log.Error("Negative weight in selector: " + num2 + " from " + list[i]);
					num2 = 0f;
				}
				num += num2;
			}
			if (list.Count == 1 && num > 0f)
			{
				return list[0];
			}
		}
		else
		{
			int num3 = 0;
			foreach (T item in source)
			{
				num3++;
				float num4 = weightSelector(item);
				if (num4 < 0f)
				{
					string text = num4.ToString();
					T val = item;
					Log.Error("Negative weight in selector: " + text + " from " + val);
					num4 = 0f;
				}
				num += num4;
			}
			if (num3 == 1 && num > 0f)
			{
				return source.First();
			}
		}
		if (num <= 0f)
		{
			Log.Error("RandomElementByWeight with totalWeight=" + num + " - use TryRandomElementByWeight.");
			return default(T);
		}
		float num5 = Rand.Value * num;
		float num6 = 0f;
		if (list != null)
		{
			for (int j = 0; j < list.Count; j++)
			{
				float num7 = weightSelector(list[j]);
				if (!(num7 <= 0f))
				{
					num6 += num7;
					if (num6 >= num5)
					{
						return list[j];
					}
				}
			}
		}
		else
		{
			foreach (T item2 in source)
			{
				float num8 = weightSelector(item2);
				if (!(num8 <= 0f))
				{
					num6 += num8;
					if (num6 >= num5)
					{
						return item2;
					}
				}
			}
		}
		return default(T);
	}

	public static T RandomElementByWeightWithFallback<T>(this IEnumerable<T> source, Func<T, float> weightSelector, T fallback = default(T))
	{
		if (source.TryRandomElementByWeight(weightSelector, out var result))
		{
			return result;
		}
		return fallback;
	}

	public static bool TryRandomElementByWeight<T>(this IEnumerable<T> source, Func<T, float> weightSelector, out T result)
	{
		if (source is IList<T> list)
		{
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				float num2 = weightSelector(list[i]);
				if (num2 < 0f)
				{
					Log.Error("Negative weight in selector: " + num2 + " from " + list[i]);
					num2 = 0f;
				}
				num += num2;
			}
			if (list.Count == 1 && num > 0f)
			{
				result = list[0];
				return true;
			}
			if (num == 0f)
			{
				result = default(T);
				return false;
			}
			num *= Rand.Value;
			for (int j = 0; j < list.Count; j++)
			{
				float num3 = weightSelector(list[j]);
				if (!(num3 <= 0f))
				{
					num -= num3;
					if (num <= 0f)
					{
						result = list[j];
						return true;
					}
				}
			}
		}
		IEnumerator<T> enumerator = source.GetEnumerator();
		result = default(T);
		float num4 = 0f;
		while (num4 == 0f && enumerator.MoveNext())
		{
			result = enumerator.Current;
			num4 = weightSelector(result);
			if (num4 < 0f)
			{
				string text = num4.ToString();
				T val = result;
				Log.Error("Negative weight in selector: " + text + " from " + val);
				num4 = 0f;
			}
		}
		if (num4 == 0f)
		{
			result = default(T);
			return false;
		}
		while (enumerator.MoveNext())
		{
			T current = enumerator.Current;
			float num5 = weightSelector(current);
			if (num5 < 0f)
			{
				string text2 = num5.ToString();
				T val = current;
				Log.Error("Negative weight in selector: " + text2 + " from " + val);
				num5 = 0f;
			}
			if (Rand.Range(0f, num4 + num5) >= num4)
			{
				result = current;
			}
			num4 += num5;
		}
		return true;
	}

	public static T RandomElementByWeightWithDefault<T>(this IEnumerable<T> source, Func<T, float> weightSelector, float defaultValueWeight)
	{
		if (defaultValueWeight < 0f)
		{
			Log.Error("Negative default value weight.");
			defaultValueWeight = 0f;
		}
		float num = 0f;
		foreach (T item in source)
		{
			float num2 = weightSelector(item);
			if (num2 < 0f)
			{
				string text = num2.ToString();
				T val = item;
				Log.Error("Negative weight in selector: " + text + " from " + val);
				num2 = 0f;
			}
			num += num2;
		}
		float num3 = defaultValueWeight + num;
		if (num3 <= 0f)
		{
			Log.Error("RandomElementByWeightWithDefault with totalWeight=" + num3);
			return default(T);
		}
		if (Rand.Value < defaultValueWeight / num3 || num == 0f)
		{
			return default(T);
		}
		return source.RandomElementByWeight(weightSelector);
	}

	public static T FirstOrFallback<T>(this IEnumerable<T> source, T fallback = default(T))
	{
		using IEnumerator<T> enumerator = source.GetEnumerator();
		if (enumerator.MoveNext())
		{
			return enumerator.Current;
		}
		return fallback;
	}

	public static T FirstOrFallback<T>(this IEnumerable<T> source, Func<T, bool> predicate, T fallback = default(T))
	{
		return source.Where(predicate).FirstOrFallback(fallback);
	}

	public static TSource MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector)
	{
		return MaxBy(source, selector, Comparer<TKey>.Default);
	}

	public static TSource MaxBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			throw new InvalidOperationException("Sequence contains no elements");
		}
		TSource val = enumerator.Current;
		TKey y = selector(val);
		while (enumerator.MoveNext())
		{
			TSource current = enumerator.Current;
			TKey val2 = selector(current);
			if (comparer.Compare(val2, y) > 0)
			{
				val = current;
				y = val2;
			}
		}
		return val;
	}

	public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, TSource fallback = default(TSource))
	{
		return source.MaxByWithFallback(selector, Comparer<TKey>.Default, fallback);
	}

	public static TSource MaxByWithFallback<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, TSource fallback = default(TSource))
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			return fallback;
		}
		TSource val = enumerator.Current;
		TKey y = selector(val);
		while (enumerator.MoveNext())
		{
			TSource current = enumerator.Current;
			TKey val2 = selector(current);
			if (comparer.Compare(val2, y) > 0)
			{
				val = current;
				y = val2;
			}
		}
		return val;
	}

	public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
	{
		return source.TryMaxBy(selector, Comparer<TKey>.Default, out value);
	}

	public static bool TryMaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			value = default(TSource);
			return false;
		}
		TSource val = enumerator.Current;
		TKey y = selector(val);
		while (enumerator.MoveNext())
		{
			TSource current = enumerator.Current;
			TKey val2 = selector(current);
			if (comparer.Compare(val2, y) > 0)
			{
				val = current;
				y = val2;
			}
		}
		value = val;
		return true;
	}

	public static TSource MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector)
	{
		return MinBy(source, selector, Comparer<TKey>.Default);
	}

	public static TSource MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			throw new InvalidOperationException("Sequence contains no elements");
		}
		TSource val = enumerator.Current;
		TKey y = selector(val);
		while (enumerator.MoveNext())
		{
			TSource current = enumerator.Current;
			TKey val2 = selector(current);
			if (comparer.Compare(val2, y) < 0)
			{
				val = current;
				y = val2;
			}
		}
		return val;
	}

	public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, out TSource value)
	{
		return source.TryMinBy(selector, Comparer<TKey>.Default, out value);
	}

	public static bool TryMinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer, out TSource value)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		if (selector == null)
		{
			throw new ArgumentNullException("selector");
		}
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		using IEnumerator<TSource> enumerator = source.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			value = default(TSource);
			return false;
		}
		TSource val = enumerator.Current;
		TKey y = selector(val);
		while (enumerator.MoveNext())
		{
			TSource current = enumerator.Current;
			TKey val2 = selector(current);
			if (comparer.Compare(val2, y) < 0)
			{
				val = current;
				y = val2;
			}
		}
		value = val;
		return true;
	}

	public static void SortBy<T, TSortBy>(this List<T> list, Func<T, TSortBy> selector) where TSortBy : IComparable<TSortBy>
	{
		if (list.Count > 1)
		{
			list.Sort((T a, T b) => selector(a).CompareTo(selector(b)));
		}
	}

	public static void SortBy<T, TSortBy, TThenBy>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>
	{
		if (list.Count > 1)
		{
			list.Sort(delegate(T a, T b)
			{
				TSortBy val = selector(a);
				TSortBy other = selector(b);
				return (!val.Equals(other)) ? val.CompareTo(other) : thenBySelector(a).CompareTo(thenBySelector(b));
			});
		}
	}

	public static void SortBy<T, TSortBy, TThenBy, TThenBy2>(this List<T> list, Func<T, TSortBy> selector, Func<T, TThenBy> thenBySelector, Func<T, TThenBy2> thenBy2Selector) where TSortBy : IComparable<TSortBy>, IEquatable<TSortBy> where TThenBy : IComparable<TThenBy>, IEquatable<TThenBy> where TThenBy2 : IComparable<TThenBy2>
	{
		if (list.Count <= 1)
		{
			return;
		}
		list.Sort(delegate(T a, T b)
		{
			TSortBy val = selector(a);
			TSortBy other = selector(b);
			if (!val.Equals(other))
			{
				return val.CompareTo(other);
			}
			TThenBy val2 = thenBySelector(a);
			TThenBy other2 = thenBySelector(b);
			return (!val2.Equals(other2)) ? val2.CompareTo(other2) : thenBy2Selector(a).CompareTo(thenBy2Selector(b));
		});
	}

	public static void SortByColor<T>(this List<T> colorDefs, Func<T, Color> getColor)
	{
		colorDefs.SortBy(delegate(T x)
		{
			Color.RGBToHSV(getColor(x), out var H, out var S, out var _);
			return (!Mathf.Approximately(S, 0f)) ? ((float)Mathf.RoundToInt(H * 100f)) : (-1f);
		}, delegate(T x)
		{
			Color.RGBToHSV(getColor(x), out var _, out var _, out var V2);
			return Mathf.RoundToInt(V2 * 100f);
		});
	}

	public static void SortByDescending<T, TSortByDescending>(this List<T> list, Func<T, TSortByDescending> selector) where TSortByDescending : IComparable<TSortByDescending>
	{
		if (list.Count > 1)
		{
			list.Sort((T a, T b) => selector(b).CompareTo(selector(a)));
		}
	}

	public static void SortByDescending<T, TSortByDescending, TThenByDescending>(this List<T> list, Func<T, TSortByDescending> selector, Func<T, TThenByDescending> thenByDescendingSelector) where TSortByDescending : IComparable<TSortByDescending>, IEquatable<TSortByDescending> where TThenByDescending : IComparable<TThenByDescending>
	{
		if (list.Count > 1)
		{
			list.Sort(delegate(T a, T b)
			{
				TSortByDescending other = selector(a);
				TSortByDescending other2 = selector(b);
				return (!other.Equals(other2)) ? other2.CompareTo(other) : thenByDescendingSelector(b).CompareTo(thenByDescendingSelector(a));
			});
		}
	}

	public static void SortStable<T>(this IList<T> list, Func<T, T, int> comparator)
	{
		if (list.Count <= 1)
		{
			return;
		}
		List<Pair<T, int>> list2;
		bool flag;
		if (SortStableTempList<T>.working)
		{
			list2 = new List<Pair<T, int>>();
			flag = false;
		}
		else
		{
			list2 = SortStableTempList<T>.list;
			SortStableTempList<T>.working = true;
			flag = true;
		}
		try
		{
			list2.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				list2.Add(new Pair<T, int>(list[i], i));
			}
			list2.Sort(delegate(Pair<T, int> lhs, Pair<T, int> rhs)
			{
				int num = comparator(lhs.First, rhs.First);
				return (num != 0) ? num : lhs.Second.CompareTo(rhs.Second);
			});
			list.Clear();
			for (int j = 0; j < list2.Count; j++)
			{
				list.Add(list2[j].First);
			}
			list2.Clear();
		}
		finally
		{
			if (flag)
			{
				SortStableTempList<T>.working = false;
			}
		}
	}

	public static IComparer<T> ThenBy<T>(this IComparer<T> first, IComparer<T> second)
	{
		return new ComparerChain<T>(first, second);
	}

	public static IComparer<T> Descending<T>(this IComparer<T> cmp)
	{
		return new DescendingComparer<T>(cmp);
	}

	public static IComparer<T> CompareBy<T, TComparable>(Func<T, TComparable> selector) where TComparable : IComparable<TComparable>
	{
		return Comparer<T>.Create((T a, T b) => selector(a).CompareTo(selector(b)));
	}

	public static int RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate)
	{
		List<TKey> list = null;
		try
		{
			foreach (KeyValuePair<TKey, TValue> item in dictionary)
			{
				if (predicate(item))
				{
					if (list == null)
					{
						list = SimplePool<List<TKey>>.Get();
					}
					list.Add(item.Key);
				}
			}
			if (list != null)
			{
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					dictionary.Remove(list[i]);
				}
				return list.Count;
			}
			return 0;
		}
		finally
		{
			if (list != null)
			{
				list.Clear();
				SimplePool<List<TKey>>.Return(list);
			}
		}
	}

	public static void RemoveAll<T>(this List<T> list, Func<T, int, bool> predicate)
	{
		int i = 0;
		int count;
		for (count = list.Count; i < count && !predicate(list[i], i); i++)
		{
		}
		if (i >= count)
		{
			return;
		}
		int j = i + 1;
		while (j < count)
		{
			for (; j < count && predicate(list[j], j); j++)
			{
			}
			if (j < count)
			{
				list[i++] = list[j++];
			}
		}
	}

	public static void RemoveLast<T>(this List<T> list)
	{
		list.RemoveAt(list.Count - 1);
	}

	public static T Pop<T>(this List<T> list)
	{
		T result = list[list.Count - 1];
		list.RemoveAt(list.Count - 1);
		return result;
	}

	public static T PopFront<T>(this List<T> list)
	{
		T result = list[0];
		list.RemoveAt(0);
		return result;
	}

	public static bool Any<T>(this List<T> list, Predicate<T> predicate)
	{
		return list.FindIndex(predicate) != -1;
	}

	public static bool Any<T>(this List<T> list)
	{
		return list.Count != 0;
	}

	public static bool Empty<T>(this List<T> list)
	{
		return list.Count == 0;
	}

	public static bool Any<T>(this HashSet<T> list)
	{
		return list.Count != 0;
	}

	public static bool Any<T>(this Stack<T> list)
	{
		return list.Count != 0;
	}

	public static void AddRange<T>(this HashSet<T> set, List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			set.Add(list[i]);
		}
	}

	public static void AddRange<T>(this HashSet<T> set, HashSet<T> other)
	{
		foreach (T item in other)
		{
			set.Add(item);
		}
	}

	public static void AddRangeNoAlloc<T>(this List<T> self, IEnumerator<T> other) where T : class
	{
		while (other.MoveNext())
		{
			self.Add(other.Current);
		}
	}

	public static void AddRangeFast<T>(this List<T> self, IEnumerable<T> other) where T : class
	{
		using IEnumerator<T> other2 = other.GetEnumerator();
		self.AddRangeNoAlloc(other2);
	}

	public static void AddRangeWhereFast<T>(this List<T> self, IEnumerable<T> other, Predicate<T> condition) where T : class
	{
		foreach (T item in other)
		{
			if (condition(item))
			{
				self.Add(item);
			}
		}
	}

	public static int Count_EnumerableBase(IEnumerable e)
	{
		if (e == null)
		{
			return 0;
		}
		if (e is ICollection collection)
		{
			return collection.Count;
		}
		int num = 0;
		foreach (object item in e)
		{
			_ = item;
			num++;
		}
		return num;
	}

	public static T FirstOrDefault<T>(this List<T> list, Predicate<T> predicate)
	{
		foreach (T item in list)
		{
			if (predicate(item))
			{
				return item;
			}
		}
		return default(T);
	}

	public static object FirstOrDefault_EnumerableBase(IEnumerable e)
	{
		if (e == null)
		{
			return null;
		}
		if (e is IList list)
		{
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}
		IEnumerator enumerator = e.GetEnumerator();
		try
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}

	public static float AverageWeighted<T>(this IEnumerable<T> list, Func<T, float> weight, Func<T, float> value)
	{
		float num = 0f;
		float num2 = 0f;
		foreach (T item in list)
		{
			float num3 = weight(item);
			num += num3;
			num2 += value(item) * num3;
		}
		return num2 / num;
	}

	public static void ExecuteEnumerable(this IEnumerable enumerable)
	{
		foreach (object item in enumerable)
		{
			_ = item;
		}
	}

	public static IEnumerable<T> OrElseEmptyEnumerable<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable == null)
		{
			return Enumerable.Empty<T>();
		}
		return enumerable;
	}

	public static bool EnumerableNullOrEmpty<T>(this IEnumerable<T> enumerable)
	{
		if (enumerable == null)
		{
			return true;
		}
		if (enumerable is ICollection collection)
		{
			return collection.Count == 0;
		}
		return !enumerable.Any();
	}

	public static int EnumerableCount(this IEnumerable enumerable)
	{
		if (enumerable == null)
		{
			return 0;
		}
		if (enumerable is ICollection collection)
		{
			return collection.Count;
		}
		int num = 0;
		foreach (object item in enumerable)
		{
			_ = item;
			num++;
		}
		return num;
	}

	public static int Count<T>(this List<T> list, Predicate<T> predicate)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (predicate(list[i]))
			{
				num++;
			}
		}
		return num;
	}

	public static float Percent<T>(this IEnumerable<T> source, Func<T, bool> predicate)
	{
		int num = 0;
		int num2 = 0;
		foreach (T item in source)
		{
			num2++;
			if (predicate(item))
			{
				num++;
			}
		}
		if (num2 == 0)
		{
			throw new InvalidOperationException("Sequence contains no elements");
		}
		return (float)num / (float)num2;
	}

	public static int FirstIndexOf<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
	{
		int num = 0;
		foreach (T item in enumerable)
		{
			if (predicate(item))
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public static V TryGetValue<T, V>(this IReadOnlyDictionary<T, V> dict, T key, V fallback = default(V))
	{
		if (key == null || !dict.TryGetValue(key, out var value))
		{
			return fallback;
		}
		return value;
	}

	public static IReadOnlyDictionary<T, V> Slice<T, V>(this IReadOnlyDictionary<T, V> dict, IEnumerable<T> keys)
	{
		return new DictionarySlice<T, V>(dict, keys);
	}

	public static IReadOnlyDictionary<T, V> Slice<T, V>(this IReadOnlyDictionary<T, V> dict, params T[] keys)
	{
		return new DictionarySlice<T, V>(dict, keys);
	}

	[IteratorStateMachine(typeof(_003CCross_003Ed__75<, >))]
	public static IEnumerable<Pair<T, V>> Cross<T, V>(this IEnumerable<T> lhs, IEnumerable<V> rhs)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCross_003Ed__75<T, V>(-2)
		{
			_003C_003E3__lhs = lhs,
			_003C_003E3__rhs = rhs
		};
	}

	[IteratorStateMachine(typeof(_003CConcat_003Ed__76<>))]
	public static IEnumerable<T> Concat<T>(this IEnumerable<T> lhs, T rhs)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConcat_003Ed__76<T>(-2)
		{
			_003C_003E3__lhs = lhs,
			_003C_003E3__rhs = rhs
		};
	}

	public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
	{
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (predicate(list[num]))
			{
				list.RemoveAt(num);
			}
		}
	}

	public static bool ContainsAny<T>(this IList<T> list, Func<T, bool> predicate)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (predicate(list[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static IEnumerable<TSource> ConcatIfNotNull<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
	{
		if (first == null)
		{
			throw new ArgumentNullException("first");
		}
		if (second == null)
		{
			return first;
		}
		return first.Concat(second);
	}

	public static LocalTargetInfo FirstValid(this List<LocalTargetInfo> source)
	{
		if (source == null)
		{
			return LocalTargetInfo.Invalid;
		}
		for (int i = 0; i < source.Count; i++)
		{
			if (source[i].IsValid)
			{
				return source[i];
			}
		}
		return LocalTargetInfo.Invalid;
	}

	[IteratorStateMachine(typeof(_003CExcept_003Ed__81<>))]
	public static IEnumerable<T> Except<T>(this IEnumerable<T> lhs, T rhs) where T : class
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CExcept_003Ed__81<T>(-2)
		{
			_003C_003E3__lhs = lhs,
			_003C_003E3__rhs = rhs
		};
	}

	[IteratorStateMachine(typeof(_003CPairwise_003Ed__82<>))]
	public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> enumerable) where T : class
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPairwise_003Ed__82<T>(-2)
		{
			_003C_003E3__enumerable = enumerable
		};
	}

	public static bool ListsEqual<T>(List<T> a, List<T> b) where T : class
	{
		if (a == b)
		{
			return true;
		}
		if (a.NullOrEmpty() && b.NullOrEmpty())
		{
			return true;
		}
		if (a.NullOrEmpty() || b.NullOrEmpty())
		{
			return false;
		}
		if (a.Count != b.Count)
		{
			return false;
		}
		EqualityComparer<T> @default = EqualityComparer<T>.Default;
		for (int i = 0; i < a.Count; i++)
		{
			if (!@default.Equals(a[i], b[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool DictsEqual<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> a, IReadOnlyDictionary<TKey, TValue> b)
	{
		int num = a?.Count ?? 0;
		int num2 = b?.Count ?? 0;
		if (num != num2)
		{
			return false;
		}
		if (num == 0)
		{
			return true;
		}
		EqualityComparer<TValue> @default = EqualityComparer<TValue>.Default;
		foreach (KeyValuePair<TKey, TValue> item in a)
		{
			if (!b.TryGetValue(item.Key, out var value) || !@default.Equals(item.Value, value))
			{
				return false;
			}
		}
		foreach (KeyValuePair<TKey, TValue> item2 in b)
		{
			if (!a.TryGetValue(item2.Key, out var value2) || !@default.Equals(item2.Value, value2))
			{
				return false;
			}
		}
		return true;
	}

	public static int DictHashCode<Key, Value>(IReadOnlyDictionary<Key, Value> dict)
	{
		int num = 0;
		foreach (KeyValuePair<Key, Value> item in dict ?? Enumerable.Empty<KeyValuePair<Key, Value>>())
		{
			num ^= HashCode.Combine(item.Key, item.Value);
		}
		return num;
	}

	public static void Deconstruct<Key, Value>(this KeyValuePair<Key, Value> tuple, out Key key, out Value value)
	{
		key = tuple.Key;
		value = tuple.Value;
	}

	public static bool SetsEqual<T>(this List<T> a, List<T> b)
	{
		if (a == b)
		{
			return true;
		}
		if (a.NullOrEmpty() && b.NullOrEmpty())
		{
			return true;
		}
		if (a.NullOrEmpty() || b.NullOrEmpty())
		{
			return false;
		}
		for (int i = 0; i < a.Count; i++)
		{
			if (!b.Contains(a[i]))
			{
				return false;
			}
		}
		for (int j = 0; j < b.Count; j++)
		{
			if (!a.Contains(b[j]))
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CTakeRandom_003Ed__88<>))]
	public static IEnumerable<T> TakeRandom<T>(this List<T> list, int count)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CTakeRandom_003Ed__88<T>(-2)
		{
			_003C_003E3__list = list,
			_003C_003E3__count = count
		};
	}

	public static List<T> TakeRandomDistinct<T>(this List<T> list, int count)
	{
		List<T> list2 = new List<T>();
		foreach (T item in list.Distinct().InRandomOrder())
		{
			list2.Add(item);
			if (list2.Count == count)
			{
				return list2;
			}
		}
		return list2;
	}

	public static List<T> TakeRandomDistinct<T>(this IEnumerable<T> list, int count)
	{
		List<T> list2 = new List<T>();
		foreach (T item in list.Distinct().InRandomOrder())
		{
			list2.Add(item);
			if (list2.Count == count)
			{
				return list2;
			}
		}
		return list2;
	}

	public static void AddDistinct<T>(this List<T> list, T obj)
	{
		list.AddUnique(obj);
	}

	public static void AddUnique<T>(this List<T> list, T obj)
	{
		if (!list.Contains(obj))
		{
			list.Add(obj);
		}
	}

	public static void AddRangeUnique<T>(this List<T> list, List<T> other)
	{
		for (int i = 0; i < other.Count; i++)
		{
			if (!list.Contains(other[i]))
			{
				list.Add(other[i]);
			}
		}
	}

	public static void AddRangeUnique<T>(this List<T> list, IEnumerable<T> other)
	{
		foreach (T item in other)
		{
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
	}

	public static int Replace<T>(this IList<T> list, T replace, T with) where T : class
	{
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == replace)
			{
				list[i] = with;
				num++;
			}
		}
		return num;
	}

	public static Pair<K, List<E>> ConvertIGroupingToPair<K, E>(IGrouping<K, E> g)
	{
		return new Pair<K, List<E>>(g.Key, g.ToList());
	}

	public static int GetCountGreaterOrEqualInSortedList(List<int> list, int val)
	{
		int num = list.BinarySearch(val);
		if (num >= 0)
		{
			return list.Count - num;
		}
		int num2 = ~num;
		return list.Count - num2;
	}

	public static void InsertIntoSortedList<T>(this List<T> list, T val, IComparer<T> cmp)
	{
		if (list.Count == 0)
		{
			list.Add(val);
			return;
		}
		int num = list.BinarySearch(val, cmp);
		if (num >= 0)
		{
			list.Insert(num, val);
		}
		else
		{
			list.Insert(~num, val);
		}
	}

	public static void RemoveBatchUnordered<T>(this List<T> list, List<int> indices)
	{
		if (indices.Count == 0)
		{
			return;
		}
		int num = list.Count - 1;
		foreach (int index in indices)
		{
			if (num == index)
			{
				num--;
				continue;
			}
			if (num <= 0)
			{
				break;
			}
			list[index] = list[num];
			num--;
		}
		for (int num2 = list.Count - 1; num2 > num; num2--)
		{
			list.RemoveAt(num2);
		}
	}

	public static ValueType GetWithFallback<KeyType, ValueType>(this Dictionary<KeyType, ValueType> dictionary, KeyType key, ValueType fallback = default(ValueType))
	{
		if (dictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		return fallback;
	}

	public static void ClearNullAndDestroyed<T>(this List<T> list) where T : Thing
	{
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (list[num].DestroyedOrNull())
			{
				list.RemoveAt(num);
			}
		}
	}

	public static bool TryPop<T>(this Stack<T> stack, out T element)
	{
		if (stack.Count == 0)
		{
			element = default(T);
			return false;
		}
		element = stack.Pop();
		return true;
	}

	public static bool TryDequeue<T>(this Queue<T> queue, out T element)
	{
		if (queue.Count == 0)
		{
			element = default(T);
			return false;
		}
		element = queue.Dequeue();
		return true;
	}

	public static void CopyToList<T>(this IEnumerable<T> source, List<T> target, bool clear = true)
	{
		if (clear)
		{
			target.Clear();
		}
		if (source is ICollection<T> collection)
		{
			int num = ((!clear) ? target.Count : 0) + collection.Count;
			if (target.Capacity < num)
			{
				target.Capacity = num;
			}
		}
		foreach (T item in source)
		{
			target.Add(item);
		}
	}
}
