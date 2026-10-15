using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompEggContainer : CompThingContainer
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompEggContainer _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__6(int _003C_003E1__state)
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
				CompEggContainer CS_0024_003C_003E8__locals4 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompThingContainer)CS_0024_003C_003E8__locals4).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos && CS_0024_003C_003E8__locals4.Empty)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Fill with eggs",
							action = delegate
							{
								Thing thing = ThingMaker.MakeThing(ThingDef.Named("EggChickenUnfertilized"));
								thing.stackCount = CS_0024_003C_003E8__locals4.Props.stackLimit;
								CS_0024_003C_003E8__locals4.innerContainer.TryAdd(thing);
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public bool CanEmpty
	{
		get
		{
			if (base.Empty)
			{
				return false;
			}
			if (EggsRuined)
			{
				return true;
			}
			return base.ContainedThing.stackCount >= base.Props.minCountToEmpty;
		}
	}

	private bool EggsRuined
	{
		get
		{
			if (base.Empty)
			{
				return false;
			}
			CompTemperatureRuinable compTemperatureRuinable = base.ContainedThing.TryGetComp<CompTemperatureRuinable>();
			if (compTemperatureRuinable != null && compTemperatureRuinable.Ruined)
			{
				return true;
			}
			CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
			if (compHatcher != null && compHatcher.TemperatureDamaged)
			{
				return true;
			}
			return false;
		}
	}

	public override bool Accepts(ThingDef thingDef)
	{
		if (base.ContainedThing != null && EggsRuined)
		{
			return false;
		}
		if (thingDef.thingCategories != null && (thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized)))
		{
			return base.Accepts(thingDef);
		}
		return false;
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (!base.Empty && !EggsRuined)
		{
			CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
			if (compHatcher != null)
			{
				string text2 = compHatcher.CompInspectStringExtra();
				if (!text2.NullOrEmpty())
				{
					text = text + "\n" + text2;
				}
			}
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__6))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
