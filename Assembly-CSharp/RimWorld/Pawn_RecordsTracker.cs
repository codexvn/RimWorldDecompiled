using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024D0 RID: 9424
	public class Pawn_RecordsTracker : IExposable
	{
		// Token: 0x17001F75 RID: 8053
		// (get) Token: 0x0600CD6E RID: 52590 RVA: 0x003C4494 File Offset: 0x003C2694
		public Battle BattleActive
		{
			get
			{
				if (this.battleExitTick < Find.TickManager.TicksGame)
				{
					return null;
				}
				if (this.battleActive == null)
				{
					return null;
				}
				while (this.battleActive.AbsorbedBy != null)
				{
					this.battleActive = this.battleActive.AbsorbedBy;
				}
				return this.battleActive;
			}
		}

		// Token: 0x17001F76 RID: 8054
		// (get) Token: 0x0600CD6F RID: 52591 RVA: 0x003C44E3 File Offset: 0x003C26E3
		public int LastBattleTick
		{
			get
			{
				return this.battleExitTick;
			}
		}

		// Token: 0x0600CD70 RID: 52592 RVA: 0x003C44EB File Offset: 0x003C26EB
		public Pawn_RecordsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CD71 RID: 52593 RVA: 0x003C4505 File Offset: 0x003C2705
		public void RecordsTickInterval(int delta)
		{
			if (this.pawn.Dead)
			{
				return;
			}
			if (this.pawn.IsHashIntervalTick(80, delta))
			{
				this.RecordsTickUpdate(80);
				this.battleActive = this.BattleActive;
			}
		}

		// Token: 0x0600CD72 RID: 52594 RVA: 0x003C4539 File Offset: 0x003C2739
		public void RecordsTickMothballed(int interval)
		{
			this.RecordsTickUpdate(interval);
		}

		// Token: 0x0600CD73 RID: 52595 RVA: 0x003C4544 File Offset: 0x003C2744
		private void RecordsTickUpdate(int interval)
		{
			List<RecordDef> allDefsListForReading = DefDatabase<RecordDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].type == RecordType.Time && allDefsListForReading[i].Worker.ShouldMeasureTimeNow(this.pawn))
				{
					DefMap<RecordDef, float> defMap = this.records;
					RecordDef def = allDefsListForReading[i];
					defMap[def] += (float)interval;
				}
			}
		}

		// Token: 0x0600CD74 RID: 52596 RVA: 0x003C45B0 File Offset: 0x003C27B0
		public void Increment(RecordDef def)
		{
			if (def.type != RecordType.Int)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to increment record \"",
					def.defName,
					"\" whose record type is \"",
					def.type.ToString(),
					"\"."
				}));
				return;
			}
			this.records[def] = Mathf.Round(this.records[def] + 1f);
		}

		// Token: 0x0600CD75 RID: 52597 RVA: 0x003C4630 File Offset: 0x003C2830
		public void AddTo(RecordDef def, float value)
		{
			if (def.type == RecordType.Int)
			{
				this.records[def] = Mathf.Round(this.records[def] + Mathf.Round(value));
				return;
			}
			if (def.type == RecordType.Float)
			{
				DefMap<RecordDef, float> defMap = this.records;
				defMap[def] += value;
				return;
			}
			Log.Error(string.Concat(new string[]
			{
				"Tried to add value to record \"",
				def.defName,
				"\" whose record type is \"",
				def.type.ToString(),
				"\"."
			}));
		}

		// Token: 0x0600CD76 RID: 52598 RVA: 0x003C46D4 File Offset: 0x003C28D4
		public float GetValue(RecordDef def)
		{
			float num = this.records[def];
			if (def.type == RecordType.Int || def.type == RecordType.Time)
			{
				return Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x0600CD77 RID: 52599 RVA: 0x003C4707 File Offset: 0x003C2907
		public int GetAsInt(RecordDef def)
		{
			return Mathf.RoundToInt(this.records[def]);
		}

		// Token: 0x0600CD78 RID: 52600 RVA: 0x003C471A File Offset: 0x003C291A
		public void EnterBattle(Battle battle)
		{
			this.battleActive = battle;
			this.battleExitTick = Find.TickManager.TicksGame + 5000;
		}

		// Token: 0x0600CD79 RID: 52601 RVA: 0x003C473C File Offset: 0x003C293C
		public void ExposeData()
		{
			this.battleActive = this.BattleActive;
			Scribe_Deep.Look<DefMap<RecordDef, float>>(ref this.records, "records", Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.battleActive, "battleActive", false);
			Scribe_Values.Look<int>(ref this.battleExitTick, "battleExitTick", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04008BE8 RID: 35816
		public Pawn pawn;

		// Token: 0x04008BE9 RID: 35817
		private DefMap<RecordDef, float> records = new DefMap<RecordDef, float>();

		// Token: 0x04008BEA RID: 35818
		private Battle battleActive;

		// Token: 0x04008BEB RID: 35819
		private int battleExitTick;

		// Token: 0x04008BEC RID: 35820
		private const int UpdateTimeRecordsIntervalTicks = 80;
	}
}
