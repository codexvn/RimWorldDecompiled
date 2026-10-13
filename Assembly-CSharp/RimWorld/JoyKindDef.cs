using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5F RID: 6751
	public class JoyKindDef : Def
	{
		// Token: 0x060097B6 RID: 38838 RVA: 0x002C4D7C File Offset: 0x002C2F7C
		public bool PawnCanDo(Pawn pawn)
		{
			if (pawn.royalty != null)
			{
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.conceited && royalTitle.def.JoyKindDisabled(this))
					{
						return false;
					}
				}
				if (this.titleRequiredAny == null)
				{
					return true;
				}
				bool flag = false;
				foreach (RoyalTitle royalTitle2 in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (this.titleRequiredAny.Contains(royalTitle2.def))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x04006512 RID: 25874
		public List<RoyalTitleDef> titleRequiredAny;

		// Token: 0x04006513 RID: 25875
		public bool needsThing = true;
	}
}
