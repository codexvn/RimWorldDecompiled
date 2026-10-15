using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThingSetMaker_StackCount : ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__3 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		public ThingSetMaker_StackCount _003C_003E4__this;

		private TechLevel _003CtechLevel_003E5__2;

		private IEnumerator<ThingDef> _003C_003E7__wrap2;

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
		public _003CAllGeneratableThingsDebugSub_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingSetMaker_StackCount thingSetMaker_StackCount = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CtechLevel_003E5__2 = parms.techLevel.GetValueOrDefault();
					_003C_003E7__wrap2 = thingSetMaker_StackCount.AllowedThingDefs(parms).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					if (!parms.maxTotalMass.HasValue || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || !(ThingSetMakerUtility.GetMinMass(current, _003CtechLevel_003E5__2) > parms.maxTotalMass))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CAllGeneratableThingsDebugSub_003Ed__3 _003CAllGeneratableThingsDebugSub_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebugSub_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebugSub_003Ed__ = new _003CAllGeneratableThingsDebugSub_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllGeneratableThingsDebugSub_003Ed__.parms = _003C_003E3__parms;
			return _003CAllGeneratableThingsDebugSub_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	protected override bool CanGenerateSub(ThingSetMakerParams parms)
	{
		if (!AllowedThingDefs(parms).Any())
		{
			return false;
		}
		if (parms.countRange.HasValue && parms.countRange.Value.max <= 0)
		{
			return false;
		}
		if (parms.maxTotalMass.HasValue && parms.maxTotalMass.GetValueOrDefault() != float.MaxValue && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(AllowedThingDefs(parms), parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, parms.countRange?.max ?? 1))
		{
			return false;
		}
		return true;
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		IEnumerable<ThingDef> enumerable = AllowedThingDefs(parms);
		if (!enumerable.Any())
		{
			return;
		}
		TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
		IntRange intRange = parms.countRange ?? IntRange.One;
		float num = parms.maxTotalMass ?? float.MaxValue;
		int num2 = Mathf.Max(intRange.RandomInRange, 1);
		float num3 = 0f;
		int num4 = num2;
		ThingStuffPair thingStuffPair;
		while (num4 > 0 && ThingSetMakerUtility.TryGetRandomThingWhichCanWeighNoMoreThan(enumerable, valueOrDefault, (num == float.MaxValue) ? float.MaxValue : (num - num3), parms.qualityGenerator, out thingStuffPair))
		{
			Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
			ThingSetMakerUtility.AssignQuality(thing, parms.qualityGenerator);
			int num5 = num4;
			if (num != float.MaxValue && !(thing is Pawn))
			{
				num5 = Mathf.Min(num5, Mathf.FloorToInt((num - num3) / thing.GetStatValue(StatDefOf.Mass)));
			}
			num4 -= (thing.stackCount = Mathf.Clamp(num5, 1, thing.def.stackLimit));
			outThings.Add(thing);
			if (!(thing is Pawn))
			{
				num3 += thing.GetStatValue(StatDefOf.Mass) * (float)thing.stackCount;
			}
		}
	}

	protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
	{
		return ThingSetMakerUtility.GetAllowedThingDefs(parms);
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__3))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}
}
