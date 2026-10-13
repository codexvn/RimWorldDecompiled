using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E71 RID: 11889
	public class CompSubstructureFootprint : ThingComp
	{
		// Token: 0x17002AF8 RID: 11000
		// (get) Token: 0x06010BDE RID: 68574 RVA: 0x004D3603 File Offset: 0x004D1803
		public CompProperties_SubstructureFootprint Props
		{
			get
			{
				return (CompProperties_SubstructureFootprint)this.props;
			}
		}

		// Token: 0x17002AF9 RID: 11001
		// (get) Token: 0x06010BDF RID: 68575 RVA: 0x004D3610 File Offset: 0x004D1810
		public bool DisplaySubstructureOverlay
		{
			get
			{
				return this.Props.displaySubstructureOverlayWhenSelected && this.displaySubstructureOverlay;
			}
		}

		// Token: 0x17002AFA RID: 11002
		// (get) Token: 0x06010BE0 RID: 68576 RVA: 0x004D3627 File Offset: 0x004D1827
		public bool Valid
		{
			get
			{
				return this.facilityComp == null || this.facilityComp.CanBeActive;
			}
		}

		// Token: 0x06010BE1 RID: 68577 RVA: 0x004D363E File Offset: 0x004D183E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			SubstructureGrid substructureGrid = this.parent.Map.substructureGrid;
			if (substructureGrid != null)
			{
				substructureGrid.MarkDirty();
			}
			this.facilityComp = this.parent.TryGetComp<CompGravshipFacility>();
		}

		// Token: 0x06010BE2 RID: 68578 RVA: 0x004D3673 File Offset: 0x004D1873
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			SubstructureGrid substructureGrid = map.substructureGrid;
			if (substructureGrid == null)
			{
				return;
			}
			substructureGrid.MarkDirty();
		}

		// Token: 0x06010BE3 RID: 68579 RVA: 0x004D368D File Offset: 0x004D188D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Props.displaySubstructureOverlayWhenSelected && this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandShowSubstructureOverlay".Translate(),
					defaultDesc = "CommandShowSubstructureOverlayDesc".Translate(),
					icon = CompSubstructureFootprint.DisplaySubstructureTex.Texture,
					isActive = (() => this.displaySubstructureOverlay),
					toggleAction = delegate()
					{
						this.displaySubstructureOverlay = !this.displaySubstructureOverlay;
					}
				};
			}
			yield break;
		}

		// Token: 0x06010BE4 RID: 68580 RVA: 0x004D369D File Offset: 0x004D189D
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.displaySubstructureOverlay, "displaySubstructureOverlay", true, false);
		}

		// Token: 0x0400AE76 RID: 44662
		[Unsaved(false)]
		private CompGravshipFacility facilityComp;

		// Token: 0x0400AE77 RID: 44663
		private bool displaySubstructureOverlay = true;

		// Token: 0x0400AE78 RID: 44664
		private static readonly CachedTexture DisplaySubstructureTex = new CachedTexture("UI/Commands/ShowSubstructureOverlay");
	}
}
