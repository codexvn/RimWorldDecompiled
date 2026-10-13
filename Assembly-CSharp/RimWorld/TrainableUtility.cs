using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002555 RID: 9557
	public static class TrainableUtility
	{
		// Token: 0x17002043 RID: 8259
		// (get) Token: 0x0600D142 RID: 53570 RVA: 0x003D8EFC File Offset: 0x003D70FC
		public static List<TrainableDef> TrainableDefsInListOrder
		{
			get
			{
				return TrainableUtility.defsInListOrder;
			}
		}

		// Token: 0x0600D143 RID: 53571 RVA: 0x003D8F04 File Offset: 0x003D7104
		public static void Reset()
		{
			TrainableUtility.defsInListOrder.Clear();
			TrainableUtility.defsInListOrder.AddRange(from td in DefDatabase<TrainableDef>.AllDefsListForReading
			orderby td.listPriority descending
			select td);
			bool flag;
			do
			{
				flag = false;
				for (int i = 0; i < TrainableUtility.defsInListOrder.Count; i++)
				{
					TrainableDef trainableDef = TrainableUtility.defsInListOrder[i];
					if (trainableDef.prerequisites != null)
					{
						for (int j = 0; j < trainableDef.prerequisites.Count; j++)
						{
							if (trainableDef.indent <= trainableDef.prerequisites[j].indent)
							{
								trainableDef.indent = trainableDef.prerequisites[j].indent + 1;
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			while (flag);
		}

		// Token: 0x0600D144 RID: 53572 RVA: 0x003D8FCB File Offset: 0x003D71CB
		public static string MasterString(Pawn pawn)
		{
			if (pawn.playerSettings.Master == null)
			{
				return "(" + "NoneLower".TranslateSimple() + ")";
			}
			return RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
		}

		// Token: 0x0600D145 RID: 53573 RVA: 0x003D9005 File Offset: 0x003D7205
		public static int MinimumHandlingSkill(Pawn p)
		{
			return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill, true, -1));
		}

		// Token: 0x0600D146 RID: 53574 RVA: 0x003D901C File Offset: 0x003D721C
		public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, Pawn>(rect, pawn, new Func<Pawn, Pawn>(TrainableUtility.MasterSelectButton_GetMaster), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Pawn>>>(TrainableUtility.MasterSelectButton_GenerateMenu), TrainableUtility.MasterString(pawn).Truncate(rect.width, null), null, TrainableUtility.MasterString(pawn), null, null, paintable);
		}

		// Token: 0x0600D147 RID: 53575 RVA: 0x003D9065 File Offset: 0x003D7265
		private static Pawn MasterSelectButton_GetMaster(Pawn pet)
		{
			return pet.playerSettings.Master;
		}

		// Token: 0x0600D148 RID: 53576 RVA: 0x003D9072 File Offset: 0x003D7272
		private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
		{
			yield return new Widgets.DropdownMenuElement<Pawn>
			{
				option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate()
				{
					p.playerSettings.Master = null;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
				payload = null
			};
			using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Pawn col = enumerator.Current;
					string text = RelationsUtility.LabelWithBondInfo(col, p);
					Action action = null;
					if (TrainableUtility.CanBeMaster(col, p, true))
					{
						action = delegate()
						{
							p.playerSettings.Master = col;
						};
					}
					else
					{
						int level = col.skills.GetSkill(SkillDefOf.Animals).Level;
						int num = TrainableUtility.MinimumHandlingSkill(p);
						if (level < num)
						{
							action = null;
							text += " (" + "SkillTooLow".Translate(SkillDefOf.Animals.LabelCap, level, num) + ")";
						}
					}
					yield return new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = col
					};
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600D149 RID: 53577 RVA: 0x003D9084 File Offset: 0x003D7284
		public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
		{
			if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
			{
				return false;
			}
			if (ModsConfig.IdeologyActive && animal.RaceProps.animalType == AnimalType.Dryad && animal.connections != null)
			{
				foreach (Thing thing in animal.connections.ConnectedThings)
				{
					CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
					if (compTreeConnection != null && compTreeConnection.ConnectedPawn == master)
					{
						return true;
					}
				}
				return false;
			}
			if (master.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				return true;
			}
			int level = master.skills.GetSkill(SkillDefOf.Animals).Level;
			int num = TrainableUtility.MinimumHandlingSkill(animal);
			return level >= num;
		}

		// Token: 0x0600D14A RID: 53578 RVA: 0x003D9158 File Offset: 0x003D7358
		public static string GetIconTooltipText(Pawn pawn)
		{
			string text = "";
			if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
			{
				text += string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
			}
			IEnumerable<Pawn> allColonistBondsFor = TrainableUtility.GetAllColonistBondsFor(pawn);
			if (allColonistBondsFor.Any<Pawn>())
			{
				text += string.Format("{0}: {1}\n", "BondedTo".Translate(), (from bond in allColonistBondsFor
				select bond.LabelShort).ToCommaList(true, false));
			}
			return text.TrimEndNewlines();
		}

		// Token: 0x0600D14B RID: 53579 RVA: 0x003D9214 File Offset: 0x003D7414
		public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
		{
			return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonist
			select bond.otherPawn;
		}

		// Token: 0x0600D14C RID: 53580 RVA: 0x003D9274 File Offset: 0x003D7474
		public static int DegradationPeriodTicks(ThingDef def)
		{
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(def.GetStatValueAbstract(StatDefOf.Wildness, null)) * 60000f);
		}

		// Token: 0x0600D14D RID: 53581 RVA: 0x003D9297 File Offset: 0x003D7497
		public static int DegradationPeriodTicks(Pawn pawn)
		{
			if (pawn == null)
			{
				return 0;
			}
			return Mathf.RoundToInt(TrainableUtility.DecayIntervalDaysFromWildnessCurve.Evaluate(pawn.GetStatValue(StatDefOf.Wildness, true, 1)) * 60000f);
		}

		// Token: 0x0600D14E RID: 53582 RVA: 0x003D92C0 File Offset: 0x003D74C0
		public static bool TamenessCanDecay(ThingDef def)
		{
			return !def.race.FenceBlocked && def.GetStatValueAbstract(StatDefOf.Wildness, null) > 0.101f;
		}

		// Token: 0x0600D14F RID: 53583 RVA: 0x003D92E4 File Offset: 0x003D74E4
		public static bool TamenessCanDecay(Pawn pawn)
		{
			return !pawn.RaceProps.FenceBlocked && pawn.GetStatValue(StatDefOf.Wildness, true, 1) > 0.101f;
		}

		// Token: 0x0600D150 RID: 53584 RVA: 0x003D9309 File Offset: 0x003D7509
		public static bool TrainedTooRecently(Pawn animal)
		{
			return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 15000;
		}

		// Token: 0x0600D151 RID: 53585 RVA: 0x003D9328 File Offset: 0x003D7528
		public static bool IsReleasedToAttack(this Pawn pawn)
		{
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_PlayerSettings playerSettings = pawn.playerSettings;
				if (playerSettings == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn master = playerSettings.Master;
					flag = (((master != null) ? master.playerSettings : null) != null);
				}
			}
			return flag && pawn.training != null && pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release);
		}

		// Token: 0x0600D152 RID: 53586 RVA: 0x003D9390 File Offset: 0x003D7590
		public static TrainabilityDef GetTrainability(Pawn pawn)
		{
			if (pawn == null)
			{
				return null;
			}
			if (ModsConfig.OdysseyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.SentienceCatalyst, false))
			{
				return TrainableUtility.GetNextHighestTrainability(pawn.RaceProps.trainability);
			}
			return pawn.RaceProps.trainability;
		}

		// Token: 0x0600D153 RID: 53587 RVA: 0x003D93E0 File Offset: 0x003D75E0
		private static TrainabilityDef GetNextHighestTrainability(TrainabilityDef racePropsTrainability)
		{
			if (TrainableUtility.cachedTrainabilityDefs == null)
			{
				TrainableUtility.cachedTrainabilityDefs = DefDatabase<TrainabilityDef>.AllDefsListForReading.ToList<TrainabilityDef>();
				TrainableUtility.cachedTrainabilityDefs.SortBy((TrainabilityDef td) => td.intelligenceOrder);
			}
			for (int i = 0; i < TrainableUtility.cachedTrainabilityDefs.Count; i++)
			{
				if (TrainableUtility.cachedTrainabilityDefs[i] == racePropsTrainability)
				{
					return TrainableUtility.cachedTrainabilityDefs[Mathf.Min(TrainableUtility.cachedTrainabilityDefs.Count - 1, i + 1)];
				}
			}
			return null;
		}

		// Token: 0x04008E1D RID: 36381
		private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

		// Token: 0x04008E1E RID: 36382
		public const int MinTrainInterval = 15000;

		// Token: 0x04008E1F RID: 36383
		private static readonly SimpleCurve DecayIntervalDaysFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 12f),
				true
			},
			{
				new CurvePoint(1f, 6f),
				true
			}
		};

		// Token: 0x04008E20 RID: 36384
		private static List<TrainabilityDef> cachedTrainabilityDefs;
	}
}
