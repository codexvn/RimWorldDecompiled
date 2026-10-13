using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024D3 RID: 9427
	public class RoyalTitle : IExposable
	{
		// Token: 0x17001F84 RID: 8068
		// (get) Token: 0x0600CD9C RID: 52636 RVA: 0x003C4F1C File Offset: 0x003C311C
		public string Label
		{
			get
			{
				return this.def.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17001F85 RID: 8069
		// (get) Token: 0x0600CD9D RID: 52637 RVA: 0x003C4F2F File Offset: 0x003C312F
		private int RoomRequirementsGracePeriodTicksLeft
		{
			get
			{
				int[] array = new int[3];
				array[0] = 180000 - (GenTicks.TicksGame - this.receivedTick);
				array[1] = MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft;
				return Mathf.Max(array);
			}
		}

		// Token: 0x17001F86 RID: 8070
		// (get) Token: 0x0600CD9E RID: 52638 RVA: 0x003C4F59 File Offset: 0x003C3159
		public float RoomRequirementGracePeriodDaysLeft
		{
			get
			{
				return this.RoomRequirementsGracePeriodTicksLeft.TicksToDays();
			}
		}

		// Token: 0x0600CD9F RID: 52639 RVA: 0x003C4F66 File Offset: 0x003C3166
		public bool RoomRequirementGracePeriodActive(Pawn pawn)
		{
			return this.RoomRequirementsGracePeriodTicksLeft > 0 && !pawn.IsQuestLodger();
		}

		// Token: 0x0600CDA0 RID: 52640 RVA: 0x003C4F7C File Offset: 0x003C317C
		public RoyalTitle()
		{
		}

		// Token: 0x0600CDA1 RID: 52641 RVA: 0x003C4F8B File Offset: 0x003C318B
		public RoyalTitle(RoyalTitle other)
		{
			this.faction = other.faction;
			this.def = other.def;
			this.receivedTick = other.receivedTick;
			this.pawn = other.pawn;
		}

		// Token: 0x0600CDA2 RID: 52642 RVA: 0x003C4FCC File Offset: 0x003C31CC
		public void RoyalTitleTick(int delta)
		{
			if (this.pawn.IsHashIntervalTick(833, delta) && this.conceited && this.pawn.Spawned && this.pawn.IsFreeColonist && (!this.pawn.IsQuestLodger() || this.pawn.LodgerAllowedDecrees()) && this.def.decreeMtbDays > 0f && this.pawn.Awake() && Rand.MTBEventOccurs(this.def.decreeMtbDays, 60000f, 833f) && (float)(Find.TickManager.TicksGame - this.pawn.royalty.lastDecreeTicks) >= this.def.decreeMinIntervalDays * 60000f)
			{
				this.pawn.royalty.IssueDecree(false, null);
			}
		}

		// Token: 0x0600CDA3 RID: 52643 RVA: 0x003C50B0 File Offset: 0x003C32B0
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.receivedTick, "receivedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.wasInherited, "wasInherited", false, false);
			Scribe_Values.Look<bool>(ref this.conceited, "conceited", false, false);
		}

		// Token: 0x04008BF7 RID: 35831
		public Faction faction;

		// Token: 0x04008BF8 RID: 35832
		public RoyalTitleDef def;

		// Token: 0x04008BF9 RID: 35833
		public Pawn pawn;

		// Token: 0x04008BFA RID: 35834
		public int receivedTick = -1;

		// Token: 0x04008BFB RID: 35835
		public bool wasInherited;

		// Token: 0x04008BFC RID: 35836
		public bool conceited;

		// Token: 0x04008BFD RID: 35837
		private const int DecreeCheckInterval = 833;

		// Token: 0x04008BFE RID: 35838
		private const int RoomRequirementsGracePeriodTicks = 180000;
	}
}
