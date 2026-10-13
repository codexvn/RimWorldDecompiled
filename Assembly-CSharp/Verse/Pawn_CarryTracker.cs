using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020007CF RID: 1999
	public class Pawn_CarryTracker : IThingHolder, IExposable
	{
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060032B9 RID: 12985 RVA: 0x00109598 File Offset: 0x00107798
		public Thing CarriedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060032BA RID: 12986 RVA: 0x001095B5 File Offset: 0x001077B5
		public bool Full
		{
			get
			{
				return this.AvailableStackSpace(this.CarriedThing.def) <= 0;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060032BB RID: 12987 RVA: 0x001095CE File Offset: 0x001077CE
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060032BC RID: 12988 RVA: 0x001095D6 File Offset: 0x001077D6
		public Pawn_CarryTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep, true);
		}

		// Token: 0x060032BD RID: 12989 RVA: 0x001095F4 File Offset: 0x001077F4
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<Thing>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x060032BE RID: 12990 RVA: 0x00109610 File Offset: 0x00107810
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060032BF RID: 12991 RVA: 0x00109618 File Offset: 0x00107818
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x00109628 File Offset: 0x00107828
		public int AvailableStackSpace(ThingDef td)
		{
			int num = this.MaxStackSpaceEver(td);
			if (this.CarriedThing != null)
			{
				num -= this.CarriedThing.stackCount;
			}
			return num;
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x00109654 File Offset: 0x00107854
		public int MaxStackSpaceEver(ThingDef td)
		{
			int b = Mathf.RoundToInt(this.pawn.GetStatValue(StatDefOf.CarryingCapacity, true, -1) / td.VolumePerUnit);
			return Mathf.Min(td.stackLimit, b);
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x0010968C File Offset: 0x0010788C
		public bool TryStartCarry(Thing item)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				string str = "Dead/downed/deathresting pawn ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " tried to start carry item.");
				return false;
			}
			if (this.innerContainer.TryAdd(item, true))
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				return true;
			}
			return false;
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x0010971C File Offset: 0x0010791C
		public int TryStartCarry(Thing item, int count, bool reserve = true)
		{
			if (this.pawn.Dead || this.pawn.Downed)
			{
				string str = "Dead/downed/deathresting pawn ";
				Pawn pawn = this.pawn;
				Log.Error(str + ((pawn != null) ? pawn.ToString() : null) + " tried to start carry " + item.ToStringSafe<Thing>());
				return 0;
			}
			count = Mathf.Min(count, this.AvailableStackSpace(item.def));
			count = Mathf.Min(count, item.stackCount);
			bool flag = Find.Selector.IsSelected(item);
			Thing thing = item.SplitOff(count);
			int num = this.innerContainer.TryAdd(thing, count, true);
			if (num > 0 && thing != item)
			{
				this.TryUpdateTransferables(thing);
			}
			if (num > 0)
			{
				item.def.soundPickup.PlayOneShot(new TargetInfo(item.Position, this.pawn.Map, false));
				if (reserve)
				{
					this.pawn.Reserve(this.CarriedThing, this.pawn.CurJob, 1, -1, null, true, false);
				}
				if (flag)
				{
					if (!thing.Destroyed)
					{
						Find.Selector.Select(thing, true, true);
					}
					Find.Selector.Select(this.CarriedThing, true, true);
				}
				this.pawn.MapHeld.resourceCounter.UpdateResourceCounts();
			}
			return num;
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00109860 File Offset: 0x00107A60
		private void TryUpdateTransferables(Thing splitStack)
		{
			if (splitStack == null)
			{
				return;
			}
			Pawn_JobTracker jobs = this.pawn.jobs;
			JobDriver_HaulToTransporter jobDriver_HaulToTransporter = ((jobs != null) ? jobs.curDriver : null) as JobDriver_HaulToTransporter;
			if (jobDriver_HaulToTransporter != null)
			{
				CompTransporter transporter = jobDriver_HaulToTransporter.Transporter;
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(splitStack, (transporter != null) ? transporter.leftToLoad : null, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null && !transferableOneWay.things.Contains(splitStack) && transferableOneWay.MaxCount + splitStack.stackCount <= transferableOneWay.CountToTransfer)
				{
					transferableOneWay.things.Add(splitStack);
				}
			}
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x001098E0 File Offset: 0x00107AE0
		public bool TryDropCarriedThing(IntVec3 dropLoc, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				this.pawn.MapHeld.resourceCounter.UpdateResourceCounts();
				return true;
			}
			return false;
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x00109950 File Offset: 0x00107B50
		public bool TryDropCarriedThing(IntVec3 dropLoc, int count, ThingPlaceMode mode, out Thing resultingThing, Action<Thing, int> placedAction = null)
		{
			if (this.innerContainer.TryDrop(this.CarriedThing, dropLoc, this.pawn.MapHeld, mode, count, out resultingThing, placedAction, null))
			{
				if (resultingThing != null && this.pawn.Faction.HostileTo(Faction.OfPlayer))
				{
					resultingThing.SetForbidden(true, false);
				}
				this.pawn.MapHeld.resourceCounter.UpdateResourceCounts();
				return true;
			}
			return false;
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x001099C4 File Offset: 0x00107BC4
		public int CarriedCount(ThingDef def)
		{
			int num = 0;
			foreach (Thing thing in this.innerContainer)
			{
				if (thing.def == def)
				{
					num += thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x060032C8 RID: 13000 RVA: 0x00109A28 File Offset: 0x00107C28
		public void DestroyCarriedThing()
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x00109A38 File Offset: 0x00107C38
		public void CarryHandsTickInterval(int delta)
		{
			Pawn pawn = this.CarriedThing as Pawn;
			if (pawn != null && pawn.DevelopmentalStage.Baby())
			{
				Pawn_IdeoTracker ideo = pawn.ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.IncreaseIdeoExposureIfBabyTick(this.pawn.Ideo, delta);
			}
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x00109A7D File Offset: 0x00107C7D
		public IEnumerable<Gizmo> GetGizmos()
		{
			Gizmo gizmo = ContainingSelectionUtility.SelectCarriedThingGizmo(this.pawn, this.CarriedThing);
			if (gizmo != null)
			{
				yield return gizmo;
			}
			if (this.pawn.Drafted && this.CarriedThing is Pawn)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandDropPawn".Translate(this.CarriedThing),
					defaultDesc = "CommandDropPawnDesc".Translate(),
					action = delegate()
					{
						Thing thing;
						this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
					},
					icon = TexCommand.DropCarriedPawn
				};
			}
			if (ModsConfig.BiotechActive && DebugSettings.ShowDevGizmos)
			{
				CompDissolution compDissolution = this.CarriedThing.TryGetComp<CompDissolution>();
				if (compDissolution != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Dissolution event",
						action = delegate()
						{
							compDissolution.TriggerDissolutionEvent(1);
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x0400268F RID: 9871
		public Pawn pawn;

		// Token: 0x04002690 RID: 9872
		public ThingOwner<Thing> innerContainer;
	}
}
