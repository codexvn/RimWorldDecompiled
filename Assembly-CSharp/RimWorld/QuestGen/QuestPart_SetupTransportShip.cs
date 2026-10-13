using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A19 RID: 14873
	public class QuestPart_SetupTransportShip : QuestPart
	{
		// Token: 0x060145F9 RID: 83449 RVA: 0x00616C28 File Offset: 0x00614E28
		public override bool QuestPartReserves(Pawn p)
		{
			if (p == null)
			{
				return false;
			}
			if (this.pawns.NotNullAndContains(p))
			{
				return true;
			}
			TransportShip transportShip = this.transportShip;
			bool flag;
			if (transportShip == null)
			{
				flag = (null != null);
			}
			else
			{
				CompTransporter transporterComp = transportShip.TransporterComp;
				flag = (((transporterComp != null) ? transporterComp.innerContainer : null) != null);
			}
			return flag && this.transportShip.TransporterComp.innerContainer.Contains(p);
		}

		// Token: 0x060145FA RID: 83450 RVA: 0x00616C82 File Offset: 0x00614E82
		public override bool QuestPartReserves(TransportShip ship)
		{
			return ship == this.transportShip;
		}

		// Token: 0x060145FB RID: 83451 RVA: 0x00616C8D File Offset: 0x00614E8D
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			List<Pawn> list = this.pawns;
			if (list == null)
			{
				return;
			}
			list.Replace(replace, with);
		}

		// Token: 0x060145FC RID: 83452 RVA: 0x00616CA4 File Offset: 0x00614EA4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				if (!this.pawns.NullOrEmpty<Pawn>())
				{
					using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsWorldPawn())
							{
								Log.Error("Trying to transfer a non-world pawn to a transportShip.");
							}
						}
					}
					this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(this.pawns, true, true);
					this.pawns.Clear();
				}
				if (!this.items.NullOrEmpty<Thing>())
				{
					this.transportShip.TransporterComp.innerContainer.TryAddRangeOrTransfer(this.items, true, true);
					this.items.Clear();
				}
				this.transportShip.Start();
			}
		}

		// Token: 0x060145FD RID: 83453 RVA: 0x00616D8C File Offset: 0x00614F8C
		public override void Cleanup()
		{
			base.Cleanup();
			this.transportShip = null;
			this.items = null;
			this.pawns = null;
		}

		// Token: 0x060145FE RID: 83454 RVA: 0x00616DAC File Offset: 0x00614FAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.items.NullOrEmpty<Thing>())
				{
					this.items.RemoveAll((Thing x) => x == null);
				}
				if (!this.pawns.NullOrEmpty<Pawn>())
				{
					this.pawns.RemoveAll((Pawn x) => x == null);
				}
			}
		}

		// Token: 0x0400E105 RID: 57605
		public TransportShip transportShip;

		// Token: 0x0400E106 RID: 57606
		public List<Pawn> pawns;

		// Token: 0x0400E107 RID: 57607
		public List<Thing> items;

		// Token: 0x0400E108 RID: 57608
		public string inSignal;
	}
}
