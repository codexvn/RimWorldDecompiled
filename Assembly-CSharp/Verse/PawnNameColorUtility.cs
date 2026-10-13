using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005DB RID: 1499
	public static class PawnNameColorUtility
	{
		// Token: 0x06002672 RID: 9842 RVA: 0x000CB4BC File Offset: 0x000C96BC
		static PawnNameColorUtility()
		{
			for (int i = 0; i < 10; i++)
			{
				PawnNameColorUtility.ColorsNeutral.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseNeutral, i));
				PawnNameColorUtility.ColorsHostile.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBaseHostile, i));
				PawnNameColorUtility.ColorsPrisoner.Add(PawnNameColorUtility.RandomShiftOf(PawnNameColorUtility.ColorBasePrisoner, i));
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000CB6F8 File Offset: 0x000C98F8
		private static Color RandomShiftOf(Color color, int i)
		{
			return new Color(Mathf.Clamp01(color.r * PawnNameColorUtility.ColorShifts[i].r), Mathf.Clamp01(color.g * PawnNameColorUtility.ColorShifts[i].g), Mathf.Clamp01(color.b * PawnNameColorUtility.ColorShifts[i].b), color.a);
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000CB764 File Offset: 0x000C9964
		public static Color PawnNameColorOf(Pawn pawn)
		{
			if (pawn.MentalStateDef != null)
			{
				return pawn.MentalStateDef.nameColor;
			}
			int index;
			if (pawn.Faction == null)
			{
				index = 0;
			}
			else
			{
				index = pawn.Faction.randomKey % 10;
			}
			if (pawn.IsPrisoner)
			{
				return PawnNameColorUtility.ColorsPrisoner[index];
			}
			if (pawn.IsSlave && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return PawnNameColorUtility.ColorBaseHostile;
			}
			if (pawn.IsSlave)
			{
				return PawnNameColorUtility.ColorSlave;
			}
			if (pawn.IsWildMan())
			{
				return PawnNameColorUtility.ColorWildMan;
			}
			if (pawn.Faction == null)
			{
				return PawnNameColorUtility.ColorsNeutral[index];
			}
			if (pawn.IsColonyMechRequiringMechanitor())
			{
				return PawnNameColorUtility.ColorUncontrolledPlayerMech;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return PawnNameColorUtility.ColorColony;
			}
			if (pawn.Faction.HostileTo(Faction.OfPlayer))
			{
				return PawnNameColorUtility.ColorsHostile[index];
			}
			return PawnNameColorUtility.ColorsNeutral[index];
		}

		// Token: 0x04001FD5 RID: 8149
		private static readonly List<Color> ColorsNeutral = new List<Color>();

		// Token: 0x04001FD6 RID: 8150
		private static readonly List<Color> ColorsHostile = new List<Color>();

		// Token: 0x04001FD7 RID: 8151
		private static readonly List<Color> ColorsPrisoner = new List<Color>();

		// Token: 0x04001FD8 RID: 8152
		private static readonly Color ColorBaseNeutral = new Color(0.4f, 0.85f, 0.9f);

		// Token: 0x04001FD9 RID: 8153
		public static readonly Color ColorBaseHostile = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04001FDA RID: 8154
		private static readonly Color ColorBasePrisoner = new Color(1f, 0.85f, 0.5f);

		// Token: 0x04001FDB RID: 8155
		private static readonly Color ColorSlave = new Color32(222, 192, 22, byte.MaxValue);

		// Token: 0x04001FDC RID: 8156
		private static readonly Color ColorColony = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04001FDD RID: 8157
		private static readonly Color ColorWildMan = new Color(1f, 0.8f, 1f);

		// Token: 0x04001FDE RID: 8158
		private static readonly Color ColorUncontrolledPlayerMech = new Color(0.9f, 0.2f, 0.2f);

		// Token: 0x04001FDF RID: 8159
		private const int ColorShiftCount = 10;

		// Token: 0x04001FE0 RID: 8160
		private static readonly List<Color> ColorShifts = new List<Color>
		{
			new Color(1f, 1f, 1f),
			new Color(0.8f, 1f, 1f),
			new Color(0.8f, 0.8f, 1f),
			new Color(0.8f, 0.8f, 0.8f),
			new Color(1.2f, 1f, 1f),
			new Color(0.8f, 1.2f, 1f),
			new Color(0.8f, 1.2f, 1.2f),
			new Color(1.2f, 1.2f, 1.2f),
			new Color(1f, 1.2f, 1f),
			new Color(1.2f, 1f, 0.8f)
		};
	}
}
