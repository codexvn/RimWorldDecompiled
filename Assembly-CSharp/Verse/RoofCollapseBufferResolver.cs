using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200051A RID: 1306
	public class RoofCollapseBufferResolver
	{
		// Token: 0x0600221B RID: 8731 RVA: 0x000B1892 File Offset: 0x000AFA92
		public RoofCollapseBufferResolver(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000B18B8 File Offset: 0x000AFAB8
		public void CollapseRoofsMarkedToCollapse()
		{
			RoofCollapseBuffer roofCollapseBuffer = this.map.roofCollapseBuffer;
			if (roofCollapseBuffer.CellsMarkedToCollapse.Any<IntVec3>())
			{
				this.tmpCrushedThings.Clear();
				RoofCollapserImmediate.DropRoofInCells(roofCollapseBuffer.CellsMarkedToCollapse, this.map, this.tmpCrushedThings);
				if (this.tmpCrushedThings.Any<Thing>())
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("RoofCollapsed".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("TheseThingsCrushed".Translate());
					this.tmpCrushedNames.Clear();
					for (int i = 0; i < this.tmpCrushedThings.Count; i++)
					{
						Thing thing = this.tmpCrushedThings[i];
						Corpse corpse = thing as Corpse;
						if (corpse == null || !corpse.Bugged)
						{
							string item = thing.LabelShortCap;
							if (thing.def.category == ThingCategory.Pawn)
							{
								item = thing.LabelCap;
							}
							if (!this.tmpCrushedNames.Contains(item))
							{
								this.tmpCrushedNames.Add(item);
							}
						}
					}
					foreach (string str in this.tmpCrushedNames)
					{
						stringBuilder.AppendLine("    -" + str);
					}
					Find.LetterStack.ReceiveLetter("LetterLabelRoofCollapsed".Translate(), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.NegativeEvent, new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), null, null, null, null, 0, true);
				}
				else
				{
					Messages.Message("RoofCollapsed".Translate(), new TargetInfo(roofCollapseBuffer.CellsMarkedToCollapse[0], this.map, false), MessageTypeDefOf.SilentInput, true);
				}
				this.tmpCrushedThings.Clear();
				roofCollapseBuffer.Clear();
			}
		}

		// Token: 0x04001CBF RID: 7359
		private Map map;

		// Token: 0x04001CC0 RID: 7360
		private List<Thing> tmpCrushedThings = new List<Thing>();

		// Token: 0x04001CC1 RID: 7361
		private HashSet<string> tmpCrushedNames = new HashSet<string>();
	}
}
