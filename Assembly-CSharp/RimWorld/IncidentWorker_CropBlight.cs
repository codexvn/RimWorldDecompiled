using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D89 RID: 7561
	public class IncidentWorker_CropBlight : IncidentWorker
	{
		// Token: 0x0600A941 RID: 43329 RVA: 0x00307040 File Offset: 0x00305240
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Plant plant;
			return this.TryFindRandomBlightablePlant((Map)parms.target, out plant);
		}

		// Token: 0x0600A942 RID: 43330 RVA: 0x00307060 File Offset: 0x00305260
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			float num = IncidentWorker_CropBlight.RadiusFactorPerPointsCurve.Evaluate(parms.points);
			Plant plant;
			if (!this.TryFindRandomBlightablePlant(map, out plant))
			{
				return false;
			}
			Room room = plant.GetRoom(RegionType.Set_All);
			int i = 0;
			int num2 = GenRadial.NumCellsInRadius(11f * num);
			while (i < num2)
			{
				IntVec3 intVec = plant.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map) == room)
				{
					Plant firstBlightableNowPlant = BlightUtility.GetFirstBlightableNowPlant(intVec, map);
					if (firstBlightableNowPlant != null && firstBlightableNowPlant.def == plant.def && Rand.Chance(this.BlightChance(firstBlightableNowPlant.Position, plant.Position, num)))
					{
						firstBlightableNowPlant.CropBlighted();
					}
				}
				i++;
			}
			base.SendStandardLetter("LetterLabelCropBlight".Translate(new NamedArgument(plant.def, "PLANTDEF")), "LetterCropBlight".Translate(new NamedArgument(plant.def, "PLANTDEF")), LetterDefOf.NegativeEvent, parms, new TargetInfo(plant.Position, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A943 RID: 43331 RVA: 0x00307184 File Offset: 0x00305384
		private bool TryFindRandomBlightablePlant(Map map, out Plant plant)
		{
			Thing thing;
			bool result = (from x in map.listerThings.ThingsInGroup(ThingRequestGroup.Plant)
			where ((Plant)x).BlightableNow
			select x).TryRandomElement(out thing);
			plant = (Plant)thing;
			return result;
		}

		// Token: 0x0600A944 RID: 43332 RVA: 0x003071D4 File Offset: 0x003053D4
		private float BlightChance(IntVec3 c, IntVec3 root, float radiusFactor)
		{
			float x = c.DistanceTo(root) / radiusFactor;
			return IncidentWorker_CropBlight.BlightChancePerRadius.Evaluate(x);
		}

		// Token: 0x0400751F RID: 29983
		private const float Radius = 11f;

		// Token: 0x04007520 RID: 29984
		private static readonly SimpleCurve BlightChancePerRadius = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(8f, 1f),
				true
			},
			{
				new CurvePoint(11f, 0.3f),
				true
			}
		};

		// Token: 0x04007521 RID: 29985
		private static readonly SimpleCurve RadiusFactorPerPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0.6f),
				true
			},
			{
				new CurvePoint(500f, 1f),
				true
			},
			{
				new CurvePoint(2000f, 2f),
				true
			}
		};
	}
}
