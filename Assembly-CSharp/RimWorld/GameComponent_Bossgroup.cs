using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037D6 RID: 14294
	public class GameComponent_Bossgroup : GameComponent
	{
		// Token: 0x1700329C RID: 12956
		// (get) Token: 0x06013CE1 RID: 81121 RVA: 0x005E05A6 File Offset: 0x005DE7A6
		public List<BossDef> AllBosses
		{
			get
			{
				if (this.allBosses.NullOrEmpty<BossDef>())
				{
					this.allBosses = new List<BossDef>();
					this.allBosses.AddRange(DefDatabase<BossDef>.AllDefs);
				}
				return this.allBosses;
			}
		}

		// Token: 0x06013CE2 RID: 81122 RVA: 0x005E05D6 File Offset: 0x005DE7D6
		public GameComponent_Bossgroup(Game game)
		{
		}

		// Token: 0x06013CE3 RID: 81123 RVA: 0x005E0600 File Offset: 0x005DE800
		public bool ReservedByBossgroup(PawnKindDef bossgroupKidDef)
		{
			BossDef bossForKind = this.GetBossForKind(bossgroupKidDef);
			if (bossForKind != null)
			{
				if (bossForKind.appearAfterTicks < Find.TickManager.TicksGame)
				{
					return false;
				}
				if (!this.IsDefeated(bossForKind))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013CE4 RID: 81124 RVA: 0x005E0638 File Offset: 0x005DE838
		public bool IsDefeated(BossDef bossDef)
		{
			return this.killedBosses.Contains(bossDef);
		}

		// Token: 0x06013CE5 RID: 81125 RVA: 0x005E0648 File Offset: 0x005DE848
		private BossDef GetBossForKind(PawnKindDef kindDef)
		{
			return this.AllBosses.FirstOrDefault((BossDef b) => b.kindDef == kindDef);
		}

		// Token: 0x06013CE6 RID: 81126 RVA: 0x005E0679 File Offset: 0x005DE879
		public int NumTimesCalledBossgroup(BossgroupDef bossgroupDef)
		{
			if (!this.timesCalledBossgroups.ContainsKey(bossgroupDef))
			{
				this.timesCalledBossgroups.Add(bossgroupDef, 0);
			}
			return this.timesCalledBossgroups[bossgroupDef];
		}

		// Token: 0x06013CE7 RID: 81127 RVA: 0x005E06A2 File Offset: 0x005DE8A2
		public void ResetProgress()
		{
			this.timesCalledBossgroups.Clear();
		}

		// Token: 0x06013CE8 RID: 81128 RVA: 0x005E06B0 File Offset: 0x005DE8B0
		public void Notify_BossgroupCalled(BossgroupDef bossgroupDef)
		{
			this.lastBossgroupCalled = Find.TickManager.TicksGame;
			if (this.timesCalledBossgroups.ContainsKey(bossgroupDef))
			{
				Dictionary<BossgroupDef, int> dictionary = this.timesCalledBossgroups;
				int num = dictionary[bossgroupDef];
				dictionary[bossgroupDef] = num + 1;
				return;
			}
			this.timesCalledBossgroups.Add(bossgroupDef, 0);
		}

		// Token: 0x06013CE9 RID: 81129 RVA: 0x005E0704 File Offset: 0x005DE904
		public virtual void Notify_PawnKilled(Pawn pawn)
		{
			BossDef bossForKind = this.GetBossForKind(pawn.kindDef);
			if (bossForKind != null && !this.killedBosses.Contains(bossForKind))
			{
				this.killedBosses.Add(bossForKind);
			}
		}

		// Token: 0x06013CEA RID: 81130 RVA: 0x005E073B File Offset: 0x005DE93B
		public void DebugResetDefeatedPawns()
		{
			this.killedBosses.Clear();
		}

		// Token: 0x06013CEB RID: 81131 RVA: 0x005E0748 File Offset: 0x005DE948
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastBossgroupCalled, "lastBossgroupCalled", -9999999, false);
			Scribe_Collections.Look<BossDef>(ref this.killedBosses, "killedBosses", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<BossgroupDef, int>(ref this.timesCalledBossgroups, "timesCalledBossgroups", LookMode.Def, LookMode.Value, ref this.bossgroupTmp, ref this.calledTmp, true, false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				HashSet<PawnKindDef> hashSet = null;
				Scribe_Collections.Look<PawnKindDef>(ref hashSet, "killedBossgroupMechs", LookMode.Undefined);
				if (hashSet != null)
				{
					if (this.killedBosses == null)
					{
						this.killedBosses = new List<BossDef>();
					}
					foreach (PawnKindDef kindDef in hashSet)
					{
						BossDef bossForKind = this.GetBossForKind(kindDef);
						if (bossForKind != null && !this.killedBosses.Contains(bossForKind))
						{
							this.killedBosses.Add(bossForKind);
						}
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.timesCalledBossgroups == null)
				{
					this.timesCalledBossgroups = new Dictionary<BossgroupDef, int>();
				}
				if (this.killedBosses == null)
				{
					this.killedBosses = new List<BossDef>();
				}
			}
		}

		// Token: 0x0400DB65 RID: 56165
		public int lastBossgroupCalled = -9999999;

		// Token: 0x0400DB66 RID: 56166
		private Dictionary<BossgroupDef, int> timesCalledBossgroups = new Dictionary<BossgroupDef, int>();

		// Token: 0x0400DB67 RID: 56167
		private List<BossDef> allBosses;

		// Token: 0x0400DB68 RID: 56168
		private List<BossDef> killedBosses = new List<BossDef>();

		// Token: 0x0400DB69 RID: 56169
		private List<BossgroupDef> bossgroupTmp;

		// Token: 0x0400DB6A RID: 56170
		private List<int> calledTmp;
	}
}
