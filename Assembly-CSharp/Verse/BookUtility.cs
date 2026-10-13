using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001AB RID: 427
	public static class BookUtility
	{
		// Token: 0x06000BFE RID: 3070 RVA: 0x000392F8 File Offset: 0x000374F8
		public static bool CanReadEver(Pawn reader)
		{
			return reader.DevelopmentalStage != DevelopmentalStage.Baby && !StatDefOf.ReadingSpeed.Worker.IsDisabledFor(reader) && reader.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && reader.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00039357 File Offset: 0x00037557
		public static bool CanReadNow(Pawn reader)
		{
			return BookUtility.CanReadEver(reader) && BookUtility.GetReadingModifier(reader) > 0f;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00039374 File Offset: 0x00037574
		public static bool CanReadBook(Book book, Pawn reader, out string reason)
		{
			if (!book.IsReadable)
			{
				reason = "BookNotReadable".Translate(book.Named("BOOK"));
				return false;
			}
			DevelopmentalStage developmentalStageFilter = book.BookComp.Props.developmentalStageFilter;
			if (!developmentalStageFilter.HasAny(reader.DevelopmentalStage))
			{
				string arg = developmentalStageFilter.ToCommaList(false);
				reason = "BookCantBeStage".Translate(reader.Named("PAWN"), arg.Named("STAGES"));
				return false;
			}
			if (!reader.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				reason = "BookBlind".Translate(reader.Named("PAWN"));
				return false;
			}
			if (!BookUtility.CanReadEver(reader))
			{
				reason = "BookCantRead".Translate(reader.Named("PAWN"));
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00039456 File Offset: 0x00037656
		public static float GetReadingModifier(Pawn reader)
		{
			if (reader == null || StatDefOf.ReadingSpeed.Worker.IsDisabledFor(reader))
			{
				return 1f;
			}
			return reader.GetStatValue(StatDefOf.ReadingSpeed, true, -1);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00039480 File Offset: 0x00037680
		public static Book MakeBook(ArtGenerationContext context, QualityGenerator? qualityGenerator = null)
		{
			return BookUtility.MakeBook(BookUtility.GetBookDefs().RandomElementByWeight((ThingDef x) => x.GetCompProperties<CompProperties_Book>().pickWeight), context, qualityGenerator);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x000394B4 File Offset: 0x000376B4
		public static Book MakeBook(ThingDef def, ArtGenerationContext context, QualityGenerator? qualityGenerator = null)
		{
			ThingDef stuff = GenStuff.RandomStuffFor(def);
			Thing thing = ThingMaker.MakeThing(def, stuff);
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory q = (qualityGenerator != null) ? QualityUtility.GenerateQuality(qualityGenerator.Value) : QualityUtility.GenerateQualityRandomEqualChance();
				compQuality.SetQuality(q, new ArtGenerationContext?(context));
			}
			return thing as Book;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00039508 File Offset: 0x00037708
		private static List<ThingDef> GetBookDefs()
		{
			return (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.HasComp<CompBook>()
			select x).ToList<ThingDef>();
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00039538 File Offset: 0x00037738
		public static float GetReadingBonus(Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room != null && room.ProperRoom && !room.PsychologicallyOutdoors)
			{
				return room.GetStat(RoomStatDefOf.ReadingBonus);
			}
			return 1f;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00039574 File Offset: 0x00037774
		public static bool TryGetRandomBookToRead(Pawn pawn, out Book book)
		{
			book = null;
			BookUtility.TmpCandidates.Clear();
			BookUtility.TmpOutcomeCandidates.Clear();
			BookUtility.TmpCandidates.AddRange(from thing in pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Book)
			where BookUtility.IsValidBook(thing, pawn)
			select thing);
			BookUtility.TmpCandidates.AddRange(from thing in pawn.Map.listerThings.GetThingsOfType<Building_Bookcase>().SelectMany((Building_Bookcase x) => x.HeldBooks)
			where BookUtility.IsValidBook(thing, pawn)
			select thing);
			if (BookUtility.TmpCandidates.Empty<Thing>())
			{
				return false;
			}
			foreach (Thing thing2 in BookUtility.TmpCandidates)
			{
				Book book2 = thing2 as Book;
				if (book2 != null && book2.ProvidesOutcome(pawn))
				{
					BookUtility.TmpOutcomeCandidates.Add(thing2);
				}
			}
			book = (Book)(BookUtility.TmpOutcomeCandidates.Any<Thing>() ? BookUtility.TmpOutcomeCandidates.RandomElement<Thing>() : BookUtility.TmpCandidates.RandomElement<Thing>());
			BookUtility.TmpCandidates.Clear();
			BookUtility.TmpOutcomeCandidates.Clear();
			return true;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000396D8 File Offset: 0x000378D8
		private static bool IsValidBook(Thing thing, Pawn pawn)
		{
			if (thing is Book && !thing.IsForbiddenHeld(pawn))
			{
				Pawn_ReadingTracker reading = pawn.reading;
				if (((reading != null) ? reading.CurrentPolicy : null) != null && pawn.reading.CurrentPolicy.defFilter.Allows(thing) && pawn.reading.CurrentPolicy.effectFilter.Allows(thing) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false) && !thing.Fogged() && thing.IsPoliticallyProper(pawn))
				{
					return !thing.VacuumConcernTo(pawn);
				}
			}
			return false;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0003976B File Offset: 0x0003796B
		public static float GetResearchExpForQuality(QualityCategory quality)
		{
			return BookUtility.QualityResearchExpTick.Evaluate((float)quality);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00039779 File Offset: 0x00037979
		public static float GetAnomalyExpForQuality(QualityCategory quality)
		{
			return BookUtility.QualityAnomalyExpTick.Evaluate((float)quality);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00039787 File Offset: 0x00037987
		public static float GetSkillExpForQuality(QualityCategory quality)
		{
			return BookUtility.QualitySkillExpTick.Evaluate((float)quality);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00039795 File Offset: 0x00037995
		public static float GetNovelJoyFactorForQuality(QualityCategory quality)
		{
			return BookUtility.QualityJoyFactor.Evaluate((float)quality);
		}

		// Token: 0x040006EF RID: 1775
		private static readonly List<Thing> TmpCandidates = new List<Thing>();

		// Token: 0x040006F0 RID: 1776
		private static readonly List<Thing> TmpOutcomeCandidates = new List<Thing>();

		// Token: 0x040006F1 RID: 1777
		private static readonly SimpleCurve QualityResearchExpTick = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.008f),
				true
			},
			{
				new CurvePoint(1f, 0.012f),
				true
			},
			{
				new CurvePoint(2f, 0.016f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			},
			{
				new CurvePoint(4f, 0.024f),
				true
			},
			{
				new CurvePoint(5f, 0.028f),
				true
			},
			{
				new CurvePoint(6f, 0.032f),
				true
			}
		};

		// Token: 0x040006F2 RID: 1778
		private static readonly SimpleCurve QualityAnomalyExpTick = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3E-05f),
				true
			},
			{
				new CurvePoint(1f, 6E-05f),
				true
			},
			{
				new CurvePoint(2f, 9E-05f),
				true
			},
			{
				new CurvePoint(3f, 0.00012f),
				true
			},
			{
				new CurvePoint(4f, 0.00015f),
				true
			},
			{
				new CurvePoint(5f, 0.00018f),
				true
			},
			{
				new CurvePoint(6f, 0.00021f),
				true
			}
		};

		// Token: 0x040006F3 RID: 1779
		private static readonly SimpleCurve QualitySkillExpTick = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.075f),
				true
			},
			{
				new CurvePoint(2f, 0.1f),
				true
			},
			{
				new CurvePoint(3f, 0.125f),
				true
			},
			{
				new CurvePoint(4f, 0.15f),
				true
			},
			{
				new CurvePoint(5f, 0.175f),
				true
			},
			{
				new CurvePoint(6f, 0.2f),
				true
			}
		};

		// Token: 0x040006F4 RID: 1780
		private static readonly SimpleCurve QualityJoyFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.2f),
				true
			},
			{
				new CurvePoint(1f, 1.4f),
				true
			},
			{
				new CurvePoint(2f, 1.6f),
				true
			},
			{
				new CurvePoint(3f, 1.8f),
				true
			},
			{
				new CurvePoint(4f, 2f),
				true
			},
			{
				new CurvePoint(5f, 2.25f),
				true
			},
			{
				new CurvePoint(6f, 2.5f),
				true
			}
		};
	}
}
