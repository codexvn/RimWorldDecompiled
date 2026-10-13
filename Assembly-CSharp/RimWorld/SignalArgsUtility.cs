using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D22 RID: 7458
	public static class SignalArgsUtility
	{
		// Token: 0x0600A6D8 RID: 42712 RVA: 0x002FC8EC File Offset: 0x002FAAEC
		public static bool TryGetLookTargets(SignalArgs args, string name, out LookTargets lookTargets)
		{
			if (args.TryGetArg<LookTargets>(name, out lookTargets))
			{
				return true;
			}
			Thing t;
			if (args.TryGetArg<Thing>(name, out t))
			{
				lookTargets = t;
				return true;
			}
			WorldObject o;
			if (args.TryGetArg<WorldObject>(name, out o))
			{
				lookTargets = o;
				return true;
			}
			GlobalTargetInfo target;
			if (args.TryGetArg<GlobalTargetInfo>(name, out target))
			{
				lookTargets = target;
				return true;
			}
			TargetInfo target2;
			if (args.TryGetArg<TargetInfo>(name, out target2))
			{
				lookTargets = target2;
				return true;
			}
			List<Thing> targets;
			if (args.TryGetArg<List<Thing>>(name, out targets))
			{
				lookTargets = targets;
				return true;
			}
			lookTargets = LookTargets.Invalid;
			return false;
		}
	}
}
