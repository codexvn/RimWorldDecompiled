using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B9 RID: 6329
	public class ThoughtWorker_WearingColor_Ideo : ThoughtWorker_WearingColor
	{
		// Token: 0x06009089 RID: 37001 RVA: 0x002A987C File Offset: 0x002A7A7C
		protected override Color? Color(Pawn p)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return null;
			}
			return new Color?(ideo.ApparelColor);
		}
	}
}
