using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B46 RID: 11078
	public class ActiveTransporter : Thing, IActiveTransporter, IThingHolder
	{
		// Token: 0x170026DE RID: 9950
		// (get) Token: 0x0600F7A6 RID: 63398 RVA: 0x0048007F File Offset: 0x0047E27F
		// (set) Token: 0x0600F7A7 RID: 63399 RVA: 0x00480087 File Offset: 0x0047E287
		public ActiveTransporterInfo Contents
		{
			get
			{
				return this.contents;
			}
			set
			{
				if (this.contents != null)
				{
					this.contents.parent = null;
				}
				if (value != null)
				{
					value.parent = this;
				}
				this.contents = value;
			}
		}

		// Token: 0x0600F7A8 RID: 63400 RVA: 0x004800AE File Offset: 0x0047E2AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Deep.Look<ActiveTransporterInfo>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x0600F7A9 RID: 63401 RVA: 0x00002C42 File Offset: 0x00000E42
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x0600F7AA RID: 63402 RVA: 0x004800E2 File Offset: 0x0047E2E2
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.contents != null)
			{
				outChildren.Add(this.contents);
			}
		}

		// Token: 0x0600F7AB RID: 63403 RVA: 0x00480104 File Offset: 0x0047E304
		protected override void Tick()
		{
			if (this.contents == null)
			{
				return;
			}
			if (!base.Spawned)
			{
				return;
			}
			this.age++;
			if (this.age > this.contents.openDelay)
			{
				this.PodOpen();
			}
		}

		// Token: 0x0600F7AC RID: 63404 RVA: 0x00480140 File Offset: 0x0047E340
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.contents != null)
			{
				this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			}
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				for (int i = 0; i < 1; i++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0600F7AD RID: 63405 RVA: 0x004801AC File Offset: 0x0047E3AC
		private void PodOpen()
		{
			Map map = base.Map;
			if (this.contents.despawnPodBeforeSpawningThing)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			for (int i = this.contents.innerContainer.Count - 1; i >= 0; i--)
			{
				Thing thing = this.contents.innerContainer[i];
				Rot4 rot = this.contents.setRotation ?? Rot4.North;
				if (this.contents.moveItemsAsideBeforeSpawning)
				{
					GenSpawn.CheckMoveItemsAside(base.Position, rot, thing.def, map);
				}
				Thing thing2;
				if (this.contents.spawnWipeMode == null)
				{
					GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, out thing2, delegate(Thing placedThing, int count)
					{
						if (Find.TickManager.TicksGame < 1200 && TutorSystem.TutorialMode && placedThing.def.category == ThingCategory.Item)
						{
							Find.TutorialState.AddStartingItem(placedThing);
						}
					}, null, new Rot4?(rot), 1);
				}
				else if (this.contents.setRotation != null)
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.setRotation.Value, this.contents.spawnWipeMode.Value, false, false);
				}
				else
				{
					thing2 = GenSpawn.Spawn(thing, base.Position, map, this.contents.spawnWipeMode.Value);
				}
				Pawn pawn = thing2 as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						TaleRecorder.RecordTale(TaleDefOf.LandedInPod, new object[]
						{
							pawn
						});
					}
					if (pawn.IsColonist && pawn.Spawned && !map.IsPlayerHome)
					{
						pawn.drafter.Drafted = true;
					}
					if (pawn.guest != null && pawn.guest.IsPrisoner)
					{
						pawn.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
				}
			}
			this.contents.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.contents.leaveSlag)
			{
				for (int j = 0; j < 1; j++)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), base.Position, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			if (this.def.soundOpen != null)
			{
				this.def.soundOpen.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0400A2ED RID: 41709
		public int age;

		// Token: 0x0400A2EE RID: 41710
		private ActiveTransporterInfo contents;
	}
}
