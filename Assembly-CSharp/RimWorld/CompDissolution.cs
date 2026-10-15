using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompDissolution : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass28_0
	{
		public int ticks;

		public CompDissolution _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__4()
		{
			_003C_003E4__this.dissolveTicks = _003C_003E4__this.DissolutionIntervalTicks - ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__28 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompDissolution _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__28(int _003C_003E1__state)
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
			CompDissolution CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Dissolution event",
						action = delegate
						{
							CS_0024_003C_003E8__locals11.TriggerDissolutionEvent();
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Dissolution event until destroyed",
					action = delegate
					{
						int num2 = 1000;
						while (!CS_0024_003C_003E8__locals11.parent.Destroyed && num2 > 0)
						{
							CS_0024_003C_003E8__locals11.TriggerDissolutionEvent();
							num2--;
						}
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Dissolution progress +25%",
					action = delegate
					{
						CS_0024_003C_003E8__locals11.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)CS_0024_003C_003E8__locals11.DissolutionAfterDamage * 0.25f));
					}
				};
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set next dissolve time",
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[11]
						{
							60, 120, 180, 240, 300, 600, 900, 1200, 1500, 1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							_003C_003Ec__DisplayClass28_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass28_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals11,
								ticks = array[i]
							};
							list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals6.ticks.ToStringSecondsFromTicks("F0"), delegate
							{
								CS_0024_003C_003E8__locals6._003C_003E4__this.dissolveTicks = CS_0024_003C_003E8__locals6._003C_003E4__this.DissolutionIntervalTicks - CS_0024_003C_003E8__locals6.ticks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					}
				};
				_003C_003E1__state = 4;
				return true;
			case 4:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__28 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__28(0)
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

	private int dissolveTicks;

	private static List<string> tmpDissolutionReasons = new List<string>();

	private CompProperties_Dissolution Props => (CompProperties_Dissolution)props;

	public int DeterioarationRate => (int)parent.GetStatValue(StatDefOf.DeteriorationRate);

	public int DissolutionAfterDamage => DeterioarationRate * Props.dissolutionAfterDays;

	public int DefaultTicksUntilDissolution => 60000 * Props.dissolutionAfterDays;

	public bool IsOutdoors
	{
		get
		{
			Map mapHeld = parent.MapHeld;
			IntVec3 position = parent.Position;
			if (mapHeld != null)
			{
				Room room = position.GetRoom(mapHeld);
				if (room != null && room.UsesOutdoorTemperature)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool IsFrozen => parent.AmbientTemperature <= 0f;

	public bool InAtomizer
	{
		get
		{
			if (parent.ParentHolder != null)
			{
				return parent.ParentHolder is CompAtomizer;
			}
			return false;
		}
	}

	public bool CanDissolveNow
	{
		get
		{
			if (!IsFrozen)
			{
				return !InAtomizer;
			}
			return false;
		}
	}

	public bool IsBeingRainedOn
	{
		get
		{
			if (IsOutdoors)
			{
				return parent.MapHeld.weatherManager.curWeather.rainRate > 0.1f;
			}
			return false;
		}
	}

	public int DissolutionIntervalTicks
	{
		get
		{
			float num = DefaultTicksUntilDissolution;
			if (!IsOutdoors)
			{
				num /= Props.dissolutinFactorIndoors;
			}
			if (IsBeingRainedOn)
			{
				num /= Props.dissolutionFactorRain;
			}
			return (int)num;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotech("Dissolution"))
		{
			parent.Destroy();
		}
		else
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}
	}

	public void TriggerDissolutionEvent(int amountToDissolve = 1)
	{
		amountToDissolve = Mathf.Min(amountToDissolve, parent.stackCount);
		if (parent.MapHeld != null)
		{
			DissolveMap(amountToDissolve);
		}
		else
		{
			DissolveWorld(amountToDissolve, parent.Tile);
		}
		parent.stackCount -= amountToDissolve;
		dissolveTicks = 0;
		if (parent.stackCount <= 0 && !parent.Destroyed)
		{
			parent.Destroy();
		}
	}

	private void DissolveMap(int amount)
	{
		List<ThingComp> allComps = parent.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			if (allComps[i] is CompDissolutionEffect compDissolutionEffect)
			{
				compDissolutionEffect.DoDissolutionEffectMap(amount);
			}
		}
		Find.Storyteller.Notify_DissolutionEvent(parent);
	}

	private void DissolveWorld(int amount, PlanetTile tile)
	{
		List<ThingComp> allComps = parent.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			if (allComps[i] is CompDissolutionEffect compDissolutionEffect)
			{
				compDissolutionEffect.DoDissolutionEffectWorld(amount, tile);
			}
		}
	}

	public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
	{
		base.PostPreApplyDamage(ref dinfo, out absorbed);
		if (dinfo.Def.harmsHealth && dinfo.Amount >= (float)parent.HitPoints)
		{
			DissolveMap(parent.stackCount);
		}
	}

	public override void Notify_AbandonedAtTile(PlanetTile tile)
	{
		DissolveWorld(parent.stackCount, tile);
	}

	public override void CompTick()
	{
		base.CompTick();
		if (CanDissolveNow)
		{
			dissolveTicks++;
			if (dissolveTicks >= DissolutionIntervalTicks)
			{
				TriggerDissolutionEvent();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__28))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__28(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		if (InAtomizer)
		{
			return base.CompInspectStringExtra();
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.CompInspectStringExtra());
		if (stringBuilder.Length > 0)
		{
			stringBuilder.AppendLine();
		}
		if (IsFrozen)
		{
			stringBuilder.Append("DissolutionFrozen".Translate());
			return stringBuilder.ToString();
		}
		tmpDissolutionReasons.Clear();
		if (!IsOutdoors)
		{
			tmpDissolutionReasons.Add("DissolutionRateIndoors".Translate() + " x" + Props.dissolutinFactorIndoors.ToStringPercent());
		}
		if (IsBeingRainedOn)
		{
			tmpDissolutionReasons.Add("DissolutionRain".Translate() + " x" + Props.dissolutionFactorRain.ToStringPercent());
		}
		stringBuilder.Append("DissolvesEvery".Translate(DissolutionIntervalTicks.ToStringTicksToPeriod()));
		if (tmpDissolutionReasons.Count > 0)
		{
			string str = tmpDissolutionReasons.ToLineList();
			stringBuilder.Append(" (" + str.CapitalizeFirst() + ")");
			tmpDissolutionReasons.Clear();
		}
		return stringBuilder.ToString();
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref dissolveTicks, "dissolveTicks", 0);
	}
}
