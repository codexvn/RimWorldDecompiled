using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D64 RID: 11620
	public class CompMetalhorrorInfectible : ThingComp
	{
		// Token: 0x17002983 RID: 10627
		// (get) Token: 0x06010533 RID: 66867 RVA: 0x004B699F File Offset: 0x004B4B9F
		public int Infections
		{
			get
			{
				return this.infectedPawns.Count;
			}
		}

		// Token: 0x06010534 RID: 66868 RVA: 0x004B69AC File Offset: 0x004B4BAC
		public override void Notify_RecipeProduced(Pawn pawn)
		{
			if (Rand.Chance(0.04f) && MetalhorrorUtility.IsInfected(pawn))
			{
				this.infectedPawns.Add(pawn);
			}
		}

		// Token: 0x06010535 RID: 66869 RVA: 0x004B69D0 File Offset: 0x004B4BD0
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompMetalhorrorInfectible compMetalhorrorInfectible = piece.TryGetComp<CompMetalhorrorInfectible>();
			Pawn item;
			if (this.TryTakeInfector(out item))
			{
				compMetalhorrorInfectible.infectedPawns.Add(item);
			}
		}

		// Token: 0x06010536 RID: 66870 RVA: 0x004B6A04 File Offset: 0x004B4C04
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompMetalhorrorInfectible compMetalhorrorInfectible = otherStack.TryGetComp<CompMetalhorrorInfectible>();
			this.infectedPawns.AddRange(compMetalhorrorInfectible.infectedPawns);
		}

		// Token: 0x06010537 RID: 66871 RVA: 0x004B6A34 File Offset: 0x004B4C34
		public override void PostIngested(Pawn ingester)
		{
			Pawn source;
			if (this.TryTakeInfector(out source))
			{
				MetalhorrorUtility.Infect(ingester, source, "FoodImplant", null);
			}
		}

		// Token: 0x06010538 RID: 66872 RVA: 0x004B6A58 File Offset: 0x004B4C58
		public override string CompInspectStringExtra()
		{
			if (!DebugSettings.godMode || this.infectedPawns.Empty<Pawn>())
			{
				return null;
			}
			return string.Format("Infections: {0}", this.infectedPawns.Count);
		}

		// Token: 0x06010539 RID: 66873 RVA: 0x004B6A8C File Offset: 0x004B4C8C
		private bool TryTakeInfector(out Pawn infector)
		{
			if (this.Infections <= 0)
			{
				infector = null;
				return false;
			}
			int index = Rand.Range(0, this.infectedPawns.Count);
			Pawn pawn = this.infectedPawns[index];
			this.infectedPawns.RemoveAt(index);
			infector = pawn;
			return true;
		}

		// Token: 0x0601053A RID: 66874 RVA: 0x004B6AD6 File Offset: 0x004B4CD6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.infectedPawns, "infectedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0400AA4E RID: 43598
		private List<Pawn> infectedPawns = new List<Pawn>();

		// Token: 0x0400AA4F RID: 43599
		private const float InfectionChance = 0.04f;
	}
}
