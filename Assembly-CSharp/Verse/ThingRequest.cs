using System;

namespace Verse
{
	// Token: 0x0200044E RID: 1102
	public struct ThingRequest
	{
		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001C90 RID: 7312 RVA: 0x0008F36F File Offset: 0x0008D56F
		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x0008F384 File Offset: 0x0008D584
		public bool CanBeFoundInRegion
		{
			get
			{
				return !this.IsUndefined && (this.singleDef != null || this.group == ThingRequestGroup.Nothing || this.group.StoreInRegion());
			}
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x0008F3B0 File Offset: 0x0008D5B0
		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x0008F3D8 File Offset: 0x0008D5D8
		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x0008F400 File Offset: 0x0008D600
		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x0008F426 File Offset: 0x0008D626
		public bool Accepts(Thing t)
		{
			if (this.singleDef != null)
			{
				return t.def == this.singleDef;
			}
			return this.group == ThingRequestGroup.Everything || this.group.Includes(t.def);
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0008F45C File Offset: 0x0008D65C
		public override string ToString()
		{
			string str;
			if (this.singleDef != null)
			{
				str = "singleDef " + this.singleDef.defName;
			}
			else
			{
				str = "group " + this.group.ToString();
			}
			return "ThingRequest(" + str + ")";
		}

		// Token: 0x04001870 RID: 6256
		public ThingDef singleDef;

		// Token: 0x04001871 RID: 6257
		public ThingRequestGroup group;
	}
}
