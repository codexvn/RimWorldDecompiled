using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003675 RID: 13941
	public static class VerbDefsHardcodedNative
	{
		// Token: 0x06013590 RID: 79248 RVA: 0x005C1918 File Offset: 0x005BFB18
		public static IEnumerable<VerbProperties> AllVerbDefs()
		{
			yield return new VerbProperties
			{
				verbClass = typeof(Verb_BeatFire),
				category = VerbCategory.BeatFire,
				range = 1.42f,
				noiseRadius = 3f,
				targetParams = 
				{
					canTargetFires = true,
					canTargetPawns = false,
					canTargetBuildings = false,
					mapObjectTargetsMustBeAutoAttackable = false
				},
				warmupTime = 0f,
				defaultCooldownTime = 1.1f,
				soundCast = SoundDefOf.Interact_BeatFire
			};
			yield return new VerbProperties
			{
				verbClass = typeof(Verb_Ignite),
				category = VerbCategory.Ignite,
				range = 1.42f,
				noiseRadius = 3f,
				targetParams = 
				{
					onlyTargetFlammables = true,
					canTargetBuildings = true,
					canTargetPawns = false,
					mapObjectTargetsMustBeAutoAttackable = false
				},
				warmupTime = 3f,
				defaultCooldownTime = 1.3f,
				soundCast = SoundDefOf.Interact_Ignite
			};
			yield break;
		}
	}
}
