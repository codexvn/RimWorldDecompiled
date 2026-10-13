using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E1E RID: 11806
	public class CompSmokeCloudMaker : ThingComp
	{
		// Token: 0x17002A9A RID: 10906
		// (get) Token: 0x06010A0B RID: 68107 RVA: 0x004CBA82 File Offset: 0x004C9C82
		private CompProperties_SmokeCloudMaker Props
		{
			get
			{
				return (CompProperties_SmokeCloudMaker)this.props;
			}
		}

		// Token: 0x06010A0C RID: 68108 RVA: 0x004CBA90 File Offset: 0x004C9C90
		public override void CompTick()
		{
			if (!ModLister.CheckIdeology("Smoke cloud maker"))
			{
				return;
			}
			if (!this.parent.Spawned)
			{
				return;
			}
			CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if ((compRefuelable == null || compRefuelable.HasFuel) & (compPowerTrader == null || compPowerTrader.PowerOn))
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.sourceStreamEffect.Spawn();
					this.effecter.Trigger(this.parent, this.parent, -1);
				}
				this.effecter.EffectTick(this.parent, this.parent);
				if (Rand.MTBEventOccurs(this.Props.fleckSpawnMTB, 1f, 1f))
				{
					CompSmokeCloudMaker.tmpValidCells.Clear();
					Room room = this.parent.GetRoom(RegionType.Set_All);
					CellRect cellRect = this.parent.OccupiedRect();
					foreach (IntVec3 intVec in cellRect.ExpandedBy(Mathf.FloorToInt(this.Props.cloudRadius + 1f)))
					{
						if (cellRect.ClosestCellTo(intVec).DistanceTo(intVec) <= this.Props.cloudRadius && intVec.GetRoom(this.parent.Map) == room)
						{
							CompSmokeCloudMaker.tmpValidCells.Add(intVec);
						}
					}
					if (CompSmokeCloudMaker.tmpValidCells.Count > 0)
					{
						Vector3 loc = CompSmokeCloudMaker.tmpValidCells.RandomElement<IntVec3>().ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
						this.parent.Map.flecks.CreateFleck(FleckMaker.GetDataStatic(loc, this.parent.Map, this.Props.cloudFleck, this.Props.fleckScale));
						return;
					}
				}
			}
			else
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
			}
		}

		// Token: 0x06010A0D RID: 68109 RVA: 0x004CBCA8 File Offset: 0x004C9EA8
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x0400AD4C RID: 44364
		private Effecter effecter;

		// Token: 0x0400AD4D RID: 44365
		private static List<IntVec3> tmpValidCells = new List<IntVec3>();
	}
}
