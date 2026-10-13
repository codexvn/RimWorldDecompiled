using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C60 RID: 3168
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x06005114 RID: 20756 RVA: 0x001A569B File Offset: 0x001A389B
		private void Start()
		{
			if (Screen.width != 0 && Screen.height != 0)
			{
				Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
			}
		}
	}
}
