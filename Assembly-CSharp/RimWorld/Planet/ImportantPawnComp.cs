using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E2E RID: 15918
	public abstract class ImportantPawnComp : WorldObjectComp, ISuspendableThingHolder, IThingHolder
	{
		// Token: 0x1700364D RID: 13901
		// (get) Token: 0x06015BEC RID: 89068 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700364E RID: 13902
		// (get) Token: 0x06015BED RID: 89069
		protected abstract string PawnSaveKey { get; }

		// Token: 0x06015BEE RID: 89070 RVA: 0x00680793 File Offset: 0x0067E993
		public ImportantPawnComp()
		{
			this.pawn = new ThingOwner<Pawn>(this, true, LookMode.Deep, true);
		}

		// Token: 0x06015BEF RID: 89071 RVA: 0x006807AA File Offset: 0x0067E9AA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawn, this.PawnSaveKey, new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06015BF0 RID: 89072 RVA: 0x006807D3 File Offset: 0x0067E9D3
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06015BF1 RID: 89073 RVA: 0x006807E1 File Offset: 0x0067E9E1
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawn;
		}

		// Token: 0x06015BF2 RID: 89074 RVA: 0x006807EC File Offset: 0x0067E9EC
		public override void CompTickInterval(int delta)
		{
			if (this.pawn.Any && !base.ParentHasMap)
			{
				if (!this.pawn.Any || this.pawn[0].Destroyed)
				{
					this.parent.Destroy();
					return;
				}
				Pawn pawn = this.pawn[0];
				if (pawn.needs.food != null)
				{
					pawn.needs.food.CurLevelPercentage = 0.8f;
				}
			}
		}

		// Token: 0x06015BF3 RID: 89075 RVA: 0x00680869 File Offset: 0x0067EA69
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.RemovePawnOnWorldObjectRemoved();
		}

		// Token: 0x06015BF4 RID: 89076
		protected abstract void RemovePawnOnWorldObjectRemoved();

		// Token: 0x0400F14F RID: 61775
		public ThingOwner<Pawn> pawn;

		// Token: 0x0400F150 RID: 61776
		private const float AutoFoodLevel = 0.8f;
	}
}
