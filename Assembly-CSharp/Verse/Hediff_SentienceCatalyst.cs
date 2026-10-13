using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200076A RID: 1898
	public class Hediff_SentienceCatalyst : Hediff
	{
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002E89 RID: 11913 RVA: 0x000EF45C File Offset: 0x000ED65C
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				TrainabilityDef trainability = TrainableUtility.GetTrainability(this.pawn);
				if (this.pawn.training == null || trainability == null || trainability == this.pawn.RaceProps.trainability)
				{
					return text;
				}
				text += "\n";
				return text + ("Trainability".Translate() + ": " + trainability.LabelCap);
			}
		}
	}
}
