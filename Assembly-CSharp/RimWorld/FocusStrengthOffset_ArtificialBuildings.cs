using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D56 RID: 11606
	public class FocusStrengthOffset_ArtificialBuildings : FocusStrengthOffset_Curve
	{
		// Token: 0x17002979 RID: 10617
		// (get) Token: 0x060104EA RID: 66794 RVA: 0x004B57E7 File Offset: 0x004B39E7
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_NearbyArtificialStructures";
			}
		}

		// Token: 0x060104EB RID: 66795 RVA: 0x0020FBF8 File Offset: 0x0020DDF8
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent.Spawned;
		}

		// Token: 0x060104EC RID: 66796 RVA: 0x004B57EE File Offset: 0x004B39EE
		protected override float SourceValue(Thing parent)
		{
			return (float)(parent.Spawned ? parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius).Count : 0);
		}

		// Token: 0x060104ED RID: 66797 RVA: 0x004B581D File Offset: 0x004B3A1D
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			MeditationUtility.DrawArtificialBuildingOverlay(parent.Position, parent.def, parent.Map, this.radius);
		}

		// Token: 0x060104EE RID: 66798 RVA: 0x004B5844 File Offset: 0x004B3A44
		public override string InspectStringExtra(Thing parent, Pawn user = null)
		{
			if (parent.Spawned)
			{
				List<Thing> forCell = parent.Map.listerArtificialBuildingsForMeditation.GetForCell(parent.Position, this.radius);
				if (forCell.Count > 0)
				{
					IEnumerable<string> source = (from c in forCell
					select GenLabel.ThingLabel(c, 1, false, true)).Distinct<string>();
					TaggedString taggedString = "MeditationFocusImpacted".Translate() + ": " + source.Take(3).ToCommaList(false, false).CapitalizeFirst();
					if (source.Count<string>() > 3)
					{
						taggedString += " " + "Etc".Translate();
					}
					return taggedString;
				}
			}
			return "";
		}

		// Token: 0x0400AA2E RID: 43566
		public float radius = 10f;
	}
}
