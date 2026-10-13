using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003853 RID: 14419
	public class HediffComp_DissolveGearOnDeath : HediffComp
	{
		// Token: 0x170032EA RID: 13034
		// (get) Token: 0x06013F0B RID: 81675 RVA: 0x005EA8EF File Offset: 0x005E8AEF
		public HediffCompProperties_DissolveGearOnDeath Props
		{
			get
			{
				return (HediffCompProperties_DissolveGearOnDeath)this.props;
			}
		}

		// Token: 0x06013F0C RID: 81676 RVA: 0x005EA8FC File Offset: 0x005E8AFC
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			if (this.Props.injuryCreatedOnDeath != null)
			{
				List<BodyPartRecord> list = new List<BodyPartRecord>(from part in base.Pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
				where part.coverageAbs > 0f
				select part);
				int num = Mathf.Min(this.Props.injuryCount.RandomInRange, list.Count);
				for (int i = 0; i < num; i++)
				{
					int index = Rand.Range(0, list.Count);
					BodyPartRecord bodyPartRecord = list[index];
					list.RemoveAt(index);
					if (base.Pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(bodyPartRecord))
					{
						base.Pawn.health.AddHediff(this.Props.injuryCreatedOnDeath, bodyPartRecord, null, null);
					}
				}
			}
		}

		// Token: 0x06013F0D RID: 81677 RVA: 0x005EA9F4 File Offset: 0x005E8BF4
		public override void Notify_PawnKilled()
		{
			base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			if (!base.Pawn.Spawned)
			{
				return;
			}
			if (this.Props.mote != null || this.Props.fleck != null)
			{
				Vector3 drawPos = base.Pawn.DrawPos;
				for (int i = 0; i < this.Props.moteCount; i++)
				{
					Vector2 vector = Rand.InsideUnitCircle * this.Props.moteOffsetRange.RandomInRange * (float)Rand.Sign;
					Vector3 loc = new Vector3(drawPos.x + vector.x, drawPos.y, drawPos.z + vector.y);
					if (this.Props.mote != null)
					{
						MoteMaker.MakeStaticMote(loc, base.Pawn.Map, this.Props.mote, 1f, false, 0f);
					}
					else
					{
						FleckMaker.Static(loc, base.Pawn.Map, this.Props.fleck, 1f);
					}
				}
			}
			if (this.Props.filth != null)
			{
				FilthMaker.TryMakeFilth(base.Pawn.Position, base.Pawn.Map, this.Props.filth, this.Props.filthCount, FilthSourceFlags.None, true);
			}
			if (this.Props.sound != null)
			{
				this.Props.sound.PlayOneShot(SoundInfo.InMap(base.Pawn, MaintenanceType.None));
			}
		}
	}
}
