using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E3E RID: 15934
	public static class WorldObjectMaker
	{
		// Token: 0x06015C6B RID: 89195 RVA: 0x00681A9C File Offset: 0x0067FC9C
		public static WorldObject MakeWorldObject(WorldObjectDef def)
		{
			WorldObject obj = (WorldObject)Activator.CreateInstance(def.worldObjectClass);
			return WorldObjectMaker.MakeWorldObject(def, obj);
		}

		// Token: 0x06015C6C RID: 89196 RVA: 0x00681AC1 File Offset: 0x0067FCC1
		public static WorldObject MakeWorldObject(WorldObjectDef def, WorldObject obj)
		{
			obj.def = def;
			obj.ID = Find.UniqueIDsManager.GetNextWorldObjectID();
			obj.creationGameTicks = Find.TickManager.TicksGame;
			obj.PostMake();
			return obj;
		}
	}
}
