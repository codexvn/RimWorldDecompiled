using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C04 RID: 15364
	public class QuestPart_Hyperlinks : QuestPart
	{
		// Token: 0x17003386 RID: 13190
		// (get) Token: 0x06014D67 RID: 85351 RVA: 0x0063E11F File Offset: 0x0063C31F
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = this.GetHyperlinks();
				}
				return this.cachedHyperlinks;
			}
		}

		// Token: 0x06014D68 RID: 85352 RVA: 0x0063E13B File Offset: 0x0063C33B
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			if (this.thingDefs != null)
			{
				int num;
				for (int i = 0; i < this.thingDefs.Count; i = num + 1)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.thingDefs[i], -1);
					num = i;
				}
			}
			if (this.pawns != null)
			{
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					if (this.pawns[i].royalty != null && this.pawns[i].royalty.AllTitlesForReading.Any<RoyalTitle>())
					{
						RoyalTitle mostSeniorTitle = this.pawns[i].royalty.MostSeniorTitle;
						if (mostSeniorTitle != null)
						{
							yield return new Dialog_InfoCard.Hyperlink(mostSeniorTitle.def, mostSeniorTitle.faction, -1);
						}
					}
					num = i;
				}
			}
			if (this.factions != null)
			{
				int num;
				for (int i = 0; i < this.factions.Count; i = num + 1)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.factions[i], -1, false);
					num = i;
				}
			}
			if (this.researchProjects != null)
			{
				int num;
				for (int i = 0; i < this.researchProjects.Count; i = num + 1)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.researchProjects[i]);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06014D69 RID: 85353 RVA: 0x0063E14C File Offset: 0x0063C34C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ThingDef>(ref this.thingDefs, "thingDefs", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingDefs == null)
				{
					this.thingDefs = new List<ThingDef>();
				}
				this.thingDefs.RemoveAll((ThingDef x) => x == null);
				if (this.pawns == null)
				{
					this.pawns = new List<Pawn>();
				}
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06014D6A RID: 85354 RVA: 0x0063E20F File Offset: 0x0063C40F
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400E854 RID: 59476
		public List<ThingDef> thingDefs = new List<ThingDef>();

		// Token: 0x0400E855 RID: 59477
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400E856 RID: 59478
		public List<Faction> factions = new List<Faction>();

		// Token: 0x0400E857 RID: 59479
		public List<ResearchProjectDef> researchProjects = new List<ResearchProjectDef>();

		// Token: 0x0400E858 RID: 59480
		private IEnumerable<Dialog_InfoCard.Hyperlink> cachedHyperlinks;
	}
}
