using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A32 RID: 14898
	public class QuestNode_GenerateWorldObject : QuestNode
	{
		// Token: 0x0601464B RID: 83531 RVA: 0x00617F54 File Offset: 0x00616154
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(this.def.GetValue(slate));
			worldObject.Tile = this.tile.GetValue(slate);
			if (this.faction.GetValue(slate) != null)
			{
				worldObject.SetFaction(this.faction.GetValue(slate));
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<WorldObject>(this.storeAs.GetValue(slate), worldObject, false);
			}
		}

		// Token: 0x0601464C RID: 83532 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E166 RID: 57702
		public SlateRef<WorldObjectDef> def;

		// Token: 0x0400E167 RID: 57703
		public SlateRef<PlanetTile> tile;

		// Token: 0x0400E168 RID: 57704
		public SlateRef<Faction> faction;

		// Token: 0x0400E169 RID: 57705
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
