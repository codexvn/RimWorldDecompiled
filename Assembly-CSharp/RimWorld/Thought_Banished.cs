using System;

namespace RimWorld
{
	// Token: 0x02001855 RID: 6229
	public class Thought_Banished : Thought_Memory
	{
		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x06008F90 RID: 36752 RVA: 0x002A64D1 File Offset: 0x002A46D1
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.otherPawn.Faction == this.pawn.Faction;
			}
		}
	}
}
