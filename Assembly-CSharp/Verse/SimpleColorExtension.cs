using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF3 RID: 3315
	public static class SimpleColorExtension
	{
		// Token: 0x0600548D RID: 21645 RVA: 0x001B39A4 File Offset: 0x001B1BA4
		public static Color ToUnityColor(this SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return Color.white;
			case SimpleColor.Red:
				return Color.red;
			case SimpleColor.Green:
				return Color.green;
			case SimpleColor.Blue:
				return Color.blue;
			case SimpleColor.Magenta:
				return Color.magenta;
			case SimpleColor.Yellow:
				return Color.yellow;
			case SimpleColor.Cyan:
				return Color.cyan;
			case SimpleColor.Orange:
				return ColorLibrary.Orange;
			default:
				throw new ArgumentException();
			}
		}
	}
}
