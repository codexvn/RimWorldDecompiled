using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025E9 RID: 9705
	public static class IdeoDevelopmentUtility
	{
		// Token: 0x0600D4E8 RID: 54504 RVA: 0x003EDC44 File Offset: 0x003EBE44
		public static int DevelopmentPointsForQuestSuccess(Ideo ideo, QuestScriptDef root)
		{
			int num = 0;
			if (root.successHistoryEvent == null || !ideo.Fluid)
			{
				return num;
			}
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				for (int j = 0; j < preceptsListForReading[i].def.comps.Count; j++)
				{
					PreceptComp_DevelopmentPoints preceptComp_DevelopmentPoints = preceptsListForReading[i].def.comps[j] as PreceptComp_DevelopmentPoints;
					if (preceptComp_DevelopmentPoints != null && preceptComp_DevelopmentPoints.eventDef == root.successHistoryEvent)
					{
						num += preceptComp_DevelopmentPoints.points;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D4E9 RID: 54505 RVA: 0x003EDCDC File Offset: 0x003EBEDC
		public static SimpleCurve GetDevelopmentPointsOverOutcomeIndexCurveForRitual(Ideo ideo, Precept_Ritual ritual)
		{
			RitualOutcomeEffectWorker_FromQuality ritualOutcomeEffectWorker_FromQuality = ritual.outcomeEffect as RitualOutcomeEffectWorker_FromQuality;
			if (ritualOutcomeEffectWorker_FromQuality == null || !ritualOutcomeEffectWorker_FromQuality.GivesDevelopmentPoints)
			{
				return null;
			}
			if (ritual.isAnytime || ritual.def == PreceptDefOf.Funeral)
			{
				return IdeoDevelopmentUtility.AnytimeAndFuneralRitualDevelopmentPointsOverOutcomeIndex;
			}
			if (ritual.IsDateTriggered)
			{
				int num = Mathf.Clamp(ideo.PreceptsListForReading.Count(delegate(Precept p)
				{
					Precept_Ritual precept_Ritual = p as Precept_Ritual;
					return precept_Ritual != null && precept_Ritual.IsDateTriggered;
				}) - 1, 0, IdeoDevelopmentUtility.DateRitualDevelopmentCurvesByCount.Length - 1);
				return IdeoDevelopmentUtility.DateRitualDevelopmentCurvesByCount[num];
			}
			return null;
		}

		// Token: 0x0600D4EA RID: 54506 RVA: 0x003EDD6C File Offset: 0x003EBF6C
		public static void GetAllRitualsThatGiveDevelopmentPoints(Ideo ideo, List<Precept_Ritual> rituals)
		{
			if (!ideo.Fluid)
			{
				return;
			}
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept_Ritual precept_Ritual = preceptsListForReading[i] as Precept_Ritual;
				if (precept_Ritual != null && IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(ideo, precept_Ritual) != null)
				{
					rituals.Add(precept_Ritual);
				}
			}
		}

		// Token: 0x0600D4EB RID: 54507 RVA: 0x003EDDBC File Offset: 0x003EBFBC
		public static void GetAllQuestSuccessEventsThatGiveDevelopmentPoints(Ideo ideo, List<HistoryEventDef> successEvents)
		{
			if (!ideo.Fluid)
			{
				return;
			}
			foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
			{
				if (!successEvents.Contains(questScriptDef.successHistoryEvent) && IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(ideo, questScriptDef) > 0)
				{
					successEvents.Add(questScriptDef.successHistoryEvent);
				}
			}
		}

		// Token: 0x0600D4EC RID: 54508 RVA: 0x003EDE30 File Offset: 0x003EC030
		public static void ConfirmChangesToIdeo(Ideo ideo, Ideo newIdeo, Action confirmCallback)
		{
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
			IdeoDevelopmentUtility.GetPreceptsToRemove(ideo, newIdeo, IdeoDevelopmentUtility.toRemovePrecepts);
			string text = "";
			for (int i = 0; i < IdeoDevelopmentUtility.toRemovePrecepts.Count; i++)
			{
				string str;
				if (IdeoDevelopmentUtility.toRemovePrecepts[i].TryGetLostByReformingWarning(out str))
				{
					text += str;
				}
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n" + "ReformIdeoContinue".Translate();
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmCallback, false, null, WindowLayer.Dialog));
			}
			else
			{
				confirmCallback();
			}
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
		}

		// Token: 0x0600D4ED RID: 54509 RVA: 0x003EDEE0 File Offset: 0x003EC0E0
		public static void ApplyChangesToIdeo(Ideo ideo, Ideo newIdeo)
		{
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
			IdeoDevelopmentUtility.GetPreceptsToRemove(ideo, newIdeo, IdeoDevelopmentUtility.toRemovePrecepts);
			ideo.development.Notify_PreReform(newIdeo);
			newIdeo.CopyTo(ideo);
			IdeoDevelopmentUtility.UpdateAllStyleReferences(ideo);
			ideo.development.Notify_Reformed();
			for (int i = 0; i < IdeoDevelopmentUtility.toRemovePrecepts.Count; i++)
			{
				IdeoDevelopmentUtility.toRemovePrecepts[i].Notify_RemovedByReforming();
			}
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
		}

		// Token: 0x0600D4EE RID: 54510 RVA: 0x003EDF58 File Offset: 0x003EC158
		private static void UpdateAllStyleReferences(Ideo ideo)
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Building thing in map.listerBuildings.allBuildingsColonist)
				{
					IdeoDevelopmentUtility.UpdateThingStyleReference(thing, ideo);
				}
			}
		}

		// Token: 0x0600D4EF RID: 54511 RVA: 0x003EDFE8 File Offset: 0x003EC1E8
		private static void UpdateThingStyleReference(Thing thing, Ideo ideo)
		{
			if (thing.StyleSourcePrecept == null || thing.StyleSourcePrecept.ideo != ideo)
			{
				return;
			}
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			Precept_ThingStyle precept_ThingStyle = (Precept_ThingStyle)preceptsListForReading.FirstOrDefault((Precept p) => p.Id == thing.StyleSourcePrecept.Id);
			if (precept_ThingStyle != null)
			{
				thing.StyleSourcePrecept = precept_ThingStyle;
			}
			else
			{
				precept_ThingStyle = (Precept_ThingStyle)preceptsListForReading.FirstOrDefault(delegate(Precept p)
				{
					Precept_ThingStyle precept_ThingStyle2 = p as Precept_ThingStyle;
					return precept_ThingStyle2 != null && precept_ThingStyle2.ThingDef == thing.def;
				});
				thing.StyleSourcePrecept = precept_ThingStyle;
			}
			if (thing.Spawned)
			{
				thing.DirtyMapMesh(thing.Map);
			}
		}

		// Token: 0x0600D4F0 RID: 54512 RVA: 0x003EE09C File Offset: 0x003EC29C
		private static void GetPreceptsToRemove(Ideo ideo, Ideo newIdeo, List<Precept> preceptsOut)
		{
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			List<Precept> preceptsListForReading2 = newIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < preceptsListForReading2.Count; j++)
				{
					if (preceptsListForReading2[j].Id == preceptsListForReading[i].Id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					preceptsOut.Add(preceptsListForReading[i]);
				}
			}
		}

		// Token: 0x0400909A RID: 37018
		private static readonly SimpleCurve AnytimeAndFuneralRitualDevelopmentPointsOverOutcomeIndex = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 2f),
				true
			}
		};

		// Token: 0x0400909B RID: 37019
		private static readonly SimpleCurve[] DateRitualDevelopmentCurvesByCount = new SimpleCurve[]
		{
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 2f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				},
				{
					new CurvePoint(2f, 6f),
					true
				},
				{
					new CurvePoint(3f, 7f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				},
				{
					new CurvePoint(2f, 3f),
					true
				},
				{
					new CurvePoint(3f, 4f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				},
				{
					new CurvePoint(2f, 2f),
					true
				},
				{
					new CurvePoint(3f, 3f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				},
				{
					new CurvePoint(2f, 1f),
					true
				},
				{
					new CurvePoint(3f, 2f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				},
				{
					new CurvePoint(2f, 1f),
					true
				},
				{
					new CurvePoint(3f, 1f),
					true
				}
			}
		};

		// Token: 0x0400909C RID: 37020
		private static List<Precept> toRemovePrecepts = new List<Precept>();
	}
}
