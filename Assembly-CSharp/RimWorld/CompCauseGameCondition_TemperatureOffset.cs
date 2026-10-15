using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class CompCauseGameCondition_TemperatureOffset : CompCauseGameCondition
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompCauseGameCondition_TemperatureOffset _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
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
			CompCauseGameCondition_TemperatureOffset CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!Prefs.DevMode || !DebugSettings.godMode)
				{
					return false;
				}
				Command_Action command_Action4 = new Command_Action
				{
					defaultLabel = "-10"
				};
				command_Action4.action = (Action)Delegate.Combine(command_Action4.action, (Action)delegate
				{
					CS_0024_003C_003E8__locals8.SetTemperatureOffset(CS_0024_003C_003E8__locals8.temperatureOffset - 10f);
				});
				command_Action4.hotKey = KeyBindingDefOf.Misc1;
				_003C_003E2__current = command_Action4;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action3 = new Command_Action
				{
					defaultLabel = "-1"
				};
				command_Action3.action = (Action)Delegate.Combine(command_Action3.action, (Action)delegate
				{
					CS_0024_003C_003E8__locals8.SetTemperatureOffset(CS_0024_003C_003E8__locals8.temperatureOffset - 1f);
				});
				command_Action3.hotKey = KeyBindingDefOf.Misc2;
				_003C_003E2__current = command_Action3;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action2 = new Command_Action
				{
					defaultLabel = "+1"
				};
				command_Action2.action = (Action)Delegate.Combine(command_Action2.action, (Action)delegate
				{
					CS_0024_003C_003E8__locals8.SetTemperatureOffset(CS_0024_003C_003E8__locals8.temperatureOffset + 1f);
				});
				command_Action2.hotKey = KeyBindingDefOf.Misc3;
				_003C_003E2__current = command_Action2;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "+10"
				};
				command_Action.action = (Action)Delegate.Combine(command_Action.action, (Action)delegate
				{
					CS_0024_003C_003E8__locals8.SetTemperatureOffset(CS_0024_003C_003E8__locals8.temperatureOffset + 10f);
				});
				command_Action.hotKey = KeyBindingDefOf.Misc4;
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
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

	public float temperatureOffset;

	private const float MaxTempForMinOffset = -5f;

	private const float MinTempForMaxOffset = 20f;

	public new CompProperties_CausesGameCondition_ClimateAdjuster Props => (CompProperties_CausesGameCondition_ClimateAdjuster)props;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		temperatureOffset = Props.temperatureOffsetRange.min;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref temperatureOffset, "temperatureOffset", 0f);
	}

	private string GetFloatStringWithSign(float val)
	{
		if (val < 0f)
		{
			return val.ToString("0");
		}
		return "+" + val.ToString("0");
	}

	public void SetTemperatureOffset(float offset)
	{
		temperatureOffset = Props.temperatureOffsetRange.ClampToRange(offset);
		ReSetupAllConditions();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		return text + ("Temperature".Translate() + ": " + GetFloatStringWithSign(temperatureOffset));
	}

	protected override void SetupCondition(GameCondition condition, Map map)
	{
		base.SetupCondition(condition, map);
		((GameCondition_TemperatureOffset)condition).tempOffset = temperatureOffset;
	}

	public override void RandomizeSettings(Site site)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (WorldObject allWorldObject in Find.WorldObjects.AllWorldObjects)
		{
			if (!(allWorldObject is Settlement settlement) || settlement.Faction != Faction.OfPlayer)
			{
				continue;
			}
			if (settlement.Map != null)
			{
				bool flag3 = false;
				foreach (GameCondition activeCondition in settlement.Map.GameConditionManager.ActiveConditions)
				{
					if (activeCondition is GameCondition_TemperatureOffset)
					{
						float num = activeCondition.TemperatureOffset();
						if (num > 0f)
						{
							flag3 = true;
							flag = true;
							flag2 = false;
						}
						else if (num < 0f)
						{
							flag3 = true;
							flag2 = true;
							flag = false;
						}
						if (flag3)
						{
							break;
						}
					}
				}
				if (flag3)
				{
					break;
				}
			}
			PlanetTile tile = allWorldObject.Tile;
			if ((float)Find.WorldGrid.TraversalDistanceBetween(site.Tile, tile, passImpassable: true, Props.worldRange + 1) <= (float)Props.worldRange)
			{
				float num2 = GenTemperature.MinTemperatureAtTile(tile);
				float num3 = GenTemperature.MaxTemperatureAtTile(tile);
				if (num2 < -5f)
				{
					flag2 = true;
				}
				if (num3 > 20f)
				{
					flag = true;
				}
			}
		}
		if (flag2 == flag)
		{
			temperatureOffset = (Rand.Bool ? Props.temperatureOffsetRange.min : Props.temperatureOffsetRange.max);
		}
		else if (flag2)
		{
			temperatureOffset = Props.temperatureOffsetRange.min;
		}
		else if (flag)
		{
			temperatureOffset = Props.temperatureOffsetRange.max;
		}
	}
}
