using System;
using RimWorld.Planet;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Verse
{
	// Token: 0x02000356 RID: 854
	public static class Current
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x060015D7 RID: 5591 RVA: 0x00066BFC File Offset: 0x00064DFC
		public static Root Root
		{
			get
			{
				return Current.rootInt;
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x00066C03 File Offset: 0x00064E03
		public static Root_Entry Root_Entry
		{
			get
			{
				return Current.rootEntryInt;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x00066C0A File Offset: 0x00064E0A
		public static Root_Play Root_Play
		{
			get
			{
				return Current.rootPlayInt;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x060015DA RID: 5594 RVA: 0x00066C11 File Offset: 0x00064E11
		public static Camera Camera
		{
			get
			{
				return Current.cameraInt;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x060015DB RID: 5595 RVA: 0x00066C18 File Offset: 0x00064E18
		public static CameraDriver CameraDriver
		{
			get
			{
				return Current.cameraDriverInt;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x060015DC RID: 5596 RVA: 0x00066C1F File Offset: 0x00064E1F
		public static ColorCorrectionCurves ColorCorrectionCurves
		{
			get
			{
				return Current.colorCorrectionCurvesInt;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060015DD RID: 5597 RVA: 0x00066C26 File Offset: 0x00064E26
		public static SubcameraDriver SubcameraDriver
		{
			get
			{
				return Current.subcameraDriverInt;
			}
		}

		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x00066C2D File Offset: 0x00064E2D
		// (set) Token: 0x060015DF RID: 5599 RVA: 0x00066C34 File Offset: 0x00064E34
		public static Game Game
		{
			get
			{
				return Current.gameInt;
			}
			set
			{
				Current.gameInt = value;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x00066C3C File Offset: 0x00064E3C
		// (set) Token: 0x060015E1 RID: 5601 RVA: 0x00066C43 File Offset: 0x00064E43
		public static World CreatingWorld
		{
			get
			{
				return Current.creatingWorldInt;
			}
			set
			{
				Current.creatingWorldInt = value;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x00066C4B File Offset: 0x00064E4B
		// (set) Token: 0x060015E3 RID: 5603 RVA: 0x00066C52 File Offset: 0x00064E52
		public static ProgramState ProgramState
		{
			get
			{
				return Current.programStateInt;
			}
			set
			{
				Current.programStateInt = value;
			}
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x00066C5C File Offset: 0x00064E5C
		public static void Notify_LoadedSceneChanged()
		{
			Current.cameraInt = GameObject.Find("Camera").GetComponent<Camera>();
			if (GenScene.InEntryScene)
			{
				Current.ProgramState = ProgramState.Entry;
				Current.rootEntryInt = GameObject.Find("GameRoot").GetComponent<Root_Entry>();
				Current.rootPlayInt = null;
				Current.rootInt = Current.rootEntryInt;
				Current.cameraDriverInt = null;
				Current.colorCorrectionCurvesInt = null;
				return;
			}
			if (GenScene.InPlayScene)
			{
				Current.ProgramState = ProgramState.MapInitializing;
				Current.rootEntryInt = null;
				Current.rootPlayInt = GameObject.Find("GameRoot").GetComponent<Root_Play>();
				Current.rootInt = Current.rootPlayInt;
				Current.cameraDriverInt = Current.cameraInt.GetComponent<CameraDriver>();
				Current.colorCorrectionCurvesInt = Current.cameraInt.GetComponent<ColorCorrectionCurves>();
				Current.subcameraDriverInt = GameObject.Find("Subcameras").GetComponent<SubcameraDriver>();
			}
		}

		// Token: 0x0400142D RID: 5165
		private static ProgramState programStateInt;

		// Token: 0x0400142E RID: 5166
		private static Root rootInt;

		// Token: 0x0400142F RID: 5167
		private static Root_Entry rootEntryInt;

		// Token: 0x04001430 RID: 5168
		private static Root_Play rootPlayInt;

		// Token: 0x04001431 RID: 5169
		private static Camera cameraInt;

		// Token: 0x04001432 RID: 5170
		private static CameraDriver cameraDriverInt;

		// Token: 0x04001433 RID: 5171
		private static ColorCorrectionCurves colorCorrectionCurvesInt;

		// Token: 0x04001434 RID: 5172
		private static SubcameraDriver subcameraDriverInt;

		// Token: 0x04001435 RID: 5173
		private static Game gameInt;

		// Token: 0x04001436 RID: 5174
		private static World creatingWorldInt;
	}
}
