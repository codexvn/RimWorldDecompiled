using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD0 RID: 12240
	public class Alert_Digesting : Alert_Critical
	{
		// Token: 0x17002CBF RID: 11455
		// (get) Token: 0x06011404 RID: 70660 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002CC0 RID: 11456
		// (get) Token: 0x06011405 RID: 70661 RVA: 0x004F16A4 File Offset: 0x004EF8A4
		private List<Pawn> Digesting
		{
			get
			{
				this.digesting.Clear();
				foreach (Map map in Find.Maps)
				{
					using (IEnumerator<Pawn> enumerator2 = map.mapPawns.AllPawnsSpawned.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CompDevourer compDevourer;
							if (enumerator2.Current.TryGetComp(out compDevourer) && compDevourer.Digesting && (compDevourer.DigestingPawn.IsColonist || compDevourer.DigestingPawn.IsColonyMech))
							{
								this.digesting.Add(compDevourer.DigestingPawn);
							}
						}
					}
				}
				return this.digesting;
			}
		}

		// Token: 0x06011406 RID: 70662 RVA: 0x004F1774 File Offset: 0x004EF974
		public Alert_Digesting()
		{
			this.defaultLabel = "AlertDigestion".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x06011407 RID: 70663 RVA: 0x004F17A4 File Offset: 0x004EF9A4
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.digesting)
			{
				int ticksLeftThisToil = ((Pawn)pawn.SpawnedParentOrMe).jobs.curDriver.ticksLeftThisToil;
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve().CapitalizeFirst() + ": " + ticksLeftThisToil.ToStringSecondsFromTicks());
			}
			return string.Format("{0}:\n{1}\n\n{2}", "AlertDigestionDesc".Translate(), stringBuilder.ToString().TrimEndNewlines(), "AlertDigestionDescAppended".Translate());
		}

		// Token: 0x06011408 RID: 70664 RVA: 0x004F187C File Offset: 0x004EFA7C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Digesting);
		}

		// Token: 0x0400B2FE RID: 45822
		private readonly List<Pawn> digesting = new List<Pawn>();
	}
}
