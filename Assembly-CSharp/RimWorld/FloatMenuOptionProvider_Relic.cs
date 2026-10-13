using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003325 RID: 13093
	public class FloatMenuOptionProvider_Relic : FloatMenuOptionProvider
	{
		// Token: 0x17002FE1 RID: 12257
		// (get) Token: 0x0601268C RID: 75404 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FE2 RID: 12258
		// (get) Token: 0x0601268D RID: 75405 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FE3 RID: 12259
		// (get) Token: 0x0601268E RID: 75406 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FE4 RID: 12260
		// (get) Token: 0x0601268F RID: 75407 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012690 RID: 75408 RVA: 0x00565319 File Offset: 0x00563519
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x06012691 RID: 75409 RVA: 0x0056D26D File Offset: 0x0056B46D
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			CompRelicContainer container;
			if (clickedThing.TryGetComp(out container))
			{
				FloatMenuOption floatMenuOption;
				if (container.Full)
				{
					yield return this.ExtractRelic(container, context.FirstSelectedPawn);
					if (!context.FirstSelectedPawn.Map.IsPlayerHome && !context.FirstSelectedPawn.IsFormingCaravan())
					{
						yield return this.ExtractToInventory(container, context.FirstSelectedPawn);
					}
				}
				else if (this.InstallRelicFromContainer(container.parent, context.FirstSelectedPawn, out floatMenuOption))
				{
					yield return floatMenuOption;
				}
			}
			if (CompRelicContainer.IsRelic(clickedThing) && clickedThing.Spawned)
			{
				yield return this.InstallRelic(clickedThing, context.FirstSelectedPawn);
			}
			yield break;
		}

		// Token: 0x06012692 RID: 75410 RVA: 0x0056D28C File Offset: 0x0056B48C
		private FloatMenuOption InstallRelic(Thing containerThing, Pawn pawn)
		{
			IEnumerable<Thing> searchSet = from x in containerThing.Map.listerThings.ThingsOfDef(ThingDefOf.Reliquary)
			where x.TryGetComp<CompRelicContainer>().ContainedThing == null
			select x;
			Thing thing = GenClosest.ClosestThing_Global_Reachable(containerThing.Position, containerThing.Map, searchSet, PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => pawn.CanReserve(t, 1, -1, null, false), null, false);
			if (thing == null)
			{
				return new FloatMenuOption("InstallInReliquary".Translate() + " (" + "NoEmptyReliquary".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, containerThing, thing, thing.InteractionCell);
			job.count = 1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallInReliquary".Translate(), delegate()
			{
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(containerThing), "ReservedBy", null);
		}

		// Token: 0x06012693 RID: 75411 RVA: 0x0056D3D8 File Offset: 0x0056B5D8
		private FloatMenuOption ExtractRelic(CompRelicContainer container, Pawn pawn)
		{
			TaggedString taggedString = "ExtractRelic".Translate(container.ContainedThing.Label);
			IntVec3 c;
			IHaulDestination haulDestination;
			if (!StoreUtility.TryFindBestBetterStorageFor(container.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out c, out haulDestination, true))
			{
				return new FloatMenuOption(taggedString + " (" + HaulAIUtility.NoEmptyPlaceLowerTrans + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Job job = JobMaker.MakeJob(JobDefOf.ExtractRelic, container.parent, container.ContainedThing, c);
			job.count = 1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(container.parent), "ReservedBy", null);
		}

		// Token: 0x06012694 RID: 75412 RVA: 0x0056D4EC File Offset: 0x0056B6EC
		private bool InstallRelicFromContainer(Thing containerThing, Pawn pawn, out FloatMenuOption option)
		{
			option = null;
			IEnumerable<Thing> enumerable = from x in pawn.Map.listerThings.AllThings
			where CompRelicContainer.IsRelic(x) && pawn.CanReach(x, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn)
			select x;
			if (!enumerable.Any<Thing>())
			{
				option = new FloatMenuOption("NoRelicToInstall".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			using (IEnumerator<Thing> enumerator = enumerable.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Thing thing = enumerator.Current;
					Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, thing, containerThing, containerThing.InteractionCell);
					job.count = 1;
					option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallRelic".Translate(thing.Label), delegate()
					{
						pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(containerThing), "ReservedBy", null);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06012695 RID: 75413 RVA: 0x0056D63C File Offset: 0x0056B83C
		private FloatMenuOption ExtractToInventory(CompRelicContainer container, Pawn pawn)
		{
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ExtractRelicToInventory".Translate(container.ContainedThing.Label, 300.ToStringTicksToPeriod(true, false, true, true, false)), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.ExtractToInventory, container.parent, container.ContainedThing, container.parent.InteractionCell);
				job.count = 1;
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, new LocalTargetInfo(container.parent), "ReservedBy", null);
		}
	}
}
