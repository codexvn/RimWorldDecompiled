using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA2 RID: 7074
	public class QuestPart_Filter_AnyColonistCapableOfHacking : QuestPart_Filter
	{
		// Token: 0x06009DC0 RID: 40384 RVA: 0x002DB240 File Offset: 0x002D9440
		protected override bool Pass(SignalArgs args)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.mapParent.Map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (HackUtility.IsCapableOfHacking(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009DC1 RID: 40385 RVA: 0x002DB2C0 File Offset: 0x002D94C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x04006D48 RID: 27976
		public MapParent mapParent;
	}
}
