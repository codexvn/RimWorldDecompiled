using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A6B RID: 10859
	public class Bill_ProductionWithUft : Bill_Production
	{
		// Token: 0x170025C0 RID: 9664
		// (get) Token: 0x0600F1A0 RID: 61856 RVA: 0x00461964 File Offset: 0x0045FB64
		protected override string StatusString
		{
			get
			{
				if (this.BoundWorker == null)
				{
					return (base.StatusString ?? "").Trim();
				}
				return ("BoundWorkerIs".Translate(this.BoundWorker.LabelShort, this.BoundWorker) + base.StatusString).Trim();
			}
		}

		// Token: 0x170025C1 RID: 9665
		// (get) Token: 0x0600F1A1 RID: 61857 RVA: 0x004619CC File Offset: 0x0045FBCC
		public Pawn BoundWorker
		{
			get
			{
				if (this.boundUftInt == null)
				{
					return null;
				}
				Pawn creator = this.boundUftInt.Creator;
				if (creator == null || creator.Downed || creator.HostFaction != null || creator.Destroyed || !creator.Spawned)
				{
					this.boundUftInt = null;
					return null;
				}
				Thing thing = this.billStack.billGiver as Thing;
				if (thing != null)
				{
					WorkTypeDef workTypeDef = null;
					List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].fixedBillGiverDefs != null && allDefsListForReading[i].fixedBillGiverDefs.Contains(thing.def))
						{
							workTypeDef = allDefsListForReading[i].workType;
							break;
						}
					}
					if (workTypeDef != null && !creator.workSettings.WorkIsActive(workTypeDef))
					{
						this.boundUftInt = null;
						return null;
					}
				}
				return creator;
			}
		}

		// Token: 0x170025C2 RID: 9666
		// (get) Token: 0x0600F1A2 RID: 61858 RVA: 0x00461AA0 File Offset: 0x0045FCA0
		public UnfinishedThing BoundUft
		{
			get
			{
				return this.boundUftInt;
			}
		}

		// Token: 0x0600F1A3 RID: 61859 RVA: 0x00461AA8 File Offset: 0x0045FCA8
		public void SetBoundUft(UnfinishedThing value, bool setOtherLink = true)
		{
			if (value == this.boundUftInt)
			{
				return;
			}
			if (value != null && value.Destroyed)
			{
				Log.Error(string.Format("Tried to bound destroyed UnfinishedThing {0} to bill {1}.", value, this));
				return;
			}
			UnfinishedThing unfinishedThing = this.boundUftInt;
			this.boundUftInt = value;
			if (setOtherLink)
			{
				if (unfinishedThing != null && unfinishedThing.BoundBill == this)
				{
					unfinishedThing.BoundBill = null;
				}
				if (value != null && value.BoundBill != this)
				{
					this.boundUftInt.BoundBill = this;
				}
			}
		}

		// Token: 0x0600F1A4 RID: 61860 RVA: 0x00461B18 File Offset: 0x0045FD18
		public Bill_ProductionWithUft()
		{
		}

		// Token: 0x0600F1A5 RID: 61861 RVA: 0x00461B20 File Offset: 0x0045FD20
		public Bill_ProductionWithUft(RecipeDef recipe, Precept_ThingStyle precept = null) : base(recipe, precept)
		{
		}

		// Token: 0x0600F1A6 RID: 61862 RVA: 0x00461B2A File Offset: 0x0045FD2A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<UnfinishedThing>(ref this.boundUftInt, "boundUft", false);
		}

		// Token: 0x0600F1A7 RID: 61863 RVA: 0x00461B43 File Offset: 0x0045FD43
		public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
			this.ClearBoundUft();
			base.Notify_IterationCompleted(billDoer, ingredients);
		}

		// Token: 0x0600F1A8 RID: 61864 RVA: 0x00461B53 File Offset: 0x0045FD53
		public void ClearBoundUft()
		{
			this.boundUftInt = null;
		}

		// Token: 0x0600F1A9 RID: 61865 RVA: 0x00461B5C File Offset: 0x0045FD5C
		public override Bill Clone()
		{
			return (Bill_Production)base.Clone();
		}

		// Token: 0x04009F4A RID: 40778
		private UnfinishedThing boundUftInt;
	}
}
