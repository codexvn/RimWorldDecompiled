using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A47 RID: 6727
	public class HistoryAutoRecorderGroupDef : Def
	{
		// Token: 0x06009765 RID: 38757 RVA: 0x002C3620 File Offset: 0x002C1820
		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}

		// Token: 0x0400646B RID: 25707
		public bool useFixedScale;

		// Token: 0x0400646C RID: 25708
		public Vector2 fixedScale;

		// Token: 0x0400646D RID: 25709
		public bool integersOnly;

		// Token: 0x0400646E RID: 25710
		public bool onlyPositiveValues = true;

		// Token: 0x0400646F RID: 25711
		public bool devModeOnly;

		// Token: 0x04006470 RID: 25712
		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();
	}
}
