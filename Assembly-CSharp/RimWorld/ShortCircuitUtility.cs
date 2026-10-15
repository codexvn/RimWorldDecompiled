using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class ShortCircuitUtility
{
	[CompilerGenerated]
	private sealed class _003CGetShortCircuitablePowerConduits_003Ed__1 : IEnumerable<Building>, IEnumerable, IEnumerator<Building>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private List<Thing> _003Cconduits_003E5__2;

		private int _003Ci_003E5__3;

		Building IEnumerator<Building>.Current
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
		public _003CGetShortCircuitablePowerConduits_003Ed__1(int _003C_003E1__state)
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
			_003Cconduits_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00ca;
				}
				_003C_003E1__state = -1;
				tmpPowerNetHasActivePowerSource.Clear();
				_003C_003E1__state = -3;
				_003Cconduits_003E5__2 = map.listerThings.ThingsOfDef(ThingDefOf.PowerConduit);
				_003Ci_003E5__3 = 0;
				goto IL_00dc;
				IL_00ca:
				_003Ci_003E5__3++;
				goto IL_00dc;
				IL_00dc:
				if (_003Ci_003E5__3 < _003Cconduits_003E5__2.Count)
				{
					Building building = (Building)_003Cconduits_003E5__2[_003Ci_003E5__3];
					CompPower powerComp = building.PowerComp;
					if (powerComp != null)
					{
						if (!tmpPowerNetHasActivePowerSource.TryGetValue(powerComp.PowerNet, out var value))
						{
							value = powerComp.PowerNet.HasActivePowerSource;
							tmpPowerNetHasActivePowerSource.Add(powerComp.PowerNet, value);
						}
						if (value)
						{
							_003C_003E2__current = building;
							_003C_003E1__state = 1;
							return true;
						}
					}
					goto IL_00ca;
				}
				_003Cconduits_003E5__2 = null;
				_003C_003Em__Finally1();
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
			tmpPowerNetHasActivePowerSource.Clear();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Building> IEnumerable<Building>.GetEnumerator()
		{
			_003CGetShortCircuitablePowerConduits_003Ed__1 _003CGetShortCircuitablePowerConduits_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetShortCircuitablePowerConduits_003Ed__ = this;
			}
			else
			{
				_003CGetShortCircuitablePowerConduits_003Ed__ = new _003CGetShortCircuitablePowerConduits_003Ed__1(0);
			}
			_003CGetShortCircuitablePowerConduits_003Ed__.map = _003C_003E3__map;
			return _003CGetShortCircuitablePowerConduits_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building>)this).GetEnumerator();
		}
	}

	private static Dictionary<PowerNet, bool> tmpPowerNetHasActivePowerSource = new Dictionary<PowerNet, bool>();

	private static List<IntVec3> tmpCells = new List<IntVec3>();

	[IteratorStateMachine(typeof(_003CGetShortCircuitablePowerConduits_003Ed__1))]
	public static IEnumerable<Building> GetShortCircuitablePowerConduits(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetShortCircuitablePowerConduits_003Ed__1(-2)
		{
			_003C_003E3__map = map
		};
	}

	public static void DoShortCircuit(Building culprit)
	{
		PowerNet powerNet = culprit.PowerComp.PowerNet;
		Map map = culprit.Map;
		float totalEnergy = 0f;
		float explosionRadius = 0f;
		bool flag = false;
		if (powerNet.batteryComps.Any((CompPowerBattery x) => x.StoredEnergy > 20f))
		{
			DrainBatteriesAndCauseExplosion(powerNet, culprit, out totalEnergy, out explosionRadius);
		}
		else
		{
			flag = TryStartFireNear(culprit);
		}
		string text = ((culprit.def != ThingDefOf.PowerConduit) ? Find.ActiveLanguageWorker.WithIndefiniteArticlePostProcessed(culprit.Label) : ((string)"AnElectricalConduit".Translate()));
		StringBuilder stringBuilder = new StringBuilder();
		if (flag)
		{
			stringBuilder.Append("ShortCircuitStartedFire".Translate(text));
		}
		else
		{
			stringBuilder.Append("ShortCircuit".Translate(text));
		}
		if (totalEnergy > 0f)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("ShortCircuitDischargedEnergy".Translate(totalEnergy.ToString("F0")));
		}
		if (explosionRadius > 5f)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("ShortCircuitWasLarge".Translate());
		}
		if (explosionRadius > 8f)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("ShortCircuitWasHuge".Translate());
		}
		Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), stringBuilder.ToString(), LetterDefOf.NegativeEvent, new TargetInfo(culprit.Position, map));
	}

	public static bool TryShortCircuitInRain(Thing thing)
	{
		CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
		if ((compPowerTrader != null && compPowerTrader.PowerOn && compPowerTrader.Props.shortCircuitInRain) || (thing.TryGetComp<CompPowerBattery>() != null && thing.TryGetComp<CompPowerBattery>().StoredEnergy > 100f))
		{
			TaggedString taggedString = "ShortCircuitRain".Translate(thing.Label, thing);
			TargetInfo targetInfo = new TargetInfo(thing.Position, thing.Map);
			if (thing.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelShortCircuit".Translate(), taggedString, LetterDefOf.NegativeEvent, targetInfo);
			}
			else
			{
				Messages.Message(taggedString, targetInfo, MessageTypeDefOf.NeutralEvent);
			}
			GenExplosion.DoExplosion(thing.OccupiedRect().RandomCell, thing.Map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, applyDamageToExplosionCellsNeighbors: false, null, 0f, 1, 0f, damageFalloff: false, null, null, null);
			return true;
		}
		return false;
	}

	private static void DrainBatteriesAndCauseExplosion(PowerNet net, Building culprit, out float totalEnergy, out float explosionRadius)
	{
		totalEnergy = 0f;
		for (int i = 0; i < net.batteryComps.Count; i++)
		{
			CompPowerBattery compPowerBattery = net.batteryComps[i];
			totalEnergy += compPowerBattery.StoredEnergy;
			compPowerBattery.DrawPower(compPowerBattery.StoredEnergy);
		}
		explosionRadius = Mathf.Sqrt(totalEnergy) * 0.05f;
		explosionRadius = Mathf.Clamp(explosionRadius, 1.5f, 14.9f);
		GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, applyDamageToExplosionCellsNeighbors: false, null, 0f, 1, 0f, damageFalloff: false, null, null, null);
		if (explosionRadius > 3.5f)
		{
			GenExplosion.DoExplosion(culprit.Position, net.Map, explosionRadius * 0.3f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, applyDamageToExplosionCellsNeighbors: false, null, 0f, 1, 0f, damageFalloff: false, null, null, null);
		}
	}

	private static bool TryStartFireNear(Building b)
	{
		tmpCells.Clear();
		int num = GenRadial.NumCellsInRadius(3f);
		CellRect startRect = b.OccupiedRect();
		for (int i = 0; i < num; i++)
		{
			IntVec3 intVec = b.Position + GenRadial.RadialPattern[i];
			if (GenSight.LineOfSight(b.Position, intVec, b.Map, startRect, CellRect.SingleCell(intVec)) && FireUtility.ChanceToStartFireIn(intVec, b.Map) > 0f)
			{
				tmpCells.Add(intVec);
			}
		}
		if (tmpCells.Any())
		{
			return FireUtility.TryStartFireIn(tmpCells.RandomElement(), b.Map, Rand.Range(0.1f, 1.75f), null);
		}
		return false;
	}
}
