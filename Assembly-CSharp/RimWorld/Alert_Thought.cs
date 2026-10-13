using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003060 RID: 12384
	public abstract class Alert_Thought : Alert
	{
		// Token: 0x17002CFD RID: 11517
		// (get) Token: 0x06011635 RID: 71221
		protected abstract ThoughtDef Thought { get; }

		// Token: 0x06011636 RID: 71222 RVA: 0x004FCF10 File Offset: 0x004FB110
		public override string GetLabel()
		{
			int count = this.AffectedPawns.Count;
			string label = base.GetLabel();
			if (count > 1)
			{
				return label + " x" + count.ToString();
			}
			return label;
		}

		// Token: 0x17002CFE RID: 11518
		// (get) Token: 0x06011637 RID: 71223 RVA: 0x004FCF48 File Offset: 0x004FB148
		private List<Pawn> AffectedPawns
		{
			get
			{
				this.affectedPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					if (pawn.Dead)
					{
						string str = "Dead pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists:";
						Pawn pawn2 = pawn;
						Log.Error(str + ((pawn2 != null) ? pawn2.ToString() : null));
					}
					else if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.GetMoodThoughtsFor(this.Thought, Alert_Thought.tmpThoughts);
						for (int i = 0; i < Alert_Thought.tmpThoughts.Count; i++)
						{
							if (!ThoughtUtility.ThoughtNullified(pawn, Alert_Thought.tmpThoughts[i].def))
							{
								this.affectedPawnsResult.Add(pawn);
								break;
							}
						}
						Alert_Thought.tmpThoughts.Clear();
					}
				}
				return this.affectedPawnsResult;
			}
		}

		// Token: 0x06011638 RID: 71224 RVA: 0x004FD044 File Offset: 0x004FB244
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AffectedPawns);
		}

		// Token: 0x06011639 RID: 71225 RVA: 0x004FD054 File Offset: 0x004FB254
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.AffectedPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return this.explanationKey.Translate(stringBuilder.ToString());
		}

		// Token: 0x0400B3F6 RID: 46070
		protected string explanationKey;

		// Token: 0x0400B3F7 RID: 46071
		private static readonly List<Thought> tmpThoughts = new List<Thought>();

		// Token: 0x0400B3F8 RID: 46072
		private readonly List<Pawn> affectedPawnsResult = new List<Pawn>();
	}
}
