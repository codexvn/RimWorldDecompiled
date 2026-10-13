using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E88 RID: 11912
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x17002B15 RID: 11029
		// (get) Token: 0x06010C62 RID: 68706 RVA: 0x004D4E89 File Offset: 0x004D3089
		private CompProperties_TerrainPumpDry Props
		{
			get
			{
				return (CompProperties_TerrainPumpDry)this.props;
			}
		}

		// Token: 0x06010C63 RID: 68707 RVA: 0x004D4E96 File Offset: 0x004D3096
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x06010C64 RID: 68708 RVA: 0x004D4EAC File Offset: 0x004D30AC
		public static void AffectCell(Map map, IntVec3 c)
		{
			TerrainDef terrainDef = map.terrainGrid.TopTerrainAt(c);
			TerrainDef terrainToDryTo = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef);
			if (terrainToDryTo != null)
			{
				map.terrainGrid.SetTerrain(c, terrainToDryTo);
			}
			TerrainDef terrainDef2 = map.terrainGrid.UnderTerrainAt(c);
			if (terrainDef2 != null)
			{
				TerrainDef terrainToDryTo2 = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef2);
				if (terrainToDryTo2 != null)
				{
					map.terrainGrid.SetUnderTerrain(c, terrainToDryTo2);
				}
			}
		}

		// Token: 0x06010C65 RID: 68709 RVA: 0x004D4F06 File Offset: 0x004D3106
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06010C66 RID: 68710 RVA: 0x004D4F30 File Offset: 0x004D3130
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Props.soundWorking.NullOrUndefined() && base.Working && base.CurrentRadius < this.Props.radius - 0.0001f)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.soundWorking.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
				return;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06010C67 RID: 68711 RVA: 0x004D4FDE File Offset: 0x004D31DE
		private static TerrainDef GetTerrainToDryTo(Map map, TerrainDef terrainDef)
		{
			if (terrainDef.driesTo == null)
			{
				return null;
			}
			if (map.Biome == BiomeDefOf.SeaIce)
			{
				return TerrainDefOf.Ice;
			}
			return terrainDef.driesTo;
		}

		// Token: 0x0400AEB2 RID: 44722
		private Sustainer sustainer;
	}
}
