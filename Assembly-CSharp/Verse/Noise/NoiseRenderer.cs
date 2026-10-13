using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000D9E RID: 3486
	public static class NoiseRenderer
	{
		// Token: 0x060058EB RID: 22763 RVA: 0x001CA4D9 File Offset: 0x001C86D9
		public static Texture2D NoiseRendered(ModuleBase noise)
		{
			return NoiseRenderer.NoiseRendered(new CellRect(0, 0, NoiseRenderer.renderSize.x, NoiseRenderer.renderSize.z), noise);
		}

		// Token: 0x060058EC RID: 22764 RVA: 0x001CA4FC File Offset: 0x001C86FC
		public static Texture2D NoiseRendered(CellRect rect, ModuleBase noise)
		{
			Texture2D texture2D = new Texture2D(rect.Width, rect.Height);
			texture2D.name = "NoiseRender";
			foreach (IntVec2 intVec in rect.Cells2D)
			{
				texture2D.SetPixel(intVec.x, intVec.z, NoiseRenderer.ColorForValue(noise.GetValue(intVec)));
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x060058ED RID: 22765 RVA: 0x001CA588 File Offset: 0x001C8788
		private static Color ColorForValue(float val)
		{
			val = val * 0.5f + 0.5f;
			return ColorsFromSpectrum.Get(NoiseRenderer.spectrum, val);
		}

		// Token: 0x04004071 RID: 16497
		public static IntVec2 renderSize = new IntVec2(200, 200);

		// Token: 0x04004072 RID: 16498
		private static Color[] spectrum = new Color[]
		{
			Color.black,
			Color.blue,
			Color.green,
			Color.white
		};
	}
}
