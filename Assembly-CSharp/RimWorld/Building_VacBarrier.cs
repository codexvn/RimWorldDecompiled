using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A59 RID: 10841
	public class Building_VacBarrier : Building_SupportedDoor
	{
		// Token: 0x17002581 RID: 9601
		// (get) Token: 0x0600F0D3 RID: 61651 RVA: 0x0045D944 File Offset: 0x0045BB44
		private CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = base.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x17002582 RID: 9602
		// (get) Token: 0x0600F0D4 RID: 61652 RVA: 0x0045D96C File Offset: 0x0045BB6C
		private VacuumComponent Vacuum
		{
			get
			{
				VacuumComponent result;
				if ((result = this.intVacuum) == null)
				{
					Map mapHeld = base.MapHeld;
					result = (this.intVacuum = ((mapHeld != null) ? mapHeld.GetComponent<VacuumComponent>() : null));
				}
				return result;
			}
		}

		// Token: 0x17002583 RID: 9603
		// (get) Token: 0x0600F0D5 RID: 61653 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool FreePassage
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002584 RID: 9604
		// (get) Token: 0x0600F0D6 RID: 61654 RVA: 0x0045D99E File Offset: 0x0045BB9E
		public override bool ExchangeVacuum
		{
			get
			{
				return !this.PowerTrader.PowerOn;
			}
		}

		// Token: 0x17002585 RID: 9605
		// (get) Token: 0x0600F0D7 RID: 61655 RVA: 0x0045D9AE File Offset: 0x0045BBAE
		protected override float TempEqualizeRate
		{
			get
			{
				if (!this.PowerTrader.PowerOn)
				{
					return base.TempEqualizeRate;
				}
				return 0f;
			}
		}

		// Token: 0x17002586 RID: 9606
		// (get) Token: 0x0600F0D8 RID: 61656 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool AlwaysOpen
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002587 RID: 9607
		// (get) Token: 0x0600F0D9 RID: 61657 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected override float OpenPct
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17002588 RID: 9608
		// (get) Token: 0x0600F0DA RID: 61658 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanDrawMovers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600F0DB RID: 61659 RVA: 0x0045D9C9 File Offset: 0x0045BBC9
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff")
			{
				VacuumComponent vacuum = this.Vacuum;
				if (vacuum == null)
				{
					return;
				}
				vacuum.Dirty();
			}
		}

		// Token: 0x0600F0DC RID: 61660 RVA: 0x0045D9F8 File Offset: 0x0045BBF8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.PowerTrader.PowerOn && base.Map.Biome.inVacuum)
			{
				this.Graphic.Draw(drawLoc, flip ? base.Rotation.Opposite : base.Rotation, this, 0f);
			}
		}

		// Token: 0x04009EE7 RID: 40679
		private CompPowerTrader intPowerTrader;

		// Token: 0x04009EE8 RID: 40680
		private VacuumComponent intVacuum;
	}
}
