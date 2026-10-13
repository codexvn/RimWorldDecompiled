using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020003CE RID: 974
	public sealed class AutoSlaughterManager : IExposable
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001904 RID: 6404 RVA: 0x0007E0B8 File Offset: 0x0007C2B8
		public List<Pawn> AnimalsToSlaughter
		{
			get
			{
				if (this.cacheDirty)
				{
					try
					{
						this.animalsToSlaughterCached.Clear();
						foreach (AutoSlaughterConfig autoSlaughterConfig in this.configs)
						{
							if (autoSlaughterConfig.AnyLimit)
							{
								AutoSlaughterManager.tmpAnimals.Clear();
								AutoSlaughterManager.tmpAnimalsMale.Clear();
								AutoSlaughterManager.tmpAnimalsMaleYoung.Clear();
								AutoSlaughterManager.tmpAnimalsFemale.Clear();
								AutoSlaughterManager.tmpAnimalsFemaleYoung.Clear();
								AutoSlaughterManager.tmpAnimalsPregnant.Clear();
								foreach (Pawn pawn in this.map.mapPawns.SpawnedColonyAnimals)
								{
									if (pawn.def == autoSlaughterConfig.animal && AutoSlaughterManager.CanAutoSlaughterNow(pawn) && (autoSlaughterConfig.allowSlaughterBonded || pawn.relations.GetDirectRelationsCount(PawnRelationDefOf.Bond, null) <= 0))
									{
										if (pawn.gender == Gender.Male)
										{
											if (pawn.ageTracker.CurLifeStage.reproductive)
											{
												AutoSlaughterManager.tmpAnimalsMale.Add(pawn);
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsMaleYoung.Add(pawn);
											}
											AutoSlaughterManager.tmpAnimals.Add(pawn);
										}
										else if (pawn.gender == Gender.Female)
										{
											if (pawn.ageTracker.CurLifeStage.reproductive)
											{
												if (!pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, false))
												{
													AutoSlaughterManager.tmpAnimalsFemale.Add(pawn);
													AutoSlaughterManager.tmpAnimals.Add(pawn);
												}
												else if (autoSlaughterConfig.allowSlaughterPregnant)
												{
													AutoSlaughterManager.tmpAnimalsPregnant.Add(pawn);
												}
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsFemaleYoung.Add(pawn);
												AutoSlaughterManager.tmpAnimals.Add(pawn);
											}
										}
										else
										{
											AutoSlaughterManager.tmpAnimals.Add(pawn);
										}
									}
								}
								AutoSlaughterManager.tmpAnimals.SortByDescending((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsMale.SortByDescending((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsMaleYoung.SortByDescending((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsFemale.SortByDescending((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								AutoSlaughterManager.tmpAnimalsFemaleYoung.SortByDescending((Pawn a) => a.ageTracker.AgeBiologicalTicks);
								if (autoSlaughterConfig.allowSlaughterPregnant)
								{
									AutoSlaughterManager.tmpAnimalsPregnant.SortByDescending((Pawn a) => a.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, false).Severity);
									AutoSlaughterManager.tmpAnimalsFemale.AddRange(AutoSlaughterManager.tmpAnimalsPregnant);
									AutoSlaughterManager.tmpAnimals.AddRange(AutoSlaughterManager.tmpAnimalsPregnant);
								}
								if (autoSlaughterConfig.maxFemales != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsFemale.Count > autoSlaughterConfig.maxFemales)
									{
										Pawn item = AutoSlaughterManager.tmpAnimalsFemale.PopFront<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item);
										this.animalsToSlaughterCached.Add(item);
									}
								}
								if (autoSlaughterConfig.maxFemalesYoung != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsFemaleYoung.Count > autoSlaughterConfig.maxFemalesYoung)
									{
										Pawn item2 = AutoSlaughterManager.tmpAnimalsFemaleYoung.PopFront<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item2);
										this.animalsToSlaughterCached.Add(item2);
									}
								}
								if (autoSlaughterConfig.maxMales != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsMale.Count > autoSlaughterConfig.maxMales)
									{
										Pawn item3 = AutoSlaughterManager.tmpAnimalsMale.PopFront<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item3);
										this.animalsToSlaughterCached.Add(item3);
									}
								}
								if (autoSlaughterConfig.maxMalesYoung != -1)
								{
									while (AutoSlaughterManager.tmpAnimalsMaleYoung.Count > autoSlaughterConfig.maxMalesYoung)
									{
										Pawn item4 = AutoSlaughterManager.tmpAnimalsMaleYoung.PopFront<Pawn>();
										AutoSlaughterManager.tmpAnimals.Remove(item4);
										this.animalsToSlaughterCached.Add(item4);
									}
								}
								if (autoSlaughterConfig.maxTotal != -1)
								{
									while (AutoSlaughterManager.tmpAnimals.Count > autoSlaughterConfig.maxTotal)
									{
										Pawn pawn2 = AutoSlaughterManager.tmpAnimals.PopFront<Pawn>();
										if (pawn2.gender == Gender.Male)
										{
											if (pawn2.ageTracker.CurLifeStage.reproductive)
											{
												AutoSlaughterManager.tmpAnimalsMale.Remove(pawn2);
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsMaleYoung.Remove(pawn2);
											}
										}
										else if (pawn2.gender == Gender.Female)
										{
											if (pawn2.ageTracker.CurLifeStage.reproductive)
											{
												AutoSlaughterManager.tmpAnimalsFemale.Remove(pawn2);
											}
											else
											{
												AutoSlaughterManager.tmpAnimalsFemaleYoung.Remove(pawn2);
											}
										}
										this.animalsToSlaughterCached.Add(pawn2);
									}
								}
							}
						}
						this.cacheDirty = false;
					}
					finally
					{
						AutoSlaughterManager.tmpAnimals.Clear();
						AutoSlaughterManager.tmpAnimalsMale.Clear();
						AutoSlaughterManager.tmpAnimalsFemale.Clear();
						AutoSlaughterManager.tmpAnimalsMaleYoung.Clear();
						AutoSlaughterManager.tmpAnimalsFemaleYoung.Clear();
						AutoSlaughterManager.tmpAnimalsPregnant.Clear();
					}
				}
				return this.animalsToSlaughterCached;
			}
		}

		// Token: 0x06001905 RID: 6405 RVA: 0x0007E60C File Offset: 0x0007C80C
		public static bool CanEverAutoSlaughter(Pawn animal)
		{
			return animal.HomeFaction == Faction.OfPlayer && !animal.RaceProps.Dryad;
		}

		// Token: 0x06001906 RID: 6406 RVA: 0x0007E62B File Offset: 0x0007C82B
		public static bool CanAutoSlaughterNow(Pawn animal)
		{
			return AutoSlaughterManager.CanEverAutoSlaughter(animal) && animal.GetLord() == null && (animal.inventory == null || !animal.inventory.UnloadEverything);
		}

		// Token: 0x06001907 RID: 6407 RVA: 0x0007E659 File Offset: 0x0007C859
		public AutoSlaughterManager(Map map)
		{
			this.map = map;
			this.TryPopulateMissingAnimals();
		}

		// Token: 0x06001908 RID: 6408 RVA: 0x0007E684 File Offset: 0x0007C884
		public void Notify_PawnDespawned()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06001909 RID: 6409 RVA: 0x0007E684 File Offset: 0x0007C884
		public void Notify_PawnSpawned()
		{
			this.cacheDirty = true;
		}

		// Token: 0x0600190A RID: 6410 RVA: 0x0007E684 File Offset: 0x0007C884
		public void Notify_PawnChangedFaction()
		{
			this.cacheDirty = true;
		}

		// Token: 0x0600190B RID: 6411 RVA: 0x0007E684 File Offset: 0x0007C884
		public void Notify_ConfigChanged()
		{
			this.cacheDirty = true;
		}

		// Token: 0x0600190C RID: 6412 RVA: 0x0007E690 File Offset: 0x0007C890
		public void ExposeData()
		{
			Scribe_Collections.Look<AutoSlaughterConfig>(ref this.configs, "configs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.configs.RemoveAll((AutoSlaughterConfig x) => x.animal == null || x.animal.IsCorpse) != 0)
				{
					Log.Warning("Some auto-slaughter configs had null animals after loading.");
				}
				this.TryPopulateMissingAnimals();
			}
		}

		// Token: 0x0600190D RID: 6413 RVA: 0x0007E6F8 File Offset: 0x0007C8F8
		private void TryPopulateMissingAnimals()
		{
			HashSet<ThingDef> hashSet = new HashSet<ThingDef>();
			hashSet.AddRange(from c in this.configs
			select c.animal);
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.race != null && thingDef.race.Animal && thingDef.GetStatValueAbstract(StatDefOf.Wildness, null) < 1f && !thingDef.race.Dryad && !thingDef.IsCorpse && !hashSet.Contains(thingDef))
				{
					this.configs.Add(new AutoSlaughterConfig
					{
						animal = thingDef
					});
				}
			}
		}

		// Token: 0x0400161E RID: 5662
		private static List<Pawn> tmpAnimals = new List<Pawn>();

		// Token: 0x0400161F RID: 5663
		private static List<Pawn> tmpAnimalsMale = new List<Pawn>();

		// Token: 0x04001620 RID: 5664
		private static List<Pawn> tmpAnimalsMaleYoung = new List<Pawn>();

		// Token: 0x04001621 RID: 5665
		private static List<Pawn> tmpAnimalsFemale = new List<Pawn>();

		// Token: 0x04001622 RID: 5666
		private static List<Pawn> tmpAnimalsFemaleYoung = new List<Pawn>();

		// Token: 0x04001623 RID: 5667
		private static List<Pawn> tmpAnimalsPregnant = new List<Pawn>();

		// Token: 0x04001624 RID: 5668
		public Map map;

		// Token: 0x04001625 RID: 5669
		public List<AutoSlaughterConfig> configs = new List<AutoSlaughterConfig>();

		// Token: 0x04001626 RID: 5670
		private List<Pawn> animalsToSlaughterCached = new List<Pawn>();

		// Token: 0x04001627 RID: 5671
		private bool cacheDirty;
	}
}
