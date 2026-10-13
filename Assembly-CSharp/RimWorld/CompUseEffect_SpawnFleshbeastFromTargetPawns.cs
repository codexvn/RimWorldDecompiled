using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F6E RID: 12142
	public class CompUseEffect_SpawnFleshbeastFromTargetPawns : CompUseEffect
	{
		// Token: 0x060111A8 RID: 70056 RVA: 0x004E90C3 File Offset: 0x004E72C3
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.targetable = this.parent.GetComp<CompTargetable>();
		}

		// Token: 0x060111A9 RID: 70057 RVA: 0x004E90E0 File Offset: 0x004E72E0
		public override void DoEffect(Pawn usedBy)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (this.targetable == null)
			{
				Log.Error("CompUseEffect_SpawnFleshbeastFromTargetPawns requires a CompTargetable");
				return;
			}
			Thing[] array = this.targetable.GetTargets(null).ToArray<Thing>();
			for (int i = 0; i < array.Length; i++)
			{
				Pawn pawn = array[i] as Pawn;
				if (pawn != null)
				{
					FleshbeastUtility.SpawnFleshbeastFromPawn(pawn, false, false, Array.Empty<PawnKindDef>());
				}
			}
		}

		// Token: 0x060111AA RID: 70058 RVA: 0x004E9142 File Offset: 0x004E7342
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetable == null)
			{
				this.targetable = this.parent.GetComp<CompTargetable>();
			}
		}

		// Token: 0x0400B186 RID: 45446
		private CompTargetable targetable;
	}
}
