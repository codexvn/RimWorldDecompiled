using System;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000365 RID: 869
	public struct NamedArgument
	{
		// Token: 0x06001654 RID: 5716 RVA: 0x0006D5DD File Offset: 0x0006B7DD
		public NamedArgument(object arg, string label)
		{
			this.arg = arg;
			this.label = label;
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0006D5ED File Offset: 0x0006B7ED
		public static implicit operator NamedArgument(int value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0006D5FB File Offset: 0x0006B7FB
		public static implicit operator NamedArgument(char value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0006D609 File Offset: 0x0006B809
		public static implicit operator NamedArgument(float value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0006D617 File Offset: 0x0006B817
		public static implicit operator NamedArgument(double value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0006D625 File Offset: 0x0006B825
		public static implicit operator NamedArgument(long value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(string value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0006D63C File Offset: 0x0006B83C
		public static implicit operator NamedArgument(uint value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x0006D64A File Offset: 0x0006B84A
		public static implicit operator NamedArgument(byte value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x0006D658 File Offset: 0x0006B858
		public static implicit operator NamedArgument(ulong value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600165E RID: 5726 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(StringBuilder value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600165F RID: 5727 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(Thing value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001660 RID: 5728 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(Def value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(WorldObject value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(Faction value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0006D666 File Offset: 0x0006B866
		public static implicit operator NamedArgument(IntVec3 value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x0006D674 File Offset: 0x0006B874
		public static implicit operator NamedArgument(LocalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x0006D682 File Offset: 0x0006B882
		public static implicit operator NamedArgument(TargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x0006D690 File Offset: 0x0006B890
		public static implicit operator NamedArgument(GlobalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(Map value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x0006D69E File Offset: 0x0006B89E
		public static implicit operator NamedArgument(TaggedString value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x0006D633 File Offset: 0x0006B833
		public static implicit operator NamedArgument(Ideo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x0006D6AC File Offset: 0x0006B8AC
		public override string ToString()
		{
			return (this.label.NullOrEmpty() ? "unnamed" : this.label) + "->" + this.arg.ToStringSafe<object>();
		}

		// Token: 0x04001478 RID: 5240
		public object arg;

		// Token: 0x04001479 RID: 5241
		public string label;
	}
}
