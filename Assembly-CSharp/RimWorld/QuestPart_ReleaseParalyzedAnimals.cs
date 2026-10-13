using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7F RID: 7295
	public class QuestPart_ReleaseParalyzedAnimals : QuestPart
	{
		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x0600A28C RID: 41612 RVA: 0x002EBEF3 File Offset: 0x002EA0F3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A28D RID: 41613 RVA: 0x002EBF03 File Offset: 0x002EA103
		private bool IsParalyzed(Pawn pawn)
		{
			return pawn.health.hediffSet.HasHediff(HediffDefOf.Abasia, false);
		}

		// Token: 0x0600A28E RID: 41614 RVA: 0x002EBF1C File Offset: 0x002EA11C
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].RaceProps.Animal && this.IsParalyzed(this.pawns[i]) && this.pawns[i].Faction != null)
				{
					this.pawns[i].SetFaction(null, null);
				}
			}
		}

		// Token: 0x0600A28F RID: 41615 RVA: 0x002EBF98 File Offset: 0x002EA198
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A290 RID: 41616 RVA: 0x002EBFF4 File Offset: 0x002EA1F4
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04007076 RID: 28790
		public List<Pawn> pawns = new List<Pawn>();
	}
}
