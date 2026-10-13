using System;

namespace Verse
{
	// Token: 0x02000134 RID: 308
	public struct AnyEnum
	{
		// Token: 0x06000984 RID: 2436 RVA: 0x0002D4AC File Offset: 0x0002B6AC
		public T? As<T>() where T : struct
		{
			if (typeof(T) == this.enumType)
			{
				return new T?(GenericConverter.Convert<int, T>(this.enumCode));
			}
			return null;
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x0002D4EC File Offset: 0x0002B6EC
		public static AnyEnum None
		{
			get
			{
				return default(AnyEnum);
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002D504 File Offset: 0x0002B704
		public static AnyEnum FromEnum<T>(T reasonCode) where T : struct, Enum
		{
			return new AnyEnum
			{
				enumType = typeof(T),
				enumCode = GenericConverter.Convert<T, int>(reasonCode)
			};
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002D538 File Offset: 0x0002B738
		public override string ToString()
		{
			return Enum.ToObject(this.enumType, this.enumCode).ToStringSafe<object>();
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002D550 File Offset: 0x0002B750
		public static bool operator ==(AnyEnum lhs, AnyEnum rhs)
		{
			return lhs.enumCode == rhs.enumCode && lhs.enumType == rhs.enumType;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002D573 File Offset: 0x0002B773
		public static bool operator !=(AnyEnum lhs, AnyEnum rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002D580 File Offset: 0x0002B780
		public override bool Equals(object obj)
		{
			if (obj is AnyEnum)
			{
				AnyEnum rhs = (AnyEnum)obj;
				return this == rhs;
			}
			return false;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002D5AA File Offset: 0x0002B7AA
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombine<Type>(0, this.enumType), this.enumCode);
		}

		// Token: 0x040004BE RID: 1214
		public Type enumType;

		// Token: 0x040004BF RID: 1215
		public int enumCode;
	}
}
