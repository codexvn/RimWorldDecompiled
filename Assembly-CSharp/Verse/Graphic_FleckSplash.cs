using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000897 RID: 2199
	public class Graphic_FleckSplash : Graphic_Fleck
	{
		// Token: 0x060037BB RID: 14267 RVA: 0x00129D04 File Offset: 0x00127F04
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetColor(ShaderPropertyIDs.ShockwaveColor, new Color(1f, 1f, 1f, drawData.alpha));
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.ShockwaveSpan, drawData.calculatedShockwaveSpan);
			drawData.drawLayer = SubcameraDefOf.WaterDepth.LayerId;
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x00129D84 File Offset: 0x00127F84
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "FleckSplash(path=";
			array[1] = this.path;
			array[2] = ", shader=";
			int num = 3;
			Shader shader = base.Shader;
			array[num] = ((shader != null) ? shader.ToString() : null);
			array[4] = ", color=";
			int num2 = 5;
			Color color = this.color;
			array[num2] = color.ToString();
			array[6] = ", colorTwo=unsupported)";
			return string.Concat(array);
		}
	}
}
