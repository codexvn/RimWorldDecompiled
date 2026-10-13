using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003868 RID: 14440
	public class CompSpawnEffectersInRoom : ThingComp
	{
		// Token: 0x170032FC RID: 13052
		// (get) Token: 0x06013F5D RID: 81757 RVA: 0x005EC183 File Offset: 0x005EA383
		private CompProperties_SpawnEffectersInRoom Props
		{
			get
			{
				return (CompProperties_SpawnEffectersInRoom)this.props;
			}
		}

		// Token: 0x06013F5E RID: 81758 RVA: 0x005EC190 File Offset: 0x005EA390
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.effecters.Clear();
		}

		// Token: 0x06013F5F RID: 81759 RVA: 0x005EC1A4 File Offset: 0x005EA3A4
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			foreach (Effecter effecter in this.effecters.Values)
			{
				effecter.Cleanup();
			}
		}

		// Token: 0x06013F60 RID: 81760 RVA: 0x005EC204 File Offset: 0x005EA404
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			Room room = this.parent.GetRoom(RegionType.Set_All);
			if (room == null || room.TouchesMapEdge)
			{
				return;
			}
			if (this.parent.IsRitualTarget())
			{
				foreach (IntVec3 cell in room.Cells)
				{
					if (cell.InHorDistOf(this.parent.Position, this.Props.radius))
					{
						this.CheckEffecter(cell);
					}
				}
			}
		}

		// Token: 0x06013F61 RID: 81761 RVA: 0x005EC2A8 File Offset: 0x005EA4A8
		private void CheckEffecter(IntVec3 cell)
		{
			if (this.effecters.ContainsKey(cell))
			{
				if (this.effecters[cell] == null)
				{
					this.effecters[cell] = this.Props.effecter.Spawn();
					this.effecters[cell].Trigger(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid, -1);
				}
			}
			else
			{
				this.effecters[cell] = this.Props.effecter.Spawn();
				this.effecters[cell].Trigger(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid, -1);
			}
			this.effecters[cell].EffectTick(new TargetInfo(cell, this.parent.Map, false), TargetInfo.Invalid);
		}

		// Token: 0x0400DC6A RID: 56426
		private readonly Dictionary<IntVec3, Effecter> effecters = new Dictionary<IntVec3, Effecter>();
	}
}
