using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020029F8 RID: 10744
	[StaticConstructorOnStartup]
	public class Building_Electroharvester : Building
	{
		// Token: 0x170024AF RID: 9391
		// (get) Token: 0x0600ED53 RID: 60755 RVA: 0x0044E120 File Offset: 0x0044C320
		public CompPowerPlantElectroharvester Electroharvester
		{
			get
			{
				CompPowerPlantElectroharvester result;
				if ((result = this.cachedElectroharvester) == null)
				{
					result = (this.cachedElectroharvester = base.GetComp<CompPowerPlantElectroharvester>());
				}
				return result;
			}
		}

		// Token: 0x170024B0 RID: 9392
		// (get) Token: 0x0600ED54 RID: 60756 RVA: 0x0044E148 File Offset: 0x0044C348
		public CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.compPowerTrader) == null)
				{
					result = (this.compPowerTrader = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x170024B1 RID: 9393
		// (get) Token: 0x0600ED55 RID: 60757 RVA: 0x0044E170 File Offset: 0x0044C370
		public CompCableConnection CableConnection
		{
			get
			{
				CompCableConnection result;
				if ((result = this.cableConnection) == null)
				{
					result = (this.cableConnection = base.GetComp<CompCableConnection>());
				}
				return result;
			}
		}

		// Token: 0x170024B2 RID: 9394
		// (get) Token: 0x0600ED56 RID: 60758 RVA: 0x0044E196 File Offset: 0x0044C396
		public bool PowerOn
		{
			get
			{
				return this.PowerTrader.PowerOn;
			}
		}

		// Token: 0x0600ED57 RID: 60759 RVA: 0x0044E1A3 File Offset: 0x0044C3A3
		public override bool IsWorking()
		{
			return this.PowerOn;
		}

		// Token: 0x0600ED58 RID: 60760 RVA: 0x0044E1AB File Offset: 0x0044C3AB
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600ED59 RID: 60761 RVA: 0x0044E1C0 File Offset: 0x0044C3C0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.Electroharvester.FacilityComp.OnLinkAdded -= this.OnLinkAdded;
			this.Electroharvester.FacilityComp.OnLinkRemoved -= this.OnLinkRemoved;
		}

		// Token: 0x0600ED5A RID: 60762 RVA: 0x0044E20C File Offset: 0x0044C40C
		private void Initialize()
		{
			if (this.initalized)
			{
				this.RebuildCables();
				return;
			}
			this.initalized = true;
			this.Electroharvester.FacilityComp.OnLinkAdded += this.OnLinkAdded;
			this.Electroharvester.FacilityComp.OnLinkRemoved += this.OnLinkRemoved;
			foreach (Thing thing in this.Electroharvester.Platforms)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
				}
			}
			this.RebuildCables();
		}

		// Token: 0x0600ED5B RID: 60763 RVA: 0x0044E2D0 File Offset: 0x0044C4D0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (!this.initalized)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600ED5C RID: 60764 RVA: 0x0044E2E8 File Offset: 0x0044C4E8
		private void OnLinkRemoved(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged -= this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600ED5D RID: 60765 RVA: 0x0044E31D File Offset: 0x0044C51D
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.RebuildCables();
		}

		// Token: 0x0600ED5E RID: 60766 RVA: 0x0044E32C File Offset: 0x0044C52C
		private void OnLinkAdded(CompFacility facility, Thing thing)
		{
			Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
			if (building_HoldingPlatform == null)
			{
				return;
			}
			building_HoldingPlatform.innerContainer.OnContentsChanged += this.RebuildCables;
			this.RebuildCables();
		}

		// Token: 0x0600ED5F RID: 60767 RVA: 0x0044E361 File Offset: 0x0044C561
		private void RebuildCables()
		{
			this.CableConnection.RebuildCables(this.Electroharvester.Platforms, delegate(Thing thing)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				return building_HoldingPlatform != null && building_HoldingPlatform.Occupied;
			});
		}

		// Token: 0x04009D22 RID: 40226
		private CompPowerPlantElectroharvester cachedElectroharvester;

		// Token: 0x04009D23 RID: 40227
		private CompPowerTrader compPowerTrader;

		// Token: 0x04009D24 RID: 40228
		private bool initalized;

		// Token: 0x04009D25 RID: 40229
		private CompCableConnection cableConnection;
	}
}
