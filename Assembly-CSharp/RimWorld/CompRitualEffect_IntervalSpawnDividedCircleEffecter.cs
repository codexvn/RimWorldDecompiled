using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027FE RID: 10238
	public class CompRitualEffect_IntervalSpawnDividedCircleEffecter : CompRitualEffect_IntervalSpawnBurst
	{
		// Token: 0x1700222F RID: 8751
		// (get) Token: 0x0600DFF6 RID: 57334 RVA: 0x0041C23A File Offset: 0x0041A43A
		protected new CompProperties_RitualEffectIntervalSpawnDividedCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectIntervalSpawnDividedCircle)this.props;
			}
		}

		// Token: 0x0600DFF7 RID: 57335 RVA: 0x001E5B28 File Offset: 0x001E3D28
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x0600DFF8 RID: 57336 RVA: 0x0041C248 File Offset: 0x0041A448
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			float num = 360f / (float)this.Props.numCopies;
			for (int i = 0; i < this.Props.numCopies; i++)
			{
				Vector3 a = Quaternion.AngleAxis(num * (float)i, Vector3.up) * Vector3.forward;
				IntVec3 cell = parent.ritual.selectedTarget.Cell;
				TargetInfo targetInfo = new TargetInfo(cell, parent.ritual.Map, false);
				Vector3 vector = (a * this.Props.radius + this.Props.offset) * base.ScaleForRoom(ritual);
				Room room = (cell + vector.ToIntVec3() + this.Props.roomCheckOffset).GetRoom(ritual.Map);
				if (!this.props.onlySpawnInSameRoom || room == ritual.GetRoom)
				{
					Effecter effecter = this.Props.effecterDef.Spawn(cell, parent.ritual.Map, vector, 1f);
					effecter.Trigger(targetInfo, TargetInfo.Invalid, -1);
					parent.AddEffecterToMaintain(targetInfo, effecter);
				}
			}
		}
	}
}
