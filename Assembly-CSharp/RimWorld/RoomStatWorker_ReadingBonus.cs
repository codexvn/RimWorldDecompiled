using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002177 RID: 8567
	public class RoomStatWorker_ReadingBonus : RoomStatWorker
	{
		// Token: 0x0600BAF2 RID: 47858 RVA: 0x0035F6C4 File Offset: 0x0035D8C4
		public override float GetScore(Room room)
		{
			float num = 0f;
			float num2 = 0f;
			foreach (Building_Bookcase building_Bookcase in room.ContainedThings<Building_Bookcase>())
			{
				foreach (float num3 in building_Bookcase.CellsFilledPercentage)
				{
					num2 += num3;
				}
			}
			int num4 = 0;
			while (num2 > 0f && num < 0.2f)
			{
				float num5 = (num2 >= 1f) ? 1f : num2;
				num2 -= num5;
				num += num5 * RoomStatWorker_ReadingBonus.CellFilledFactor[Mathf.Min(num4++, RoomStatWorker_ReadingBonus.CellFilledFactor.Count - 1)];
			}
			return 1f + Mathf.Min(num, 0.2f);
		}

		// Token: 0x040080B6 RID: 32950
		private const float MaxEnhancement = 0.2f;

		// Token: 0x040080B7 RID: 32951
		private static readonly List<float> CellFilledFactor = new List<float>
		{
			0.04f,
			0.02f,
			0.01f,
			0.005f
		};
	}
}
