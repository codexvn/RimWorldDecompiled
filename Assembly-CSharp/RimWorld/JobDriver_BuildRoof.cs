using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001168 RID: 4456
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x06006D00 RID: 27904 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06006D01 RID: 27905 RVA: 0x00223203 File Offset: 0x00221403
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.BuildRoof[base.Cell]);
			this.FailOn(() => !RoofCollapseUtility.WithinRangeOfRoofHolder(base.Cell, base.Map, false));
			this.FailOn(() => !RoofCollapseUtility.ConnectedToRoofHolder(base.Cell, base.Map, true));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006D02 RID: 27906 RVA: 0x00223214 File Offset: 0x00221414
		protected override void DoEffect()
		{
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map) && base.Map.areaManager.BuildRoof[intVec] && !intVec.Roofed(base.Map) && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map, false) && RoofUtility.FirstBlockingThing(intVec, base.Map) == null)
				{
					base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
					MoteMaker.PlaceTempRoof(intVec, base.Map);
					List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						CompWakeUpDormant compWakeUpDormant;
						if (thing.def.building != null && thing.def.building.IsMortar && thing.TryGetComp(out compWakeUpDormant))
						{
							compWakeUpDormant.Activate(this.pawn, true, false, false);
						}
					}
				}
			}
		}

		// Token: 0x06006D03 RID: 27907 RVA: 0x00223335 File Offset: 0x00221535
		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}
	}
}
