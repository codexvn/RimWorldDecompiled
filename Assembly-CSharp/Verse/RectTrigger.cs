using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200087D RID: 2173
	public class RectTrigger : PawnTrigger
	{
		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06003718 RID: 14104 RVA: 0x001276D4 File Offset: 0x001258D4
		// (set) Token: 0x06003719 RID: 14105 RVA: 0x001276DC File Offset: 0x001258DC
		public CellRect Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
				if (base.Spawned)
				{
					this.rect.ClipInsideMap(base.Map);
				}
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x001276FF File Offset: 0x001258FF
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.rect.ClipInsideMap(base.Map);
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x0012771C File Offset: 0x0012591C
		protected override void Tick()
		{
			if (this.destroyIfUnfogged && !this.rect.CenterCell.Fogged(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!this.IsHashIntervalTick(60) || base.Destroyed)
			{
				return;
			}
			Map map = base.Map;
			for (int i = this.rect.minZ; i <= this.rect.maxZ; i++)
			{
				for (int j = this.rect.minX; j <= this.rect.maxX; j++)
				{
					List<Thing> thingList = new IntVec3(j, 0, i).GetThingList(map);
					for (int k = 0; k < thingList.Count; k++)
					{
						if (base.TriggeredBy(thingList[k]))
						{
							base.ActivatedBy((Pawn)thingList[k]);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x001277F4 File Offset: 0x001259F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.rect, "rect", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.destroyIfUnfogged, "destroyIfUnfogged", false, false);
			Scribe_Values.Look<bool>(ref this.activateOnExplosion, "activateOnExplosion", false, false);
		}

		// Token: 0x04002953 RID: 10579
		private CellRect rect;

		// Token: 0x04002954 RID: 10580
		public bool destroyIfUnfogged;

		// Token: 0x04002955 RID: 10581
		public bool activateOnExplosion;
	}
}
