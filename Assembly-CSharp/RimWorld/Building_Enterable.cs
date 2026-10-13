using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020029FA RID: 10746
	public abstract class Building_Enterable : Building, IThingHolder, ISuspendableThingHolder, ISearchableContents
	{
		// Token: 0x170024B3 RID: 9395
		// (get) Token: 0x0600ED64 RID: 60772 RVA: 0x0044E3C3 File Offset: 0x0044C5C3
		public bool Working
		{
			get
			{
				return this.startTick >= 0;
			}
		}

		// Token: 0x170024B4 RID: 9396
		// (get) Token: 0x0600ED65 RID: 60773 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170024B5 RID: 9397
		// (get) Token: 0x0600ED66 RID: 60774 RVA: 0x0044E3D1 File Offset: 0x0044C5D1
		public bool AnyAcceptablePawns
		{
			get
			{
				return base.Spawned && base.Map.mapPawns.AllPawnsSpawned.Any((Pawn x) => this.CanAcceptPawn(x));
			}
		}

		// Token: 0x170024B6 RID: 9398
		// (get) Token: 0x0600ED67 RID: 60775 RVA: 0x0044E3FE File Offset: 0x0044C5FE
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x170024B7 RID: 9399
		// (get) Token: 0x0600ED68 RID: 60776 RVA: 0x0044E406 File Offset: 0x0044C606
		// (set) Token: 0x0600ED69 RID: 60777 RVA: 0x0044E40E File Offset: 0x0044C60E
		public Pawn SelectedPawn
		{
			get
			{
				return this.selectedPawn;
			}
			set
			{
				this.selectedPawn = value;
			}
		}

		// Token: 0x0600ED6A RID: 60778 RVA: 0x0044E417 File Offset: 0x0044C617
		public Building_Enterable()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600ED6B RID: 60779
		public abstract AcceptanceReport CanAcceptPawn(Pawn p);

		// Token: 0x0600ED6C RID: 60780
		public abstract void TryAcceptPawn(Pawn p);

		// Token: 0x170024B8 RID: 9400
		// (get) Token: 0x0600ED6D RID: 60781
		public abstract Vector3 PawnDrawOffset { get; }

		// Token: 0x0600ED6E RID: 60782 RVA: 0x0044E432 File Offset: 0x0044C632
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600ED6F RID: 60783 RVA: 0x0044E45B File Offset: 0x0044C65B
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600ED70 RID: 60784 RVA: 0x0044E3FE File Offset: 0x0044C5FE
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600ED71 RID: 60785 RVA: 0x0044E469 File Offset: 0x0044C669
		protected virtual void SelectPawn(Pawn pawn)
		{
			this.selectedPawn = pawn;
			if (pawn.IsPrisonerOfColony)
			{
				return;
			}
			if (pawn.Downed)
			{
				return;
			}
			pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, this), new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x0600ED72 RID: 60786 RVA: 0x0044E4A8 File Offset: 0x0044C6A8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (!this.Working && this.selectedPawn != null && this.selectedPawn.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), this.selectedPawn.TrueCenter());
			}
		}

		// Token: 0x0600ED73 RID: 60787 RVA: 0x0044E4F4 File Offset: 0x0044C6F4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Thing item in ((IEnumerable<Thing>)this.innerContainer))
			{
				Gizmo gizmo2 = Building.SelectContainedItemGizmo(this, item);
				if (gizmo2 != null)
				{
					yield return gizmo2;
				}
			}
			IEnumerator<Thing> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600ED74 RID: 60788 RVA: 0x0044E504 File Offset: 0x0044C704
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_References.Look<Pawn>(ref this.selectedPawn, "selectedPawn", false);
		}

		// Token: 0x04009D28 RID: 40232
		public ThingOwner innerContainer;

		// Token: 0x04009D29 RID: 40233
		protected Pawn selectedPawn;

		// Token: 0x04009D2A RID: 40234
		protected int startTick = -1;
	}
}
