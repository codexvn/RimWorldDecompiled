using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Verse
{
	// Token: 0x020001EE RID: 494
	public static class GenDefDatabase
	{
		// Token: 0x06000D49 RID: 3401 RVA: 0x0003EF9C File Offset: 0x0003D19C
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			object obj = GenDefDatabase.cachedGetNamedLock;
			Func<string, bool, Def> func;
			lock (obj)
			{
				if (!GenDefDatabase.cachedGetNamed.TryGetValue(defType, out func))
				{
					MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed");
					func = (Func<string, bool, Def>)Delegate.CreateDelegate(typeof(Func<string, bool, Def>), method);
					GenDefDatabase.cachedGetNamed.Add(defType, func);
				}
			}
			return func(defName, errorOnFail);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003F024 File Offset: 0x0003D224
		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{
			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				return SoundDef.Named(targetDefName);
			}
			object obj = GenDefDatabase.cachedGetNamedSilentFailLock;
			Func<string, Def> func;
			lock (obj)
			{
				if (!GenDefDatabase.cachedGetNamedSilentFail.TryGetValue(type, out func))
				{
					MethodInfo method = GenGeneric.MethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail");
					func = (Func<string, Def>)Delegate.CreateDelegate(typeof(Func<string, Def>), method);
					GenDefDatabase.cachedGetNamedSilentFail.Add(type, func);
				}
			}
			return func(targetDefName);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003F0C8 File Offset: 0x0003D2C8
		public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
		{
			return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003F0E9 File Offset: 0x0003D2E9
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			foreach (Type type in typeof(Def).AllSubclasses())
			{
				if (!type.IsAbstract && !(type == typeof(Def)))
				{
					bool flag = false;
					Type baseType = type.BaseType;
					while (baseType != null && baseType != typeof(Def))
					{
						if (!baseType.IsAbstract)
						{
							flag = true;
							break;
						}
						baseType = baseType.BaseType;
					}
					if (!flag)
					{
						yield return type;
					}
				}
			}
			List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0003F0F2 File Offset: 0x0003D2F2
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}

		// Token: 0x040007B0 RID: 1968
		private static readonly Dictionary<Type, Func<string, bool, Def>> cachedGetNamed = new Dictionary<Type, Func<string, bool, Def>>();

		// Token: 0x040007B1 RID: 1969
		private static readonly object cachedGetNamedLock = new object();

		// Token: 0x040007B2 RID: 1970
		private static readonly Dictionary<Type, Func<string, Def>> cachedGetNamedSilentFail = new Dictionary<Type, Func<string, Def>>();

		// Token: 0x040007B3 RID: 1971
		private static readonly object cachedGetNamedSilentFailLock = new object();
	}
}
