using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C83 RID: 11395
	public class CompFacilityInUse : ThingComp
	{
		// Token: 0x1700285D RID: 10333
		// (get) Token: 0x0600FFCA RID: 65482 RVA: 0x004A1C33 File Offset: 0x0049FE33
		public CompProperties_FacilityInUse Props
		{
			get
			{
				return this.props as CompProperties_FacilityInUse;
			}
		}

		// Token: 0x1700285E RID: 10334
		// (get) Token: 0x0600FFCB RID: 65483 RVA: 0x004A1C40 File Offset: 0x0049FE40
		public CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x0600FFCC RID: 65484 RVA: 0x004A1C6B File Offset: 0x0049FE6B
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			Effecter effecter = this.effecterInUse;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecterInUse = null;
		}

		// Token: 0x0600FFCD RID: 65485 RVA: 0x004A1C8D File Offset: 0x0049FE8D
		public override void CompTickRare()
		{
			this.CompTick();
		}

		// Token: 0x0600FFCE RID: 65486 RVA: 0x004A1C8D File Offset: 0x0049FE8D
		public override void CompTickLong()
		{
			this.CompTick();
		}

		// Token: 0x0600FFCF RID: 65487 RVA: 0x004A1C98 File Offset: 0x0049FE98
		public override void CompTick()
		{
			CompFacility compFacility = this.parent.TryGetComp<CompFacility>();
			List<Thing> list = (compFacility != null) ? compFacility.LinkedBuildings : null;
			if (list == null)
			{
				return;
			}
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			Thing thing = null;
			foreach (Thing thing2 in list)
			{
				if (compPowerTrader.PowerOn && this.BuildingInUse(thing2))
				{
					thing = thing2;
					break;
				}
			}
			bool flag = thing != null;
			this.operatingAtHighPower = false;
			if (this.Props.inUsePowerConsumption != null)
			{
				float num = compPowerTrader.Props.PowerConsumption;
				if (flag)
				{
					num = this.Props.inUsePowerConsumption.Value;
					this.operatingAtHighPower = true;
				}
				compPowerTrader.PowerOutput = -num;
			}
			if (this.Props.effectInUse != null)
			{
				if (flag)
				{
					if (this.effecterInUse == null)
					{
						this.effecterInUse = this.Props.effectInUse.Spawn();
						this.effecterInUse.Trigger(this.parent, thing, -1);
					}
					this.effecterInUse.EffectTick(this.parent, thing);
				}
				if (!flag && this.effecterInUse != null)
				{
					this.effecterInUse.Cleanup();
					this.effecterInUse = null;
				}
			}
		}

		// Token: 0x0600FFD0 RID: 65488 RVA: 0x004A1DF8 File Offset: 0x0049FFF8
		private bool BuildingInUse(Thing building)
		{
			Building_Bed building_Bed = building as Building_Bed;
			return building_Bed != null && building_Bed.AnyOccupants;
		}

		// Token: 0x0600FFD1 RID: 65489 RVA: 0x004A1E1C File Offset: 0x004A001C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string str;
			if (this.PowerTrader.Off)
			{
				str = "PowerConsumptionOff".Translate();
			}
			else if (!this.operatingAtHighPower)
			{
				str = "PowerConsumptionLow".Translate();
			}
			else
			{
				str = "PowerConsumptionHigh".Translate();
			}
			stringBuilder.Append(string.Format("{0}: {1}", "PowerConsumptionMode".Translate(), str.CapitalizeFirst()));
			return stringBuilder.ToString();
		}

		// Token: 0x0400A74C RID: 42828
		[Unsaved(false)]
		private bool operatingAtHighPower;

		// Token: 0x0400A74D RID: 42829
		[Unsaved(false)]
		private Effecter effecterInUse;

		// Token: 0x0400A74E RID: 42830
		[Unsaved(false)]
		private CompPowerTrader intPowerTrader;
	}
}
