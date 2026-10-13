using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200264F RID: 9807
	public class PreceptWorker_Building : PreceptWorker
	{
		// Token: 0x0600D6DE RID: 55006 RVA: 0x003FB800 File Offset: 0x003F9A00
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			bool flag = false;
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				Precept_Building precept_Building = precept as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef != null && precept_Building.ThingDef.isAltar)
				{
					flag = true;
					break;
				}
			}
			if (flag && def.isAltar)
			{
				return new AcceptanceReport("IdeoAlreadyHasAltar".Translate());
			}
			if (!flag)
			{
				return def.isAltar;
			}
			if (!def.isAltar)
			{
				return true;
			}
			if (flag && def.isAltar)
			{
				return false;
			}
			return true;
		}
	}
}
