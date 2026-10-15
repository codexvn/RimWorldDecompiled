using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse;

public static class ContainmentUtility
{
	[CompilerGenerated]
	private sealed class _003CGetInhibitorAffectedCells_003Ed__8 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private IntVec3 center;

		public IntVec3 _003C_003E3__center;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private Map map;

		public Map _003C_003E3__map;

		private CompProperties_Facility _003Ccomp_003E5__2;

		private bool _003CedificeFound_003E5__3;

		private float _003Ci_003E5__4;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CGetInhibitorAffectedCells_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Ccomp_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ccomp_003E5__2 = def.GetCompProperties<CompProperties_Facility>();
				if (_003Ccomp_003E5__2 == null)
				{
					return false;
				}
				if (IsLinearBuildingBlocked(def, center, rot, map))
				{
					return false;
				}
				_003CedificeFound_003E5__3 = false;
				_003Ci_003E5__4 = _003Ccomp_003E5__2.minDistance;
				goto IL_012e;
			case 1:
				_003C_003E1__state = -1;
				goto IL_011a;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_011a;
				}
				IL_011a:
				_003Ci_003E5__4++;
				goto IL_012e;
				IL_012e:
				if (_003Ci_003E5__4 < _003Ccomp_003E5__2.maxDistance)
				{
					if (_003CedificeFound_003E5__3)
					{
						return false;
					}
					int num = Mathf.FloorToInt(_003Ci_003E5__4);
					IntVec3 c = center + IntVec3.South.RotatedBy(rot) * (num - 1 + def.size.z);
					if (c.InBounds(map))
					{
						if (c.GetEdifice(map) == null)
						{
							_003C_003E2__current = c;
							_003C_003E1__state = 1;
							return true;
						}
						_003CedificeFound_003E5__3 = true;
						_003C_003E2__current = c;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_011a;
				}
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGetInhibitorAffectedCells_003Ed__8 _003CGetInhibitorAffectedCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetInhibitorAffectedCells_003Ed__ = this;
			}
			else
			{
				_003CGetInhibitorAffectedCells_003Ed__ = new _003CGetInhibitorAffectedCells_003Ed__8(0);
			}
			_003CGetInhibitorAffectedCells_003Ed__.def = _003C_003E3__def;
			_003CGetInhibitorAffectedCells_003Ed__.center = _003C_003E3__center;
			_003CGetInhibitorAffectedCells_003Ed__.rot = _003C_003E3__rot;
			_003CGetInhibitorAffectedCells_003Ed__.map = _003C_003E3__map;
			return _003CGetInhibitorAffectedCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private static readonly SimpleCurve DaysInCaptivityEscapeFactorCurve = new SimpleCurve
	{
		new CurvePoint(15f, 1f),
		new CurvePoint(60f, 1.2f),
		new CurvePoint(120f, 1.5f),
		new CurvePoint(240f, 2f)
	};

	private static readonly SimpleCurve ColdEscapeFactorCurve = new SimpleCurve
	{
		new CurvePoint(-30f, 1.5f),
		new CurvePoint(0f, 1f)
	};

	private static readonly SimpleCurve EscapeIntervalMultiplierFromContainmentStrengthCurve = new SimpleCurve
	{
		new CurvePoint(-0.001f, 0.05f),
		new CurvePoint(0f, 1f),
		new CurvePoint(100f, 3f),
		new CurvePoint(200f, 8f)
	};

	private static readonly SimpleCurve StudyKnowledgeMultiplierFromContainmentStrengthCurve = new SimpleCurve
	{
		new CurvePoint(-0.001f, 0.5f),
		new CurvePoint(0f, 1f),
		new CurvePoint(100f, 1.3f),
		new CurvePoint(200f, 1.5f)
	};

	public static bool SafeContainerExistsFor(Thing thing)
	{
		if (!ModsConfig.AnomalyActive)
		{
			return false;
		}
		foreach (Thing item in thing.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
		{
			if (item.SafelyContains(thing))
			{
				return true;
			}
		}
		return false;
	}

	public static bool SafelyContains(this Thing container, Thing thing)
	{
		if (!ModsConfig.AnomalyActive)
		{
			return false;
		}
		CompEntityHolder compEntityHolder = container.TryGetComp<CompEntityHolder>();
		if (compEntityHolder != null)
		{
			return compEntityHolder.ContainmentStrength >= thing.GetStatValue(StatDefOf.MinimumContainmentStrength);
		}
		return false;
	}

	public static float GetEscapeIntervalMultiplier(Thing entity, CompEntityHolder entityHolder)
	{
		return EscapeIntervalMultiplierFromContainmentStrengthCurve.Evaluate(entityHolder.ContainmentStrength - entity.GetStatValue(StatDefOf.MinimumContainmentStrength));
	}

	public static float GetStudyKnowledgeAmountMultiplier(Thing entity, CompEntityHolder entityHolder)
	{
		return StudyKnowledgeMultiplierFromContainmentStrengthCurve.Evaluate(entityHolder.ContainmentStrength - entity.GetStatValue(StatDefOf.MinimumContainmentStrength));
	}

	[IteratorStateMachine(typeof(_003CGetInhibitorAffectedCells_003Ed__8))]
	public static IEnumerable<IntVec3> GetInhibitorAffectedCells(ThingDef def, IntVec3 center, Rot4 rot, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInhibitorAffectedCells_003Ed__8(-2)
		{
			_003C_003E3__def = def,
			_003C_003E3__center = center,
			_003C_003E3__rot = rot,
			_003C_003E3__map = map
		};
	}

	public static IntVec3 GetLinearBuildingFrontCell(ThingDef def, IntVec3 center, Rot4 rot)
	{
		if (def.GetCompProperties<CompProperties_Facility>() == null)
		{
			return IntVec3.Invalid;
		}
		return center + IntVec3.South.RotatedBy(rot) * (def.size.z - 1);
	}

	public static bool IsLinearBuildingBlocked(ThingDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
	{
		if (def.GetCompProperties<CompProperties_Facility>() == null)
		{
			return false;
		}
		Building edifice = GetLinearBuildingFrontCell(def, center, rot).GetEdifice(map);
		if (edifice != null)
		{
			return edifice != thingToIgnore;
		}
		return false;
	}

	public static bool ShowContainmentStats(Thing thing)
	{
		CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
		if (compStudiable != null && thing is Pawn)
		{
			return compStudiable.RequiresHoldingPlatform;
		}
		if (thing.TryGetComp<CompHoldingPlatformTarget>()?.Props.heldPawnKind != null)
		{
			return true;
		}
		return false;
	}

	public static bool TryGetColdContainmentBonus(Thing thing, out float modifier)
	{
		if (thing.TryGetComp(out CompHoldingPlatformTarget comp) && comp.Props.getsColdContainmentBonus)
		{
			modifier = ColdEscapeFactorCurve.Evaluate(thing.AmbientTemperature);
			return true;
		}
		modifier = 0f;
		return false;
	}

	public static float InitiateEscapeMtbDays(Pawn pawn, StringBuilder sb = null)
	{
		if (!CanParticipateInEscape(pawn, sb))
		{
			return -1f;
		}
		CompHoldingPlatformTarget comp = pawn.GetComp<CompHoldingPlatformTarget>();
		float baseEscapeIntervalMtbDays = comp.Props.baseEscapeIntervalMtbDays;
		pawn.GetRoom();
		float num = 1f / Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
		baseEscapeIntervalMtbDays *= num;
		if (sb != null && num > 1f)
		{
			sb.AppendLineIfNotEmpty();
			sb.Append("  - " + "FactorForMovement".Translate() + ": x" + num.ToStringPercent());
		}
		float x = (float)pawn.mindState.entityTicksInCaptivity / 60000f;
		float num2 = DaysInCaptivityEscapeFactorCurve.Evaluate(x);
		baseEscapeIntervalMtbDays *= num2;
		if (num2 > 1f && sb != null)
		{
			sb.AppendLineIfNotEmpty();
			sb.Append("  - " + "FactorForDaysInCaptivity".Translate() + ": x" + num2.ToStringPercent());
		}
		float escapeIntervalMultiplier = GetEscapeIntervalMultiplier(pawn, comp.HeldPlatform.GetComp<CompEntityHolder>());
		baseEscapeIntervalMtbDays *= escapeIntervalMultiplier;
		if (sb != null)
		{
			sb.AppendLineIfNotEmpty();
			sb.Append("  - " + "FactorContainmentStrength".Translate() + ": x" + escapeIntervalMultiplier.ToStringPercent());
		}
		if (comp.Props.getsColdContainmentBonus)
		{
			float num3 = ColdEscapeFactorCurve.Evaluate(pawn.AmbientTemperature);
			baseEscapeIntervalMtbDays *= num3;
			if (num3 > 1f && sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorForColdCaptivity".Translate() + ": x" + num3.ToStringPercent());
			}
		}
		return baseEscapeIntervalMtbDays;
	}

	public static bool CanParticipateInEscape(Pawn pawn, StringBuilder sb = null)
	{
		if (pawn.Downed)
		{
			if (sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "EntityDowned".Translate() + ": x0%");
			}
			return false;
		}
		CompHoldingPlatformTarget comp = pawn.GetComp<CompHoldingPlatformTarget>();
		if (comp == null)
		{
			return false;
		}
		if (!comp.CurrentlyHeldOnPlatform)
		{
			return false;
		}
		if (comp.Props.baseEscapeIntervalMtbDays <= 0f)
		{
			if (sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "IncapableOfEscaping".Translate() + ": x0%");
			}
			return false;
		}
		return true;
	}
}
