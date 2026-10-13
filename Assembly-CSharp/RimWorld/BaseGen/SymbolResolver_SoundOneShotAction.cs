using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200393B RID: 14651
	public class SymbolResolver_SoundOneShotAction : SymbolResolver
	{
		// Token: 0x060142C9 RID: 82633 RVA: 0x006031C0 File Offset: 0x006013C0
		public override void Resolve(ResolveParams rp)
		{
			SignalAction_SoundOneShot signalAction_SoundOneShot = (SignalAction_SoundOneShot)ThingMaker.MakeThing(ThingDefOf.SignalAction_SoundOneShot, null);
			signalAction_SoundOneShot.signalTag = rp.soundOneShotActionSignalTag;
			signalAction_SoundOneShot.sound = rp.sound;
			GenSpawn.Spawn(signalAction_SoundOneShot, rp.rect.CenterCell, BaseGen.globalSettings.map, WipeMode.Vanish);
		}

		// Token: 0x0400DF0B RID: 57099
		private const string SoundSignalPrefix = "SoundTrigger";
	}
}
