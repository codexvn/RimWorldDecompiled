using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000321 RID: 801
	public class BattleLog : IExposable
	{
		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0005E3CF File Offset: 0x0005C5CF
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x0005E3D8 File Offset: 0x0005C5D8
		public void Add(LogEntry entry)
		{
			Battle battle = null;
			foreach (Thing thing in entry.GetConcerns())
			{
				Battle battleActive = ((Pawn)thing).records.BattleActive;
				if (battle == null)
				{
					battle = battleActive;
				}
				else if (battleActive != null)
				{
					battle = ((battle.Importance > battleActive.Importance) ? battle : battleActive);
				}
			}
			if (battle == null)
			{
				battle = Battle.Create();
				this.battles.Insert(0, battle);
			}
			foreach (Thing thing2 in entry.GetConcerns())
			{
				Pawn pawn = (Pawn)thing2;
				Battle battleActive2 = pawn.records.BattleActive;
				if (battleActive2 != null && battleActive2 != battle)
				{
					battle.Absorb(battleActive2);
					this.battles.Remove(battleActive2);
				}
				pawn.records.EnterBattle(battle);
			}
			battle.Add(entry);
			this.cachedActiveEntries = null;
			this.ReduceToCapacity();
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x0005E4E4 File Offset: 0x0005C6E4
		private void ReduceToCapacity()
		{
			int num = this.battles.Count((Battle btl) => btl.AbsorbedBy == null);
			while (num > 20 && this.battles[this.battles.Count - 1].LastEntryTimestamp + Mathf.Max(420000, 5000) < Find.TickManager.TicksGame)
			{
				if (this.battles[this.battles.Count - 1].AbsorbedBy == null)
				{
					num--;
				}
				this.battles.RemoveAt(this.battles.Count - 1);
				this.cachedActiveEntries = null;
			}
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x0005E59E File Offset: 0x0005C79E
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0005E5D4 File Offset: 0x0005C7D4
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.battles.Count; i++)
			{
				if (this.battles[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0005E610 File Offset: 0x0005C810
		public bool IsEntryActive(LogEntry log)
		{
			if (this.cachedActiveEntries == null)
			{
				this.cachedActiveEntries = new HashSet<LogEntry>();
				for (int i = 0; i < this.battles.Count; i++)
				{
					List<LogEntry> entries = this.battles[i].Entries;
					for (int j = 0; j < entries.Count; j++)
					{
						this.cachedActiveEntries.Add(entries[j]);
					}
				}
			}
			return this.cachedActiveEntries.Contains(log);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0005E688 File Offset: 0x0005C888
		public void RemoveEntry(LogEntry log)
		{
			int num = 0;
			while (num < this.battles.Count && !this.battles[num].Entries.Remove(log))
			{
				num++;
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0005E6CC File Offset: 0x0005C8CC
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
			this.cachedActiveEntries = null;
		}

		// Token: 0x040012F3 RID: 4851
		private List<Battle> battles = new List<Battle>();

		// Token: 0x040012F4 RID: 4852
		private const int BattleHistoryLength = 20;

		// Token: 0x040012F5 RID: 4853
		private HashSet<LogEntry> cachedActiveEntries;
	}
}
