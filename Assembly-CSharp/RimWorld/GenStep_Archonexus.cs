using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002054 RID: 8276
	public class GenStep_Archonexus : GenStep_Scatterer
	{
		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x0600B5C9 RID: 46537 RVA: 0x00343A1E File Offset: 0x00341C1E
		public override int SeedPart
		{
			get
			{
				return 473957495;
			}
		}

		// Token: 0x0600B5CA RID: 46538 RVA: 0x00343A25 File Offset: 0x00341C25
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = true;
			base.Generate(map, parms);
		}

		// Token: 0x0600B5CB RID: 46539 RVA: 0x00343A40 File Offset: 0x00341C40
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			ThingDef archonexusCore = ThingDefOf.ArchonexusCore;
			IntVec3 c2 = ThingUtility.InteractionCellWhenAt(archonexusCore, c, archonexusCore.defaultPlacingRot, map);
			return map.reachability.CanReachMapEdge(c2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false));
		}

		// Token: 0x0600B5CC RID: 46540 RVA: 0x00343AA4 File Offset: 0x00341CA4
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			SitePartParams parms2 = parms.sitePart.parms;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.threatPoints = new float?(parms2.threatPoints);
			resolveParams.rect = CellRect.CenteredOn(c, 60, 60);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexus", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04007CB9 RID: 31929
		private const int Size = 60;
	}
}
