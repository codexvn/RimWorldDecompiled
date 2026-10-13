using System;
using System.Reflection;
using System.Threading.Tasks;
using Verse;

namespace RimWorld
{
	// Token: 0x02003582 RID: 13698
	public static class DefOfHelper
	{
		// Token: 0x0601336E RID: 78702 RVA: 0x005B56B4 File Offset: 0x005B38B4
		public static void RebindAllDefOfs(bool earlyTryMode)
		{
			DefOfHelper.earlyTry = earlyTryMode;
			DefOfHelper.bindingNow = true;
			try
			{
				Parallel.ForEach<Type>(GenTypes.AllTypesWithAttribute<DefOf>(), new Action<Type>(DefOfHelper.BindDefsFor));
			}
			finally
			{
				DefOfHelper.bindingNow = false;
			}
		}

		// Token: 0x0601336F RID: 78703 RVA: 0x005B5700 File Offset: 0x005B3900
		private static void BindDefsFor(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				Type fieldType = fieldInfo.FieldType;
				if (!GenTypes.IsDefThreaded(fieldType))
				{
					Type type2 = fieldType;
					Log.Error(((type2 != null) ? type2.ToString() : null) + " is not a Def.");
				}
				else
				{
					bool flag = !DefOfHelper.earlyTry;
					MayRequireAttribute mayRequireAttribute = fieldInfo.TryGetAttribute<MayRequireAttribute>();
					if (mayRequireAttribute != null && !ModsConfig.IsActive(mayRequireAttribute.modId))
					{
						flag = false;
					}
					MayRequireAnyOfAttribute mayRequireAnyOfAttribute = fieldInfo.TryGetAttribute<MayRequireAnyOfAttribute>();
					if (mayRequireAnyOfAttribute != null)
					{
						bool flag2 = false;
						string[] modIds = mayRequireAnyOfAttribute.modIds;
						for (int j = 0; j < modIds.Length; j++)
						{
							if (ModsConfig.IsActive(modIds[j]))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							flag = false;
						}
					}
					string text = fieldInfo.Name;
					DefAliasAttribute defAliasAttribute = fieldInfo.TryGetAttribute<DefAliasAttribute>();
					if (defAliasAttribute != null)
					{
						text = defAliasAttribute.defName;
					}
					if (fieldType == typeof(SoundDef))
					{
						SoundDef soundDef = SoundDef.Named(text);
						if (soundDef.isUndefined && flag)
						{
							Log.Error("Could not find SoundDef named " + text);
						}
						fieldInfo.SetValue(null, soundDef);
					}
					else
					{
						Def def = GenDefDatabase.GetDef(fieldType, text, flag);
						fieldInfo.SetValue(null, def);
					}
				}
			}
		}

		// Token: 0x06013370 RID: 78704 RVA: 0x005B5840 File Offset: 0x005B3A40
		public static void EnsureInitializedInCtor(Type defOf)
		{
			if (!DefOfHelper.bindingNow)
			{
				string text;
				if (DirectXmlToObject.currentlyInstantiatingObjectOfType.Any<Type>())
				{
					string str = "DirectXmlToObject is currently instantiating an object of type ";
					Type type = DirectXmlToObject.currentlyInstantiatingObjectOfType.Peek();
					text = str + ((type != null) ? type.ToString() : null);
				}
				else if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					text = "curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent;
				}
				else
				{
					text = "";
				}
				Log.Warning("Tried to use an uninitialized DefOf of type " + defOf.Name + ". DefOfs are initialized right after all defs all loaded. Uninitialized DefOfs will return only nulls. (hint: don't use DefOfs as default field values in Defs, try to resolve them in ResolveReferences() instead)" + (text.NullOrEmpty() ? "" : (" Debug info: " + text)));
			}
			if (GenTypes.IsDefThreaded(defOf))
			{
				Log.Warning("Possible typo: " + defOf.Name + ". Using def type name not preceded by \"Of\"");
			}
		}

		// Token: 0x0400C741 RID: 51009
		private static bool bindingNow;

		// Token: 0x0400C742 RID: 51010
		private static bool earlyTry = true;
	}
}
