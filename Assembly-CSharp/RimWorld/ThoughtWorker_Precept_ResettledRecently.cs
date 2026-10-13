using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183F RID: 6207
	public class ThoughtWorker_Precept_ResettledRecently : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x170015D4 RID: 5588
		// (get) Token: 0x06008F55 RID: 36693 RVA: 0x002A5857 File Offset: 0x002A3A57
		public int TicksSinceLastResettle
		{
			get
			{
				return GenTicks.TicksGame - Find.IdeoManager.lastResettledTick;
			}
		}

		// Token: 0x06008F56 RID: 36694 RVA: 0x002A5869 File Offset: 0x002A3A69
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!p.IsColonist)
			{
				return false;
			}
			return Find.TickManager.TicksGame > Find.IdeoManager.lastResettledTick;
		}

		// Token: 0x06008F57 RID: 36695 RVA: 0x002A58A4 File Offset: 0x002A3AA4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			float num = this.TicksSinceLastResettle.TicksToDays();
			foreach (ValueTuple<int, int> valueTuple in ThoughtWorker_Precept_ResettledRecently.Levels)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				if (num < (float)item)
				{
					return (item2 < 0) ? ThoughtState.Inactive : ThoughtState.ActiveAtStage(item2);
				}
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06008F58 RID: 36696 RVA: 0x002A5944 File Offset: 0x002A3B44
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return this.TicksSinceLastResettle.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION");
			yield break;
		}

		// Token: 0x04005C2B RID: 23595
		[TupleElementNames(new string[]
		{
			"day",
			"index"
		})]
		private static readonly List<ValueTuple<int, int>> Levels = new List<ValueTuple<int, int>>
		{
			new ValueTuple<int, int>(5, 0),
			new ValueTuple<int, int>(10, 1),
			new ValueTuple<int, int>(20, -1),
			new ValueTuple<int, int>(30, 2),
			new ValueTuple<int, int>(int.MaxValue, 3)
		};
	}
}
