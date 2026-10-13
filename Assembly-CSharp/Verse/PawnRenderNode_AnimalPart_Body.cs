using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000601 RID: 1537
	public class PawnRenderNode_AnimalPart_Body : PawnRenderNode_AnimalPart
	{
		// Token: 0x06002757 RID: 10071 RVA: 0x000D180E File Offset: 0x000CFA0E
		public PawnRenderNode_AnimalPart_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000D1BDB File Offset: 0x000CFDDB
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		protected override IEnumerable<ValueTuple<GraphicStateDef, Graphic>> StateGraphicsFor(Pawn pawn)
		{
			foreach (ValueTuple<GraphicStateDef, Graphic> valueTuple in base.StateGraphicsFor(pawn))
			{
				yield return valueTuple;
			}
			IEnumerator<ValueTuple<GraphicStateDef, Graphic>> enumerator = null;
			ValueTuple<GraphicStateDef, Graphic>? swimmingGraphic = this.GetSwimmingGraphic(pawn);
			if (swimmingGraphic != null)
			{
				yield return swimmingGraphic.Value;
			}
			ValueTuple<GraphicStateDef, Graphic>? stationaryGraphic = this.GetStationaryGraphic(pawn);
			if (stationaryGraphic != null)
			{
				yield return stationaryGraphic.Value;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x000D1BF4 File Offset: 0x000CFDF4
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		private ValueTuple<GraphicStateDef, Graphic>? GetSwimmingGraphic(Pawn pawn)
		{
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			if (curKindLifeStage.swimmingGraphicData == null)
			{
				return null;
			}
			Graphic graphic = curKindLifeStage.swimmingGraphicData.Graphic;
			if (pawn.gender == Gender.Female && curKindLifeStage.femaleSwimmingGraphicData != null)
			{
				graphic = curKindLifeStage.femaleSwimmingGraphicData.Graphic;
			}
			AlternateGraphic alternateGraphic;
			int num;
			if (pawn.TryGetAlternate(out alternateGraphic, out num))
			{
				graphic = alternateGraphic.GetSwimmingGraphic(graphic);
			}
			Color color = graphic.Color;
			Color color2 = graphic.ColorTwo;
			if (pawn.IsMutant)
			{
				color = MutantUtility.GetMutantSkinColor(pawn, color);
				color2 = MutantUtility.GetMutantSkinColor(pawn, color2);
			}
			color = pawn.health.hediffSet.GetSkinColor(color);
			color2 = pawn.health.hediffSet.GetSkinColor(color2);
			graphic = graphic.GetColoredVersion(graphic.Shader, color, color2);
			return new ValueTuple<GraphicStateDef, Graphic>?(new ValueTuple<GraphicStateDef, Graphic>(GraphicStateDefOf.Swimming, graphic));
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000D1CCC File Offset: 0x000CFECC
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		private ValueTuple<GraphicStateDef, Graphic>? GetStationaryGraphic(Pawn pawn)
		{
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			if (curKindLifeStage.stationaryGraphicData == null)
			{
				return null;
			}
			Graphic graphic = curKindLifeStage.stationaryGraphicData.Graphic;
			if (pawn.gender == Gender.Female && curKindLifeStage.femaleStationaryGraphicData != null)
			{
				graphic = curKindLifeStage.femaleStationaryGraphicData.Graphic;
			}
			AlternateGraphic alternateGraphic;
			int num;
			if (pawn.TryGetAlternate(out alternateGraphic, out num))
			{
				graphic = alternateGraphic.GetStationaryGraphic(graphic);
			}
			Color color = graphic.Color;
			Color color2 = graphic.ColorTwo;
			if (pawn.IsMutant)
			{
				color = MutantUtility.GetMutantSkinColor(pawn, color);
				color2 = MutantUtility.GetMutantSkinColor(pawn, color2);
			}
			color = pawn.health.hediffSet.GetSkinColor(color);
			color2 = pawn.health.hediffSet.GetSkinColor(color2);
			graphic = graphic.GetColoredVersion(graphic.Shader, color, color2);
			return new ValueTuple<GraphicStateDef, Graphic>?(new ValueTuple<GraphicStateDef, Graphic>(GraphicStateDefOf.Stationary, graphic));
		}
	}
}
