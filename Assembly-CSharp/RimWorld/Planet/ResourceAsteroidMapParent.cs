using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DFA RID: 15866
	public class ResourceAsteroidMapParent : SpaceMapParent
	{
		// Token: 0x170035E7 RID: 13799
		// (get) Token: 0x06015A3C RID: 88636 RVA: 0x0067A502 File Offset: 0x00678702
		public override Color ExpandingIconColor
		{
			get
			{
				return this.cachedStuffColor;
			}
		}

		// Token: 0x06015A3D RID: 88637 RVA: 0x0067A50C File Offset: 0x0067870C
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			ThingDef mineableThing = this.preciousResource.building.mineableThing;
			this.cachedStuffColor = mineableThing.stuffProps.color;
		}

		// Token: 0x0400F06C RID: 61548
		private Color cachedStuffColor;
	}
}
