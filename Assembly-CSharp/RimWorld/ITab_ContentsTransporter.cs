using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02003399 RID: 13209
	public class ITab_ContentsTransporter : ITab_ContentsBase
	{
		// Token: 0x1700308F RID: 12431
		// (get) Token: 0x06012914 RID: 76052 RVA: 0x0057B24E File Offset: 0x0057944E
		public override IList<Thing> container
		{
			get
			{
				return this.Transporter.innerContainer;
			}
		}

		// Token: 0x17003090 RID: 12432
		// (get) Token: 0x06012915 RID: 76053 RVA: 0x00002501 File Offset: 0x00000701
		public override bool UseDiscardMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003091 RID: 12433
		// (get) Token: 0x06012916 RID: 76054 RVA: 0x0057B25B File Offset: 0x0057945B
		public CompTransporter Transporter
		{
			get
			{
				return base.SelThing.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x17003092 RID: 12434
		// (get) Token: 0x06012917 RID: 76055 RVA: 0x0057B268 File Offset: 0x00579468
		public override bool IsVisible
		{
			get
			{
				return (base.SelThing.Faction == null || base.SelThing.Faction == Faction.OfPlayer) && this.Transporter != null && (this.Transporter.LoadingInProgressOrReadyToLaunch || this.Transporter.innerContainer.Any);
			}
		}

		// Token: 0x17003093 RID: 12435
		// (get) Token: 0x06012918 RID: 76056 RVA: 0x0057B2BD File Offset: 0x005794BD
		public override IntVec3 DropOffset
		{
			get
			{
				if (this.Transporter.Shuttle != null)
				{
					return base.SelThing.def.interactionCellOffset.RotatedBy(base.SelThing.Rotation);
				}
				return base.DropOffset;
			}
		}

		// Token: 0x06012919 RID: 76057 RVA: 0x0057B2F3 File Offset: 0x005794F3
		public ITab_ContentsTransporter()
		{
			this.labelKey = "TabTransporterContents";
			this.containedItemsKey = "ContainedItems";
		}

		// Token: 0x0601291A RID: 76058 RVA: 0x0057B314 File Offset: 0x00579514
		protected override void DoItemsLists(Rect inRect, ref float curY)
		{
			CompTransporter transporter = this.Transporter;
			Rect rect = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			Text.Font = GameFont.Small;
			bool flag = false;
			float a = 0f;
			Widgets.BeginGroup(rect);
			Widgets.ListSeparator(ref a, rect.width, "ItemsToLoad".Translate());
			if (transporter.leftToLoad != null)
			{
				for (int i = 0; i < transporter.leftToLoad.Count; i++)
				{
					TransferableOneWay t = transporter.leftToLoad[i];
					if (t.CountToTransfer > 0 && t.HasAnyThing)
					{
						flag = true;
						base.DoThingRow(t.ThingDef, t.CountToTransfer, t.things, rect.width, ref a, delegate(int x)
						{
							this.OnDropToLoadThing(t, x);
						});
					}
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, rect.width, null);
			}
			Widgets.EndGroup();
			Rect inRect2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			base.DoItemsLists(inRect2, ref b);
			curY += Mathf.Max(a, b);
		}

		// Token: 0x0601291B RID: 76059 RVA: 0x0057B49C File Offset: 0x0057969C
		protected override void OnDropThing(Thing t, int count)
		{
			base.OnDropThing(t, count);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				this.RemovePawnFromLoadLord(pawn);
			}
			this.Transporter.Notify_ThingRemoved(t);
		}

		// Token: 0x0601291C RID: 76060 RVA: 0x0057B4D0 File Offset: 0x005796D0
		private void RemovePawnFromLoadLord(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			if (lord != null && lord.LordJob is LordJob_LoadAndEnterTransporters)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
			}
		}

		// Token: 0x0601291D RID: 76061 RVA: 0x0057B508 File Offset: 0x00579708
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

		// Token: 0x0601291E RID: 76062 RVA: 0x0057B578 File Offset: 0x00579778
		private void EndJobForEveryoneHauling(TransferableOneWay t)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = base.SelThing.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToTransporter)
				{
					JobDriver_HaulToTransporter jobDriver_HaulToTransporter = (JobDriver_HaulToTransporter)allPawnsSpawned[i].jobs.curDriver;
					if (jobDriver_HaulToTransporter.Transporter == this.Transporter && jobDriver_HaulToTransporter.ThingToCarry != null && jobDriver_HaulToTransporter.ThingToCarry.def == t.ThingDef)
					{
						allPawnsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}
	}
}
