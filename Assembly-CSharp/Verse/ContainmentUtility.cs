using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003D6 RID: 982
	public static class ContainmentUtility
	{
		// Token: 0x0600192A RID: 6442 RVA: 0x0007F13C File Offset: 0x0007D33C
		public static bool SafeContainerExistsFor(Thing thing)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			using (List<Thing>.Enumerator enumerator = thing.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SafelyContains(thing))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0007F1AC File Offset: 0x0007D3AC
		public static bool SafelyContains(this Thing container, Thing thing)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			CompEntityHolder compEntityHolder = container.TryGetComp<CompEntityHolder>();
			return compEntityHolder != null && compEntityHolder.ContainmentStrength >= thing.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0007F1E6 File Offset: 0x0007D3E6
		public static float GetEscapeIntervalMultiplier(Thing entity, CompEntityHolder entityHolder)
		{
			return ContainmentUtility.EscapeIntervalMultiplierFromContainmentStrengthCurve.Evaluate(entityHolder.ContainmentStrength - entity.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1));
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0007F206 File Offset: 0x0007D406
		public static float GetStudyKnowledgeAmountMultiplier(Thing entity, CompEntityHolder entityHolder)
		{
			return ContainmentUtility.StudyKnowledgeMultiplierFromContainmentStrengthCurve.Evaluate(entityHolder.ContainmentStrength - entity.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1));
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0007F226 File Offset: 0x0007D426
		public static IEnumerable<IntVec3> GetInhibitorAffectedCells(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			CompProperties_Facility comp = def.GetCompProperties<CompProperties_Facility>();
			if (comp == null)
			{
				yield break;
			}
			if (ContainmentUtility.IsLinearBuildingBlocked(def, center, rot, map, null))
			{
				yield break;
			}
			bool edificeFound = false;
			float num2;
			for (float i = comp.minDistance; i < comp.maxDistance; i = num2 + 1f)
			{
				if (edificeFound)
				{
					yield break;
				}
				int num = Mathf.FloorToInt(i);
				IntVec3 intVec = center + IntVec3.South.RotatedBy(rot) * (num - 1 + def.size.z);
				if (intVec.InBounds(map))
				{
					if (intVec.GetEdifice(map) == null)
					{
						yield return intVec;
					}
					else
					{
						edificeFound = true;
						yield return intVec;
					}
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0007F24B File Offset: 0x0007D44B
		public static IntVec3 GetLinearBuildingFrontCell(ThingDef def, IntVec3 center, Rot4 rot)
		{
			if (def.GetCompProperties<CompProperties_Facility>() == null)
			{
				return IntVec3.Invalid;
			}
			return center + IntVec3.South.RotatedBy(rot) * (def.size.z - 1);
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x0007F280 File Offset: 0x0007D480
		public static bool IsLinearBuildingBlocked(ThingDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			if (def.GetCompProperties<CompProperties_Facility>() == null)
			{
				return false;
			}
			Building edifice = ContainmentUtility.GetLinearBuildingFrontCell(def, center, rot).GetEdifice(map);
			return edifice != null && edifice != thingToIgnore;
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0007F2B4 File Offset: 0x0007D4B4
		public static bool ShowContainmentStats(Thing thing)
		{
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			if (compStudiable != null && thing is Pawn)
			{
				return compStudiable.RequiresHoldingPlatform;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			return ((compHoldingPlatformTarget != null) ? compHoldingPlatformTarget.Props.heldPawnKind : null) != null;
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0007F2F8 File Offset: 0x0007D4F8
		public static bool TryGetColdContainmentBonus(Thing thing, out float modifier)
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (thing.TryGetComp(out compHoldingPlatformTarget) && compHoldingPlatformTarget.Props.getsColdContainmentBonus)
			{
				modifier = ContainmentUtility.ColdEscapeFactorCurve.Evaluate(thing.AmbientTemperature);
				return true;
			}
			modifier = 0f;
			return false;
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x0007F338 File Offset: 0x0007D538
		public static float InitiateEscapeMtbDays(Pawn pawn, StringBuilder sb = null)
		{
			if (!ContainmentUtility.CanParticipateInEscape(pawn, sb))
			{
				return -1f;
			}
			CompHoldingPlatformTarget comp = pawn.GetComp<CompHoldingPlatformTarget>();
			float num = comp.Props.baseEscapeIntervalMtbDays;
			pawn.GetRoom(RegionType.Set_All);
			float num2 = 1f / Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
			num *= num2;
			if (sb != null && num2 > 1f)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorForMovement".Translate() + ": x" + num2.ToStringPercent());
			}
			float x = (float)pawn.mindState.entityTicksInCaptivity / 60000f;
			float num3 = ContainmentUtility.DaysInCaptivityEscapeFactorCurve.Evaluate(x);
			num *= num3;
			if (num3 > 1f && sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorForDaysInCaptivity".Translate() + ": x" + num3.ToStringPercent());
			}
			float escapeIntervalMultiplier = ContainmentUtility.GetEscapeIntervalMultiplier(pawn, comp.HeldPlatform.GetComp<CompEntityHolder>());
			num *= escapeIntervalMultiplier;
			if (sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorContainmentStrength".Translate() + ": x" + escapeIntervalMultiplier.ToStringPercent());
			}
			if (comp.Props.getsColdContainmentBonus)
			{
				float num4 = ContainmentUtility.ColdEscapeFactorCurve.Evaluate(pawn.AmbientTemperature);
				num *= num4;
				if (num4 > 1f && sb != null)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("  - " + "FactorForColdCaptivity".Translate() + ": x" + num4.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x0007F51C File Offset: 0x0007D71C
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

		// Token: 0x0400163C RID: 5692
		private static readonly SimpleCurve DaysInCaptivityEscapeFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(15f, 1f),
				true
			},
			{
				new CurvePoint(60f, 1.2f),
				true
			},
			{
				new CurvePoint(120f, 1.5f),
				true
			},
			{
				new CurvePoint(240f, 2f),
				true
			}
		};

		// Token: 0x0400163D RID: 5693
		private static readonly SimpleCurve ColdEscapeFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 1.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		// Token: 0x0400163E RID: 5694
		private static readonly SimpleCurve EscapeIntervalMultiplierFromContainmentStrengthCurve = new SimpleCurve
		{
			{
				new CurvePoint(-0.001f, 0.05f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 3f),
				true
			},
			{
				new CurvePoint(200f, 8f),
				true
			}
		};

		// Token: 0x0400163F RID: 5695
		private static readonly SimpleCurve StudyKnowledgeMultiplierFromContainmentStrengthCurve = new SimpleCurve
		{
			{
				new CurvePoint(-0.001f, 0.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1.3f),
				true
			},
			{
				new CurvePoint(200f, 1.5f),
				true
			}
		};
	}
}
