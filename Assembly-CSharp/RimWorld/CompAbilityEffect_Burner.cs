using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022B3 RID: 8883
	public class CompAbilityEffect_Burner : CompAbilityEffect
	{
		// Token: 0x17001D94 RID: 7572
		// (get) Token: 0x0600C136 RID: 49462 RVA: 0x0037F22C File Offset: 0x0037D42C
		public new CompProperties_AbilityBurner Props
		{
			get
			{
				return (CompProperties_AbilityBurner)this.props;
			}
		}

		// Token: 0x0600C137 RID: 49463 RVA: 0x0037F239 File Offset: 0x0037D439
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo a, LocalTargetInfo _)
				{
					Vector3 drawPos = this.parent.pawn.DrawPos;
					IntVec3 intVec = drawPos.Yto0().ToIntVec3();
					Map map = this.parent.pawn.Map;
					IncineratorSpray incineratorSpray = GenSpawn.Spawn(ThingDefOf.IncineratorSpray, intVec, map, WipeMode.Vanish) as IncineratorSpray;
					int numStreams = this.Props.numStreams;
					Vector3 normalized = (a.CenterVector3 - drawPos).normalized;
					Func<IntVec3, bool> <>9__1;
					for (int i = 0; i < numStreams; i++)
					{
						float angle = Rand.Range(-this.Props.coneSizeDegrees, this.Props.coneSizeDegrees);
						Vector3 vector = normalized.RotatedBy(angle);
						Vector3 vect = drawPos + vector * (this.Props.range + Rand.Value * this.Props.rangeNoise);
						IntVec3 start = intVec;
						IntVec3 end = vect.ToIntVec3();
						Func<IntVec3, bool> validator;
						if ((validator = <>9__1) == null)
						{
							validator = (<>9__1 = ((IntVec3 c) => c.CanBeSeenOverFast(map)));
						}
						IntVec3 intVec2 = GenSight.LastPointOnLineOfSight(start, end, validator, true);
						if (!intVec2.IsValid)
						{
							intVec2 = vect.ToIntVec3();
						}
						float num = Vector3.Distance(intVec2.ToVector3(), drawPos);
						float num2 = Mathf.Clamp01(num / this.Props.sizeReductionDistanceThreshold);
						if (Vector3.Dot((intVec2.ToVector3() - drawPos).normalized, vector) > 0.5f)
						{
							MoteDualAttached mote = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_IncineratorBurst, new TargetInfo(intVec, map, false), new TargetInfo(intVec2, map, false));
							incineratorSpray.Add(new IncineratorProjectileMotion
							{
								mote = mote,
								targetDest = a.Cell,
								worldSource = drawPos + vector * this.Props.barrelOffsetDistance,
								worldTarget = intVec2.ToVector3(),
								moveVector = vector,
								startScale = Rand.Range(0.8f, 1.2f) * num2,
								endScale = (1f + Rand.Range(0.1f, 0.4f)) * num2,
								lifespanTicks = Mathf.FloorToInt(num * 5f) + Rand.Range(-this.Props.lifespanNoise, this.Props.lifespanNoise)
							});
							map.effecterMaintainer.AddEffecterToMaintain(this.Props.effecterDef.Spawn(intVec2, map, 1f), intVec2, 100);
						}
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}
	}
}
