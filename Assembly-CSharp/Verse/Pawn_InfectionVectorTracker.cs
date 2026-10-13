using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020007E2 RID: 2018
	public class Pawn_InfectionVectorTracker : IExposable
	{
		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x0600337A RID: 13178 RVA: 0x0010E56B File Offset: 0x0010C76B
		public IEnumerable<InfectionPathway> Pathways
		{
			get
			{
				return this.pathways.Values;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x0600337B RID: 13179 RVA: 0x0010E578 File Offset: 0x0010C778
		public int PathwaysCount
		{
			get
			{
				return this.pathways.Count;
			}
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x0010E585 File Offset: 0x0010C785
		public Pawn_InfectionVectorTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x0010E5A4 File Offset: 0x0010C7A4
		public void NotifySpawned(bool respawningAfterLoad)
		{
			if (respawningAfterLoad || this.givenPrearrival)
			{
				return;
			}
			this.givenPrearrival = true;
			if (Find.GameInfo.startingAndOptionalPawns.Contains(this.pawn) || this.pawn.DevelopmentalStage.Baby() || this.pawn.dontGivePreArrivalPathway)
			{
				return;
			}
			this.AddInfectionVector(InfectionPathwayDefOf.PrearrivalGeneric, null);
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x0010E607 File Offset: 0x0010C807
		public bool TryGetPathway(InfectionPathwayDef def, out InfectionPathway pathway)
		{
			return this.pathways.TryGetValue(def, out pathway);
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x0010E618 File Offset: 0x0010C818
		public List<InfectionPathway> GetPathwaysForHediff(HediffDef hediff)
		{
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (HediffInfectionPathway hediffInfectionPathway in hediff.possiblePathways)
			{
				InfectionPathway item;
				if (this.pathways.TryGetValue(hediffInfectionPathway.PathwayDef, out item))
				{
					Pawn_InfectionVectorTracker.TmpPathways.Add(item);
				}
			}
			return Pawn_InfectionVectorTracker.TmpPathways;
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x0010E694 File Offset: 0x0010C894
		public bool AnyPathwayForHediff(HediffDef hediff)
		{
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (HediffInfectionPathway hediffInfectionPathway in hediff.possiblePathways)
			{
				if (this.pathways.ContainsKey(hediffInfectionPathway.PathwayDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x0010E704 File Offset: 0x0010C904
		public void AddInfectionVector(InfectionPathwayDef def, Pawn source = null)
		{
			this.AddInfectionVector(new InfectionPathway(def, this.pawn, (source != null) ? source.kindDef : null));
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x0010E724 File Offset: 0x0010C924
		public void AddInfectionVector(InfectionPathway pathway)
		{
			if (this.pathways.ContainsKey(pathway.Def))
			{
				this.pathways[pathway.Def] = pathway;
				return;
			}
			if (this.pathways.Count >= 100)
			{
				this.ReducePathwaysForNewPathway();
			}
			this.pathways.Add(pathway.Def, pathway);
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x0010E780 File Offset: 0x0010C980
		public void InfectionTickInterval(int delta)
		{
			foreach (KeyValuePair<InfectionPathwayDef, InfectionPathway> keyValuePair in this.pathways)
			{
				InfectionPathwayDef infectionPathwayDef;
				InfectionPathway infectionPathway;
				keyValuePair.Deconstruct(out infectionPathwayDef, out infectionPathway);
				infectionPathway.TickInterval(delta);
			}
			if (this.pawn.IsHashIntervalTick(600, delta))
			{
				this.RemoveExpiredPathways();
			}
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x0010E7F8 File Offset: 0x0010C9F8
		private void RemoveExpiredPathways()
		{
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (KeyValuePair<InfectionPathwayDef, InfectionPathway> keyValuePair in this.pathways)
			{
				InfectionPathwayDef infectionPathwayDef;
				InfectionPathway infectionPathway;
				keyValuePair.Deconstruct(out infectionPathwayDef, out infectionPathway);
				InfectionPathway infectionPathway2 = infectionPathway;
				if (infectionPathway2.Expired)
				{
					Pawn_InfectionVectorTracker.TmpPathways.Add(infectionPathway2);
				}
			}
			foreach (InfectionPathway infectionPathway3 in Pawn_InfectionVectorTracker.TmpPathways)
			{
				this.pathways.Remove(infectionPathway3.Def);
			}
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x0010E8C8 File Offset: 0x0010CAC8
		private void ReducePathwaysForNewPathway()
		{
			if (this.pathways.Count <= 99)
			{
				return;
			}
			int num = this.pathways.Count - 100 + 1;
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (KeyValuePair<InfectionPathwayDef, InfectionPathway> keyValuePair in this.pathways)
			{
				InfectionPathwayDef infectionPathwayDef;
				InfectionPathway infectionPathway;
				keyValuePair.Deconstruct(out infectionPathwayDef, out infectionPathway);
				InfectionPathway item = infectionPathway;
				Pawn_InfectionVectorTracker.TmpPathways.Add(item);
			}
			Pawn_InfectionVectorTracker.TmpPathways.SortBy((InfectionPathway pathway) => pathway.Def.ExpiryTicks - pathway.AgeTicks);
			for (int i = 0; i < num; i++)
			{
				this.pathways.Remove(Pawn_InfectionVectorTracker.TmpPathways[i].Def);
			}
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x0010E9B8 File Offset: 0x0010CBB8
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.givenPrearrival, "givenPrearrival", false, false);
			Scribe_Collections.Look<InfectionPathwayDef, InfectionPathway>(ref this.pathways, "pathways", LookMode.Def, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pathways == null)
			{
				this.pathways = new Dictionary<InfectionPathwayDef, InfectionPathway>(20);
			}
		}

		// Token: 0x040026F5 RID: 9973
		private readonly Pawn pawn;

		// Token: 0x040026F6 RID: 9974
		private Dictionary<InfectionPathwayDef, InfectionPathway> pathways = new Dictionary<InfectionPathwayDef, InfectionPathway>(20);

		// Token: 0x040026F7 RID: 9975
		private bool givenPrearrival;

		// Token: 0x040026F8 RID: 9976
		private const int UpdateRateTicks = 600;

		// Token: 0x040026F9 RID: 9977
		private const int MaximumPathways = 100;

		// Token: 0x040026FA RID: 9978
		private static readonly List<InfectionPathway> TmpPathways = new List<InfectionPathway>();
	}
}
