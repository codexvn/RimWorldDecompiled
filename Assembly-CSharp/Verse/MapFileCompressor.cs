using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003D4 RID: 980
	public class MapFileCompressor : IExposable
	{
		// Token: 0x06001923 RID: 6435 RVA: 0x0007EE6C File Offset: 0x0007D06C
		public MapFileCompressor(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001924 RID: 6436 RVA: 0x0007EE7B File Offset: 0x0007D07B
		public void ExposeData()
		{
			DataExposeUtility.LookByteArray(ref this.compressedData, "compressedThingMap");
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0007EE8D File Offset: 0x0007D08D
		public void BuildCompressedString()
		{
			this.compressibilityDecider = new CompressibilityDecider(this.map);
			this.compressibilityDecider.DetermineReferences();
			this.compressedData = MapSerializeUtility.SerializeUshort(this.map, new Func<IntVec3, ushort>(this.HashValueForSquare));
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0007EEC8 File Offset: 0x0007D0C8
		private ushort HashValueForSquare(IntVec3 curSq)
		{
			ushort num = 0;
			foreach (Thing thing in this.map.thingGrid.ThingsAt(curSq))
			{
				if (thing.IsSaveCompressible())
				{
					if (num != 0)
					{
						string str = "Found two compressible things in ";
						IntVec3 intVec = curSq;
						string str2 = intVec.ToString();
						string str3 = ". The last was ";
						Thing thing2 = thing;
						Log.Error(str + str2 + str3 + ((thing2 != null) ? thing2.ToString() : null));
					}
					num = thing.def.shortHash;
				}
			}
			return num;
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0007EF64 File Offset: 0x0007D164
		public IEnumerable<Thing> ThingsToSpawnAfterLoad()
		{
			Dictionary<ushort, ThingDef> thingDefsByShortHash = new Dictionary<ushort, ThingDef>();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				ThingDef arg;
				if (thingDefsByShortHash.TryGetValue(thingDef.shortHash, out arg))
				{
					Log.Error(string.Format("Hash collision between {0} and  {1}: both have short hash {2}", thingDef, arg, thingDef.shortHash));
				}
				else
				{
					thingDefsByShortHash.Add(thingDef.shortHash, thingDef);
				}
			}
			int major = ScribeMetaHeaderUtility.loadedGameVersionMajor;
			int minor = ScribeMetaHeaderUtility.loadedGameVersionMinor;
			List<Thing> loadables = new List<Thing>();
			MapSerializeUtility.LoadUshort(this.compressedData, this.map, delegate(IntVec3 c, ushort val)
			{
				if (val == 0)
				{
					return;
				}
				ThingDef thingDef2 = BackCompatibility.BackCompatibleThingDefWithShortHash_Force(val, major, minor);
				if (thingDef2 == null)
				{
					try
					{
						thingDef2 = thingDefsByShortHash[val];
					}
					catch (KeyNotFoundException)
					{
						ThingDef thingDef3 = BackCompatibility.BackCompatibleThingDefWithShortHash(val);
						if (thingDef3 != null)
						{
							thingDef2 = thingDef3;
							thingDefsByShortHash.Add(val, thingDef3);
						}
						else
						{
							Log.Error("Map compressor decompression error: No thingDef with short hash " + val.ToString() + ". Adding as null to dictionary.");
							thingDefsByShortHash.Add(val, null);
						}
					}
				}
				if (thingDef2 != null)
				{
					try
					{
						if (!thingDef2.saveCompressible)
						{
							Log.Warning("Tried loading non-compressible thing as compressed thing: " + thingDef2.defName);
						}
						else
						{
							Thing thing = ThingMaker.MakeThing(thingDef2, null);
							thing.SetPositionDirect(c);
							loadables.Add(thing);
						}
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate compressed thing: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			});
			return loadables;
		}

		// Token: 0x04001635 RID: 5685
		private Map map;

		// Token: 0x04001636 RID: 5686
		private byte[] compressedData;

		// Token: 0x04001637 RID: 5687
		public CompressibilityDecider compressibilityDecider;
	}
}
