using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Verse;

public static class GenDefDatabase
{
	[CompilerGenerated]
	private sealed class _003CAllDefTypesWithDatabases_003Ed__7 : IEnumerable<Type>, IEnumerable, IEnumerator<Type>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Type _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Type>.Enumerator _003C_003E7__wrap1;

		Type IEnumerator<Type>.Current
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
		public _003CAllDefTypesWithDatabases_003Ed__7(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Type>.Enumerator);
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
					_003C_003E7__wrap1 = typeof(Def).AllSubclasses().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Type current = _003C_003E7__wrap1.Current;
					if (current.IsAbstract || current == typeof(Def))
					{
						continue;
					}
					bool flag = false;
					Type baseType = current.BaseType;
					while (baseType != null && baseType != typeof(Def))
					{
						if (!baseType.IsAbstract)
						{
							flag = true;
							break;
						}
						baseType = baseType.BaseType;
					}
					if (!flag)
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Type>.Enumerator);
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
		IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllDefTypesWithDatabases_003Ed__7(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Type>)this).GetEnumerator();
		}
	}

	private static readonly Dictionary<Type, Func<string, bool, Def>> cachedGetNamed = new Dictionary<Type, Func<string, bool, Def>>();

	private static readonly object cachedGetNamedLock = new object();

	private static readonly Dictionary<Type, Func<string, Def>> cachedGetNamedSilentFail = new Dictionary<Type, Func<string, Def>>();

	private static readonly object cachedGetNamedSilentFailLock = new object();

	public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
	{
		Func<string, bool, Def> value;
		lock (cachedGetNamedLock)
		{
			if (!cachedGetNamed.TryGetValue(defType, out value))
			{
				MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed");
				value = (Func<string, bool, Def>)Delegate.CreateDelegate(typeof(Func<string, bool, Def>), method);
				cachedGetNamed.Add(defType, value);
			}
		}
		return value(defName, errorOnFail);
	}

	public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
	{
		if (specialCaseForSoundDefs && type == typeof(SoundDef))
		{
			return SoundDef.Named(targetDefName);
		}
		Func<string, Def> value;
		lock (cachedGetNamedSilentFailLock)
		{
			if (!cachedGetNamedSilentFail.TryGetValue(type, out value))
			{
				MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail");
				value = (Func<string, Def>)Delegate.CreateDelegate(typeof(Func<string, Def>), method);
				cachedGetNamedSilentFail.Add(type, value);
			}
		}
		return value(targetDefName);
	}

	public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
	{
		return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
	}

	[IteratorStateMachine(typeof(_003CAllDefTypesWithDatabases_003Ed__7))]
	public static IEnumerable<Type> AllDefTypesWithDatabases()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllDefTypesWithDatabases_003Ed__7(-2);
	}

	public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
	{
		return mod.AllDefs.OfType<T>();
	}
}
