using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5C RID: 7004
	public class HistoryAutoRecorder : IExposable
	{
		// Token: 0x06009C79 RID: 40057 RVA: 0x002D625C File Offset: 0x002D445C
		public void Tick()
		{
			if (Find.TickManager.TicksGame % this.def.recordTicksFrequency == 0 || !this.records.Any<float>())
			{
				if (Find.AnyPlayerHomeMap == null)
				{
					this.records.Add(this.records.Last<float>());
					return;
				}
				float item = this.def.Worker.PullRecord();
				this.records.Add(item);
			}
		}

		// Token: 0x06009C7A RID: 40058 RVA: 0x002D62CC File Offset: 0x002D44CC
		public void ExposeData()
		{
			Scribe_Defs.Look<HistoryAutoRecorderDef>(ref this.def, "def");
			byte[] recordsFromBytes = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				recordsFromBytes = this.RecordsToBytes();
			}
			DataExposeUtility.LookByteArray(ref recordsFromBytes, "records");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetRecordsFromBytes(recordsFromBytes);
			}
		}

		// Token: 0x06009C7B RID: 40059 RVA: 0x002D6318 File Offset: 0x002D4518
		private byte[] RecordsToBytes()
		{
			byte[] array = new byte[this.records.Count * 4];
			for (int i = 0; i < this.records.Count; i++)
			{
				byte[] bytes = BitConverter.GetBytes(this.records[i]);
				for (int j = 0; j < 4; j++)
				{
					array[i * 4 + j] = bytes[j];
				}
			}
			return array;
		}

		// Token: 0x06009C7C RID: 40060 RVA: 0x002D6378 File Offset: 0x002D4578
		private void SetRecordsFromBytes(byte[] bytes)
		{
			int num = bytes.Length / 4;
			this.records.Clear();
			for (int i = 0; i < num; i++)
			{
				float item = BitConverter.ToSingle(bytes, i * 4);
				this.records.Add(item);
			}
		}

		// Token: 0x04006C84 RID: 27780
		public HistoryAutoRecorderDef def;

		// Token: 0x04006C85 RID: 27781
		public List<float> records = new List<float>();
	}
}
