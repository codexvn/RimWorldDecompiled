using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D17 RID: 11543
	public class CompLaunchable_TransportPod : CompLaunchable
	{
		// Token: 0x1700292B RID: 10539
		// (get) Token: 0x0601038C RID: 66444 RVA: 0x004B1296 File Offset: 0x004AF496
		private new CompProperties_Launchable_TransportPod Props
		{
			get
			{
				return (CompProperties_Launchable_TransportPod)this.props;
			}
		}

		// Token: 0x1700292C RID: 10540
		// (get) Token: 0x0601038D RID: 66445 RVA: 0x004B12A3 File Offset: 0x004AF4A3
		public CompRefuelable FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map).TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x1700292D RID: 10541
		// (get) Token: 0x0601038E RID: 66446 RVA: 0x004B12C5 File Offset: 0x004AF4C5
		public bool ConnectedToFuelingPort
		{
			get
			{
				return this.FuelingPortSource != null;
			}
		}

		// Token: 0x1700292E RID: 10542
		// (get) Token: 0x0601038F RID: 66447 RVA: 0x004B12D0 File Offset: 0x004AF4D0
		public override CompRefuelable Refuelable
		{
			get
			{
				return this.FuelingPortSource;
			}
		}

		// Token: 0x1700292F RID: 10543
		// (get) Token: 0x06010390 RID: 66448 RVA: 0x004B12D8 File Offset: 0x004AF4D8
		public override float FuelLevel
		{
			get
			{
				return this.FuelingPortSourceFuel;
			}
		}

		// Token: 0x17002930 RID: 10544
		// (get) Token: 0x06010391 RID: 66449 RVA: 0x004B12E0 File Offset: 0x004AF4E0
		public override bool RequiresFuelingPort
		{
			get
			{
				return this.Props.requiresFuelingPort;
			}
		}

		// Token: 0x17002931 RID: 10545
		// (get) Token: 0x06010392 RID: 66450 RVA: 0x004B12ED File Offset: 0x004AF4ED
		public override float MaxFuelLevel
		{
			get
			{
				if (!this.Props.requiresFuelingPort)
				{
					return float.PositiveInfinity;
				}
				return this.FuelingPortSource.Props.fuelCapacity;
			}
		}

		// Token: 0x17002932 RID: 10546
		// (get) Token: 0x06010393 RID: 66451 RVA: 0x004B1312 File Offset: 0x004AF512
		private float FuelingPortSourceFuel
		{
			get
			{
				if (!this.RequiresFuelingPort)
				{
					return float.PositiveInfinity;
				}
				if (this.FuelingPortSource == null)
				{
					return 0f;
				}
				return this.FuelingPortSource.Fuel;
			}
		}

		// Token: 0x17002933 RID: 10547
		// (get) Token: 0x06010394 RID: 66452 RVA: 0x004B133C File Offset: 0x004AF53C
		private bool AllInGroupConnectedToFuelingPort
		{
			get
			{
				foreach (CompTransporter compTransporter in base.TransportersInGroup)
				{
					CompLaunchable_TransportPod compLaunchable_TransportPod = compTransporter.Launchable as CompLaunchable_TransportPod;
					if (compLaunchable_TransportPod != null && !compLaunchable_TransportPod.ConnectedToFuelingPort)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06010395 RID: 66453 RVA: 0x004B13AC File Offset: 0x004AF5AC
		public override AcceptanceReport CanLaunch(float? overrideFuelLevel)
		{
			if (base.Transporter.LoadingInProgressOrReadyToLaunch && this.Props.requiresFuelingPort && !this.AllInGroupConnectedToFuelingPort)
			{
				return "CommandLaunchGroupFailNotConnectedToFuelingPort".Translate();
			}
			return base.CanLaunch(overrideFuelLevel);
		}

		// Token: 0x06010396 RID: 66454 RVA: 0x004B13E8 File Offset: 0x004AF5E8
		public override string CompInspectStringExtra()
		{
			if (base.Transporter.LoadingInProgressOrReadyToLaunch && this.Props.requiresFuelingPort && !this.AllInGroupConnectedToFuelingPort)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate().CapitalizeFirst() + ".";
			}
			return base.CompInspectStringExtra();
		}
	}
}
