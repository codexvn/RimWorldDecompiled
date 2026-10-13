using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023D0 RID: 9168
	public class FactionPermit : IExposable
	{
		// Token: 0x17001E1D RID: 7709
		// (get) Token: 0x0600C6C6 RID: 50886 RVA: 0x0039ED65 File Offset: 0x0039CF65
		public RoyalTitleDef Title
		{
			get
			{
				return this.title;
			}
		}

		// Token: 0x17001E1E RID: 7710
		// (get) Token: 0x0600C6C7 RID: 50887 RVA: 0x0039ED6D File Offset: 0x0039CF6D
		public RoyalTitlePermitDef Permit
		{
			get
			{
				return this.permit;
			}
		}

		// Token: 0x17001E1F RID: 7711
		// (get) Token: 0x0600C6C8 RID: 50888 RVA: 0x0039ED75 File Offset: 0x0039CF75
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17001E20 RID: 7712
		// (get) Token: 0x0600C6C9 RID: 50889 RVA: 0x0039ED7D File Offset: 0x0039CF7D
		public int LastUsedTick
		{
			get
			{
				return this.lastUsedTick;
			}
		}

		// Token: 0x17001E21 RID: 7713
		// (get) Token: 0x0600C6CA RID: 50890 RVA: 0x0039ED85 File Offset: 0x0039CF85
		public bool OnCooldown
		{
			get
			{
				return this.LastUsedTick > 0 && Find.TickManager.TicksGame < this.LastUsedTick + this.permit.CooldownTicks;
			}
		}

		// Token: 0x0600C6CB RID: 50891 RVA: 0x0039EDB0 File Offset: 0x0039CFB0
		public FactionPermit()
		{
		}

		// Token: 0x0600C6CC RID: 50892 RVA: 0x0039EDBF File Offset: 0x0039CFBF
		public FactionPermit(Faction faction, RoyalTitleDef title, RoyalTitlePermitDef permit)
		{
			this.title = title;
			this.faction = faction;
			this.permit = permit;
		}

		// Token: 0x0600C6CD RID: 50893 RVA: 0x0039EDE3 File Offset: 0x0039CFE3
		public void Notify_Used()
		{
			this.lastUsedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600C6CE RID: 50894 RVA: 0x0039EDF5 File Offset: 0x0039CFF5
		public void ResetCooldown()
		{
			this.lastUsedTick = -1;
		}

		// Token: 0x0600C6CF RID: 50895 RVA: 0x0039EE00 File Offset: 0x0039D000
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Defs.Look<RoyalTitleDef>(ref this.title, "title");
			Scribe_Defs.Look<RoyalTitlePermitDef>(ref this.permit, "permit");
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", -1, false);
		}

		// Token: 0x04008772 RID: 34674
		private RoyalTitleDef title;

		// Token: 0x04008773 RID: 34675
		private RoyalTitlePermitDef permit;

		// Token: 0x04008774 RID: 34676
		private Faction faction;

		// Token: 0x04008775 RID: 34677
		private int lastUsedTick = -1;
	}
}
