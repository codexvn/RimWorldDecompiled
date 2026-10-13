using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02001F8D RID: 8077
	public static class ResolveParamsUtility
	{
		// Token: 0x0600B268 RID: 45672 RVA: 0x003317B4 File Offset: 0x0032F9B4
		public static void SetCustom<T>(ref Dictionary<string, object> custom, string name, T obj, bool inherit = false)
		{
			if (custom == null)
			{
				custom = new Dictionary<string, object>();
			}
			else
			{
				custom = new Dictionary<string, object>(custom);
			}
			if (!custom.ContainsKey(name))
			{
				custom.Add(name, obj);
				return;
			}
			if (!inherit)
			{
				custom[name] = obj;
			}
		}

		// Token: 0x0600B269 RID: 45673 RVA: 0x00331801 File Offset: 0x0032FA01
		public static void RemoveCustom(ref Dictionary<string, object> custom, string name)
		{
			if (custom == null)
			{
				return;
			}
			custom = new Dictionary<string, object>(custom);
			custom.Remove(name);
		}

		// Token: 0x0600B26A RID: 45674 RVA: 0x0033181C File Offset: 0x0032FA1C
		public static bool TryGetCustom<T>(Dictionary<string, object> custom, string name, out T obj)
		{
			object obj2;
			if (custom == null || !custom.TryGetValue(name, out obj2))
			{
				obj = default(T);
				return false;
			}
			obj = (T)((object)obj2);
			return true;
		}

		// Token: 0x0600B26B RID: 45675 RVA: 0x00331850 File Offset: 0x0032FA50
		public static T GetCustom<T>(Dictionary<string, object> custom, string name)
		{
			object obj;
			if (custom == null || !custom.TryGetValue(name, out obj))
			{
				return default(T);
			}
			return (T)((object)obj);
		}
	}
}
