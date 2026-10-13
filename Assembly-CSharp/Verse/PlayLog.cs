using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000341 RID: 833
	public class PlayLog : IExposable
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x00064275 File Offset: 0x00062475
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x0006427D File Offset: 0x0006247D
		public int LastTick
		{
			get
			{
				if (this.entries.Count == 0)
				{
					return 0;
				}
				return this.entries[0].Tick;
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0006429F File Offset: 0x0006249F
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x000642B4 File Offset: 0x000624B4
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000642E8 File Offset: 0x000624E8
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00064300 File Offset: 0x00062500
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						string[] array = new string[5];
						array[0] = "Discarding pawn ";
						array[1] = ((p != null) ? p.ToString() : null);
						array[2] = ", but he is referenced by a play log entry ";
						int num = 3;
						LogEntry logEntry = this.entries[i];
						array[num] = ((logEntry != null) ? logEntry.ToString() : null);
						array[4] = ".";
						Log.Warning(string.Concat(array));
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x000643A8 File Offset: 0x000625A8
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000643E0 File Offset: 0x000625E0
		public void Notify_IdeoRemoved(Ideo ideo)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].Notify_IdeoRemoved(ideo);
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00064415 File Offset: 0x00062615
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00064424 File Offset: 0x00062624
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040013CD RID: 5069
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x040013CE RID: 5070
		private const int Capacity = 150;
	}
}
