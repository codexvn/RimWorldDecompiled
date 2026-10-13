using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000F16 RID: 3862
	public static class Toils_Effects
	{
		// Token: 0x06006059 RID: 24665 RVA: 0x001E90B4 File Offset: 0x001E72B4
		public static Toil MakeSound(SoundDef soundDef)
		{
			Toil toil = ToilMaker.MakeToil("MakeSound");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return toil;
		}
	}
}
