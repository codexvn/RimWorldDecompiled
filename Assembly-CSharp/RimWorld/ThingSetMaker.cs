using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebug_003Ed__9 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingSetMaker _003C_003E4__this;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllGeneratableThingsDebug_003Ed__9(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				ThingSetMaker thingSetMaker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!thingSetMaker.CanGenerate(parms))
					{
						return false;
					}
					ThingSetMakerParams thingSetMakerParams = thingSetMaker.ApplyFixedParams(parms);
					_003C_003E7__wrap1 = thingSetMaker.AllGeneratableThingsDebugSub(thingSetMakerParams).Distinct().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllGeneratableThingsDebug_003Ed__9 _003CAllGeneratableThingsDebug_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebug_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebug_003Ed__ = new _003CAllGeneratableThingsDebug_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllGeneratableThingsDebug_003Ed__.parms = _003C_003E3__parms;
			return _003CAllGeneratableThingsDebug_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__17 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CConfigErrors_003Ed__17(int _003C_003E1__state)
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CConfigErrors_003Ed__17(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public ThingSetMakerParams fixedParams;

	public static List<List<Thing>> thingsBeingGeneratedNow;

	static ThingSetMaker()
	{
		thingsBeingGeneratedNow = new List<List<Thing>>();
	}

	public List<Thing> Generate()
	{
		return Generate(default(ThingSetMakerParams));
	}

	public List<Thing> Generate(ThingSetMakerParams parms)
	{
		List<Thing> list = new List<Thing>();
		thingsBeingGeneratedNow.Add(list);
		try
		{
			ThingSetMakerParams parms2 = ApplyFixedParams(parms);
			Generate(parms2, list);
			PostProcess(list);
		}
		catch (Exception ex)
		{
			Log.Error("Exception while generating thing set: " + ex);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list[num].Destroy();
				list.RemoveAt(num);
			}
		}
		finally
		{
			thingsBeingGeneratedNow.Remove(list);
		}
		return list;
	}

	public bool CanGenerate(ThingSetMakerParams parms)
	{
		ThingSetMakerParams parms2 = ApplyFixedParams(parms);
		return CanGenerateSub(parms2);
	}

	protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
	{
		return true;
	}

	protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

	public IEnumerable<ThingDef> AllGeneratableThingsDebug()
	{
		return AllGeneratableThingsDebug(default(ThingSetMakerParams));
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebug_003Ed__9))]
	public IEnumerable<ThingDef> AllGeneratableThingsDebug(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebug_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}

	public virtual float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
	{
		return 1f;
	}

	protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

	private void PostProcess(List<Thing> things)
	{
		if (things.RemoveAll((Thing x) => x == null) != 0)
		{
			Log.Error(GetType()?.ToString() + " generated null things.");
		}
		ChangeDeadPawnsToTheirCorpses(things);
		for (int num = things.Count - 1; num >= 0; num--)
		{
			if (things[num].Destroyed)
			{
				Log.Error(GetType()?.ToString() + " generated destroyed thing " + things[num].ToStringSafe());
				things.RemoveAt(num);
			}
			else if (things[num].stackCount <= 0)
			{
				Log.Error(GetType()?.ToString() + " generated " + things[num].ToStringSafe() + " with stackCount=" + things[num].stackCount);
				things.RemoveAt(num);
			}
		}
		Minify(things);
	}

	private void Minify(List<Thing> things)
	{
		for (int i = 0; i < things.Count; i++)
		{
			if (things[i].def.Minifiable)
			{
				int stackCount = things[i].stackCount;
				things[i].stackCount = 1;
				MinifiedThing minifiedThing = things[i].MakeMinified();
				minifiedThing.stackCount = stackCount;
				things[i] = minifiedThing;
			}
		}
	}

	private void ChangeDeadPawnsToTheirCorpses(List<Thing> things)
	{
		for (int i = 0; i < things.Count; i++)
		{
			if (things[i].ParentHolder is Corpse)
			{
				things[i] = (Corpse)things[i].ParentHolder;
			}
		}
	}

	private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
	{
		ThingSetMakerParams replaceIn = fixedParams;
		Gen.ReplaceNullFields(ref replaceIn, parms);
		return replaceIn;
	}

	public virtual void ResolveReferences()
	{
		if (fixedParams.filter != null)
		{
			fixedParams.filter.ResolveReferences();
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__17))]
	public virtual IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__17(-2);
	}
}
