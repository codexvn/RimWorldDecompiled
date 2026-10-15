using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThingSetMaker_Sum : ThingSetMaker
{
	public class Option
	{
		public ThingSetMaker thingSetMaker;

		public float chance = 1f;

		public float? maxMarketValue;

		public float minMarketValue;

		public float minTotalMarketValue;

		public const float MaxMarketValueLeewayFactor = 0.8f;
	}

	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__7 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingSetMaker_Sum _003C_003E4__this;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		private int _003Ci_003E5__2;

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
		public _003CAllGeneratableThingsDebugSub_003Ed__7(int _003C_003E1__state)
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
				ThingSetMaker_Sum thingSetMaker_Sum = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00ab;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_00d7;
				IL_00d7:
				if (_003Ci_003E5__2 < thingSetMaker_Sum.options.Count)
				{
					if (!(thingSetMaker_Sum.options[_003Ci_003E5__2].chance <= 0f))
					{
						_003C_003E7__wrap2 = thingSetMaker_Sum.options[_003Ci_003E5__2].thingSetMaker.AllGeneratableThingsDebug(parms).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00ab;
					}
					goto IL_00c5;
				}
				return false;
				IL_00ab:
				if (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				goto IL_00c5;
				IL_00c5:
				_003Ci_003E5__2++;
				goto IL_00d7;
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
			_003CAllGeneratableThingsDebugSub_003Ed__7 _003CAllGeneratableThingsDebugSub_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebugSub_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebugSub_003Ed__ = new _003CAllGeneratableThingsDebugSub_003Ed__7(0)
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

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__8 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingSetMaker_Sum _003C_003E4__this;

		private int _003Ci_003E5__2;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
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
				ThingSetMaker_Sum thingSetMaker_Sum = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (thingSetMaker_Sum.options.NullOrEmpty())
					{
						_003C_003E2__current = "no options.";
						_003C_003E1__state = 1;
						return true;
					}
					_003Ci_003E5__2 = 0;
					goto IL_012c;
				case 1:
					_003C_003E1__state = -1;
					break;
				case 2:
					_003C_003E1__state = -1;
					goto IL_011a;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_0100;
					}
					IL_011a:
					_003Ci_003E5__2++;
					goto IL_012c;
					IL_012c:
					if (_003Ci_003E5__2 >= thingSetMaker_Sum.options.Count)
					{
						break;
					}
					if (thingSetMaker_Sum.options[_003Ci_003E5__2].thingSetMaker == null)
					{
						_003C_003E2__current = "null thingSetMaker";
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E7__wrap2 = thingSetMaker_Sum.options[_003Ci_003E5__2].thingSetMaker.ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0100;
					IL_0100:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					goto IL_011a;
				}
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__8(0)
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

	public List<Option> options;

	public bool resolveInOrder;

	private List<Option> optionsInResolveOrder = new List<Option>();

	protected override bool CanGenerateSub(ThingSetMakerParams parms)
	{
		for (int i = 0; i < options.Count; i++)
		{
			if (options[i].chance > 0f && options[i].thingSetMaker.CanGenerate(parms))
			{
				return true;
			}
		}
		return false;
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		optionsInResolveOrder.Clear();
		optionsInResolveOrder.AddRange(options);
		if (!resolveInOrder)
		{
			optionsInResolveOrder.Shuffle();
		}
		for (int i = 0; i < optionsInResolveOrder.Count; i++)
		{
			ThingSetMakerParams parms2 = parms;
			if (parms2.countRange.HasValue)
			{
				parms2.countRange = new IntRange(Mathf.Max(parms2.countRange.Value.min - num, 0), parms2.countRange.Value.max - num);
				if (parms2.countRange.Value.max <= 0)
				{
					continue;
				}
			}
			if (parms2.totalMarketValueRange.HasValue)
			{
				if (parms2.totalMarketValueRange.Value.max < optionsInResolveOrder[i].minTotalMarketValue)
				{
					continue;
				}
				float min = ((i < optionsInResolveOrder.Count - 1) ? 0f : Mathf.Max(parms2.totalMarketValueRange.Value.min - num2, 0f));
				float max = parms2.totalMarketValueRange.Value.max - num2;
				parms2.totalMarketValueRange = new FloatRange(min, max);
				if (optionsInResolveOrder[i].maxMarketValue.HasValue)
				{
					parms2.totalMarketValueRange = new FloatRange(Mathf.Min(parms2.totalMarketValueRange.Value.min, optionsInResolveOrder[i].maxMarketValue.Value * 0.8f), Mathf.Min(Mathf.Min(parms2.totalMarketValueRange.Value.max, optionsInResolveOrder[i].maxMarketValue.Value)));
				}
				if (parms2.totalMarketValueRange.Value.max <= 0f || parms2.totalMarketValueRange.Value.max < optionsInResolveOrder[i].minMarketValue)
				{
					continue;
				}
			}
			if (parms2.totalNutritionRange.HasValue)
			{
				parms2.totalNutritionRange = new FloatRange(Mathf.Max(parms2.totalNutritionRange.Value.min - num3, 0f), parms2.totalNutritionRange.Value.max - num3);
			}
			if (parms2.maxTotalMass.HasValue)
			{
				parms2.maxTotalMass -= num4;
			}
			if (!Rand.Chance(optionsInResolveOrder[i].chance) || !optionsInResolveOrder[i].thingSetMaker.CanGenerate(parms2))
			{
				continue;
			}
			List<Thing> list = optionsInResolveOrder[i].thingSetMaker.Generate(parms2);
			num += list.Count;
			for (int j = 0; j < list.Count; j++)
			{
				num2 += list[j].MarketValue * (float)list[j].stackCount;
				if (list[j].def.IsIngestible)
				{
					num3 += list[j].GetStatValue(StatDefOf.Nutrition) * (float)list[j].stackCount;
				}
				if (!(list[j] is Pawn))
				{
					num4 += list[j].GetStatValue(StatDefOf.Mass) * (float)list[j].stackCount;
				}
			}
			outThings.AddRange(list);
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		for (int i = 0; i < options.Count; i++)
		{
			options[i].thingSetMaker.ResolveReferences();
		}
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__7))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__8))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
