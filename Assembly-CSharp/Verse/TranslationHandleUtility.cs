using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Verse
{
	// Token: 0x020003A1 RID: 929
	public static class TranslationHandleUtility
	{
		// Token: 0x060017C8 RID: 6088 RVA: 0x00078C60 File Offset: 0x00076E60
		public static int GetElementIndexByHandle(object list, string handle, int handleIndex)
		{
			if (list == null)
			{
				throw new InvalidOperationException("Tried to get element by handle on null object.");
			}
			if (handleIndex < 0)
			{
				handleIndex = 0;
			}
			PropertyInfo property = list.GetType().GetProperty("Count");
			if (property == null)
			{
				throw new InvalidOperationException("Tried to get element by handle on non-list (missing 'Count' property).");
			}
			PropertyInfo property2 = list.GetType().GetProperty("Item");
			if (property2 == null)
			{
				throw new InvalidOperationException("Tried to get element by handle on non-list (missing 'Item' property).");
			}
			int num = (int)property.GetValue(list, null);
			FieldInfo fieldInfo = null;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				object value = property2.GetValue(list, new object[]
				{
					i
				});
				if (value != null)
				{
					foreach (FieldInfo fieldInfo2 in value.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						TranslationHandleAttribute translationHandleAttribute = fieldInfo2.TryGetAttribute<TranslationHandleAttribute>();
						if (translationHandleAttribute != null)
						{
							object value2 = fieldInfo2.GetValue(value);
							if (value2 != null && TranslationHandleUtility.HandlesMatch(value2, handle))
							{
								int priority = translationHandleAttribute.Priority;
								if (fieldInfo == null || priority > num2)
								{
									fieldInfo = fieldInfo2;
									num2 = priority;
								}
							}
						}
					}
				}
			}
			if (fieldInfo == null)
			{
				throw new InvalidOperationException("None of the list elements have a handle named " + handle + ".");
			}
			int num3 = 0;
			for (int k = 0; k < num; k++)
			{
				object value3 = property2.GetValue(list, new object[]
				{
					k
				});
				if (value3 != null)
				{
					foreach (FieldInfo fieldInfo3 in value3.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (TranslationHandleUtility.FieldInfosEqual(fieldInfo3, fieldInfo))
						{
							object value4 = fieldInfo3.GetValue(value3);
							if (value4 != null && TranslationHandleUtility.HandlesMatch(value4, handle))
							{
								if (num3 == handleIndex)
								{
									return k;
								}
								num3++;
							}
						}
					}
				}
			}
			throw new InvalidOperationException(string.Concat(new string[]
			{
				"Tried to access handle ",
				handle,
				"[",
				handleIndex.ToString(),
				"], but there are only ",
				num3.ToString(),
				" handles matching this name."
			}));
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x00078E74 File Offset: 0x00077074
		public static string GetBestHandleWithIndexForListElement(object list, object element)
		{
			if (list == null || element == null)
			{
				return null;
			}
			PropertyInfo property = list.GetType().GetProperty("Count");
			if (property == null)
			{
				return null;
			}
			PropertyInfo property2 = list.GetType().GetProperty("Item");
			if (property2 == null)
			{
				return null;
			}
			FieldInfo fieldInfo = null;
			string handle = null;
			int num = 0;
			foreach (FieldInfo fieldInfo2 in element.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				TranslationHandleAttribute translationHandleAttribute = fieldInfo2.TryGetAttribute<TranslationHandleAttribute>();
				if (translationHandleAttribute != null)
				{
					object value = fieldInfo2.GetValue(element);
					if (value != null)
					{
						Type type = value as Type;
						string text;
						if (type != null)
						{
							text = type.Name;
						}
						else
						{
							try
							{
								text = value.ToString();
							}
							catch
							{
								return null;
							}
						}
						if (!text.NullOrEmpty())
						{
							int priority = translationHandleAttribute.Priority;
							if (fieldInfo == null || priority > num)
							{
								fieldInfo = fieldInfo2;
								handle = text;
								num = priority;
							}
						}
					}
				}
			}
			if (fieldInfo == null)
			{
				return null;
			}
			int num2 = 0;
			int num3 = -1;
			int num4 = (int)property.GetValue(list, null);
			for (int j = 0; j < num4; j++)
			{
				object value2 = property2.GetValue(list, new object[]
				{
					j
				});
				if (value2 != null)
				{
					if (value2 == element)
					{
						num3 = num2;
						num2++;
					}
					else
					{
						foreach (FieldInfo fieldInfo3 in value2.GetType().GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
						{
							if (TranslationHandleUtility.FieldInfosEqual(fieldInfo3, fieldInfo))
							{
								object value3 = fieldInfo3.GetValue(value2);
								if (value3 != null && TranslationHandleUtility.HandlesMatch(value3, handle))
								{
									num2++;
									break;
								}
							}
						}
					}
				}
			}
			if (num3 < 0)
			{
				return null;
			}
			string text2 = TranslationHandleUtility.NormalizedHandle(handle);
			if (num2 <= 1)
			{
				return text2;
			}
			return text2 + "-" + num3.ToString();
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x00079068 File Offset: 0x00077268
		public static bool HandlesMatch(object item, string handle)
		{
			if (item == null)
			{
				return false;
			}
			if (handle.NullOrEmpty())
			{
				return false;
			}
			handle = TranslationHandleUtility.NormalizedHandle(handle);
			if (handle.NullOrEmpty())
			{
				return false;
			}
			Type type = item as Type;
			if (type != null)
			{
				return TranslationHandleUtility.NormalizedHandle(type.Name) == handle || TranslationHandleUtility.NormalizedHandle(type.FullName) == handle || TranslationHandleUtility.NormalizedHandle(type.ToString()) == handle;
			}
			string text;
			try
			{
				text = item.ToString();
			}
			catch (Exception ex)
			{
				string str = "Could not get element by handle because one of the elements threw an exception in its ToString(): ";
				Exception ex2 = ex;
				throw new InvalidOperationException(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			return !text.NullOrEmpty() && TranslationHandleUtility.NormalizedHandle(text) == handle;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x00079130 File Offset: 0x00077330
		private static string NormalizedHandle(string handle)
		{
			if (handle.NullOrEmpty())
			{
				return handle;
			}
			handle = handle.Trim();
			handle = handle.Replace(' ', '_');
			handle = handle.Replace('\n', '_');
			handle = handle.Replace("\r", "");
			handle = handle.Replace('\t', '_');
			handle = handle.Replace(".", "");
			if (handle.IndexOf('-') >= 0)
			{
				handle = handle.Replace('-'.ToString(), "");
			}
			if (handle.IndexOf("{") >= 0)
			{
				handle = TranslationHandleUtility.StringFormatSymbolsRegex.Replace(handle, "");
			}
			TranslationHandleUtility.tmpStringBuilder.Length = 0;
			for (int i = 0; i < handle.Length; i++)
			{
				if ("qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-_".IndexOf(handle[i]) >= 0)
				{
					TranslationHandleUtility.tmpStringBuilder.Append(handle[i]);
				}
			}
			handle = TranslationHandleUtility.tmpStringBuilder.ToString();
			TranslationHandleUtility.tmpStringBuilder.Length = 0;
			for (int j = 0; j < handle.Length; j++)
			{
				if (j == 0 || handle[j] != '_' || handle[j - 1] != '_')
				{
					TranslationHandleUtility.tmpStringBuilder.Append(handle[j]);
				}
			}
			handle = TranslationHandleUtility.tmpStringBuilder.ToString();
			handle = handle.Trim('_');
			if (!handle.NullOrEmpty() && handle.All(new Func<char, bool>(char.IsDigit)))
			{
				handle = "_" + handle;
			}
			return handle;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x000792B0 File Offset: 0x000774B0
		private static bool FieldInfosEqual(FieldInfo lhs, FieldInfo rhs)
		{
			return lhs.DeclaringType == rhs.DeclaringType && lhs.Name == rhs.Name;
		}

		// Token: 0x0400153F RID: 5439
		public const char HandleIndexCharacter = '-';

		// Token: 0x04001540 RID: 5440
		private static Regex StringFormatSymbolsRegex = new Regex("{.*?}");

		// Token: 0x04001541 RID: 5441
		private static StringBuilder tmpStringBuilder = new StringBuilder();
	}
}
