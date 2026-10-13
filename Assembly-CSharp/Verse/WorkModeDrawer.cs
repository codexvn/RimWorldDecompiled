using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002A2 RID: 674
	public class WorkModeDrawer
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool DrawIconAtTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0004EF38 File Offset: 0x0004D138
		public virtual void DrawControlGroupMouseOverExtra(MechanitorControlGroup group)
		{
			GlobalTargetInfo targetForLine = this.GetTargetForLine(group);
			List<Pawn> mechsForReading = group.MechsForReading;
			Map currentMap = Find.CurrentMap;
			if (targetForLine.IsValid && targetForLine.Map == currentMap)
			{
				Vector3 vector = targetForLine.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MoteOverhead);
				for (int i = 0; i < mechsForReading.Count; i++)
				{
					if (mechsForReading[i].Map == currentMap)
					{
						GenDraw.DrawLineBetween(vector, mechsForReading[i].DrawPos, SimpleColor.White, 0.1f);
						GenDraw.DrawCircleOutline(mechsForReading[i].DrawPos, 0.5f);
					}
				}
				if (this.DrawIconAtTarget)
				{
					if (this.iconMat == null)
					{
						this.iconMat = MaterialPool.MatFrom(this.def.uiIcon);
					}
					Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.identity, WorkModeDrawer.IconScale);
					Graphics.DrawMesh(MeshPool.plane14, matrix, this.iconMat, 0);
				}
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0004F02C File Offset: 0x0004D22C
		public virtual GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		{
			return group.Target;
		}

		// Token: 0x04000DFF RID: 3583
		private const float MouseoverLineWidth = 0.1f;

		// Token: 0x04000E00 RID: 3584
		private const float CircleOutlineRadius = 0.5f;

		// Token: 0x04000E01 RID: 3585
		private static readonly Vector3 IconScale = Vector3.one * 0.5f;

		// Token: 0x04000E02 RID: 3586
		public MechWorkModeDef def;

		// Token: 0x04000E03 RID: 3587
		private Material iconMat;
	}
}
