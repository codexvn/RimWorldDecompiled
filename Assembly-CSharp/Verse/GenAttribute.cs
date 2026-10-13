using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x020000F1 RID: 241
	public static class GenAttribute
	{
		// Token: 0x060006F1 RID: 1777 RVA: 0x000219D6 File Offset: 0x0001FBD6
		public static bool HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			return Attribute.IsDefined(memberInfo, typeof(T), true);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000219EC File Offset: 0x0001FBEC
		public static bool TryGetAttribute<T>(this MemberInfo memberInfo, out T customAttribute) where T : Attribute
		{
			if (!memberInfo.HasAttribute<T>())
			{
				customAttribute = default(T);
				return false;
			}
			Attribute[] customAttributes = Attribute.GetCustomAttributes(memberInfo, typeof(T), true);
			if (customAttributes.Length == 1)
			{
				return (customAttribute = (customAttributes[0] as T)) != null;
			}
			if (customAttributes.Length == 0)
			{
				customAttribute = default(T);
				return false;
			}
			for (int i = 0; i < customAttributes.Length; i++)
			{
				T t = customAttributes[i] as T;
				if (t != null)
				{
					customAttribute = t;
					return true;
				}
			}
			customAttribute = default(T);
			return false;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x00021A84 File Offset: 0x0001FC84
		public static T TryGetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
		{
			T result;
			memberInfo.TryGetAttribute(out result);
			return result;
		}
	}
}
