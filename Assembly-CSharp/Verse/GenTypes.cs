using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.QuestGen;

namespace Verse
{
	// Token: 0x0200011A RID: 282
	public static class GenTypes
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00027B47 File Offset: 0x00025D47
		private static IEnumerable<Assembly> AllActiveAssemblies
		{
			get
			{
				yield return Assembly.GetExecutingAssembly();
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					int num;
					for (int i = 0; i < mod.assemblies.loadedAssemblies.Count; i = num + 1)
					{
						yield return mod.assemblies.loadedAssemblies[i];
						num = i;
					}
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00027B50 File Offset: 0x00025D50
		public static List<Type> AllTypes
		{
			get
			{
				if (GenTypes.allTypesCached == null)
				{
					GenTypes.allTypesCached = new List<Type>();
					GenTypes.tmpAllTypesHashSet.Clear();
					foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
					{
						Type[] array = null;
						try
						{
							array = assembly.GetTypes();
						}
						catch (ReflectionTypeLoadException ex)
						{
							string str = "Exception getting types in assembly ";
							string str2 = assembly.ToString();
							string str3 = ". Some types may not work correctly. Exception: ";
							ReflectionTypeLoadException ex2 = ex;
							Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
							try
							{
								Type[] types = ex.Types;
								if (types != null)
								{
									array = (from x in types
									where x != null && x.TypeInitializer != null
									select x).ToArray<Type>();
								}
							}
							catch (Exception ex3)
							{
								string str4 = "Could not resolve assembly types fallback. Exception: ";
								Exception ex4 = ex3;
								Log.Error(str4 + ((ex4 != null) ? ex4.ToString() : null));
							}
						}
						if (array != null)
						{
							for (int i = 0; i < array.Length; i++)
							{
								if (array[i] != null && GenTypes.tmpAllTypesHashSet.Add(array[i]))
								{
									GenTypes.allTypesCached.Add(array[i]);
								}
							}
						}
					}
					GenTypes.tmpAllTypesHashSet.Clear();
				}
				return GenTypes.allTypesCached;
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00027CB4 File Offset: 0x00025EB4
		public static List<Type> AllTypesWithAttribute<TAttr>() where TAttr : Attribute
		{
			List<Type> result;
			if (GenTypes.cachedTypesWithAttribute.TryGetValue(typeof(TAttr), out result))
			{
				return result;
			}
			List<Type> list = (from x in GenTypes.AllTypes.AsParallel<Type>()
			where x.HasAttribute<TAttr>()
			select x).ToList<Type>();
			GenTypes.cachedTypesWithAttribute.Add(typeof(TAttr), list);
			return list;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00027D28 File Offset: 0x00025F28
		public static List<Type> AllSubclasses(this Type baseType)
		{
			if (!GenTypes.cachedSubclasses.ContainsKey(baseType))
			{
				GenTypes.cachedSubclasses.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where x.IsSubclassOf(baseType)
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclasses[baseType];
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00027D94 File Offset: 0x00025F94
		public static bool SameOrSubclassOf<T>(this Type baseType)
		{
			return baseType.SameOrSubclassOf(typeof(T));
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00027DA6 File Offset: 0x00025FA6
		public static bool SameOrSubclassOf(this Type baseType, Type parentType)
		{
			return baseType == parentType || baseType.IsSubclassOf(parentType);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00027DBC File Offset: 0x00025FBC
		public static List<Type> AllSubclassesNonAbstract(this Type baseType)
		{
			if (!GenTypes.cachedSubclassesNonAbstract.ContainsKey(baseType))
			{
				GenTypes.cachedSubclassesNonAbstract.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where x.IsSubclassOf(baseType) && !x.IsAbstract
				select x).ToList<Type>());
			}
			return GenTypes.cachedSubclassesNonAbstract[baseType];
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00027E28 File Offset: 0x00026028
		public static List<Type> AllAssignableOfNonAbstract(this Type baseType)
		{
			if (!GenTypes.cachedAssignableNonAbstract.ContainsKey(baseType))
			{
				GenTypes.cachedAssignableNonAbstract.Add(baseType, (from x in GenTypes.AllTypes.AsParallel<Type>()
				where baseType.IsAssignableFrom(x) && !x.IsAbstract
				select x).ToList<Type>());
			}
			return GenTypes.cachedAssignableNonAbstract[baseType];
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00027E94 File Offset: 0x00026094
		public static void ClearCache()
		{
			GenTypes.cachedSubclasses.Clear();
			GenTypes.cachedSubclassesNonAbstract.Clear();
			GenTypes.cachedAssignableNonAbstract.Clear();
			GenTypes.cachedTypesWithAttribute.Clear();
			GenTypes.allTypesCached = null;
			GenTypes.typesInIgnoredNamespacesByName.Clear();
			GenTypes.isSealedCache.Clear();
			GenTypes.initializedTypesInIgnoredNamespaces = false;
			AlertsReadout.allAlertTypesCached = null;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00027EEF File Offset: 0x000260EF
		public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
		{
			return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any<Type>()
			select type;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00027F1B File Offset: 0x0002611B
		public static IEnumerable<Type> InstantiableDescendantsAndSelf(this Type baseType)
		{
			if (!baseType.IsAbstract)
			{
				yield return baseType;
			}
			foreach (Type type in baseType.AllSubclasses())
			{
				if (!type.IsAbstract)
				{
					yield return type;
				}
			}
			List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00027F2C File Offset: 0x0002612C
		private static Type TryGetTypeInIgnoredNamespace(string typeName)
		{
			if (!GenTypes.initializedTypesInIgnoredNamespaces)
			{
				foreach (Type type in GenTypes.AllTypes)
				{
					if (type.Namespace == null || GenTypes.IgnoredNamespaceNames.Contains(type.Namespace))
					{
						GenTypes.typesInIgnoredNamespacesByName[type.Name] = type;
					}
				}
				GenTypes.initializedTypesInIgnoredNamespaces = true;
			}
			Type result;
			if (GenTypes.typesInIgnoredNamespacesByName.TryGetValue(typeName, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00027FC4 File Offset: 0x000261C4
		public static Type GetTypeInAnyAssembly(string typeName, string namespaceIfAmbiguous = null)
		{
			if (namespaceIfAmbiguous == null || GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				Type type = GenTypes.TryGetTypeInIgnoredNamespace(typeName);
				if (type != null)
				{
					return type;
				}
			}
			GenTypes.TypeCacheKey key = new GenTypes.TypeCacheKey(typeName, namespaceIfAmbiguous);
			Type type2 = null;
			if (!GenTypes.typeCache.TryGetValue(key, out type2))
			{
				type2 = GenTypes.GetTypeInAnyAssemblyInt(typeName, namespaceIfAmbiguous);
				GenTypes.typeCache.Add(key, type2);
			}
			return type2;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00028024 File Offset: 0x00026224
		public static bool HasFlagsAttribute(Type type)
		{
			bool result;
			if (GenTypes.hasFlagsAttributeCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = Attribute.IsDefined(type, typeof(FlagsAttribute));
			GenTypes.hasFlagsAttributeCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00028060 File Offset: 0x00026260
		public static bool IsList(Type type)
		{
			bool result;
			if (GenTypes.isListCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = type.HasGenericDefinition(typeof(List<>));
			GenTypes.isListCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0002809C File Offset: 0x0002629C
		public static bool IsDictionary(Type type)
		{
			bool result;
			if (GenTypes.isDictionaryCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = type.HasGenericDefinition(typeof(Dictionary<, >));
			GenTypes.isDictionaryCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000280D8 File Offset: 0x000262D8
		public static bool IsSlateRef(Type type)
		{
			bool result;
			if (GenTypes.isSlateRefCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = typeof(ISlateRef).IsAssignableFrom(type);
			GenTypes.isSlateRefCache.Add(type, flag);
			return flag;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00028114 File Offset: 0x00026314
		public static bool IsListHashSetOrDictionary(Type type)
		{
			bool result;
			if (GenTypes.isListHashSetOrDictionaryCached.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = false;
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(List<>) || genericTypeDefinition == typeof(HashSet<>) || genericTypeDefinition == typeof(Dictionary<, >))
				{
					flag = true;
				}
			}
			GenTypes.isListHashSetOrDictionaryCached.Add(type, flag);
			return flag;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00028188 File Offset: 0x00026388
		public static bool IsDef(Type type)
		{
			bool result;
			if (GenTypes.isDefCache.TryGetValue(type, out result))
			{
				return result;
			}
			bool flag = typeof(Def).IsAssignableFrom(type);
			GenTypes.isDefCache.Add(type, flag);
			return flag;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x000281C4 File Offset: 0x000263C4
		public static bool IsDefThreaded(Type type)
		{
			object obj = GenTypes.isDefCacheLock;
			bool result;
			lock (obj)
			{
				result = GenTypes.IsDef(type);
			}
			return result;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00028208 File Offset: 0x00026408
		private static Type GetTypeInAnyAssemblyInt(string typeName, string namespaceIfAmbiguous = null)
		{
			Type typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (typeInAnyAssemblyRaw != null)
			{
				return typeInAnyAssemblyRaw;
			}
			if (!namespaceIfAmbiguous.NullOrEmpty() && GenTypes.IgnoredNamespaceNames.Contains(namespaceIfAmbiguous))
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(namespaceIfAmbiguous + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			foreach (string str in GenTypes.IgnoredNamespaceNames)
			{
				typeInAnyAssemblyRaw = GenTypes.GetTypeInAnyAssemblyRaw(str + "." + typeName);
				if (typeInAnyAssemblyRaw != null)
				{
					return typeInAnyAssemblyRaw;
				}
			}
			if (GenTypes.TryGetMixedAssemblyGenericType(typeName, out typeInAnyAssemblyRaw))
			{
				return typeInAnyAssemblyRaw;
			}
			return null;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000282C4 File Offset: 0x000264C4
		private static bool TryGetMixedAssemblyGenericType(string typeName, out Type type)
		{
			type = GenTypes.GetTypeInAnyAssemblyRaw(typeName);
			if (type == null && typeName.Contains("`"))
			{
				try
				{
					Match match = Regex.Match(typeName, "(?<MainType>.+`(?<ParamCount>[0-9]+))(?<Types>\\[.*\\])");
					if (match.Success)
					{
						int capacity = int.Parse(match.Groups["ParamCount"].Value);
						string value = match.Groups["Types"].Value;
						List<string> list = new List<string>(capacity);
						foreach (object obj in Regex.Matches(value, "\\[(?<Type>.*?)\\],?"))
						{
							Match match2 = (Match)obj;
							if (match2.Success)
							{
								list.Add(match2.Groups["Type"].Value.Trim());
							}
						}
						Type[] array = new Type[list.Count];
						for (int i = 0; i < list.Count; i++)
						{
							Type type2;
							if (!GenTypes.TryGetMixedAssemblyGenericType(list[i], out type2))
							{
								return false;
							}
							array[i] = type2;
						}
						Type type3;
						if (GenTypes.TryGetMixedAssemblyGenericType(match.Groups["MainType"].Value, out type3))
						{
							type = type3.MakeGenericType(array);
						}
					}
				}
				catch (Exception ex)
				{
					string str = "Error in TryGetMixedAssemblyGenericType with typeName=";
					string str2 = ": ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + typeName + str2 + ((ex2 != null) ? ex2.ToString() : null), typeName.GetHashCode());
				}
			}
			return type != null;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00028484 File Offset: 0x00026684
		private static Type GetTypeInAnyAssemblyRaw(string typeName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeName);
			if (num <= 2299065237U)
			{
				if (num <= 1092586446U)
				{
					if (num <= 431052896U)
					{
						if (num != 296283782U)
						{
							if (num != 398550328U)
							{
								if (num == 431052896U)
								{
									if (typeName == "byte?")
									{
										return typeof(byte?);
									}
								}
							}
							else if (typeName == "string")
							{
								return typeof(string);
							}
						}
						else if (typeName == "char?")
						{
							return typeof(char?);
						}
					}
					else if (num != 513669818U)
					{
						if (num != 520654156U)
						{
							if (num == 1092586446U)
							{
								if (typeName == "float?")
								{
									return typeof(float?);
								}
							}
						}
						else if (typeName == "decimal")
						{
							return typeof(decimal);
						}
					}
					else if (typeName == "uint?")
					{
						return typeof(uint?);
					}
				}
				else if (num <= 1454009365U)
				{
					if (num != 1189328644U)
					{
						if (num != 1299622921U)
						{
							if (num == 1454009365U)
							{
								if (typeName == "sbyte?")
								{
									return typeof(sbyte?);
								}
							}
						}
						else if (typeName == "decimal?")
						{
							return typeof(decimal?);
						}
					}
					else if (typeName == "long?")
					{
						return typeof(long?);
					}
				}
				else if (num <= 1630192034U)
				{
					if (num != 1603400371U)
					{
						if (num == 1630192034U)
						{
							if (typeName == "ushort")
							{
								return typeof(ushort);
							}
						}
					}
					else if (typeName == "int?")
					{
						return typeof(int?);
					}
				}
				else if (num != 1683620383U)
				{
					if (num == 2299065237U)
					{
						if (typeName == "double?")
						{
							return typeof(double?);
						}
					}
				}
				else if (typeName == "byte")
				{
					return typeof(byte);
				}
			}
			else if (num <= 2823553821U)
			{
				if (num <= 2515107422U)
				{
					if (num != 2471414311U)
					{
						if (num != 2508976771U)
						{
							if (num == 2515107422U)
							{
								if (typeName == "int")
								{
									return typeof(int);
								}
							}
						}
						else if (typeName == "ulong?")
						{
							return typeof(ulong?);
						}
					}
					else if (typeName == "ushort?")
					{
						return typeof(ushort?);
					}
				}
				else if (num <= 2699759368U)
				{
					if (num != 2667225454U)
					{
						if (num == 2699759368U)
						{
							if (typeName == "double")
							{
								return typeof(double);
							}
						}
					}
					else if (typeName == "ulong")
					{
						return typeof(ulong);
					}
				}
				else if (num != 2797886853U)
				{
					if (num == 2823553821U)
					{
						if (typeName == "char")
						{
							return typeof(char);
						}
					}
				}
				else if (typeName == "float")
				{
					return typeof(float);
				}
			}
			else if (num <= 3286667814U)
			{
				if (num != 3122818005U)
				{
					if (num != 3270303571U)
					{
						if (num == 3286667814U)
						{
							if (typeName == "bool?")
							{
								return typeof(bool?);
							}
						}
					}
					else if (typeName == "long")
					{
						return typeof(long);
					}
				}
				else if (typeName == "short")
				{
					return typeof(short);
				}
			}
			else if (num <= 3415750305U)
			{
				if (num != 3365180733U)
				{
					if (num == 3415750305U)
					{
						if (typeName == "uint")
						{
							return typeof(uint);
						}
					}
				}
				else if (typeName == "bool")
				{
					return typeof(bool);
				}
			}
			else if (num != 3996115294U)
			{
				if (num == 4088464520U)
				{
					if (typeName == "sbyte")
					{
						return typeof(sbyte);
					}
				}
			}
			else if (typeName == "short?")
			{
				return typeof(short?);
			}
			Type type = typeof(GenTypes).Assembly.GetType(typeName, false, true);
			if (type != null)
			{
				return type;
			}
			foreach (Assembly assembly in GenTypes.AllActiveAssemblies)
			{
				Type type2 = assembly.GetType(typeName, false, true);
				if (type2 != null)
				{
					return type2;
				}
			}
			Type type3 = Type.GetType(typeName, false, true);
			if (type3 != null)
			{
				return type3;
			}
			return null;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00028A34 File Offset: 0x00026C34
		public static string GetTypeNameWithoutIgnoredNamespaces(Type type)
		{
			if (type.IsGenericType)
			{
				return type.ToString();
			}
			if (GenTypes.IgnoredNamespaceNames.Contains(type.Namespace))
			{
				return type.Name;
			}
			return type.FullName;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00028A64 File Offset: 0x00026C64
		public static bool IsCustomType(Type type)
		{
			string @namespace = type.Namespace;
			return @namespace == null || (!@namespace.StartsWith("System") && !@namespace.StartsWith("UnityEngine") && !@namespace.StartsWith("Steamworks"));
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00028AA7 File Offset: 0x00026CA7
		public static bool Isnt<T>(this object obj)
		{
			return !(obj is T);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00028AB5 File Offset: 0x00026CB5
		public static bool Isnt<T>(this object obj, out T casted) where T : class
		{
			casted = (obj as T);
			return casted == null;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00028AD8 File Offset: 0x00026CD8
		public static bool IsSealedWithCache(this Type type)
		{
			Dictionary<Type, bool> obj = GenTypes.isSealedCache;
			bool result;
			lock (obj)
			{
				bool flag2;
				if (GenTypes.isSealedCache.TryGetValue(type, out flag2))
				{
					result = flag2;
				}
				else
				{
					result = (GenTypes.isSealedCache[type] = type.IsSealed);
				}
			}
			return result;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00028B3C File Offset: 0x00026D3C
		public static bool IsMethodOverriden(this Type type, string method)
		{
			return type.GetMethod(method).IsOverriden();
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00028B4A File Offset: 0x00026D4A
		public static bool IsOverriden(this MethodInfo method)
		{
			return method.IsVirtual && method.DeclaringType != method.GetBaseDefinition().DeclaringType;
		}

		// Token: 0x04000457 RID: 1111
		public static readonly HashSet<string> IgnoredNamespaceNames = new HashSet<string>
		{
			"RimWorld",
			"Verse",
			"LudeonTK",
			"Verse.AI",
			"Verse.AI.Group",
			"Verse.Sound",
			"Verse.Grammar",
			"RimWorld.Planet",
			"RimWorld.BaseGen",
			"RimWorld.QuestGen",
			"RimWorld.SketchGen",
			"System"
		};

		// Token: 0x04000458 RID: 1112
		private static List<Type> allTypesCached;

		// Token: 0x04000459 RID: 1113
		private static HashSet<Type> tmpAllTypesHashSet = new HashSet<Type>();

		// Token: 0x0400045A RID: 1114
		private static Dictionary<Type, List<Type>> cachedTypesWithAttribute = new Dictionary<Type, List<Type>>();

		// Token: 0x0400045B RID: 1115
		private static Dictionary<Type, List<Type>> cachedSubclasses = new Dictionary<Type, List<Type>>();

		// Token: 0x0400045C RID: 1116
		private static Dictionary<Type, List<Type>> cachedSubclassesNonAbstract = new Dictionary<Type, List<Type>>();

		// Token: 0x0400045D RID: 1117
		private static readonly Dictionary<Type, List<Type>> cachedAssignableNonAbstract = new Dictionary<Type, List<Type>>();

		// Token: 0x0400045E RID: 1118
		private static readonly Dictionary<string, Type> typesInIgnoredNamespacesByName = new Dictionary<string, Type>();

		// Token: 0x0400045F RID: 1119
		private static bool initializedTypesInIgnoredNamespaces = false;

		// Token: 0x04000460 RID: 1120
		private static Dictionary<GenTypes.TypeCacheKey, Type> typeCache = new Dictionary<GenTypes.TypeCacheKey, Type>(EqualityComparer<GenTypes.TypeCacheKey>.Default);

		// Token: 0x04000461 RID: 1121
		private static Dictionary<Type, bool> hasFlagsAttributeCache = new Dictionary<Type, bool>();

		// Token: 0x04000462 RID: 1122
		private static Dictionary<Type, bool> isListCache = new Dictionary<Type, bool>();

		// Token: 0x04000463 RID: 1123
		private static Dictionary<Type, bool> isDictionaryCache = new Dictionary<Type, bool>();

		// Token: 0x04000464 RID: 1124
		private static Dictionary<Type, bool> isSlateRefCache = new Dictionary<Type, bool>();

		// Token: 0x04000465 RID: 1125
		private static Dictionary<Type, bool> isListHashSetOrDictionaryCached = new Dictionary<Type, bool>();

		// Token: 0x04000466 RID: 1126
		private static Dictionary<Type, bool> isDefCache = new Dictionary<Type, bool>();

		// Token: 0x04000467 RID: 1127
		private static object isDefCacheLock = new object();

		// Token: 0x04000468 RID: 1128
		private static readonly Dictionary<Type, bool> isSealedCache = new Dictionary<Type, bool>();

		// Token: 0x0200011B RID: 283
		private struct TypeCacheKey : IEquatable<GenTypes.TypeCacheKey>
		{
			// Token: 0x0600089D RID: 2205 RVA: 0x00028CB4 File Offset: 0x00026EB4
			public override int GetHashCode()
			{
				if (this.namespaceIfAmbiguous == null)
				{
					return this.typeName.GetHashCode();
				}
				return (17 * 31 + this.typeName.GetHashCode()) * 31 + this.namespaceIfAmbiguous.GetHashCode();
			}

			// Token: 0x0600089E RID: 2206 RVA: 0x00028CEA File Offset: 0x00026EEA
			public bool Equals(GenTypes.TypeCacheKey other)
			{
				return string.Equals(this.typeName, other.typeName) && string.Equals(this.namespaceIfAmbiguous, other.namespaceIfAmbiguous);
			}

			// Token: 0x0600089F RID: 2207 RVA: 0x00028D12 File Offset: 0x00026F12
			public override bool Equals(object obj)
			{
				return obj is GenTypes.TypeCacheKey && this.Equals((GenTypes.TypeCacheKey)obj);
			}

			// Token: 0x060008A0 RID: 2208 RVA: 0x00028D2A File Offset: 0x00026F2A
			public TypeCacheKey(string typeName, string namespaceIfAmbigous = null)
			{
				this.typeName = typeName;
				this.namespaceIfAmbiguous = namespaceIfAmbigous;
			}

			// Token: 0x04000469 RID: 1129
			public string typeName;

			// Token: 0x0400046A RID: 1130
			public string namespaceIfAmbiguous;
		}
	}
}
