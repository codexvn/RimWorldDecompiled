using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A45 RID: 6725
	public class HistoryAutoRecorderDef : Def
	{
		// Token: 0x17001722 RID: 5922
		// (get) Token: 0x06009760 RID: 38752 RVA: 0x002C35D3 File Offset: 0x002C17D3
		public HistoryAutoRecorderWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (HistoryAutoRecorderWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x06009761 RID: 38753 RVA: 0x002C35F9 File Offset: 0x002C17F9
		public static HistoryAutoRecorderDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderDef>.GetNamed(defName, true);
		}

		// Token: 0x04006465 RID: 25701
		public Type workerClass;

		// Token: 0x04006466 RID: 25702
		public int recordTicksFrequency = 60000;

		// Token: 0x04006467 RID: 25703
		public Color graphColor = Color.green;

		// Token: 0x04006468 RID: 25704
		[MustTranslate]
		public string graphLabelY;

		// Token: 0x04006469 RID: 25705
		public string valueFormat;

		// Token: 0x0400646A RID: 25706
		[Unsaved(false)]
		private HistoryAutoRecorderWorker workerInt;
	}
}
