using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C92 RID: 11410
	public class CompFleshmassBase : ThingComp
	{
		// Token: 0x1700286C RID: 10348
		// (get) Token: 0x0601000C RID: 65548 RVA: 0x004A28B0 File Offset: 0x004A0AB0
		private CompProperties_FleshmassBase Props
		{
			get
			{
				return (CompProperties_FleshmassBase)this.props;
			}
		}

		// Token: 0x1700286D RID: 10349
		// (get) Token: 0x0601000D RID: 65549 RVA: 0x004A28C0 File Offset: 0x004A0AC0
		public List<Graphic> CachedBaseGFX
		{
			get
			{
				if (CompFleshmassBase.cachedBaseGFX == null)
				{
					CompFleshmassBase.cachedBaseGFX = new List<Graphic>
					{
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1A", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1B", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2A", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2B", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3A", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3B", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white)
					};
				}
				return CompFleshmassBase.cachedBaseGFX;
			}
		}

		// Token: 0x1700286E RID: 10350
		// (get) Token: 0x0601000E RID: 65550 RVA: 0x004A29DC File Offset: 0x004A0BDC
		public List<Graphic> CachedOutlineGFX
		{
			get
			{
				if (CompFleshmassBase.cachedOutlineGFX == null)
				{
					CompFleshmassBase.cachedOutlineGFX = new List<Graphic>
					{
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1A_Outline", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_1x1B_Outline", ShaderDatabase.Cutout, new Vector2(2.3f, 2.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2A_Outline", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_2x2B_Outline", ShaderDatabase.Cutout, new Vector2(3.3f, 3.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3A_Outline", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white),
						GraphicDatabase.Get<Graphic_Single>("Things/Building/Fleshmass/Bases/FleshmassBase_3x3B_Outline", ShaderDatabase.Cutout, new Vector2(4.3f, 4.3f), Color.white)
					};
				}
				return CompFleshmassBase.cachedOutlineGFX;
			}
		}

		// Token: 0x0601000F RID: 65551 RVA: 0x004A2AF8 File Offset: 0x004A0CF8
		public override void PostDraw()
		{
			int index = (this.Props.size - 1) * 2 + this.parent.thingIDNumber % 2;
			Rot4 rot = new Rot4(this.parent.thingIDNumber % 8 / 2);
			this.CachedBaseGFX[index].Draw(this.parent.DrawPos.WithYOffset(0.01f), rot, this.parent, 0f);
			this.CachedOutlineGFX[index].Draw(this.parent.DrawPos.WithYOffset(-0.01f), rot, this.parent, 0f);
		}

		// Token: 0x0400A769 RID: 42857
		[Unsaved(false)]
		private static List<Graphic> cachedBaseGFX;

		// Token: 0x0400A76A RID: 42858
		[Unsaved(false)]
		private static List<Graphic> cachedOutlineGFX;
	}
}
