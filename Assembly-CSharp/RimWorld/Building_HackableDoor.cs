using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A24 RID: 10788
	public class Building_HackableDoor : Building_SupportedDoor, IHackable
	{
		// Token: 0x17002520 RID: 9504
		// (get) Token: 0x0600EF1A RID: 61210 RVA: 0x004568C4 File Offset: 0x00454AC4
		public CompHackable Hackable
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableInt) == null)
				{
					result = (this.hackableInt = base.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x17002521 RID: 9505
		// (get) Token: 0x0600EF1B RID: 61211 RVA: 0x004568EA File Offset: 0x00454AEA
		public bool Locked
		{
			get
			{
				return !this.Hackable.IsHacked;
			}
		}

		// Token: 0x17002522 RID: 9506
		// (get) Token: 0x0600EF1C RID: 61212 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CheckFaction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600EF1D RID: 61213 RVA: 0x004568FA File Offset: 0x00454AFA
		public override bool PawnCanOpen(Pawn p)
		{
			return !this.Locked && base.PawnCanOpen(p);
		}

		// Token: 0x0600EF1E RID: 61214 RVA: 0x000026EA File Offset: 0x000008EA
		public void OnLockedOut(Pawn pawn = null)
		{
		}

		// Token: 0x0600EF1F RID: 61215 RVA: 0x0045690D File Offset: 0x00454B0D
		public void OnHacked(Pawn pawn = null)
		{
			if (((pawn != null) ? pawn.Faction : null) != null)
			{
				this.SetFaction(pawn.Faction, null);
			}
		}

		// Token: 0x04009E0F RID: 40463
		private CompHackable hackableInt;
	}
}
