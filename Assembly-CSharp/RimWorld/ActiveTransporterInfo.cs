using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B4A RID: 11082
	public class ActiveTransporterInfo : IThingHolder, IExposable
	{
		// Token: 0x170026E1 RID: 9953
		// (get) Token: 0x0600F7BB RID: 63419 RVA: 0x004806D4 File Offset: 0x0047E8D4
		// (set) Token: 0x0600F7BC RID: 63420 RVA: 0x00480709 File Offset: 0x0047E909
		public Thing SingleContainedThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				if (this.innerContainer.Count > 1)
				{
					Log.Error("ContainedThing used on a DropPodInfo holding > 1 thing.");
				}
				return this.innerContainer[0];
			}
			set
			{
				this.innerContainer.Clear();
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x170026E2 RID: 9954
		// (get) Token: 0x0600F7BD RID: 63421 RVA: 0x00480724 File Offset: 0x0047E924
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x0600F7BE RID: 63422 RVA: 0x0048072C File Offset: 0x0047E92C
		public ActiveTransporterInfo()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600F7BF RID: 63423 RVA: 0x0048075E File Offset: 0x0047E95E
		public ActiveTransporterInfo(IThingHolder parent)
		{
			this.innerContainer = new ThingOwner<Thing>(this);
			this.parent = parent;
		}

		// Token: 0x0600F7C0 RID: 63424 RVA: 0x00480798 File Offset: 0x0047E998
		public void ExposeData()
		{
			if (this.savePawnsWithReferenceMode && Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpThings.Clear();
				this.tmpThings.AddRange(this.innerContainer);
				this.tmpSavedPawns.Clear();
				for (int i = 0; i < this.tmpThings.Count; i++)
				{
					Pawn pawn = this.tmpThings[i] as Pawn;
					if (pawn != null)
					{
						this.innerContainer.Remove(pawn);
						this.tmpSavedPawns.Add(pawn);
					}
				}
				this.tmpThings.Clear();
			}
			Scribe_Values.Look<bool>(ref this.savePawnsWithReferenceMode, "savePawnsWithReferenceMode", false, false);
			if (this.savePawnsWithReferenceMode)
			{
				Scribe_Collections.Look<Pawn>(ref this.tmpSavedPawns, "tmpSavedPawns", LookMode.Reference, Array.Empty<object>());
			}
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.openDelay, "openDelay", 110, false);
			Scribe_Values.Look<bool>(ref this.leaveSlag, "leaveSlag", false, false);
			Scribe_Values.Look<WipeMode?>(ref this.spawnWipeMode, "spawnWipeMode", null, false);
			Scribe_Values.Look<bool>(ref this.despawnPodBeforeSpawningThing, "despawnPodBeforeSpawningThing", false, false);
			Scribe_Values.Look<Rot4?>(ref this.setRotation, "setRotation", null, false);
			Scribe_Values.Look<bool>(ref this.moveItemsAsideBeforeSpawning, "moveItemsAsideBeforeSpawning", false, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<ThingDef>(ref this.sentTransporterDef, "sentPodDef");
			Scribe_References.Look<Thing>(ref this.shuttle, "transportShip", false);
			if (this.savePawnsWithReferenceMode && (Scribe.mode == LoadSaveMode.PostLoadInit || Scribe.mode == LoadSaveMode.Saving))
			{
				for (int j = 0; j < this.tmpSavedPawns.Count; j++)
				{
					this.innerContainer.TryAdd(this.tmpSavedPawns[j], true);
				}
				this.tmpSavedPawns.Clear();
			}
		}

		// Token: 0x0600F7C1 RID: 63425 RVA: 0x00480980 File Offset: 0x0047EB80
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600F7C2 RID: 63426 RVA: 0x00480988 File Offset: 0x0047EB88
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600F7C3 RID: 63427 RVA: 0x00480996 File Offset: 0x0047EB96
		public void SetShuttle(Thing shuttle)
		{
			if (shuttle.Spawned)
			{
				shuttle.DeSpawn(DestroyMode.Vanish);
			}
			this.innerContainer.TryAddOrTransfer(shuttle, true);
			this.shuttle = shuttle;
		}

		// Token: 0x0600F7C4 RID: 63428 RVA: 0x004809BC File Offset: 0x0047EBBC
		public Thing GetShuttle()
		{
			return this.shuttle;
		}

		// Token: 0x0600F7C5 RID: 63429 RVA: 0x004809C4 File Offset: 0x0047EBC4
		public Thing RemoveShuttle()
		{
			this.innerContainer.Remove(this.shuttle);
			return this.shuttle;
		}

		// Token: 0x0400A2F1 RID: 41713
		public IThingHolder parent;

		// Token: 0x0400A2F2 RID: 41714
		public ThingOwner innerContainer;

		// Token: 0x0400A2F3 RID: 41715
		public int openDelay = 110;

		// Token: 0x0400A2F4 RID: 41716
		public bool leaveSlag;

		// Token: 0x0400A2F5 RID: 41717
		public bool savePawnsWithReferenceMode;

		// Token: 0x0400A2F6 RID: 41718
		public bool despawnPodBeforeSpawningThing;

		// Token: 0x0400A2F7 RID: 41719
		public WipeMode? spawnWipeMode;

		// Token: 0x0400A2F8 RID: 41720
		public Rot4? setRotation;

		// Token: 0x0400A2F9 RID: 41721
		public bool moveItemsAsideBeforeSpawning;

		// Token: 0x0400A2FA RID: 41722
		public ThingDef sentTransporterDef;

		// Token: 0x0400A2FB RID: 41723
		private Thing shuttle;

		// Token: 0x0400A2FC RID: 41724
		public List<string> questTags;

		// Token: 0x0400A2FD RID: 41725
		public const int DefaultOpenDelay = 110;

		// Token: 0x0400A2FE RID: 41726
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400A2FF RID: 41727
		private List<Pawn> tmpSavedPawns = new List<Pawn>();
	}
}
