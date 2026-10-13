using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004E2 RID: 1250
	public class ReachabilityCache
	{
		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x000A8B68 File Offset: 0x000A6D68
		public int Count
		{
			get
			{
				return this.cacheDict.Count;
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000A8B78 File Offset: 0x000A6D78
		public BoolUnknown CachedResultFor(District A, District B, TraverseParms traverseParams)
		{
			bool flag;
			if (!this.cacheDict.TryGetValue(new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams), out flag))
			{
				return BoolUnknown.Unknown;
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.True;
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000A8BB0 File Offset: 0x000A6DB0
		public void AddCachedResult(District A, District B, TraverseParms traverseParams, bool reachable)
		{
			ReachabilityCache.CachedEntry key = new ReachabilityCache.CachedEntry(A.ID, B.ID, traverseParams);
			if (!this.cacheDict.ContainsKey(key))
			{
				this.cacheDict.Add(key, reachable);
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000A8BED File Offset: 0x000A6DED
		public void Clear()
		{
			this.cacheDict.Clear();
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000A8BFC File Offset: 0x000A6DFC
		public void ClearFor(Pawn p)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				if (keyValuePair.Key.TraverseParms.pawn == p)
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000A8CB0 File Offset: 0x000A6EB0
		public void ClearForHostile(Thing hostileTo)
		{
			ReachabilityCache.tmpCachedEntries.Clear();
			foreach (KeyValuePair<ReachabilityCache.CachedEntry, bool> keyValuePair in this.cacheDict)
			{
				Pawn pawn = keyValuePair.Key.TraverseParms.pawn;
				if (pawn != null && pawn.HostileTo(hostileTo))
				{
					ReachabilityCache.tmpCachedEntries.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < ReachabilityCache.tmpCachedEntries.Count; i++)
			{
				this.cacheDict.Remove(ReachabilityCache.tmpCachedEntries[i]);
			}
			ReachabilityCache.tmpCachedEntries.Clear();
		}

		// Token: 0x04001B98 RID: 7064
		private Dictionary<ReachabilityCache.CachedEntry, bool> cacheDict = new Dictionary<ReachabilityCache.CachedEntry, bool>();

		// Token: 0x04001B99 RID: 7065
		private static List<ReachabilityCache.CachedEntry> tmpCachedEntries = new List<ReachabilityCache.CachedEntry>();

		// Token: 0x020004E3 RID: 1251
		private struct CachedEntry : IEquatable<ReachabilityCache.CachedEntry>
		{
			// Token: 0x170005BE RID: 1470
			// (get) Token: 0x0600204F RID: 8271 RVA: 0x000A8D93 File Offset: 0x000A6F93
			// (set) Token: 0x06002050 RID: 8272 RVA: 0x000A8D9B File Offset: 0x000A6F9B
			public int FirstID { readonly get; private set; }

			// Token: 0x170005BF RID: 1471
			// (get) Token: 0x06002051 RID: 8273 RVA: 0x000A8DA4 File Offset: 0x000A6FA4
			// (set) Token: 0x06002052 RID: 8274 RVA: 0x000A8DAC File Offset: 0x000A6FAC
			public int SecondID { readonly get; private set; }

			// Token: 0x170005C0 RID: 1472
			// (get) Token: 0x06002053 RID: 8275 RVA: 0x000A8DB5 File Offset: 0x000A6FB5
			// (set) Token: 0x06002054 RID: 8276 RVA: 0x000A8DBD File Offset: 0x000A6FBD
			public TraverseParms TraverseParms { readonly get; private set; }

			// Token: 0x06002055 RID: 8277 RVA: 0x000A8DC6 File Offset: 0x000A6FC6
			public CachedEntry(int firstID, int secondID, TraverseParms traverseParms)
			{
				this = default(ReachabilityCache.CachedEntry);
				if (firstID < secondID)
				{
					this.FirstID = firstID;
					this.SecondID = secondID;
				}
				else
				{
					this.FirstID = secondID;
					this.SecondID = firstID;
				}
				this.TraverseParms = traverseParms;
			}

			// Token: 0x06002056 RID: 8278 RVA: 0x000A8DF8 File Offset: 0x000A6FF8
			public static bool operator ==(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06002057 RID: 8279 RVA: 0x000A8E02 File Offset: 0x000A7002
			public static bool operator !=(ReachabilityCache.CachedEntry lhs, ReachabilityCache.CachedEntry rhs)
			{
				return !lhs.Equals(rhs);
			}

			// Token: 0x06002058 RID: 8280 RVA: 0x000A8E0F File Offset: 0x000A700F
			public override bool Equals(object obj)
			{
				return obj is ReachabilityCache.CachedEntry && this.Equals((ReachabilityCache.CachedEntry)obj);
			}

			// Token: 0x06002059 RID: 8281 RVA: 0x000A8E27 File Offset: 0x000A7027
			public bool Equals(ReachabilityCache.CachedEntry other)
			{
				return this.FirstID == other.FirstID && this.SecondID == other.SecondID && this.TraverseParms == other.TraverseParms;
			}

			// Token: 0x0600205A RID: 8282 RVA: 0x000A8E5B File Offset: 0x000A705B
			public override int GetHashCode()
			{
				return Gen.HashCombineStruct<TraverseParms>(Gen.HashCombineInt(this.FirstID, this.SecondID), this.TraverseParms);
			}
		}
	}
}
