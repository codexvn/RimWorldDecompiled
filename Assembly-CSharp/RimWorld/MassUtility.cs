using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200366E RID: 13934
	public static class MassUtility
	{
		// Token: 0x06013572 RID: 79218 RVA: 0x005C11F8 File Offset: 0x005BF3F8
		public static float EncumbrancePercent(Pawn pawn)
		{
			return Mathf.Clamp01(MassUtility.UnboundedEncumbrancePercent(pawn));
		}

		// Token: 0x06013573 RID: 79219 RVA: 0x005C1205 File Offset: 0x005BF405
		public static float UnboundedEncumbrancePercent(Pawn pawn)
		{
			return MassUtility.GearAndInventoryMass(pawn) / MassUtility.Capacity(pawn, null);
		}

		// Token: 0x06013574 RID: 79220 RVA: 0x005C1215 File Offset: 0x005BF415
		public static bool IsOverEncumbered(Pawn pawn)
		{
			return MassUtility.UnboundedEncumbrancePercent(pawn) > 1f;
		}

		// Token: 0x06013575 RID: 79221 RVA: 0x005C1224 File Offset: 0x005BF424
		public static bool WillBeOverEncumberedAfterPickingUp(Pawn pawn, Thing thing, int count)
		{
			return MassUtility.FreeSpace(pawn) < (float)count * thing.GetStatValue(StatDefOf.Mass, true, -1);
		}

		// Token: 0x06013576 RID: 79222 RVA: 0x005C123E File Offset: 0x005BF43E
		public static int CountToPickUpUntilOverEncumbered(Pawn pawn, Thing thing)
		{
			return Mathf.FloorToInt(MassUtility.FreeSpace(pawn) / thing.GetStatValue(StatDefOf.Mass, true, -1));
		}

		// Token: 0x06013577 RID: 79223 RVA: 0x005C1259 File Offset: 0x005BF459
		public static float FreeSpace(Pawn pawn)
		{
			return Mathf.Max(MassUtility.Capacity(pawn, null) - MassUtility.GearAndInventoryMass(pawn), 0f);
		}

		// Token: 0x06013578 RID: 79224 RVA: 0x005C1273 File Offset: 0x005BF473
		public static float GearAndInventoryMass(Pawn pawn)
		{
			return MassUtility.GearMass(pawn) + MassUtility.InventoryMass(pawn);
		}

		// Token: 0x06013579 RID: 79225 RVA: 0x005C1284 File Offset: 0x005BF484
		public static float GearMass(Pawn p)
		{
			float num = 0f;
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					num += wornApparel[i].GetStatValue(StatDefOf.Mass, true, 1);
				}
			}
			if (p.equipment != null)
			{
				foreach (ThingWithComps thing in p.equipment.AllEquipmentListForReading)
				{
					num += thing.GetStatValue(StatDefOf.Mass, true, 1);
				}
			}
			return num;
		}

		// Token: 0x0601357A RID: 79226 RVA: 0x005C1334 File Offset: 0x005BF534
		public static float InventoryMass(Pawn p)
		{
			float num = 0f;
			for (int i = 0; i < p.inventory.innerContainer.Count; i++)
			{
				Thing thing = p.inventory.innerContainer[i];
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Mass, true, -1);
			}
			return num;
		}

		// Token: 0x0601357B RID: 79227 RVA: 0x005C1390 File Offset: 0x005BF590
		public static float Capacity(Pawn p, StringBuilder explanation = null)
		{
			if (!MassUtility.CanEverCarryAnything(p))
			{
				return 0f;
			}
			float num = p.BodySize * 35f;
			if (explanation != null)
			{
				if (explanation.Length > 0)
				{
					explanation.AppendLine();
				}
				explanation.Append("  - " + p.LabelShortCap + ": " + num.ToStringMassOffset());
			}
			return num;
		}

		// Token: 0x0601357C RID: 79228 RVA: 0x005C13EE File Offset: 0x005BF5EE
		public static bool CanEverCarryAnything(Pawn p)
		{
			return (p.RaceProps.ToolUser && !p.DevelopmentalStage.Baby() && !p.IsSubhuman) || p.RaceProps.packAnimal;
		}

		// Token: 0x0400D6A1 RID: 54945
		public const float MassCapacityPerBodySize = 35f;
	}
}
