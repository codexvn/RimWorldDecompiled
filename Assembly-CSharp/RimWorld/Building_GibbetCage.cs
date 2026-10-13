using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002A0D RID: 10765
	public class Building_GibbetCage : Building_CorpseCasket, IObservedThoughtGiver, INotifyHauledTo
	{
		// Token: 0x170024E8 RID: 9448
		// (get) Token: 0x0600EE30 RID: 60976 RVA: 0x00451EFB File Offset: 0x004500FB
		private float RandomCorpseRotation
		{
			get
			{
				return Rand.Range(-45f, 45f);
			}
		}

		// Token: 0x0600EE31 RID: 60977 RVA: 0x00451F0C File Offset: 0x0045010C
		public Building_GibbetCage()
		{
			if (this.corpseRotation == 0f)
			{
				this.corpseRotation = Rand.Range(-45f, 45f);
			}
		}

		// Token: 0x0600EE32 RID: 60978 RVA: 0x00451F36 File Offset: 0x00450136
		public override bool Accepts(Thing thing)
		{
			return base.Accepts(thing) && !base.HasCorpse && this.storageSettings.AllowedToAccept(thing);
		}

		// Token: 0x0600EE33 RID: 60979 RVA: 0x00451F60 File Offset: 0x00450160
		public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			Building_GibbetCage.tmpRadialPositions.Clear();
			this.corpseRotation = this.RandomCorpseRotation;
			if (base.Corpse.GetRotStage() != RotStage.Dessicated)
			{
				int num = GenRadial.NumCellsInRadius(1.5f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 item = base.Position + GenRadial.RadialPattern[i];
					Building_GibbetCage.tmpRadialPositions.Add(item);
				}
				int num2 = Mathf.Min(Building_GibbetCage.tmpRadialPositions.Count, Rand.Range(2, 4));
				for (int j = 0; j < num2; j++)
				{
					IntVec3 intVec = Building_GibbetCage.tmpRadialPositions.RandomElement<IntVec3>();
					Building_GibbetCage.tmpRadialPositions.Remove(intVec);
					FilthMaker.TryMakeFilth(intVec, base.Map, ThingDefOf.Filth_Blood, 1, FilthSourceFlags.None, true);
				}
			}
			if (this.def.building.gibbetCagePlaceCorpseEffecter != null)
			{
				this.def.building.gibbetCagePlaceCorpseEffecter.Spawn(this, base.Map, 1f);
			}
		}

		// Token: 0x0600EE34 RID: 60980 RVA: 0x00452057 File Offset: 0x00450257
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace && base.HasCorpse)
			{
				this.EjectContents();
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600EE35 RID: 60981 RVA: 0x00452072 File Offset: 0x00450272
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.corpseRotation, "corpseRotation", 0f, false);
		}

		// Token: 0x0600EE36 RID: 60982 RVA: 0x00452090 File Offset: 0x00450290
		public static Building_GibbetCage FindGibbetCageFor(Corpse c, Pawn traveler, bool ignoreOtherReservations = false)
		{
			if (Building_GibbetCage.cachedCages == null)
			{
				Building_GibbetCage.cachedCages = (from def in DefDatabase<ThingDef>.AllDefs
				where def.IsGibbetCage
				select def).ToList<ThingDef>();
			}
			Predicate<Thing> <>9__1;
			foreach (ThingDef singleDef in Building_GibbetCage.cachedCages)
			{
				IntVec3 positionHeld = c.PositionHeld;
				Map mapHeld = c.MapHeld;
				ThingRequest thingReq = ThingRequest.ForDef(singleDef);
				PathEndMode peMode = PathEndMode.InteractionCell;
				TraverseParms traverseParams = TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true);
				float maxDistance = 9999f;
				Predicate<Thing> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((Thing x) => !((Building_GibbetCage)x).HasCorpse && ((Building_GibbetCage)x).Accepts(c) && traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations)));
				}
				Building_GibbetCage building_GibbetCage = (Building_GibbetCage)GenClosest.ClosestThingReachable(positionHeld, mapHeld, thingReq, peMode, traverseParams, maxDistance, validator, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (building_GibbetCage != null)
				{
					return building_GibbetCage;
				}
			}
			return null;
		}

		// Token: 0x0600EE37 RID: 60983 RVA: 0x004521A4 File Offset: 0x004503A4
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if (observer.Ideo != null && observer.Ideo.IdeoApprovesOfSlavery())
			{
				return null;
			}
			Thought_MemoryObservation thought_MemoryObservation = (Thought_MemoryObservation)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedGibbetCage);
			thought_MemoryObservation.Target = this;
			return thought_MemoryObservation;
		}

		// Token: 0x0600EE38 RID: 60984 RVA: 0x00002C42 File Offset: 0x00000E42
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}

		// Token: 0x0600EE39 RID: 60985 RVA: 0x004521D4 File Offset: 0x004503D4
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			if (base.HasCorpse)
			{
				Vector3 drawLoc2 = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop) + this.def.building.gibbetCorposeDrawOffset;
				base.Corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(this.corpseRotation);
				base.Corpse.DynamicDrawPhaseAt(phase, drawLoc2, false);
			}
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		}

		// Token: 0x0600EE3A RID: 60986 RVA: 0x0045224C File Offset: 0x0045044C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.cageTopGraphic == null)
			{
				this.cageTopGraphic = this.def.building.gibbetCageTopGraphicData.GraphicColoredFor(this);
			}
			this.cageTopGraphic.Draw(base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Item), Rot4.North, this, 0f);
		}

		// Token: 0x0600EE3B RID: 60987 RVA: 0x004522AB File Offset: 0x004504AB
		public override void Notify_ColorChanged()
		{
			this.cageTopGraphic = null;
			base.Notify_ColorChanged();
		}

		// Token: 0x04009D90 RID: 40336
		private static List<ThingDef> cachedCages;

		// Token: 0x04009D91 RID: 40337
		[Unsaved(false)]
		private Graphic cageTopGraphic;

		// Token: 0x04009D92 RID: 40338
		private float corpseRotation;

		// Token: 0x04009D93 RID: 40339
		private static readonly List<IntVec3> tmpRadialPositions = new List<IntVec3>();
	}
}
