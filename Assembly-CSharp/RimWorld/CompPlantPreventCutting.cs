using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DAB RID: 11691
	[StaticConstructorOnStartup]
	public class CompPlantPreventCutting : ThingComp
	{
		// Token: 0x170029E6 RID: 10726
		// (get) Token: 0x06010700 RID: 67328 RVA: 0x004BE3A1 File Offset: 0x004BC5A1
		public CompProperties_PlantPreventCutting Props
		{
			get
			{
				return (CompProperties_PlantPreventCutting)this.props;
			}
		}

		// Token: 0x170029E7 RID: 10727
		// (get) Token: 0x06010701 RID: 67329 RVA: 0x004BE3AE File Offset: 0x004BC5AE
		// (set) Token: 0x06010702 RID: 67330 RVA: 0x004BE3B8 File Offset: 0x004BC5B8
		public bool PreventCutting
		{
			get
			{
				return this.preventCutting;
			}
			set
			{
				this.preventCutting = value;
				if (!this.preventCutting)
				{
					return;
				}
				this.parent.Map.designationManager.TryRemoveDesignationOn(this.parent, DesignationDefOf.CutPlant);
				this.parent.Map.designationManager.TryRemoveDesignationOn(this.parent, DesignationDefOf.HarvestPlant);
			}
		}

		// Token: 0x06010703 RID: 67331 RVA: 0x004BE415 File Offset: 0x004BC615
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.preventCutting, "canCut", false, false);
		}

		// Token: 0x06010704 RID: 67332 RVA: 0x004BE429 File Offset: 0x004BC629
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield return new Command_Toggle
			{
				defaultLabel = "PreventCutting".Translate(),
				defaultDesc = "PreventCuttingDesc".Translate(),
				toggleAction = delegate()
				{
					this.PreventCutting = !this.preventCutting;
				},
				isActive = (() => this.preventCutting),
				icon = CompPlantPreventCutting.IconTexture
			};
			yield break;
		}

		// Token: 0x0400AB79 RID: 43897
		private bool preventCutting = true;

		// Token: 0x0400AB7A RID: 43898
		private static readonly Texture2D IconTexture = ContentFinder<Texture2D>.Get("UI/Designators/PreventCutting", true);
	}
}
