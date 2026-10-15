using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class GameConditionDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__34 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GameConditionDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__34(int _003C_003E1__state)
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
				GameConditionDef gameConditionDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)gameConditionDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00bb;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (gameConditionDef.conditionClass == null)
					{
						_003C_003E2__current = "conditionClass is null";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00bb;
					IL_00bb:
					if (!gameConditionDef.layerBlacklist.NullOrEmpty() && !gameConditionDef.layerWhitelist.NullOrEmpty())
					{
						_003C_003E2__current = "Both layerBlacklist and layerWhitelist are used, this will lead to unexpected behaviour. Use one or the other.";
						_003C_003E1__state = 3;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__34(0)
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

	public Type conditionClass = typeof(GameCondition);

	private List<GameConditionDef> exclusiveConditions;

	[MustTranslate]
	public string startMessage;

	[MustTranslate]
	public string endMessage;

	[MustTranslate]
	public string letterText;

	public List<ThingDef> letterHyperlinks;

	public LetterDef letterDef;

	public bool canBePermanent;

	public bool showPermanentInTooltip = true;

	public bool allowUnderground = true;

	public bool requireFish;

	[MustTranslate]
	public string descriptionFuture;

	[NoTranslate]
	public string jumpToSourceKey = "ClickToJumpToSource";

	public List<GameConditionDef> silencedByConditions;

	public bool natural = true;

	public bool preventNeutralVisitors;

	public bool preventShuttleLaunch;

	public bool causesTraderCaravanExit;

	public bool preventIncidents;

	public bool displayOnUI = true;

	public bool pennedAnimalsSeekShelter;

	public ThingDef spreadsFilth;

	public bool canAffectAllPlanetLayers;

	public List<PlanetLayerDef> layerWhitelist;

	public List<PlanetLayerDef> layerBlacklist;

	public PsychicDroneLevel defaultDroneLevel = PsychicDroneLevel.BadMedium;

	public bool preventRain;

	public WeatherDef weatherDef;

	public float temperatureOffset = -10f;

	public float minNearbyPollution;

	public SimpleCurve mtbOverNearbyPollutionCurve;

	public float fishPopulationOffsetPerDay;

	public bool CanCoexistWith(GameConditionDef other)
	{
		if (this == other)
		{
			return false;
		}
		if (ModsConfig.AnomalyActive && this != GameConditionDefOf.UnnaturalDarkness && other == GameConditionDefOf.UnnaturalDarkness)
		{
			return false;
		}
		if (exclusiveConditions != null)
		{
			return !exclusiveConditions.Contains(other);
		}
		return true;
	}

	public static GameConditionDef Named(string defName)
	{
		return DefDatabase<GameConditionDef>.GetNamed(defName);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__34))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__34(-2)
		{
			_003C_003E4__this = this
		};
	}
}
