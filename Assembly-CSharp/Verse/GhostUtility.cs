using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BF9 RID: 3065
	public static class GhostUtility
	{
		// Token: 0x06004E66 RID: 20070 RVA: 0x0018EDBC File Offset: 0x0018CFBC
		public static Graphic GhostGraphicFor(Graphic baseGraphic, ThingDef thingDef, Color ghostCol, ThingDef stuff = null)
		{
			if (thingDef.useSameGraphicForGhost)
			{
				return baseGraphic;
			}
			int num = 0;
			num = Gen.HashCombine<Graphic>(num, baseGraphic);
			num = Gen.HashCombine<ThingDef>(num, thingDef);
			num = Gen.HashCombineStruct<Color>(num, ghostCol);
			num = Gen.HashCombine<ThingDef>(num, stuff);
			Graphic graphic;
			if (!GhostUtility.ghostGraphics.TryGetValue(num, out graphic))
			{
				if (thingDef.graphicData.Linked || (thingDef.IsDoor && !thingDef.building.isSupportDoor))
				{
					graphic = GraphicDatabase.Get<Graphic_Single>(thingDef.uiIconPath, ShaderTypeDefOf.EdgeDetect.Shader, thingDef.graphicData.drawSize, ghostCol);
				}
				else
				{
					if (thingDef.useBlueprintGraphicAsGhost)
					{
						baseGraphic = thingDef.blueprintDef.graphic;
					}
					else if (baseGraphic == null)
					{
						baseGraphic = thingDef.graphic;
					}
					GraphicData graphicData = null;
					if (baseGraphic.data != null)
					{
						graphicData = new GraphicData();
						graphicData.CopyFrom(baseGraphic.data);
						graphicData.shadowData = null;
					}
					string path = baseGraphic.path;
					Graphic_Appearances graphic_Appearances = baseGraphic as Graphic_Appearances;
					if (graphic_Appearances != null && stuff != null)
					{
						graphic = GraphicDatabase.Get<Graphic_Single>(graphic_Appearances.SubGraphicFor(stuff).path, ShaderTypeDefOf.EdgeDetect.Shader, thingDef.graphicData.drawSize, ghostCol, Color.white, graphicData, null);
					}
					else
					{
						graphic = GraphicDatabase.Get(baseGraphic.GetType(), path, ShaderTypeDefOf.EdgeDetect.Shader, baseGraphic.drawSize, ghostCol, Color.white, graphicData, null, null);
					}
				}
				GhostUtility.ghostGraphics.Add(num, graphic);
			}
			return graphic;
		}

		// Token: 0x040037B5 RID: 14261
		private static Dictionary<int, Graphic> ghostGraphics = new Dictionary<int, Graphic>();
	}
}
