using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002593 RID: 9619
	public static class FactionRelationKindUtility
	{
		// Token: 0x0600D2D6 RID: 53974 RVA: 0x003E1708 File Offset: 0x003DF908
		public static string GetLabel(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "HostileLower".Translate();
			case FactionRelationKind.Neutral:
				return "NeutralLower".Translate();
			case FactionRelationKind.Ally:
				return "AllyLower".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600D2D7 RID: 53975 RVA: 0x003E1760 File Offset: 0x003DF960
		public static string GetLabelCap(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return "Hostile".Translate();
			case FactionRelationKind.Neutral:
				return "Neutral".Translate();
			case FactionRelationKind.Ally:
				return "Ally".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x0600D2D8 RID: 53976 RVA: 0x003E17B6 File Offset: 0x003DF9B6
		public static Color GetColor(this FactionRelationKind kind)
		{
			switch (kind)
			{
			case FactionRelationKind.Hostile:
				return ColorLibrary.RedReadable;
			case FactionRelationKind.Neutral:
				return new Color(0f, 0.75f, 1f);
			case FactionRelationKind.Ally:
				return Color.green;
			default:
				return Color.white;
			}
		}
	}
}
