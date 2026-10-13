using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000896 RID: 2198
	public class Graphic_FleckPulse : Graphic_Fleck
	{
		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool AllowInstancing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x00129C34 File Offset: 0x00127E34
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			drawData.propertyBlock = (drawData.propertyBlock ?? batch.GetPropertyBlock());
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, drawData.ageSecs);
			drawData.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, drawData.id);
			base.DrawFleck(drawData, batch);
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x00129C8C File Offset: 0x00127E8C
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Graphic_FleckPulse(path=";
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
