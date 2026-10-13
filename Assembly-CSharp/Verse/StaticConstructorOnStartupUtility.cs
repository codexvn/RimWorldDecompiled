using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200016A RID: 362
	public static class StaticConstructorOnStartupUtility
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x00031F8C File Offset: 0x0003018C
		public static void CallAll()
		{
			DeepProfiler.Start("StaticConstructorOnStartupUtility.CallAll()");
			foreach (Type type in GenTypes.AllTypesWithAttribute<StaticConstructorOnStartup>())
			{
				try
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
				catch (Exception ex)
				{
					string str = "Error in static constructor of ";
					Type type2 = type;
					string str2 = (type2 != null) ? type2.ToString() : null;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			DeepProfiler.End();
			StaticConstructorOnStartupUtility.coreStaticAssetsLoaded = true;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00032038 File Offset: 0x00030238
		public static void ReportProbablyMissingAttributes()
		{
			BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Parallel.ForEach<Type>(GenTypes.AllTypes, delegate(Type t)
			{
				if (t.HasAttribute<StaticConstructorOnStartup>())
				{
					return;
				}
				FieldInfo fieldInfo = t.GetFields(bindingFlags).FirstOrDefault(delegate(FieldInfo x)
				{
					Type type = x.FieldType;
					if (type.IsArray)
					{
						type = type.GetElementType();
					}
					return typeof(Texture).IsAssignableFrom(type) || typeof(Material).IsAssignableFrom(type) || typeof(Shader).IsAssignableFrom(type) || typeof(Graphic).IsAssignableFrom(type) || typeof(GameObject).IsAssignableFrom(type) || typeof(MaterialPropertyBlock).IsAssignableFrom(type);
				});
				if (fieldInfo != null)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Type ",
						t.Name,
						" probably needs a StaticConstructorOnStartup attribute, because it has a field ",
						fieldInfo.Name,
						" of type ",
						fieldInfo.FieldType.Name,
						". All assets must be loaded in the main thread."
					}));
				}
			});
		}

		// Token: 0x04000591 RID: 1425
		public static bool coreStaticAssetsLoaded;
	}
}
