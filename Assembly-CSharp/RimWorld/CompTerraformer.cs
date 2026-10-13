using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E83 RID: 11907
	public class CompTerraformer : ThingComp
	{
		// Token: 0x17002B09 RID: 11017
		// (get) Token: 0x06010C36 RID: 68662 RVA: 0x004D47FA File Offset: 0x004D29FA
		public CompProperties_Terraformer Props
		{
			get
			{
				return (CompProperties_Terraformer)this.props;
			}
		}

		// Token: 0x17002B0A RID: 11018
		// (get) Token: 0x06010C37 RID: 68663 RVA: 0x004D4807 File Offset: 0x004D2A07
		public int TicksTillConvert
		{
			get
			{
				return Mathf.Max(this.convertTick - GenTicks.TicksGame, 0);
			}
		}

		// Token: 0x17002B0B RID: 11019
		// (get) Token: 0x06010C38 RID: 68664 RVA: 0x004D481C File Offset: 0x004D2A1C
		private bool Active
		{
			get
			{
				IntVec3 intVec;
				return this.parent.Spawned && this.TryGetCellToCovert(out intVec);
			}
		}

		// Token: 0x06010C39 RID: 68665 RVA: 0x004D4845 File Offset: 0x004D2A45
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.convertTick = GenTicks.TicksGame + this.Props.secondsPerConvert.SecondsToTicks();
			}
		}

		// Token: 0x06010C3A RID: 68666 RVA: 0x004D487C File Offset: 0x004D2A7C
		private bool TryGetCellToCovert(out IntVec3 cell)
		{
			int num = GenRadial.NumCellsInRadius(this.Props.radius);
			Map map = this.parent.Map;
			for (int i = 0; i < num; i++)
			{
				cell = this.parent.Position + GenRadial.RadialPattern[i];
				if (CompTerraformer.CanEverConvertCell(cell, map, this.Props.convertTerrainDef))
				{
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06010C3B RID: 68667 RVA: 0x004D48FC File Offset: 0x004D2AFC
		public static bool CanEverConvertCell(IntVec3 cell, Map map, TerrainDef skip = null)
		{
			if (!cell.InBounds(map))
			{
				return false;
			}
			if (cell.GetEdifice(map) != null)
			{
				return false;
			}
			TerrainDef terrain = cell.GetTerrain(map);
			return terrain.canEverTerraform && (skip == null || terrain != skip) && terrain.passability != Traversability.Impassable && terrain.affordances.Contains(TerrainAffordanceDefOf.Light) && !terrain.isFoundation;
		}

		// Token: 0x06010C3C RID: 68668 RVA: 0x004D4964 File Offset: 0x004D2B64
		private void Convert()
		{
			IntVec3 c;
			if (!this.TryGetCellToCovert(out c))
			{
				return;
			}
			this.parent.Map.terrainGrid.SetTerrain(c, this.Props.convertTerrainDef);
		}

		// Token: 0x06010C3D RID: 68669 RVA: 0x004D499D File Offset: 0x004D2B9D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!DebugSettings.ShowDevGizmos)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Convert",
				action = new Action(this.Convert),
				Disabled = !this.Active
			};
			yield break;
		}

		// Token: 0x06010C3E RID: 68670 RVA: 0x004D49AD File Offset: 0x004D2BAD
		public override void CompTickRare()
		{
			if (GenTicks.TicksGame >= this.convertTick && this.Active)
			{
				this.convertTick = GenTicks.TicksGame + this.Props.secondsPerConvert.SecondsToTicks();
				this.Convert();
			}
		}

		// Token: 0x06010C3F RID: 68671 RVA: 0x004D49E6 File Offset: 0x004D2BE6
		public override void PostDrawExtraSelectionOverlays()
		{
			GenDraw.DrawRadiusRing(this.parent.Position, this.Props.radius);
		}

		// Token: 0x06010C40 RID: 68672 RVA: 0x004D4A03 File Offset: 0x004D2C03
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.convertTick, "convertTick", 0, false);
		}

		// Token: 0x0400AEA5 RID: 44709
		private int convertTick;
	}
}
