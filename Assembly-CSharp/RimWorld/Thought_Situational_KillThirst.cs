using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C1 RID: 6337
	public class Thought_Situational_KillThirst : Thought_Situational
	{
		// Token: 0x060090A2 RID: 37026 RVA: 0x002A9B2C File Offset: 0x002A7D2C
		public override float MoodOffset()
		{
			Need_KillThirst need_KillThirst;
			if (this.pawn.needs == null || !this.pawn.needs.TryGetNeed<Need_KillThirst>(out need_KillThirst))
			{
				return 0f;
			}
			return Thought_Situational_KillThirst.MoodOffsetCurve.Evaluate(need_KillThirst.CurLevel);
		}

		// Token: 0x04005C49 RID: 23625
		private static readonly SimpleCurve MoodOffsetCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.301f, 0f),
				true
			},
			{
				new CurvePoint(0.3f, -4f),
				true
			},
			{
				new CurvePoint(0f, -18f),
				true
			}
		};
	}
}
