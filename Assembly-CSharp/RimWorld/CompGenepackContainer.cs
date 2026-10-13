using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CB1 RID: 11441
	public class CompGenepackContainer : ThingComp, IThingHolder, ISearchableContents
	{
		// Token: 0x17002897 RID: 10391
		// (get) Token: 0x060100EC RID: 65772 RVA: 0x004A5CEB File Offset: 0x004A3EEB
		public CompProperties_GenepackContainer Props
		{
			get
			{
				return (CompProperties_GenepackContainer)this.props;
			}
		}

		// Token: 0x17002898 RID: 10392
		// (get) Token: 0x060100ED RID: 65773 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002899 RID: 10393
		// (get) Token: 0x060100EE RID: 65774 RVA: 0x00494A7B File Offset: 0x00492C7B
		public bool PowerOn
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>().PowerOn;
			}
		}

		// Token: 0x1700289A RID: 10394
		// (get) Token: 0x060100EF RID: 65775 RVA: 0x004A5CF8 File Offset: 0x004A3EF8
		public bool Full
		{
			get
			{
				return this.ContainedGenepacks.Count >= this.Props.maxCapacity;
			}
		}

		// Token: 0x1700289B RID: 10395
		// (get) Token: 0x060100F0 RID: 65776 RVA: 0x004A5D15 File Offset: 0x004A3F15
		public bool CanLoadMore
		{
			get
			{
				return !this.Full && this.ContainedGenepacks.Count + this.leftToLoad.Count < this.Props.maxCapacity;
			}
		}

		// Token: 0x1700289C RID: 10396
		// (get) Token: 0x060100F1 RID: 65777 RVA: 0x004A5D45 File Offset: 0x004A3F45
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x1700289D RID: 10397
		// (get) Token: 0x060100F2 RID: 65778 RVA: 0x004A5D50 File Offset: 0x004A3F50
		public List<Genepack> ContainedGenepacks
		{
			get
			{
				this.tmpGenepacks.Clear();
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Genepack genepack = this.innerContainer[i] as Genepack;
					if (genepack != null && genepack.GeneSet.GenesListForReading.Any<GeneDef>())
					{
						this.tmpGenepacks.Add(genepack);
					}
				}
				return this.tmpGenepacks;
			}
		}

		// Token: 0x060100F3 RID: 65779 RVA: 0x004A5DB7 File Offset: 0x004A3FB7
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Genepack container"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x060100F4 RID: 65780 RVA: 0x004A5DE4 File Offset: 0x004A3FE4
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060100F5 RID: 65781 RVA: 0x004A5D45 File Offset: 0x004A3F45
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060100F6 RID: 65782 RVA: 0x004A5DF2 File Offset: 0x004A3FF2
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x060100F7 RID: 65783 RVA: 0x004A5E08 File Offset: 0x004A4008
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.EjectContents(map);
			}
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				((Genepack)this.leftToLoad[i]).targetContainer = null;
			}
			this.leftToLoad.Clear();
		}

		// Token: 0x060100F8 RID: 65784 RVA: 0x004A5E58 File Offset: 0x004A4058
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.leftToLoad.Count; i++)
			{
				if (this.leftToLoad[i].Map == this.parent.Map)
				{
					GenDraw.DrawLineBetween(this.parent.DrawPos, this.leftToLoad[i].DrawPos);
				}
			}
		}

		// Token: 0x060100F9 RID: 65785 RVA: 0x004A5EBC File Offset: 0x004A40BC
		public void EjectContents(Map destMap = null)
		{
			if (destMap == null)
			{
				destMap = this.parent.Map;
			}
			IntVec3 dropLoc = this.parent.def.hasInteractionCell ? this.parent.InteractionCell : this.parent.Position;
			this.innerContainer.TryDropAll(dropLoc, destMap, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x060100FA RID: 65786 RVA: 0x004A5F16 File Offset: 0x004A4116
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer && this.innerContainer.Any)
			{
				yield return new Command_Action
				{
					defaultLabel = "EjectAll".Translate(),
					defaultDesc = "EjectAllDesc".Translate(),
					icon = CompGenepackContainer.EjectTex.Texture,
					action = delegate()
					{
						this.EjectContents(this.parent.Map);
					}
				};
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Fill with new packs",
					action = delegate()
					{
						this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
						for (int i = 0; i < this.Props.maxCapacity; i++)
						{
							this.innerContainer.TryAdd(ThingMaker.MakeThing(ThingDefOf.Genepack, null), true);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x060100FB RID: 65787 RVA: 0x004A5F28 File Offset: 0x004A4128
		public override string CompInspectStringExtra()
		{
			return "GenepacksStored".Translate() + string.Format(": {0} / {1}\n", this.innerContainer.Count, this.Props.maxCapacity) + "CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
		}

		// Token: 0x060100FC RID: 65788 RVA: 0x004A5FA4 File Offset: 0x004A41A4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<Thing>(ref this.leftToLoad, "leftToLoad", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.autoLoad, "autoLoad", true, false);
		}

		// Token: 0x0400A7D2 RID: 42962
		public ThingOwner innerContainer;

		// Token: 0x0400A7D3 RID: 42963
		public List<Thing> leftToLoad = new List<Thing>();

		// Token: 0x0400A7D4 RID: 42964
		public bool autoLoad = true;

		// Token: 0x0400A7D5 RID: 42965
		[Unsaved(false)]
		private List<Genepack> tmpGenepacks = new List<Genepack>();

		// Token: 0x0400A7D6 RID: 42966
		private static readonly CachedTexture EjectTex = new CachedTexture("UI/Gizmos/EjectAll");
	}
}
