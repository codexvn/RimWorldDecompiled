using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7A RID: 8058
	public class TileMutatorConditionMapComponent : MapComponent
	{
		// Token: 0x0600B210 RID: 45584 RVA: 0x00094327 File Offset: 0x00092527
		public TileMutatorConditionMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x0600B211 RID: 45585 RVA: 0x0032E2B4 File Offset: 0x0032C4B4
		public override void MapGenerated()
		{
			if (this.map.TileInfo.Mutators.EnumerableNullOrEmpty<TileMutatorDef>())
			{
				return;
			}
			foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
			{
				if (!tileMutatorDef.additionalGameConditions.NullOrEmpty<GameConditionDef>())
				{
					foreach (GameConditionDef def in tileMutatorDef.additionalGameConditions)
					{
						this.map.GameConditionManager.RegisterCondition(GameConditionMaker.MakeConditionPermanent(def));
					}
				}
			}
		}
	}
}
