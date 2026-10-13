using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001827 RID: 6183
	public class ThoughtWorker_GauranlenConnectionDesired : ThoughtWorker_Precept
	{
		// Token: 0x06008F12 RID: 36626 RVA: 0x002A4BDC File Offset: 0x002A2DDC
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ModsConfig.BiotechActive && p.DevelopmentalStage.Juvenile())
			{
				return ThoughtState.Inactive;
			}
			if (p.connections == null || !p.connections.ConnectedThings.Any<Thing>())
			{
				if (p.playerSettings == null || p.IsSlave || p.IsPrisoner || p.IsQuestLodger())
				{
					return ThoughtState.Inactive;
				}
				if (p.WorkTypeIsDisabled(WorkTypeDefOf.PlantCutting))
				{
					return ThoughtState.Inactive;
				}
				int num = Mathf.Max(new int[]
				{
					0,
					p.ideo.joinTick,
					p.playerSettings.joinTick
				});
				if (Find.TickManager.TicksGame - num < 900000)
				{
					return ThoughtState.Inactive;
				}
			}
			int stageIndex = 0;
			if (p.connections == null || p.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				if (p.MapHeld == null)
				{
					stageIndex = 1;
				}
				else
				{
					ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
					if (expectationDef != null)
					{
						if (expectationDef.order <= ExpectationDefOf.VeryLow.order)
						{
							stageIndex = 1;
						}
						else if (expectationDef.order <= ExpectationDefOf.Low.order)
						{
							stageIndex = 2;
						}
						else if (expectationDef.order <= ExpectationDefOf.Moderate.order)
						{
							stageIndex = 3;
						}
						else if (expectationDef.order <= ExpectationDefOf.High.order)
						{
							stageIndex = 4;
						}
						else
						{
							stageIndex = 5;
						}
					}
				}
			}
			return ThoughtState.ActiveAtStage(stageIndex);
		}

		// Token: 0x04005C23 RID: 23587
		private const int TicksJoinedMin = 900000;
	}
}
