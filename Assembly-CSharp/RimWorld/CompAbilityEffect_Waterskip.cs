using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022A7 RID: 8871
	public class CompAbilityEffect_Waterskip : CompAbilityEffect
	{
		// Token: 0x0600C0FC RID: 49404 RVA: 0x0037E424 File Offset: 0x0037C624
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			foreach (IntVec3 c in this.AffectedCells(target, map))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						if (thingList[i] is Fire)
						{
							thingList[i].Destroy(DestroyMode.Vanish);
						}
						else
						{
							Pawn pawn = thingList[i] as Pawn;
							if (pawn != null)
							{
								HediffComp_Invisibility invisibilityComp = pawn.GetInvisibilityComp();
								if (invisibilityComp != null)
								{
									invisibilityComp.DisruptInvisibility();
								}
							}
						}
					}
					if (!c.Filled(map))
					{
						FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Water, 1, FilthSourceFlags.None, true);
					}
					FleckCreationData dataStatic = FleckMaker.GetDataStatic(c.ToVector3Shifted(), map, FleckDefOf.WaterskipSplashParticles, 1f);
					dataStatic.rotationRate = (float)Rand.Range(-30, 30);
					dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
					map.flecks.CreateFleck(dataStatic);
					CompAbilityEffect_Teleport.SendSkipUsedSignal(c, this.parent.pawn);
				}
			}
		}

		// Token: 0x0600C0FD RID: 49405 RVA: 0x0037E580 File Offset: 0x0037C780
		private IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			if (!target.Cell.InBounds(map) || target.Cell.Filled(this.parent.pawn.Map))
			{
				yield break;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(target.Cell, this.parent.def.EffectRadius, true))
			{
				if (intVec.InBounds(map) && GenSight.LineOfSightToEdges(target.Cell, intVec, map, true, null))
				{
					yield return intVec;
				}
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C0FE RID: 49406 RVA: 0x0037E5A0 File Offset: 0x0037C7A0
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target, this.parent.pawn.Map).ToList<IntVec3>(), this.Valid(target, false) ? Color.white : Color.red, null, null, 2900);
		}

		// Token: 0x0600C0FF RID: 49407 RVA: 0x0037E5F4 File Offset: 0x0037C7F4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Filled(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityOccupiedCells".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
