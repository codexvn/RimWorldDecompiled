using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020024C9 RID: 9417
	public class Pawn_MeleeVerbs_TerrainSource : IExposable, IVerbOwner
	{
		// Token: 0x17001F50 RID: 8016
		// (get) Token: 0x0600CD09 RID: 52489 RVA: 0x003C2290 File Offset: 0x003C0490
		public VerbTracker VerbTracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x17001F51 RID: 8017
		// (get) Token: 0x0600CD0A RID: 52490 RVA: 0x00002C42 File Offset: 0x00000E42
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001F52 RID: 8018
		// (get) Token: 0x0600CD0B RID: 52491 RVA: 0x003C2298 File Offset: 0x003C0498
		public List<Tool> Tools
		{
			get
			{
				TerrainDef terrainDef = this.def;
				if (terrainDef == null)
				{
					return null;
				}
				return terrainDef.tools;
			}
		}

		// Token: 0x17001F53 RID: 8019
		// (get) Token: 0x0600CD0C RID: 52492 RVA: 0x003C22AB File Offset: 0x003C04AB
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.parent.Pawn;
			}
		}

		// Token: 0x17001F54 RID: 8020
		// (get) Token: 0x0600CD0D RID: 52493 RVA: 0x003C22B8 File Offset: 0x003C04B8
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Terrain;
			}
		}

		// Token: 0x0600CD0E RID: 52494 RVA: 0x003C22BF File Offset: 0x003C04BF
		public static Pawn_MeleeVerbs_TerrainSource Create(Pawn_MeleeVerbs parent, TerrainDef terrainDef)
		{
			Pawn_MeleeVerbs_TerrainSource pawn_MeleeVerbs_TerrainSource = new Pawn_MeleeVerbs_TerrainSource();
			pawn_MeleeVerbs_TerrainSource.parent = parent;
			pawn_MeleeVerbs_TerrainSource.def = terrainDef;
			pawn_MeleeVerbs_TerrainSource.tracker = new VerbTracker(pawn_MeleeVerbs_TerrainSource);
			return pawn_MeleeVerbs_TerrainSource;
		}

		// Token: 0x0600CD0F RID: 52495 RVA: 0x003C22E0 File Offset: 0x003C04E0
		public void ExposeData()
		{
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Deep.Look<VerbTracker>(ref this.tracker, "tracker", new object[]
			{
				this
			});
		}

		// Token: 0x0600CD10 RID: 52496 RVA: 0x003C230C File Offset: 0x003C050C
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "TerrainVerbs_" + this.parent.Pawn.ThingID;
		}

		// Token: 0x0600CD11 RID: 52497 RVA: 0x003C2328 File Offset: 0x003C0528
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.parent.Pawn && p.Spawned && this.def == p.Position.GetTerrain(p.Map) && Find.TickManager.TicksGame >= this.parent.lastTerrainBasedVerbUseTick + 1200;
		}

		// Token: 0x04008BA5 RID: 35749
		public Pawn_MeleeVerbs parent;

		// Token: 0x04008BA6 RID: 35750
		public TerrainDef def;

		// Token: 0x04008BA7 RID: 35751
		public VerbTracker tracker;
	}
}
