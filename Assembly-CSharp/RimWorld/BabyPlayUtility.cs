using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200153C RID: 5436
	public static class BabyPlayUtility
	{
		// Token: 0x06008115 RID: 33045 RVA: 0x0026DBF0 File Offset: 0x0026BDF0
		public static bool PlayTickCheckEnd(Pawn baby, Pawn adult, float playFactor, int delta, Thing playSource = null)
		{
			if (baby.needs.play == null)
			{
				return true;
			}
			float num = 0.0002f * (float)delta;
			if (playSource != null)
			{
				num *= playSource.GetStatValue(StatDefOf.BabyPlayGainFactor, true, -1);
			}
			num *= playFactor;
			baby.needs.play.Play(num);
			float num2 = 0.000144f * (float)delta;
			if (playSource != null)
			{
				num2 *= playSource.GetStatValue(StatDefOf.JoyGainFactor, true, -1);
			}
			Need_Joy joy = adult.needs.joy;
			if (joy != null)
			{
				joy.GainJoy(num2, JoyKindDefOf.Social);
			}
			return baby.needs.play.CurLevel >= 0.99f;
		}

		// Token: 0x06008116 RID: 33046 RVA: 0x0026DC94 File Offset: 0x0026BE94
		public static float GetRoomPlayGainFactors(Pawn baby)
		{
			float num = 1f;
			Room room = baby.PositionHeld.GetRoom(baby.MapHeld);
			if (room == null || room.IsHuge)
			{
				return num;
			}
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i].def == ThingDefOf.BabyDecoration && containedAndAdjacentThings[i].Position.DistanceTo(baby.Position) < 9.9f)
				{
					num *= containedAndAdjacentThings[i].GetStatValue(StatDefOf.BabyPlayGainFactor, true, -1);
					break;
				}
			}
			return num;
		}

		// Token: 0x04005624 RID: 22052
		private const float PlayPerTick = 0.0002f;

		// Token: 0x04005625 RID: 22053
		private const float EndJobMaxPlay = 0.99f;

		// Token: 0x04005626 RID: 22054
		private const float MaxBabyDecorationDistance = 9.9f;
	}
}
