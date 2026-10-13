using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200033D RID: 829
	public class GameInitData
	{
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x00063A33 File Offset: 0x00061C33
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00063A4D File Offset: 0x00061C4D
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00063A5A File Offset: 0x00061C5A
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = PlanetTile.Invalid;
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00063A84 File Offset: 0x00061C84
		public override string ToString()
		{
			return string.Format("startedFromEntry: {0}\nstartingAndOptionalPawns: {1}", this.startedFromEntry, this.startingAndOptionalPawns.Count);
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00063AAC File Offset: 0x00061CAC
		public void PrepForMapGen()
		{
			while (this.startingAndOptionalPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingAndOptionalPawns[this.startingPawnCount]);
				this.startingAndOptionalPawns[this.startingPawnCount].wasLeftBehindStartingPawn = true;
				Find.WorldPawns.PassToWorld(this.startingAndOptionalPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				Pawn pawn = this.startingAndOptionalPawns[this.startingPawnCount];
				this.startingAndOptionalPawns.Remove(pawn);
				this.startingPossessions.Remove(pawn);
			}
			List<Pawn> list = this.startingAndOptionalPawns;
			foreach (Pawn pawn2 in list)
			{
				pawn2.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2, false);
			}
			foreach (Pawn pawn3 in list)
			{
				pawn3.workSettings.DisableAll();
			}
			using (IEnumerator<WorkTypeDef> enumerator2 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					WorkTypeDef w = enumerator2.Current;
					if (w.alwaysStartActive)
					{
						IEnumerable<Pawn> source = list;
						Func<Pawn, bool> predicate;
						Func<Pawn, bool> <>9__0;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((Pawn col) => !col.WorkTypeIsDisabled(w)));
						}
						using (IEnumerator<Pawn> enumerator3 = source.Where(predicate).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn4 = enumerator3.Current;
								pawn4.workSettings.SetPriority(w, 3);
							}
							continue;
						}
					}
					bool flag = false;
					foreach (Pawn pawn5 in list)
					{
						if (!pawn5.WorkTypeIsDisabled(w) && pawn5.skills.AverageOfRelevantSkillsFor(w) >= 6f)
						{
							pawn5.workSettings.SetPriority(w, 3);
							flag = true;
						}
					}
					if (!flag)
					{
						IEnumerable<Pawn> source2 = from col in list
						where !col.WorkTypeIsDisabled(w)
						select col;
						if (source2.Any<Pawn>())
						{
							source2.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w)).workSettings.SetPriority(w, 3);
						}
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				for (int i = 0; i < this.startingAndOptionalPawns.Count; i++)
				{
					CustomXenotype forcedCustomXenotype = StartingPawnUtility.GetGenerationRequest(i).ForcedCustomXenotype;
					if (forcedCustomXenotype != null && !Current.Game.customXenotypeDatabase.customXenotypes.Contains(forcedCustomXenotype))
					{
						Current.Game.customXenotypeDatabase.customXenotypes.Add(forcedCustomXenotype);
					}
				}
			}
		}

		// Token: 0x040013A9 RID: 5033
		public PlanetTile startingTile = PlanetTile.Invalid;

		// Token: 0x040013AA RID: 5034
		public MapGeneratorDef mapGeneratorDef;

		// Token: 0x040013AB RID: 5035
		public int mapSize = 250;

		// Token: 0x040013AC RID: 5036
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x040013AD RID: 5037
		public Dictionary<Pawn, List<ThingDefCount>> startingPossessions = new Dictionary<Pawn, List<ThingDefCount>>();

		// Token: 0x040013AE RID: 5038
		public int startingPawnCount = -1;

		// Token: 0x040013AF RID: 5039
		public Faction playerFaction;

		// Token: 0x040013B0 RID: 5040
		public Season startingSeason;

		// Token: 0x040013B1 RID: 5041
		public bool permadeathChosen;

		// Token: 0x040013B2 RID: 5042
		public bool permadeath;

		// Token: 0x040013B3 RID: 5043
		public PawnKindDef startingPawnKind;

		// Token: 0x040013B4 RID: 5044
		public List<PawnKindCount> startingPawnsRequired;

		// Token: 0x040013B5 RID: 5045
		public List<XenotypeCount> startingXenotypesRequired;

		// Token: 0x040013B6 RID: 5046
		public List<MutantCount> startingMutantsRequired;

		// Token: 0x040013B7 RID: 5047
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x040013B8 RID: 5048
		public bool startedFromEntry;

		// Token: 0x040013B9 RID: 5049
		public string gameToLoad;

		// Token: 0x040013BA RID: 5050
		public const int DefaultMapSize = 250;
	}
}
