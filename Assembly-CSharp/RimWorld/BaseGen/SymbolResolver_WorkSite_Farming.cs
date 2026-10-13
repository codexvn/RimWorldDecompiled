using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200398A RID: 14730
	public class SymbolResolver_WorkSite_Farming : SymbolResolver_WorkSite
	{
		// Token: 0x060143BC RID: 82876 RVA: 0x0060A140 File Offset: 0x00608340
		public override void Resolve(ResolveParams rp)
		{
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CellRect rect = rp.rect;
			float workSitePoints = rp.workSitePoints;
			int num = Mathf.FloorToInt(SymbolResolver_WorkSite_Farming.FieldRadiusThreatPointsCurve.Evaluate(workSitePoints));
			int num2 = Mathf.FloorToInt(SymbolResolver_WorkSite_Farming.FieldCountThreatPointsCurve.Evaluate(workSitePoints));
			Map map = BaseGen.globalSettings.map;
			float num3 = 3f + (float)num / 2f;
			float num4 = 15f + (float)num / 2f;
			ThingDef thingDef;
			if (!rp.stockpileConcreteContents.NullOrEmpty<Thing>())
			{
				thingDef = DefDatabase<ThingDef>.AllDefs.First((ThingDef def) => def.plant != null && def.plant.harvestedThingDef == rp.stockpileConcreteContents.First<Thing>().def);
			}
			else
			{
				thingDef = (from def in DefDatabase<ThingDef>.AllDefs
				where def.plant != null && def.plant.Sowable
				select def).RandomElement<ThingDef>();
			}
			List<CellRect> list = new List<CellRect>();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(rect.CenterCell, num4 + (float)Mathf.Max(rect.Width, rect.Height), false))
			{
				if (intVec.InBounds(map))
				{
					float num5 = Mathf.Sqrt(rect.ClosestDistSquaredTo(intVec));
					if (num5 >= num3 && num5 <= num4)
					{
						CellRect _r = CellRect.CenteredOn(intVec, num);
						if (!list.Any((CellRect fieldRect) => fieldRect.Overlaps(_r)))
						{
							bool flag = false;
							foreach (IntVec3 c in _r)
							{
								if (!c.InBounds(map) || c.GetEdifice(map) != null || !thingDef.CanEverPlantAt(c, map, true, true))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								BaseGen.symbolStack.Push("cultivatedPlants", new ResolveParams
								{
									rect = _r,
									cultivatedPlantDef = thingDef,
									fixedCulativedPlantGrowth = new float?(0.25f)
								}, null);
								list.Add(_r);
								orGenerateVar.Add(_r);
								if (list.Count >= num2)
								{
									break;
								}
							}
						}
					}
				}
			}
			base.Resolve(rp);
		}

		// Token: 0x0400DF98 RID: 57240
		private static readonly SimpleCurve FieldRadiusThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(200f, 3f),
				true
			},
			{
				new CurvePoint(500f, 5f),
				true
			},
			{
				new CurvePoint(1000f, 7f),
				true
			}
		};

		// Token: 0x0400DF99 RID: 57241
		private static readonly SimpleCurve FieldCountThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(1000f, 4f),
				true
			}
		};

		// Token: 0x0400DF9A RID: 57242
		private const float MinDistField = 3f;

		// Token: 0x0400DF9B RID: 57243
		private const float MaxDistField = 15f;
	}
}
