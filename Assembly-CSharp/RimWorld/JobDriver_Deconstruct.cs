using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200116E RID: 4462
	public class JobDriver_Deconstruct : JobDriver_RemoveBuilding
	{
		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06006D28 RID: 27944 RVA: 0x00223A33 File Offset: 0x00221C33
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Deconstruct;
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x06006D29 RID: 27945 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override EffecterDef WorkEffecter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x06006D2A RID: 27946 RVA: 0x00223A3A File Offset: 0x00221C3A
		protected override float TotalNeededWork
		{
			get
			{
				return Mathf.Clamp(base.Building.GetStatValue(StatDefOf.WorkToBuild, true, -1), 20f, 3000f);
			}
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x00223A5D File Offset: 0x00221C5D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Building == null || !base.Building.DeconstructibleBy(this.pawn.Faction));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x00223A6D File Offset: 0x00221C6D
		protected override void FinishedRemoving()
		{
			base.Target.Destroy(DestroyMode.Deconstruct);
			this.pawn.records.Increment(RecordDefOf.ThingsDeconstructed);
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x00223A90 File Offset: 0x00221C90
		protected override void TickActionInterval(int delta)
		{
			if (this.pawn.skills != null && base.Building.def.CostListAdjusted(base.Building.Stuff, true).Count > 0)
			{
				this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta, false, false);
			}
		}

		// Token: 0x04004C84 RID: 19588
		private const float MaxDeconstructWork = 3000f;

		// Token: 0x04004C85 RID: 19589
		private const float MinDeconstructWork = 20f;
	}
}
