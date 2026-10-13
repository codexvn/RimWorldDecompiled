using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E8B RID: 11915
	public class CompThingContainer : ThingComp, IThingHolder, ISearchableContents
	{
		// Token: 0x17002B16 RID: 11030
		// (get) Token: 0x06010C6B RID: 68715 RVA: 0x004D5032 File Offset: 0x004D3232
		public CompProperties_ThingContainer Props
		{
			get
			{
				return (CompProperties_ThingContainer)this.props;
			}
		}

		// Token: 0x17002B17 RID: 11031
		// (get) Token: 0x06010C6C RID: 68716 RVA: 0x004D503F File Offset: 0x004D323F
		public Thing ContainedThing
		{
			get
			{
				if (!this.innerContainer.Any)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x17002B18 RID: 11032
		// (get) Token: 0x06010C6D RID: 68717 RVA: 0x004D505C File Offset: 0x004D325C
		public bool Empty
		{
			get
			{
				return this.ContainedThing == null;
			}
		}

		// Token: 0x17002B19 RID: 11033
		// (get) Token: 0x06010C6E RID: 68718 RVA: 0x004D5067 File Offset: 0x004D3267
		public int TotalStackCount
		{
			get
			{
				if (!this.Empty)
				{
					return this.innerContainer.TotalStackCountOfDef(this.innerContainer[0].def);
				}
				return 0;
			}
		}

		// Token: 0x17002B1A RID: 11034
		// (get) Token: 0x06010C6F RID: 68719 RVA: 0x004D508F File Offset: 0x004D328F
		public bool Full
		{
			get
			{
				return !this.Empty && this.TotalStackCount >= this.Props.stackLimit;
			}
		}

		// Token: 0x17002B1B RID: 11035
		// (get) Token: 0x06010C70 RID: 68720 RVA: 0x004D50B1 File Offset: 0x004D32B1
		public string LabelCapWithTotalCount
		{
			get
			{
				if (!this.Empty)
				{
					return this.ContainedThing.LabelCapNoCount + " x" + this.TotalStackCount.ToStringCached();
				}
				return null;
			}
		}

		// Token: 0x17002B1C RID: 11036
		// (get) Token: 0x06010C71 RID: 68721 RVA: 0x004D50DD File Offset: 0x004D32DD
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x06010C72 RID: 68722 RVA: 0x004D50E5 File Offset: 0x004D32E5
		public CompThingContainer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06010C73 RID: 68723 RVA: 0x004D50F9 File Offset: 0x004D32F9
		public virtual bool Accepts(ThingDef thingDef)
		{
			return this.Empty || (this.TotalStackCount < this.Props.stackLimit && thingDef == this.ContainedThing.def);
		}

		// Token: 0x06010C74 RID: 68724 RVA: 0x004D5128 File Offset: 0x004D3328
		public virtual bool Accepts(Thing thing)
		{
			return this.Accepts(thing.def);
		}

		// Token: 0x06010C75 RID: 68725 RVA: 0x004D5136 File Offset: 0x004D3336
		public override void CompTick()
		{
			this.innerContainer.DoTick();
		}

		// Token: 0x06010C76 RID: 68726 RVA: 0x004D5144 File Offset: 0x004D3344
		public override void PostDraw()
		{
			if (!this.Empty && this.Props.drawContainedThing)
			{
				this.ContainedThing.DrawNowAt((this.parent.Position + this.Props.containedThingOffset.RotatedBy(this.parent.Rotation)).ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop), false);
			}
		}

		// Token: 0x06010C77 RID: 68727 RVA: 0x004D51A8 File Offset: 0x004D33A8
		public override void DrawGUIOverlay()
		{
			if (this.parent.Spawned && this.Props.drawStackLabel && !this.Empty && Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				GenMapUI.DrawThingLabel(this.parent, this.TotalStackCount.ToStringCached());
			}
		}

		// Token: 0x06010C78 RID: 68728 RVA: 0x004D51F9 File Offset: 0x004D33F9
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x06010C79 RID: 68729 RVA: 0x004D521B File Offset: 0x004D341B
		public override string CompInspectStringExtra()
		{
			return "Contents".Translate() + ": " + (this.Empty ? "Nothing".Translate() : this.LabelCapWithTotalCount);
		}

		// Token: 0x06010C7A RID: 68730 RVA: 0x004D525A File Offset: 0x004D345A
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (!this.Empty)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "Contents".Translate(), this.LabelCapWithTotalCount, this.LabelCapWithTotalCount, 1200, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.ContainedThing, -1, false)), false, false);
			}
			yield break;
		}

		// Token: 0x06010C7B RID: 68731 RVA: 0x004D526A File Offset: 0x004D346A
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06010C7C RID: 68732 RVA: 0x004D50DD File Offset: 0x004D32DD
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06010C7D RID: 68733 RVA: 0x004D5278 File Offset: 0x004D3478
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06010C7E RID: 68734 RVA: 0x004D529A File Offset: 0x004D349A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Thing item in ((IEnumerable<Thing>)this.innerContainer))
			{
				Gizmo gizmo = Building.SelectContainedItemGizmo(this.parent, item);
				if (gizmo != null)
				{
					yield return gizmo;
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400AEBA RID: 44730
		public ThingOwner innerContainer;
	}
}
