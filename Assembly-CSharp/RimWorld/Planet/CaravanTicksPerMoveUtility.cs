using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D68 RID: 15720
	public static class CaravanTicksPerMoveUtility
	{
		// Token: 0x0601569F RID: 87711 RVA: 0x0066855E File Offset: 0x0066675E
		public static int GetTicksPerMove(Caravan caravan, StringBuilder explanation = null)
		{
			if (caravan == null)
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				return 3300;
			}
			return CaravanTicksPerMoveUtility.GetTicksPerMove(new CaravanTicksPerMoveUtility.CaravanInfo(caravan), explanation);
		}

		// Token: 0x060156A0 RID: 87712 RVA: 0x0066857E File Offset: 0x0066677E
		public static int GetTicksPerMove(CaravanTicksPerMoveUtility.CaravanInfo caravanInfo, StringBuilder explanation = null)
		{
			return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.pawns, caravanInfo.massUsage, caravanInfo.massCapacity, caravanInfo.isShuttle, explanation);
		}

		// Token: 0x060156A1 RID: 87713 RVA: 0x006685A0 File Offset: 0x006667A0
		public static int GetTicksPerMove(List<Pawn> pawns, float massUsage, float massCapacity, bool isShuttle = false, StringBuilder explanation = null)
		{
			CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Clear();
			if (!pawns.Any<Pawn>())
			{
				if (explanation != null)
				{
					CaravanTicksPerMoveUtility.AppendUsingDefaultTicksPerMoveInfo(explanation);
				}
				return 3300;
			}
			if (isShuttle)
			{
				if (explanation != null)
				{
					explanation.Append("CaravanMovementSpeedShuttle".Translate());
				}
				return 0;
			}
			int num = 0;
			foreach (Pawn pawn in pawns)
			{
				if (pawn.RaceProps.Humanlike)
				{
					num++;
				}
				else if (pawn.IsCaravanRideable())
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Add(pawn.GetStatValue(StatDefOf.CaravanRidingSpeedFactor, true, -1));
				}
			}
			float num2 = 1f;
			int num3 = 0;
			int count = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count;
			if (count > 0 && num > 0)
			{
				CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Sort();
				CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Reverse();
				if (CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count > num)
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.RemoveRange(num, CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count - num);
				}
				num3 = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count;
				while (CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Count < num)
				{
					CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Add(1f);
				}
				num2 = CaravanTicksPerMoveUtility.caravanAnimalSpeedFactors.Average();
			}
			float num4 = 0f;
			int num5 = 0;
			foreach (Pawn pawn2 in pawns)
			{
				if (CaravanBonusUtility.HasCaravanBonus(pawn2))
				{
					num4 += pawn2.GetStatValue(StatDefOf.CaravanBonusSpeedFactor, true, -1);
					num5++;
				}
			}
			if (num5 != 0)
			{
				num4 /= (float)num5;
			}
			else
			{
				num4 = 1f;
			}
			float num6 = (float)CaravanTicksPerMoveUtility.BaseHumanlikeTicksPerCell() * 340f;
			float moveSpeedFactorFromMass = CaravanTicksPerMoveUtility.GetMoveSpeedFactorFromMass(massUsage, massCapacity);
			int num7 = Mathf.Max(Mathf.RoundToInt(num6 / (moveSpeedFactorFromMass * num2 * num4)), 1);
			bool flag = massUsage > massCapacity;
			if (explanation != null)
			{
				float num8 = 60000f / num6;
				explanation.Append("CaravanMovementSpeedFull".Translate() + ":");
				explanation.AppendLine();
				explanation.Append("  " + "StatsReport_BaseValue".Translate() + ": " + num8.ToString("0.#") + " " + "TilesPerDay".Translate());
				explanation.AppendLine();
				explanation.Append("  " + "RideableAnimalsPerPeople".Translate() + string.Format(": {0} / {1}", count, num));
				if (num3 > 0)
				{
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierFromRiddenAnimals".Translate() + ": " + num2.ToStringPercent());
				}
				if (!Mathf.Approximately(num4, 1f))
				{
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierFromMoveSpeedBonus".Translate() + ": " + num4.ToStringPercent());
				}
				if (!flag)
				{
					explanation.AppendLine();
					explanation.Append("  " + "MultiplierForCarriedMass".Translate(moveSpeedFactorFromMass.ToStringPercent()));
				}
				float num9 = 60000f / (float)num7;
				explanation.AppendLine();
				explanation.Append("  " + "FinalCaravanPawnsMovementSpeed".Translate() + ": " + num9.ToString("0.#") + " " + "TilesPerDay".Translate());
			}
			return num7;
		}

		// Token: 0x060156A2 RID: 87714 RVA: 0x0066899C File Offset: 0x00666B9C
		private static int BaseHumanlikeTicksPerCell()
		{
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.MoveSpeed, null) / 60f;
			return Mathf.RoundToInt(1f / num);
		}

		// Token: 0x060156A3 RID: 87715 RVA: 0x006689CC File Offset: 0x00666BCC
		private static float GetMoveSpeedFactorFromMass(float massUsage, float massCapacity)
		{
			if (massCapacity <= 0f)
			{
				return 1f;
			}
			float t = massUsage / massCapacity;
			return Mathf.Lerp(2f, 1f, t);
		}

		// Token: 0x060156A4 RID: 87716 RVA: 0x006689FC File Offset: 0x00666BFC
		private static void AppendUsingDefaultTicksPerMoveInfo(StringBuilder sb)
		{
			sb.Append("CaravanMovementSpeedFull".Translate() + ":");
			float num = 18.181818f;
			sb.AppendLine();
			sb.Append("  " + "Default".Translate() + ": " + num.ToString("0.#") + " " + "TilesPerDay".Translate());
		}

		// Token: 0x0400EE59 RID: 61017
		public const float CellToTilesConversionRatio = 340f;

		// Token: 0x0400EE5A RID: 61018
		public const int DefaultTicksPerMove = 3300;

		// Token: 0x0400EE5B RID: 61019
		private const float MoveSpeedFactorAtZeroMass = 2f;

		// Token: 0x0400EE5C RID: 61020
		private static readonly List<float> caravanAnimalSpeedFactors = new List<float>();

		// Token: 0x0400EE5D RID: 61021
		private static readonly List<float> nomadSpeedFactors = new List<float>();

		// Token: 0x02003D69 RID: 15721
		public struct CaravanInfo
		{
			// Token: 0x060156A6 RID: 87718 RVA: 0x00668AA0 File Offset: 0x00666CA0
			public CaravanInfo(Caravan caravan)
			{
				this.pawns = caravan.PawnsListForReading;
				this.massUsage = caravan.MassUsage;
				this.massCapacity = caravan.MassCapacity;
				this.isShuttle = (caravan.Shuttle != null);
			}

			// Token: 0x060156A7 RID: 87719 RVA: 0x00668AD5 File Offset: 0x00666CD5
			public CaravanInfo(Dialog_FormCaravan formCaravanDialog)
			{
				this.pawns = TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables);
				this.massUsage = formCaravanDialog.MassUsage;
				this.massCapacity = formCaravanDialog.MassCapacity;
				this.isShuttle = false;
			}

			// Token: 0x0400EE5E RID: 61022
			public List<Pawn> pawns;

			// Token: 0x0400EE5F RID: 61023
			public float massUsage;

			// Token: 0x0400EE60 RID: 61024
			public float massCapacity;

			// Token: 0x0400EE61 RID: 61025
			public bool isShuttle;
		}
	}
}
