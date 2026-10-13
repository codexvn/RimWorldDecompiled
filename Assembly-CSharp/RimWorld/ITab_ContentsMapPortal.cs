using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02003396 RID: 13206
	public class ITab_ContentsMapPortal : ITab_ContentsBase
	{
		// Token: 0x17003086 RID: 12422
		// (get) Token: 0x060128FD RID: 76029 RVA: 0x0057AC85 File Offset: 0x00578E85
		public override IList<Thing> container
		{
			get
			{
				return this.tmpContainer;
			}
		}

		// Token: 0x17003087 RID: 12423
		// (get) Token: 0x060128FE RID: 76030 RVA: 0x00002501 File Offset: 0x00000701
		public override bool UseDiscardMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003088 RID: 12424
		// (get) Token: 0x060128FF RID: 76031 RVA: 0x0057AC8D File Offset: 0x00578E8D
		public MapPortal Portal
		{
			get
			{
				return base.SelThing as MapPortal;
			}
		}

		// Token: 0x17003089 RID: 12425
		// (get) Token: 0x06012900 RID: 76032 RVA: 0x0057AC9A File Offset: 0x00578E9A
		public override bool IsVisible
		{
			get
			{
				return this.Portal != null && this.Portal.LoadInProgress;
			}
		}

		// Token: 0x1700308A RID: 12426
		// (get) Token: 0x06012901 RID: 76033 RVA: 0x00003D0D File Offset: 0x00001F0D
		public override IntVec3 DropOffset
		{
			get
			{
				return IntVec3.Zero;
			}
		}

		// Token: 0x06012902 RID: 76034 RVA: 0x0057ACB1 File Offset: 0x00578EB1
		public ITab_ContentsMapPortal()
		{
			this.labelKey = "TabMapPortalContents";
			this.containedItemsKey = "";
		}

		// Token: 0x06012903 RID: 76035 RVA: 0x0057ACDC File Offset: 0x00578EDC
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			MapPortal portal = this.Portal;
			Text.Font = GameFont.Small;
			bool flag = false;
			float num = 0f;
			Widgets.BeginGroup(inRect);
			Rect viewRect = inRect;
			viewRect.y = 0f;
			viewRect.height = this.lastHeight;
			if (this.lastHeight > inRect.height)
			{
				viewRect.width -= 16f;
			}
			Widgets.BeginScrollView(inRect, ref this.scroll, viewRect, true);
			Widgets.ListSeparator(ref num, inRect.width, this.Portal.EnteringString);
			if (portal.leftToLoad != null)
			{
				for (int i = 0; i < portal.leftToLoad.Count; i++)
				{
					TransferableOneWay t = portal.leftToLoad[i];
					if (t.CountToTransfer > 0 && t.HasAnyThing)
					{
						flag = true;
						base.DoThingRow(t.ThingDef, t.CountToTransfer, t.things, viewRect.width, ref num, delegate(int x)
						{
							this.OnDropToLoadThing(t, x);
						});
					}
				}
			}
			this.lastHeight = num;
			Widgets.EndScrollView();
			if (!flag)
			{
				Widgets.NoneLabel(ref num, inRect.width, null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012904 RID: 76036 RVA: 0x0057AE38 File Offset: 0x00579038
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
		}

		// Token: 0x06012905 RID: 76037 RVA: 0x0057AE60 File Offset: 0x00579060
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x06012906 RID: 76038 RVA: 0x0057AE98 File Offset: 0x00579098
		private void OnDropToLoadThing(TransferableOneWay t, int count)
		{
			t.ForceTo(t.CountToTransfer - count);
			this.EndJobForEveryoneHauling(t);
			foreach (Thing thing in t.things)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					this.RemovePawnFromLoadLord(pawn);
				}
			}
		}

		// Token: 0x06012907 RID: 76039 RVA: 0x0057AF08 File Offset: 0x00579108
		private void EndJobForEveryoneHauling(TransferableOneWay t)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = base.SelThing.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
				{
					JobDriver_HaulToPortal jobDriver_HaulToPortal = (JobDriver_HaulToPortal)allPawnsSpawned[i].jobs.curDriver;
					if (jobDriver_HaulToPortal.MapPortal == this.Portal && jobDriver_HaulToPortal.ThingToCarry != null && jobDriver_HaulToPortal.ThingToCarry.def == t.ThingDef)
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x0400C0D4 RID: 49364
		private List<Thing> tmpContainer = new List<Thing>();

		// Token: 0x0400C0D5 RID: 49365
		private Vector2 scroll;

		// Token: 0x0400C0D6 RID: 49366
		private float lastHeight;
	}
}
