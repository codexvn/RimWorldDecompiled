using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000600 RID: 1536
	public class PawnRenderNode_AnimalPart : PawnRenderNode
	{
		// Token: 0x06002754 RID: 10068 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_AnimalPart(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x000D18A0 File Offset: 0x000CFAA0
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			Graphic graphic = this.GraphicFor(pawn);
			if (graphic != null)
			{
				return MeshPool.GetMeshSetForSize(graphic.drawSize.x, graphic.drawSize.y);
			}
			return null;
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x000D18D8 File Offset: 0x000CFAD8
		public override Graphic GraphicFor(Pawn pawn)
		{
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			AlternateGraphic alternateGraphic;
			int num;
			Graphic graphic;
			if (pawn.TryGetAlternate(out alternateGraphic, out num))
			{
				graphic = alternateGraphic.GetGraphic(curKindLifeStage.bodyGraphicData.Graphic);
			}
			else if (pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
			{
				graphic = curKindLifeStage.bodyGraphicData.Graphic;
			}
			else
			{
				graphic = curKindLifeStage.femaleGraphicData.Graphic;
			}
			if ((pawn.Dead || (pawn.IsMutant && pawn.mutant.Def.useCorpseGraphics)) && curKindLifeStage.corpseGraphicData != null)
			{
				if (pawn.gender != Gender.Female || curKindLifeStage.femaleCorpseGraphicData == null)
				{
					graphic = curKindLifeStage.corpseGraphicData.Graphic.GetColoredVersion(curKindLifeStage.corpseGraphicData.Graphic.Shader, graphic.Color, graphic.ColorTwo);
				}
				else
				{
					graphic = curKindLifeStage.femaleCorpseGraphicData.Graphic.GetColoredVersion(curKindLifeStage.femaleCorpseGraphicData.Graphic.Shader, graphic.Color, graphic.ColorTwo);
				}
			}
			Color color = graphic.Color;
			Color color2 = graphic.ColorTwo;
			switch (pawn.Drawer.renderer.CurRotDrawMode)
			{
			case RotDrawMode.Fresh:
				if (pawn.IsMutant)
				{
					color = MutantUtility.GetMutantSkinColor(pawn, color);
					color2 = MutantUtility.GetMutantSkinColor(pawn, color2);
				}
				color = pawn.health.hediffSet.GetSkinColor(color);
				color2 = pawn.health.hediffSet.GetSkinColor(color2);
				return graphic.GetColoredVersion(graphic.Shader, color, color2);
			case RotDrawMode.Rotting:
			{
				color = pawn.health.hediffSet.GetSkinColor(color);
				color2 = pawn.health.hediffSet.GetSkinColor(color2);
				color = PawnRenderUtility.GetRottenColor(color);
				color2 = PawnRenderUtility.GetRottenColor(color2);
				Graphic graphic2;
				if (alternateGraphic != null)
				{
					graphic2 = alternateGraphic.GetRottingGraphic(graphic);
				}
				else if (curKindLifeStage.femaleRottingGraphicData != null && pawn.gender == Gender.Female)
				{
					graphic2 = curKindLifeStage.femaleRottingGraphicData.Graphic;
				}
				else if (curKindLifeStage.rottingGraphicData != null)
				{
					graphic2 = curKindLifeStage.rottingGraphicData.Graphic;
				}
				else
				{
					graphic2 = graphic;
				}
				Shader newShader = graphic2.Shader;
				if (graphic2.Shader == ShaderDatabase.CutoutComplex)
				{
					newShader = ShaderDatabase.Cutout;
				}
				return graphic2.GetColoredVersion(newShader, color, color2);
			}
			case RotDrawMode.Dessicated:
				if (curKindLifeStage.dessicatedBodyGraphicData != null)
				{
					Graphic graphic3;
					if (pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid)
					{
						Color dessicatedColorInsect = PawnRenderUtility.DessicatedColorInsect;
						if (pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
						{
							graphic3 = curKindLifeStage.dessicatedBodyGraphicData.Graphic.GetColoredVersion(ShaderDatabase.Cutout, dessicatedColorInsect, dessicatedColorInsect);
						}
						else
						{
							graphic3 = curKindLifeStage.femaleDessicatedBodyGraphicData.Graphic.GetColoredVersion(graphic.Shader, dessicatedColorInsect, dessicatedColorInsect);
						}
					}
					else if (pawn.gender != Gender.Female || curKindLifeStage.femaleDessicatedBodyGraphicData == null)
					{
						graphic3 = curKindLifeStage.dessicatedBodyGraphicData.GraphicColoredFor(pawn);
					}
					else
					{
						graphic3 = curKindLifeStage.femaleDessicatedBodyGraphicData.GraphicColoredFor(pawn);
					}
					if (pawn.IsShambler)
					{
						graphic3.ShadowGraphic = graphic.ShadowGraphic;
					}
					if (alternateGraphic != null)
					{
						graphic3 = alternateGraphic.GetDessicatedGraphic(graphic3);
					}
					return graphic3;
				}
				break;
			}
			return null;
		}
	}
}
