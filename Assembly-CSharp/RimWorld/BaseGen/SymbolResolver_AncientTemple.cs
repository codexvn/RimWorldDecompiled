using System;
using System.Linq;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200394A RID: 14666
	public class SymbolResolver_AncientTemple : SymbolResolver
	{
		// Token: 0x060142F1 RID: 82673 RVA: 0x00604678 File Offset: 0x00602878
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			CellRect cellRect = CellRect.Empty;
			SketchResolveParams sketchResolveParams = default(SketchResolveParams);
			sketchResolveParams.sketch = new Sketch();
			sketchResolveParams.monumentOpen = new bool?(false);
			sketchResolveParams.monumentSize = new IntVec2?(new IntVec2(rp.rect.Width, rp.rect.Height));
			sketchResolveParams.allowMonumentDoors = new bool?(false);
			sketchResolveParams.allowWood = new bool?(false);
			sketchResolveParams.allowFlammableWalls = new bool?(false);
			if (rp.allowedMonumentThings != null)
			{
				sketchResolveParams.allowedMonumentThings = rp.allowedMonumentThings;
			}
			else
			{
				sketchResolveParams.allowedMonumentThings = new ThingFilter();
				sketchResolveParams.allowedMonumentThings.SetAllowAll(null, true);
			}
			sketchResolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
			Sketch sketch = SketchGen.Generate(SketchResolverDefOf.Monument, sketchResolveParams);
			sketch.Spawn(map, rp.rect.CenterCell, null, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, true, false, true, null, false, true, null, null, null);
			CellRect rect = SketchGenUtility.FindBiggestRect(sketch, delegate(IntVec3 x)
			{
				if (sketch.TerrainAt(x) != null)
				{
					return sketch.ThingsAt(x).All((SketchThing y) => y.def != ThingDefOf.Wall);
				}
				return false;
			}, null, 3).MovedBy(rp.rect.CenterCell);
			for (int i = 0; i < sketch.Things.Count; i++)
			{
				if (sketch.Things[i].def == ThingDefOf.Wall)
				{
					IntVec3 intVec = sketch.Things[i].pos + rp.rect.CenterCell;
					if (cellRect.IsEmpty)
					{
						cellRect = CellRect.SingleCell(intVec);
					}
					else
					{
						cellRect = CellRect.FromLimits(Mathf.Min(cellRect.minX, intVec.x), Mathf.Min(cellRect.minZ, intVec.z), Mathf.Max(cellRect.maxX, intVec.x), Mathf.Max(cellRect.maxZ, intVec.z));
					}
				}
			}
			if (!rect.IsEmpty)
			{
				ResolveParams resolveParams = rp;
				resolveParams.rect = rect;
				if (rp.allowedMonumentThings != null)
				{
					resolveParams.allowedMonumentThings = rp.allowedMonumentThings;
				}
				else
				{
					resolveParams.allowedMonumentThings = new ThingFilter();
					resolveParams.allowedMonumentThings.SetAllowAll(null, true);
				}
				if (ModsConfig.RoyaltyActive)
				{
					resolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Drape, false);
				}
				BaseGen.symbolStack.Push("interior_ancientTemple", resolveParams, null);
			}
			if (rp.makeWarningLetter != null && rp.makeWarningLetter.Value)
			{
				string signalTag = string.Format("ancientTempleApproached-{0}", Find.UniqueIDsManager.GetNextSignalTagID());
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = signalTag;
				rectTrigger.Rect = cellRect.ExpandedBy(1).ClipInsideMap(map);
				rectTrigger.destroyIfUnfogged = true;
				GenSpawn.Spawn(rectTrigger, cellRect.CenterCell, map, WipeMode.Vanish);
				SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
				signalAction_Letter.signalTag = signalTag;
				signalAction_Letter.letterDef = LetterDefOf.ThreatBig;
				signalAction_Letter.letterLabelKey = "LetterLabelAncientShrineWarning";
				signalAction_Letter.letterMessageKey = "AncientShrineWarning";
				GenSpawn.Spawn(signalAction_Letter, cellRect.CenterCell, map, WipeMode.Vanish);
			}
		}
	}
}
