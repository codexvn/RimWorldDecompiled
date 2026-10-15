using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThingSetMaker_RandomOption : ThingSetMaker
{
	public class Option
	{
		public ThingSetMaker thingSetMaker;

		public float weight;

		public float? weightIfPlayerHasNoItem;

		public ThingDef weightIfPlayerHasNoItemItem;

		public float? weightIfPlayerHasXenotype;

		public XenotypeDef weightIfPlayerHasXenotypeXenotype;
	}

	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__6 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingSetMaker_RandomOption _003C_003E4__this;

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
		public _003CAllGeneratableThingsDebugSub_003Ed__6(int _003C_003E1__state)
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
				ThingSetMaker_RandomOption thingSetMaker_RandomOption = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00ee;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_011a;
				IL_011a:
				if (_003Ci_003E5__2 < thingSetMaker_RandomOption.options.Count)
				{
					float num2 = thingSetMaker_RandomOption.options[_003Ci_003E5__2].weight;
					if (thingSetMaker_RandomOption.options[_003Ci_003E5__2].weightIfPlayerHasNoItem.HasValue)
					{
						num2 = Mathf.Max(num2, thingSetMaker_RandomOption.options[_003Ci_003E5__2].weightIfPlayerHasNoItem.Value);
					}
					if (!(num2 <= 0f))
					{
						_003C_003E7__wrap2 = thingSetMaker_RandomOption.options[_003Ci_003E5__2].thingSetMaker.AllGeneratableThingsDebug(parms).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00ee;
					}
					goto IL_0108;
				}
				return false;
				IL_00ee:
				if (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				goto IL_0108;
				IL_0108:
				_003Ci_003E5__2++;
				goto IL_011a;
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
			_003CAllGeneratableThingsDebugSub_003Ed__6 _003CAllGeneratableThingsDebugSub_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebugSub_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebugSub_003Ed__ = new _003CAllGeneratableThingsDebugSub_003Ed__6(0)
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
	private sealed class _003CConfigErrors_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingSetMaker_RandomOption _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__7(int _003C_003E1__state)
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
				ThingSetMaker_RandomOption thingSetMaker_RandomOption = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (thingSetMaker_RandomOption.options.NullOrEmpty())
					{
						_003C_003E2__current = "no options.";
						_003C_003E1__state = 1;
						return true;
					}
					_003Ci_003E5__2 = 0;
					goto IL_015e;
				case 1:
					_003C_003E1__state = -1;
					break;
				case 2:
					_003C_003E1__state = -1;
					goto IL_014c;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_0132;
					}
					IL_014c:
					_003Ci_003E5__2++;
					goto IL_015e;
					IL_015e:
					if (_003Ci_003E5__2 >= thingSetMaker_RandomOption.options.Count)
					{
						break;
					}
					if (thingSetMaker_RandomOption.options[_003Ci_003E5__2].thingSetMaker == null)
					{
						_003C_003E2__current = "options[" + _003Ci_003E5__2 + "]=null.";
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E7__wrap2 = thingSetMaker_RandomOption.options[_003Ci_003E5__2].thingSetMaker.ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0132;
					IL_0132:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = "options[" + _003Ci_003E5__2 + "]: " + current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					goto IL_014c;
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
			_003CConfigErrors_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__7(0)
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

	protected override bool CanGenerateSub(ThingSetMakerParams parms)
	{
		for (int i = 0; i < options.Count; i++)
		{
			if (options[i].thingSetMaker.CanGenerate(parms) && GetSelectionWeight(options[i], parms) > 0f)
			{
				return true;
			}
		}
		return false;
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		if (options.Where((Option x) => x.thingSetMaker.CanGenerate(parms)).TryRandomElementByWeight((Option x) => GetSelectionWeight(x, parms), out var result))
		{
			outThings.AddRange(result.thingSetMaker.Generate(parms));
		}
	}

	private float GetSelectionWeight(Option option, ThingSetMakerParams parms)
	{
		if (option.weightIfPlayerHasNoItem.HasValue && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(option.weightIfPlayerHasNoItemItem))
		{
			return option.weightIfPlayerHasNoItem.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}
		if (ModsConfig.BiotechActive && option.weightIfPlayerHasXenotype.HasValue && option.weightIfPlayerHasXenotypeXenotype != null && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype(option.weightIfPlayerHasXenotypeXenotype).Count > 0)
		{
			return option.weightIfPlayerHasXenotype.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}
		return option.weight * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		for (int i = 0; i < options.Count; i++)
		{
			options[i].thingSetMaker.ResolveReferences();
		}
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__6))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__7))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
