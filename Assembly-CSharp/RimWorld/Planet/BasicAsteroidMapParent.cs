using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DF9 RID: 15865
	public class BasicAsteroidMapParent : SpaceMapParent
	{
		// Token: 0x06015A3A RID: 88634 RVA: 0x0067A4A8 File Offset: 0x006786A8
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.preciousResource != null)
			{
				text += "\n" + "TracesOfPreciousResource".Translate(this.preciousResource.Named("RESOURCE"));
			}
			return text.Trim();
		}
	}
}
