using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200394D RID: 14669
	public class SymbolResolver_Archonexus : SymbolResolver
	{
		// Token: 0x060142F8 RID: 82680 RVA: 0x00604A30 File Offset: 0x00602C30
		public override void Resolve(ResolveParams rp)
		{
			rp.floorDef = TerrainDefOf.Sandstone_Smooth;
			rp.chanceToSkipFloor = new float?(0.05f);
			SymbolResolver_Archonexus.MinorSupersturctureSites.Clear();
			SymbolResolver_Archonexus.MajorSupersturctureSites.Clear();
			float? threatPoints = rp.threatPoints;
			float num = 0f;
			if ((threatPoints.GetValueOrDefault() > num & threatPoints != null) && Faction.OfMechanoids != null)
			{
				string text = "ArchonexusMechanoidsWakeUp" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
				ResolveParams resolveParams = rp;
				resolveParams.rect = rp.rect.ExpandedBy(5);
				resolveParams.rectTriggerSignalTag = text;
				resolveParams.threatPoints = rp.threatPoints;
				BaseGen.symbolStack.Push("rectTrigger", resolveParams, null);
				ResolveParams resolveParams2 = rp;
				resolveParams2.sleepingMechanoidsWakeupSignalTag = text;
				resolveParams2.threatPoints = rp.threatPoints;
				BaseGen.symbolStack.Push("sleepingMechanoids", resolveParams2, null);
				ResolveParams resolveParams3 = rp;
				resolveParams3.sound = SoundDefOf.ArchonexusThreatsAwakened_Alarm;
				resolveParams3.soundOneShotActionSignalTag = text;
				BaseGen.symbolStack.Push("soundOneShotAction", resolveParams3, null);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.dessicatedCorpseDensityRange = new FloatRange?(new FloatRange(0.003f, 0.006f));
			BaseGen.symbolStack.Push("desiccatedCorpses", resolveParams4, null);
			Vector3 v = IntVec3.North.ToVector3();
			ThingDef archonexusCore = ThingDefOf.ArchonexusCore;
			ResolveParams resolveParams5 = rp;
			resolveParams5.rect = CellRect.CenteredOn(rp.rect.CenterCell, archonexusCore.size.x, archonexusCore.size.z);
			resolveParams5.singleThingDef = ThingDefOf.ArchonexusCore;
			BaseGen.symbolStack.Push("thing", resolveParams5, null);
			resolveParams5.rect = resolveParams5.rect.ExpandedBy(1);
			BaseGen.symbolStack.Push("floor", resolveParams5, null);
			BaseGen.symbolStack.Push("clear", resolveParams5, null);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 a = GenAdj.DiagonalDirections[i];
				CellRect cellRect = CellRect.CenteredOn(rp.rect.CenterCell + a * 11, ThingDefOf.MajorArchotechStructure.size.x, ThingDefOf.MajorArchotechStructure.size.z);
				ResolveParams resolveParams6 = rp;
				Thing thing = ThingMaker.MakeThing(ThingDefOf.MajorArchotechStructure, null);
				CompSpawnImmortalSubplantsAround compSpawnImmortalSubplantsAround = thing.TryGetComp<CompSpawnImmortalSubplantsAround>();
				if (compSpawnImmortalSubplantsAround != null)
				{
					compSpawnImmortalSubplantsAround.Disable();
				}
				resolveParams6.singleThingToSpawn = thing;
				resolveParams6.rect = cellRect;
				BaseGen.symbolStack.Push("thing", resolveParams6, null);
				ResolveParams resolveParams7 = resolveParams6;
				resolveParams7.rect = resolveParams6.rect.ExpandedBy(1);
				resolveParams7.clearRoof = new bool?(true);
				BaseGen.symbolStack.Push("floor", resolveParams7, null);
				BaseGen.symbolStack.Push("clear", resolveParams7, null);
				SymbolResolver_Archonexus.MajorSupersturctureSites.Add(cellRect);
			}
			float num2 = 40f;
			for (int j = 0; j < 9; j++)
			{
				float angle = (float)j * num2;
				Vector3 vect = v.RotatedBy(angle) * 28f;
				CellRect cellRect2 = CellRect.CenteredOn(rp.rect.CenterCell + vect.ToIntVec3(), ThingDefOf.ArchotechTower.size.x, ThingDefOf.ArchotechTower.size.z);
				ResolveParams resolveParams8 = rp;
				resolveParams8.singleThingDef = ThingDefOf.ArchotechTower;
				resolveParams8.rect = cellRect2;
				BaseGen.symbolStack.Push("thing", resolveParams8, null);
				ResolveParams resolveParams9 = resolveParams8;
				resolveParams9.rect = resolveParams8.rect.ExpandedBy(1);
				resolveParams9.clearRoof = new bool?(true);
				BaseGen.symbolStack.Push("floor", resolveParams9, null);
				BaseGen.symbolStack.Push("clear", resolveParams9, null);
				SymbolResolver_Archonexus.MinorSupersturctureSites.Add(cellRect2);
			}
			rp.chanceToSkipFloor = new float?(0.95f);
			BaseGen.symbolStack.Push("floor", rp, null);
			for (int k = 0; k < SymbolResolver_Archonexus.MajorSupersturctureSites.Count; k++)
			{
				BaseGenUtility.DoPathwayBetween(resolveParams5.rect.CenterCell, SymbolResolver_Archonexus.MajorSupersturctureSites[k].CenterCell, rp.floorDef, 3);
			}
			for (int l = 0; l < SymbolResolver_Archonexus.MinorSupersturctureSites.Count; l++)
			{
				CellRect current = SymbolResolver_Archonexus.MinorSupersturctureSites[l];
				int index = GenMath.PositiveMod(l - 1, SymbolResolver_Archonexus.MinorSupersturctureSites.Count);
				BaseGenUtility.DoPathwayBetween(SymbolResolver_Archonexus.MinorSupersturctureSites[index].CenterCell, current.CenterCell, rp.floorDef, 3);
				BaseGenUtility.DoPathwayBetween(SymbolResolver_Archonexus.MajorSupersturctureSites.MinBy((CellRect c) => c.CenterCell.DistanceToSquared(current.CenterCell)).CenterCell, current.CenterCell, rp.floorDef, 3);
			}
			foreach (IntVec3 intVec in rp.rect)
			{
				if (intVec.DistanceTo(resolveParams5.rect.CenterCell) <= 28f)
				{
					Plant plant = intVec.GetPlant(BaseGen.globalSettings.map);
					if (plant != null && plant.def.destroyable)
					{
						plant.Destroy(DestroyMode.Vanish);
					}
					Building edifice = intVec.GetEdifice(BaseGen.globalSettings.map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Vanish);
					}
					BaseGen.globalSettings.map.roofGrid.SetRoof(intVec, null);
				}
			}
			BaseGenUtility.DoPathwayBetween(resolveParams5.rect.CenterCell, resolveParams5.rect.CenterCell + IntVec3.South * 25, rp.floorDef, 3);
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			foreach (CellRect item in SymbolResolver_Archonexus.MinorSupersturctureSites)
			{
				orGenerateVar.Add(item);
			}
			foreach (CellRect item2 in SymbolResolver_Archonexus.MajorSupersturctureSites)
			{
				orGenerateVar.Add(item2);
			}
			SymbolResolver_Archonexus.MinorSupersturctureSites.Clear();
			SymbolResolver_Archonexus.MajorSupersturctureSites.Clear();
		}

		// Token: 0x0400DF23 RID: 57123
		private const int SuperstructureDistance = 11;

		// Token: 0x0400DF24 RID: 57124
		private const int MinorSuperstructureDistance = 28;

		// Token: 0x0400DF25 RID: 57125
		private const int MinorSuperstructureCount = 9;

		// Token: 0x0400DF26 RID: 57126
		private static List<CellRect> MinorSupersturctureSites = new List<CellRect>();

		// Token: 0x0400DF27 RID: 57127
		private static List<CellRect> MajorSupersturctureSites = new List<CellRect>();

		// Token: 0x0400DF28 RID: 57128
		private const string MechanoidsWakeUpSignalPrefix = "ArchonexusMechanoidsWakeUp";
	}
}
