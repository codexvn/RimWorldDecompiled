using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D15 RID: 7445
	public sealed class RelationshipRecords : IExposable
	{
		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x0600A682 RID: 42626 RVA: 0x002FA917 File Offset: 0x002F8B17
		public IReadOnlyDictionary<int, RelationshipRecord> Records
		{
			get
			{
				return this.records;
			}
		}

		// Token: 0x0600A683 RID: 42627 RVA: 0x002FA920 File Offset: 0x002F8B20
		public RelationshipRecord GetOrCreateRecord(Pawn pawn)
		{
			RelationshipRecord result;
			if (this.records.TryGetValue(pawn.thingIDNumber, out result))
			{
				return result;
			}
			return this.CreateRecord(pawn);
		}

		// Token: 0x0600A684 RID: 42628 RVA: 0x002FA94C File Offset: 0x002F8B4C
		public RelationshipRecord CreateRecord(Pawn pawn)
		{
			RelationshipRecord relationshipRecord = new RelationshipRecord(pawn.thingIDNumber, pawn.gender, pawn.Name.ToStringShort);
			this.records.Add(relationshipRecord.ID, relationshipRecord);
			return relationshipRecord;
		}

		// Token: 0x0600A685 RID: 42629 RVA: 0x002FA989 File Offset: 0x002F8B89
		public RelationshipRecord GetRecord(int id)
		{
			return this.records[id];
		}

		// Token: 0x0600A686 RID: 42630 RVA: 0x002FA998 File Offset: 0x002F8B98
		public int CleanupUnusedRecords()
		{
			RelationshipRecords.toRemove.Clear();
			foreach (KeyValuePair<int, RelationshipRecord> keyValuePair in this.records)
			{
				int num;
				RelationshipRecord relationshipRecord;
				keyValuePair.Deconstruct(out num, out relationshipRecord);
				int item = num;
				if (relationshipRecord.References.Count <= 1)
				{
					RelationshipRecords.toRemove.Add(item);
				}
			}
			foreach (int key in RelationshipRecords.toRemove)
			{
				foreach (Pawn pawn in this.records[key].References)
				{
					pawn.relations.RemoveRelation(this.records[key]);
				}
				this.records.Remove(key);
			}
			int count = RelationshipRecords.toRemove.Count;
			RelationshipRecords.toRemove.Clear();
			return count;
		}

		// Token: 0x0600A687 RID: 42631 RVA: 0x002FAAD0 File Offset: 0x002F8CD0
		public void ExposeData()
		{
			Scribe_Collections.Look<int, RelationshipRecord>(ref this.records, "records", LookMode.Value, LookMode.Deep, ref this.tmpKeys, ref this.tmpValues, true, false, false);
		}

		// Token: 0x0400734C RID: 29516
		private Dictionary<int, RelationshipRecord> records = new Dictionary<int, RelationshipRecord>();

		// Token: 0x0400734D RID: 29517
		private static readonly HashSet<int> toRemove = new HashSet<int>();

		// Token: 0x0400734E RID: 29518
		private List<int> tmpKeys;

		// Token: 0x0400734F RID: 29519
		private List<RelationshipRecord> tmpValues;
	}
}
