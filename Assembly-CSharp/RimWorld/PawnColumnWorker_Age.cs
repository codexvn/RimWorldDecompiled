using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003453 RID: 13395
	public class PawnColumnWorker_Age : PawnColumnWorker_Text
	{
		// Token: 0x17003145 RID: 12613
		// (get) Token: 0x06012D71 RID: 77169 RVA: 0x000E9647 File Offset: 0x000E7847
		protected override TextAnchor Anchor
		{
			get
			{
				return TextAnchor.MiddleCenter;
			}
		}

		// Token: 0x06012D72 RID: 77170 RVA: 0x00595E0A File Offset: 0x0059400A
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 50);
		}

		// Token: 0x06012D73 RID: 77171 RVA: 0x00595E1C File Offset: 0x0059401C
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalYears.CompareTo(b.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x06012D74 RID: 77172 RVA: 0x00595E48 File Offset: 0x00594048
		protected override string GetTextFor(Pawn pawn)
		{
			return pawn.ageTracker.AgeBiologicalYears.ToString();
		}
	}
}
