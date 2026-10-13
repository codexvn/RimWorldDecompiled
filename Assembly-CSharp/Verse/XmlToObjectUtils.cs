using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using JetBrains.Annotations;

namespace Verse
{
	// Token: 0x0200083D RID: 2109
	public static class XmlToObjectUtils
	{
		// Token: 0x06003581 RID: 13697 RVA: 0x0011CD84 File Offset: 0x0011AF84
		[CanBeNull]
		public static FieldInfo DoFieldSearch(Type typeBeingDeserialized, XmlNode fieldNode, XmlNode xmlRootForDebug)
		{
			string name = fieldNode.Name;
			FieldInfo fieldInfo = XmlToObjectUtils.DirectGetFieldByName(typeBeingDeserialized, name, xmlRootForDebug);
			if (fieldInfo != null)
			{
				return fieldInfo;
			}
			DeepProfiler.Start("Search for field alias");
			try
			{
				XmlToObjectUtils.FieldAliasCache key = new XmlToObjectUtils.FieldAliasCache(typeBeingDeserialized, name);
				if (!XmlToObjectUtils.fieldAliases.TryGetValue(key, out fieldInfo))
				{
					foreach (FieldInfo fieldInfo2 in typeBeingDeserialized.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						using (IEnumerator<LoadAliasAttribute> enumerator = fieldInfo2.GetCustomAttributes(true).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.alias.EqualsIgnoreCase(name))
								{
									fieldInfo = fieldInfo2;
									break;
								}
							}
						}
						if (fieldInfo != null)
						{
							break;
						}
					}
					XmlToObjectUtils.fieldAliases.Add(key, fieldInfo);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			if (fieldInfo != null)
			{
				UnsavedAttribute unsavedAttribute = fieldInfo.TryGetAttribute<UnsavedAttribute>();
				if (unsavedAttribute != null && !unsavedAttribute.allowLoading)
				{
					Log.Error(string.Concat(new string[]
					{
						"XML error: ",
						fieldNode.OuterXml,
						" corresponds to a field in type ",
						typeBeingDeserialized.Name,
						" which has an Unsaved attribute. Context: ",
						xmlRootForDebug.OuterXml
					}));
					return null;
				}
			}
			if (fieldInfo != null)
			{
				return fieldInfo;
			}
			DeepProfiler.Start("Check for ignored missing field");
			FieldInfo result;
			try
			{
				bool flag = false;
				XmlAttributeCollection attributes = fieldNode.Attributes;
				XmlAttribute xmlAttribute = (attributes != null) ? attributes["IgnoreIfNoMatchingField"] : null;
				if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
				{
					flag = true;
				}
				else
				{
					object[] customAttributes = typeBeingDeserialized.GetCustomAttributes(typeof(IgnoreSavedElementAttribute), true);
					for (int i = 0; i < customAttributes.Length; i++)
					{
						if (string.Equals(((IgnoreSavedElementAttribute)customAttributes[i]).elementToIgnore, name, StringComparison.OrdinalIgnoreCase))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					Log.Error(string.Concat(new string[]
					{
						"XML error: ",
						fieldNode.OuterXml,
						" doesn't correspond to any field in type ",
						typeBeingDeserialized.Name,
						". Context: ",
						xmlRootForDebug.OuterXml
					}));
				}
				result = null;
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x0011CFC0 File Offset: 0x0011B1C0
		private static FieldInfo DirectGetFieldByName(Type type, string token, XmlNode debugXmlNode)
		{
			Dictionary<string, FieldInfo> dictionary;
			if (!XmlToObjectUtils.fieldInfoLookup.TryGetValue(type, out dictionary))
			{
				dictionary = new Dictionary<string, FieldInfo>();
				XmlToObjectUtils.fieldInfoLookup.Add(type, dictionary);
			}
			FieldInfo fieldInfo;
			if (dictionary.TryGetValue(token, out fieldInfo))
			{
				return fieldInfo;
			}
			fieldInfo = XmlToObjectUtils.SearchTypeHierarchy(type, token, false);
			if (fieldInfo != null)
			{
				dictionary.Add(token, fieldInfo);
				return fieldInfo;
			}
			fieldInfo = XmlToObjectUtils.SearchTypeHierarchy(type, token, true);
			if (fieldInfo == null)
			{
				return null;
			}
			dictionary.Add(token, fieldInfo);
			if (type.HasAttribute<CaseInsensitiveXMLParsing>())
			{
				return fieldInfo;
			}
			string text = string.Format("Attempt to use string {0} to refer to field {1} in type {2}; xml tags are now case-sensitive", token, fieldInfo.Name, type);
			if (debugXmlNode != null)
			{
				text = text + ". XML: " + debugXmlNode.OuterXml;
			}
			Log.Error(text);
			return fieldInfo;
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x0011D06C File Offset: 0x0011B26C
		private static FieldInfo SearchTypeHierarchy(Type type, string name, bool ignoreCase)
		{
			Dictionary<ValueTuple<Type, string>, FieldInfo> dictionary = ignoreCase ? XmlToObjectUtils.getFieldIgnoreCaseCache : XmlToObjectUtils.getFieldCache;
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			if (ignoreCase)
			{
				bindingFlags |= BindingFlags.IgnoreCase;
			}
			FieldInfo field;
			for (;;)
			{
				if (dictionary.TryGetValue(new ValueTuple<Type, string>(type, name), out field))
				{
					if (field != null)
					{
						break;
					}
				}
				else
				{
					field = type.GetField(name, bindingFlags);
					dictionary.Add(new ValueTuple<Type, string>(type, name), field);
					if (field != null)
					{
						return field;
					}
				}
				Type baseType = type.BaseType;
				if (baseType == null || !(baseType != typeof(object)))
				{
					goto IL_79;
				}
				type = baseType;
			}
			return field;
			IL_79:
			return null;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x0011D0F4 File Offset: 0x0011B2F4
		public static MethodInfo CustomDataLoadMethodOf(Type type)
		{
			MethodInfo result;
			if (XmlToObjectUtils.customDataLoadMethodCache.TryGetValue(type, out result))
			{
				return result;
			}
			MethodInfo method = type.GetMethod("LoadDataFromXmlCustom", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			XmlToObjectUtils.customDataLoadMethodCache.Add(type, method);
			return method;
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x0011D130 File Offset: 0x0011B330
		public static MethodInfo PostLoadMethodOf(Type type)
		{
			MethodInfo result;
			if (XmlToObjectUtils.postLoadMethodCache.TryGetValue(type, out result))
			{
				return result;
			}
			MethodInfo method = type.GetMethod("PostLoad");
			XmlToObjectUtils.postLoadMethodCache.Add(type, method);
			return method;
		}

		// Token: 0x04002852 RID: 10322
		private static readonly Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoLookup = new Dictionary<Type, Dictionary<string, FieldInfo>>();

		// Token: 0x04002853 RID: 10323
		private static readonly Dictionary<XmlToObjectUtils.FieldAliasCache, FieldInfo> fieldAliases = new Dictionary<XmlToObjectUtils.FieldAliasCache, FieldInfo>(EqualityComparer<XmlToObjectUtils.FieldAliasCache>.Default);

		// Token: 0x04002854 RID: 10324
		private static readonly Dictionary<ValueTuple<Type, string>, FieldInfo> getFieldCache = new Dictionary<ValueTuple<Type, string>, FieldInfo>();

		// Token: 0x04002855 RID: 10325
		private static readonly Dictionary<ValueTuple<Type, string>, FieldInfo> getFieldIgnoreCaseCache = new Dictionary<ValueTuple<Type, string>, FieldInfo>();

		// Token: 0x04002856 RID: 10326
		private static Dictionary<Type, MethodInfo> customDataLoadMethodCache = new Dictionary<Type, MethodInfo>();

		// Token: 0x04002857 RID: 10327
		public const string PostLoadMethodName = "PostLoad";

		// Token: 0x04002858 RID: 10328
		private static Dictionary<Type, MethodInfo> postLoadMethodCache = new Dictionary<Type, MethodInfo>();

		// Token: 0x0200083E RID: 2110
		private struct FieldAliasCache : IEquatable<XmlToObjectUtils.FieldAliasCache>
		{
			// Token: 0x06003587 RID: 13703 RVA: 0x0011D1B6 File Offset: 0x0011B3B6
			public FieldAliasCache(Type type, string fieldName)
			{
				this.type = type;
				this.fieldName = fieldName.ToLower();
			}

			// Token: 0x06003588 RID: 13704 RVA: 0x0011D1CB File Offset: 0x0011B3CB
			public bool Equals(XmlToObjectUtils.FieldAliasCache other)
			{
				return this.type == other.type && string.Equals(this.fieldName, other.fieldName);
			}

			// Token: 0x06003589 RID: 13705 RVA: 0x0011D1F3 File Offset: 0x0011B3F3
			public override int GetHashCode()
			{
				return HashCode.Combine<Type, string>(this.type, this.fieldName);
			}

			// Token: 0x04002859 RID: 10329
			public Type type;

			// Token: 0x0400285A RID: 10330
			public string fieldName;
		}
	}
}
