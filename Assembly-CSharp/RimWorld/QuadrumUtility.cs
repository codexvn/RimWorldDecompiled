using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003573 RID: 13683
	public static class QuadrumUtility
	{
		// Token: 0x170031C1 RID: 12737
		// (get) Token: 0x0601333E RID: 78654 RVA: 0x00002501 File Offset: 0x00000701
		public static Quadrum FirstQuadrum
		{
			get
			{
				return Quadrum.Aprimay;
			}
		}

		// Token: 0x170031C2 RID: 12738
		// (get) Token: 0x0601333F RID: 78655 RVA: 0x005B4ABD File Offset: 0x005B2CBD
		public static List<Quadrum> Quadrums
		{
			get
			{
				return QuadrumUtility.quadrumList;
			}
		}

		// Token: 0x170031C3 RID: 12739
		// (get) Token: 0x06013340 RID: 78656 RVA: 0x005B4AC4 File Offset: 0x005B2CC4
		public static List<Quadrum> QuadrumsInChronologicalOrder
		{
			get
			{
				return QuadrumUtility.quadrumListInChronologicanOrder;
			}
		}

		// Token: 0x06013341 RID: 78657 RVA: 0x005B4ACB File Offset: 0x005B2CCB
		public static Twelfth GetFirstTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return Twelfth.First;
			case Quadrum.Jugust:
				return Twelfth.Fourth;
			case Quadrum.Septober:
				return Twelfth.Seventh;
			case Quadrum.Decembary:
				return Twelfth.Tenth;
			default:
				return Twelfth.Undefined;
			}
		}

		// Token: 0x06013342 RID: 78658 RVA: 0x005B4AF0 File Offset: 0x005B2CF0
		public static Twelfth GetMiddleTwelfth(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return Twelfth.Second;
			case Quadrum.Jugust:
				return Twelfth.Fifth;
			case Quadrum.Septober:
				return Twelfth.Eighth;
			case Quadrum.Decembary:
				return Twelfth.Eleventh;
			default:
				return Twelfth.Undefined;
			}
		}

		// Token: 0x06013343 RID: 78659 RVA: 0x005B4B15 File Offset: 0x005B2D15
		public static float GetMiddleYearPct(this Quadrum quadrum)
		{
			return quadrum.GetMiddleTwelfth().GetMiddleYearPct();
		}

		// Token: 0x06013344 RID: 78660 RVA: 0x005B4B24 File Offset: 0x005B2D24
		public static string Label(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return "QuadrumAprimay".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober".Translate();
			case Quadrum.Decembary:
				return "QuadrumDecembary".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		// Token: 0x06013345 RID: 78661 RVA: 0x005B4B90 File Offset: 0x005B2D90
		public static string LabelShort(this Quadrum quadrum)
		{
			switch (quadrum)
			{
			case Quadrum.Aprimay:
				return "QuadrumAprimay_Short".Translate();
			case Quadrum.Jugust:
				return "QuadrumJugust_Short".Translate();
			case Quadrum.Septober:
				return "QuadrumSeptober_Short".Translate();
			case Quadrum.Decembary:
				return "QuadrumDecembary_Short".Translate();
			default:
				return "Unknown quadrum";
			}
		}

		// Token: 0x06013346 RID: 78662 RVA: 0x005B4BFA File Offset: 0x005B2DFA
		public static Season GetSeason(this Quadrum q, Vector2 location)
		{
			return q.GetSeason(location.y);
		}

		// Token: 0x06013347 RID: 78663 RVA: 0x005B4C08 File Offset: 0x005B2E08
		public static Season GetSeason(this Quadrum q, float latitude)
		{
			return SeasonUtility.GetReportedSeason(q.GetMiddleYearPct(), latitude);
		}

		// Token: 0x06013348 RID: 78664 RVA: 0x005B4C18 File Offset: 0x005B2E18
		public static string QuadrumsRangeLabel(List<Twelfth> twelfths)
		{
			if (twelfths.Count == 0)
			{
				return "";
			}
			if (twelfths.Count == 12)
			{
				return "WholeYear".Translate();
			}
			string text = "";
			for (int i = 0; i < 12; i++)
			{
				Twelfth twelfth = (Twelfth)i;
				if (twelfths.Contains(twelfth))
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += QuadrumUtility.QuadrumsContinuousRangeLabel(twelfths, twelfth);
				}
			}
			return text;
		}

		// Token: 0x06013349 RID: 78665 RVA: 0x005B4C90 File Offset: 0x005B2E90
		private static string QuadrumsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			for (Twelfth twelfth = leftMostTwelfth; twelfth != rightMostTwelfth; twelfth = TwelfthUtility.TwelfthAfter(twelfth))
			{
				if (!twelfths.Contains(twelfth))
				{
					Log.Error(string.Concat(new string[]
					{
						"Twelfths doesn't contain ",
						twelfth.ToString(),
						" (",
						leftMostTwelfth.ToString(),
						"..",
						rightMostTwelfth.ToString(),
						")"
					}));
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.QuadrumDateStringAt(leftMostTwelfth) + " - " + GenDate.QuadrumDateStringAt(rightMostTwelfth);
		}

		// Token: 0x0400C723 RID: 50979
		private static readonly List<Quadrum> quadrumList = new List<Quadrum>
		{
			Quadrum.Aprimay,
			Quadrum.Decembary,
			Quadrum.Jugust,
			Quadrum.Septober
		};

		// Token: 0x0400C724 RID: 50980
		private static readonly List<Quadrum> quadrumListInChronologicanOrder = new List<Quadrum>
		{
			Quadrum.Aprimay,
			Quadrum.Jugust,
			Quadrum.Septober,
			Quadrum.Decembary
		};
	}
}
