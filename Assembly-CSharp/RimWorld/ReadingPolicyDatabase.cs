using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B49 RID: 6985
	public class ReadingPolicyDatabase : IExposable
	{
		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x06009BFA RID: 39930 RVA: 0x002D3EF8 File Offset: 0x002D20F8
		public List<ReadingPolicy> AllReadingPolicies
		{
			get
			{
				return this.readingPolicies;
			}
		}

		// Token: 0x06009BFB RID: 39931 RVA: 0x002D3F00 File Offset: 0x002D2100
		public ReadingPolicyDatabase()
		{
			this.GenerateStartingPolicies();
		}

		// Token: 0x06009BFC RID: 39932 RVA: 0x002D3F19 File Offset: 0x002D2119
		public void ExposeData()
		{
			Scribe_Collections.Look<ReadingPolicy>(ref this.readingPolicies, "readingPolicies", LookMode.Deep, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06009BFD RID: 39933 RVA: 0x002D3F37 File Offset: 0x002D2137
		public ReadingPolicy DefaultReadingPolicy()
		{
			if (this.readingPolicies.Count == 0)
			{
				this.MakeNewReadingPolicy();
			}
			return this.readingPolicies[0];
		}

		// Token: 0x06009BFE RID: 39934 RVA: 0x002D3F5C File Offset: 0x002D215C
		public void SetDefault(ReadingPolicy policy)
		{
			int index = this.readingPolicies.IndexOf(policy);
			ReadingPolicy value = this.readingPolicies[0];
			this.readingPolicies[0] = policy;
			this.readingPolicies[index] = value;
		}

		// Token: 0x06009BFF RID: 39935 RVA: 0x002D3FA0 File Offset: 0x002D21A0
		public AcceptanceReport TryDelete(ReadingPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.reading != null && pawn.reading.CurrentPolicy == policy)
				{
					return new AcceptanceReport("ReadingPolicyInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.reading != null && pawn2.reading.CurrentPolicy == policy)
				{
					pawn2.reading.CurrentPolicy = null;
				}
			}
			this.readingPolicies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009C00 RID: 39936 RVA: 0x002D4090 File Offset: 0x002D2290
		public ReadingPolicy MakeNewReadingPolicy()
		{
			int num;
			if (!this.readingPolicies.Any<ReadingPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.readingPolicies.Max((ReadingPolicy o) => o.id) + 1;
			}
			int id = num;
			ReadingPolicy readingPolicy = new ReadingPolicy(id, string.Format("{0} {1}", "ReadingPolicy".Translate(), id.ToString()));
			this.readingPolicies.Add(readingPolicy);
			return readingPolicy;
		}

		// Token: 0x06009C01 RID: 39937 RVA: 0x002D4110 File Offset: 0x002D2310
		private void GenerateStartingPolicies()
		{
			ReadingPolicy readingPolicy = this.MakeNewReadingPolicy();
			readingPolicy.label = "AllReadingPolicy".Translate();
			readingPolicy.defFilter.SetDisallowAll(null, null);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				if (thingDef.thingClass.SameOrSubclassOf<Book>())
				{
					readingPolicy.defFilter.SetAllow(thingDef, true);
				}
			}
			ReadingPolicy readingPolicy2 = this.MakeNewReadingPolicy();
			readingPolicy2.label = "TextbookPolicy".Translate();
			readingPolicy2.defFilter.SetDisallowAll(null, null);
			readingPolicy2.defFilter.SetAllow(ThingDefOf.TextBook, true);
			ReadingPolicy readingPolicy3 = this.MakeNewReadingPolicy();
			readingPolicy3.label = "SchematicPolicy".Translate();
			readingPolicy3.defFilter.SetDisallowAll(null, null);
			readingPolicy3.defFilter.SetAllow(ThingDefOf.Schematic, true);
			ReadingPolicy readingPolicy4 = this.MakeNewReadingPolicy();
			readingPolicy4.label = "NovelPolicy".Translate();
			readingPolicy4.defFilter.SetDisallowAll(null, null);
			readingPolicy4.defFilter.SetAllow(ThingDefOf.Novel, true);
			if (ModsConfig.AnomalyActive)
			{
				ReadingPolicy readingPolicy5 = this.MakeNewReadingPolicy();
				readingPolicy5.label = "TomePolicy".Translate();
				readingPolicy5.defFilter.SetDisallowAll(null, null);
				readingPolicy5.defFilter.SetAllow(ThingDefOf.Tome, true);
			}
			if (ModsConfig.OdysseyActive)
			{
				ReadingPolicy readingPolicy6 = this.MakeNewReadingPolicy();
				readingPolicy6.label = "MapPolicy".Translate();
				readingPolicy6.defFilter.SetDisallowAll(null, null);
				readingPolicy6.defFilter.SetAllow(ThingDefOf.Map, true);
			}
			ReadingPolicy readingPolicy7 = this.MakeNewReadingPolicy();
			readingPolicy7.label = "NoneReadingPolicy".Translate();
			readingPolicy7.defFilter.SetDisallowAll(null, null);
		}

		// Token: 0x04006C3C RID: 27708
		private List<ReadingPolicy> readingPolicies = new List<ReadingPolicy>();
	}
}
