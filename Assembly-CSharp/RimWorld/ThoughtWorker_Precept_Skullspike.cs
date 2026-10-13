using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E4 RID: 6116
	public class ThoughtWorker_Precept_Skullspike : ThoughtWorker_Precept
	{
		// Token: 0x06008E39 RID: 36409 RVA: 0x002A2698 File Offset: 0x002A0898
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.surroundings == null || p.IsSlave)
			{
				return false;
			}
			int val = p.surroundings.NumSkullspikeSightings();
			if (ThoughtWorker_Precept_Skullspike.LowRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (ThoughtWorker_Precept_Skullspike.MediumRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (ThoughtWorker_Precept_Skullspike.MaxRange.Includes(val))
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return false;
		}

		// Token: 0x04005BF1 RID: 23537
		public static readonly IntRange LowRange = new IntRange(1, 3);

		// Token: 0x04005BF2 RID: 23538
		public static readonly IntRange MediumRange = new IntRange(4, 8);

		// Token: 0x04005BF3 RID: 23539
		public static readonly IntRange MaxRange = new IntRange(9, int.MaxValue);
	}
}
