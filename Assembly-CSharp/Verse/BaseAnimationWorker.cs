using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005E9 RID: 1513
	[StaticConstructorOnStartup]
	public abstract class BaseAnimationWorker
	{
		// Token: 0x060026C4 RID: 9924 RVA: 0x000CD650 File Offset: 0x000CB850
		static BaseAnimationWorker()
		{
			foreach (Type type in typeof(BaseAnimationWorker).AllSubclassesNonAbstract())
			{
				GenWorker<BaseAnimationWorker>.Get(type);
			}
		}

		// Token: 0x060026C5 RID: 9925
		public abstract bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x060026C6 RID: 9926
		public abstract void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix);

		// Token: 0x060026C7 RID: 9927
		public abstract Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x060026C8 RID: 9928
		public abstract float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x060026C9 RID: 9929
		public abstract Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);

		// Token: 0x060026CA RID: 9930
		public abstract GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms);
	}
}
