using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A74 RID: 10868
	[StaticConstructorOnStartup]
	public class Building_WorkTableAutonomous : Building_WorkTable, IThingHolder, INotifyHauledTo
	{
		// Token: 0x170025D4 RID: 9684
		// (get) Token: 0x0600F1EF RID: 61935 RVA: 0x00462CC1 File Offset: 0x00460EC1
		// (set) Token: 0x0600F1F0 RID: 61936 RVA: 0x00462CC9 File Offset: 0x00460EC9
		public Bill_Autonomous ActiveBill
		{
			get
			{
				return this.activeBill;
			}
			set
			{
				if (this.activeBill != value)
				{
					this.activeBill = value;
				}
			}
		}

		// Token: 0x170025D5 RID: 9685
		// (get) Token: 0x0600F1F1 RID: 61937 RVA: 0x00462CDC File Offset: 0x00460EDC
		public float CurrentBillFormingPercent
		{
			get
			{
				if (this.activeBill == null || this.activeBill.State != FormingState.Forming)
				{
					return 0f;
				}
				return 1f - this.activeBill.formingTicks / (float)this.activeBill.recipe.formingTicks;
			}
		}

		// Token: 0x170025D6 RID: 9686
		// (get) Token: 0x0600F1F2 RID: 61938 RVA: 0x00458C04 File Offset: 0x00456E04
		public GenDraw.FillableBarRequest BarDrawData
		{
			get
			{
				return this.def.building.BarDrawDataFor(base.Rotation);
			}
		}

		// Token: 0x0600F1F3 RID: 61939 RVA: 0x00462D29 File Offset: 0x00460F29
		public Building_WorkTableAutonomous()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600F1F4 RID: 61940 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_StartForming(Pawn billDoer)
		{
		}

		// Token: 0x0600F1F5 RID: 61941 RVA: 0x00462D40 File Offset: 0x00460F40
		public virtual void Notify_FormingCompleted()
		{
			Thing thing = this.activeBill.CreateProducts();
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (thing != null)
			{
				this.innerContainer.TryAdd(thing, true);
			}
		}

		// Token: 0x0600F1F6 RID: 61942 RVA: 0x00462D76 File Offset: 0x00460F76
		public override void Notify_BillDeleted(Bill bill)
		{
			if (this.activeBill == bill)
			{
				this.EjectContents();
				this.activeBill = null;
			}
		}

		// Token: 0x0600F1F7 RID: 61943 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
		}

		// Token: 0x0600F1F8 RID: 61944 RVA: 0x00462D8E File Offset: 0x00460F8E
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Bill_Autonomous bill_Autonomous = this.activeBill;
			if (bill_Autonomous != null)
			{
				bill_Autonomous.Reset();
			}
			this.EjectContents();
			base.Destroy(mode);
		}

		// Token: 0x0600F1F9 RID: 61945 RVA: 0x00462DAE File Offset: 0x00460FAE
		public virtual void EjectContents()
		{
			this.innerContainer.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, true);
			Bill_Autonomous bill_Autonomous = this.activeBill;
			if (bill_Autonomous == null)
			{
				return;
			}
			bill_Autonomous.Reset();
		}

		// Token: 0x0600F1FA RID: 61946 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanWork()
		{
			return true;
		}

		// Token: 0x0600F1FB RID: 61947 RVA: 0x00462DDC File Offset: 0x00460FDC
		protected override void Tick()
		{
			base.Tick();
			if (this.activeBill != null && this.CanWork())
			{
				this.activeBill.BillTick();
			}
		}

		// Token: 0x0600F1FC RID: 61948 RVA: 0x00462DFF File Offset: 0x00460FFF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_References.Look<Bill_Autonomous>(ref this.activeBill, "activeBill", false);
		}

		// Token: 0x0600F1FD RID: 61949 RVA: 0x00462E34 File Offset: 0x00461034
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			string inspectStringExtra = this.GetInspectStringExtra();
			if (!inspectStringExtra.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectStringExtra);
			}
			if (this.CanWork() && this.activeBill != null)
			{
				this.activeBill.AppendInspectionData(stringBuilder);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600F1FE RID: 61950 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual string GetInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600F1FF RID: 61951 RVA: 0x00462E9C File Offset: 0x0046109C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				Bill_Autonomous bill_Autonomous = this.ActiveBill;
				if (bill_Autonomous != null && bill_Autonomous.State == FormingState.Forming)
				{
					yield return new Command_Action
					{
						action = delegate()
						{
							this.ActiveBill.formingTicks -= (float)this.ActiveBill.recipe.formingTicks * 0.25f;
						},
						defaultLabel = "DEV: Forming cycle +25%"
					};
					yield return new Command_Action
					{
						action = delegate()
						{
							this.ActiveBill.formingTicks = 0f;
						},
						defaultLabel = "DEV: Complete cycle"
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F200 RID: 61952 RVA: 0x00462EAC File Offset: 0x004610AC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600F201 RID: 61953 RVA: 0x00462EB4 File Offset: 0x004610B4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x04009F69 RID: 40809
		public ThingOwner innerContainer;

		// Token: 0x04009F6A RID: 40810
		protected Bill_Autonomous activeBill;
	}
}
