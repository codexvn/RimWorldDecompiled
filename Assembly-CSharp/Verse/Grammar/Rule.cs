using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000DEA RID: 3562
	public abstract class Rule
	{
		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06005AB4 RID: 23220
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06005AB5 RID: 23221 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float Priority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x001D19E8 File Offset: 0x001CFBE8
		public virtual Rule DeepCopy()
		{
			Rule rule = (Rule)Activator.CreateInstance(base.GetType());
			rule.keyword = this.keyword;
			rule.tag = this.tag;
			rule.requiredTag = this.requiredTag;
			if (this.constantConstraints != null)
			{
				rule.constantConstraints = this.constantConstraints.ToList<Rule.ConstantConstraint>();
			}
			return rule;
		}

		// Token: 0x06005AB7 RID: 23223
		public abstract string Generate();

		// Token: 0x06005AB8 RID: 23224 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Init()
		{
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x001D1A44 File Offset: 0x001CFC44
		public void AddConstantConstraint(string key, string value, Rule.ConstantConstraint.Type type)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<Rule.ConstantConstraint>();
			}
			this.constantConstraints.Add(new Rule.ConstantConstraint
			{
				key = key,
				value = value,
				type = type
			});
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x001D1A90 File Offset: 0x001CFC90
		public void AddConstantConstraint(string key, string value, string op)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(op);
			Rule.ConstantConstraint.Type type;
			if (num <= 1678027958U)
			{
				if (num <= 957132539U)
				{
					if (num != 284975636U)
					{
						if (num == 957132539U)
						{
							if (op == "<")
							{
								type = Rule.ConstantConstraint.Type.Less;
								goto IL_123;
							}
						}
					}
					else if (op == ">=")
					{
						type = Rule.ConstantConstraint.Type.GreaterOrEqual;
						goto IL_123;
					}
				}
				else if (num != 990687777U)
				{
					if (num == 1678027958U)
					{
						if (op == "[less_than]")
						{
							type = Rule.ConstantConstraint.Type.Less;
							goto IL_123;
						}
					}
				}
				else if (op == ">")
				{
					type = Rule.ConstantConstraint.Type.Greater;
					goto IL_123;
				}
			}
			else if (num <= 2431966415U)
			{
				if (num != 2428715011U)
				{
					if (num == 2431966415U)
					{
						if (op == "==")
						{
							type = Rule.ConstantConstraint.Type.Equal;
							goto IL_123;
						}
					}
				}
				else if (op == "!=")
				{
					type = Rule.ConstantConstraint.Type.NotEqual;
					goto IL_123;
				}
			}
			else if (num != 2499223986U)
			{
				if (num == 4183126053U)
				{
					if (op == "[greater_than]")
					{
						type = Rule.ConstantConstraint.Type.Greater;
						goto IL_123;
					}
				}
			}
			else if (op == "<=")
			{
				type = Rule.ConstantConstraint.Type.LessOrEqual;
				goto IL_123;
			}
			type = Rule.ConstantConstraint.Type.Equal;
			Log.Error("Unknown ConstantConstraint type: " + op);
			IL_123:
			this.AddConstantConstraint(key, value, type);
		}

		// Token: 0x06005ABB RID: 23227 RVA: 0x001D1BCC File Offset: 0x001CFDCC
		public bool ValidateConstraints(Dictionary<string, string> constraints)
		{
			bool result = true;
			if (this.constantConstraints != null)
			{
				for (int i = 0; i < this.constantConstraints.Count; i++)
				{
					Rule.ConstantConstraint constantConstraint = this.constantConstraints[i];
					string text = (constraints != null) ? constraints.TryGetValue(constantConstraint.key, "") : "";
					float num = 0f;
					float num2 = 0f;
					bool flag = !text.NullOrEmpty() && !constantConstraint.value.NullOrEmpty() && float.TryParse(text, out num) && float.TryParse(constantConstraint.value, out num2);
					bool flag2;
					switch (constantConstraint.type)
					{
					case Rule.ConstantConstraint.Type.Equal:
						flag2 = text.EqualsIgnoreCase(constantConstraint.value);
						break;
					case Rule.ConstantConstraint.Type.NotEqual:
						flag2 = !text.EqualsIgnoreCase(constantConstraint.value);
						break;
					case Rule.ConstantConstraint.Type.Less:
						flag2 = (flag && num < num2);
						break;
					case Rule.ConstantConstraint.Type.Greater:
						flag2 = (flag && num > num2);
						break;
					case Rule.ConstantConstraint.Type.LessOrEqual:
						flag2 = (flag && num <= num2);
						break;
					case Rule.ConstantConstraint.Type.GreaterOrEqual:
						flag2 = (flag && num >= num2);
						break;
					default:
						Log.Error("Unknown ConstantConstraint type: " + constantConstraint.type.ToString());
						flag2 = false;
						break;
					}
					if (!flag2)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x040041A4 RID: 16804
		[MayTranslate]
		public string keyword;

		// Token: 0x040041A5 RID: 16805
		[NoTranslate]
		public string tag;

		// Token: 0x040041A6 RID: 16806
		[NoTranslate]
		public string requiredTag;

		// Token: 0x040041A7 RID: 16807
		public int? usesLimit;

		// Token: 0x040041A8 RID: 16808
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x02000DEB RID: 3563
		public struct ConstantConstraint
		{
			// Token: 0x040041A9 RID: 16809
			[MayTranslate]
			public string key;

			// Token: 0x040041AA RID: 16810
			[MayTranslate]
			public string value;

			// Token: 0x040041AB RID: 16811
			public Rule.ConstantConstraint.Type type;

			// Token: 0x02000DEC RID: 3564
			public enum Type
			{
				// Token: 0x040041AD RID: 16813
				Equal,
				// Token: 0x040041AE RID: 16814
				NotEqual,
				// Token: 0x040041AF RID: 16815
				Less,
				// Token: 0x040041B0 RID: 16816
				Greater,
				// Token: 0x040041B1 RID: 16817
				LessOrEqual,
				// Token: 0x040041B2 RID: 16818
				GreaterOrEqual
			}
		}
	}
}
