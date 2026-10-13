using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1F RID: 7455
	public struct SignalArgs
	{
		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x0600A6B9 RID: 42681 RVA: 0x002FBDB8 File Offset: 0x002F9FB8
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600A6BA RID: 42682 RVA: 0x002FBDC0 File Offset: 0x002F9FC0
		public IEnumerable<NamedArgument> Args
		{
			get
			{
				if (this.count == 0)
				{
					yield break;
				}
				if (this.args != null)
				{
					int num;
					for (int i = 0; i < this.args.Length; i = num + 1)
					{
						yield return this.args[i];
						num = i;
					}
				}
				else
				{
					yield return this.arg1;
					if (this.count >= 2)
					{
						yield return this.arg2;
					}
					if (this.count >= 3)
					{
						yield return this.arg3;
					}
					if (this.count >= 4)
					{
						yield return this.arg4;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600A6BB RID: 42683 RVA: 0x002FBDD8 File Offset: 0x002F9FD8
		public SignalArgs(SignalArgs args)
		{
			this.count = args.count;
			this.arg1 = args.arg1;
			this.arg2 = args.arg2;
			this.arg3 = args.arg3;
			this.arg4 = args.arg4;
			this.args = args.args;
		}

		// Token: 0x0600A6BC RID: 42684 RVA: 0x002FBE2D File Offset: 0x002FA02D
		public SignalArgs(NamedArgument arg1)
		{
			this.count = 1;
			this.arg1 = arg1;
			this.arg2 = default(NamedArgument);
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x0600A6BD RID: 42685 RVA: 0x002FBE68 File Offset: 0x002FA068
		public SignalArgs(NamedArgument arg1, NamedArgument arg2)
		{
			this.count = 2;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x0600A6BE RID: 42686 RVA: 0x002FBE9E File Offset: 0x002FA09E
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.count = 3;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x0600A6BF RID: 42687 RVA: 0x002FBECF File Offset: 0x002FA0CF
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.count = 4;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = arg4;
			this.args = null;
		}

		// Token: 0x0600A6C0 RID: 42688 RVA: 0x002FBEFC File Offset: 0x002FA0FC
		public SignalArgs(params NamedArgument[] args)
		{
			this.count = args.Length;
			if (args.Length > 4)
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
				this.args = new NamedArgument[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					this.args[i] = args[i];
				}
				return;
			}
			if (args.Length == 1)
			{
				this.arg1 = args[0];
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 2)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 3)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 4)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = args[3];
			}
			else
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.args = null;
		}

		// Token: 0x0600A6C1 RID: 42689 RVA: 0x002FC0A0 File Offset: 0x002FA2A0
		public bool TryGetArg(int index, out NamedArgument arg)
		{
			if (index < 0 || index >= this.count)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				arg = this.args[index];
			}
			else if (index == 0)
			{
				arg = this.arg1;
			}
			else if (index == 1)
			{
				arg = this.arg2;
			}
			else if (index == 2)
			{
				arg = this.arg3;
			}
			else
			{
				arg = this.arg4;
			}
			return true;
		}

		// Token: 0x0600A6C2 RID: 42690 RVA: 0x002FC124 File Offset: 0x002FA324
		public bool TryGetArg(string name, out NamedArgument arg)
		{
			if (this.count == 0)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				for (int i = 0; i < this.args.Length; i++)
				{
					if (this.args[i].label == name)
					{
						arg = this.args[i];
						return true;
					}
				}
			}
			else
			{
				if (this.count >= 1 && this.arg1.label == name)
				{
					arg = this.arg1;
					return true;
				}
				if (this.count >= 2 && this.arg2.label == name)
				{
					arg = this.arg2;
					return true;
				}
				if (this.count >= 3 && this.arg3.label == name)
				{
					arg = this.arg3;
					return true;
				}
				if (this.count >= 4 && this.arg4.label == name)
				{
					arg = this.arg4;
					return true;
				}
			}
			arg = default(NamedArgument);
			return false;
		}

		// Token: 0x0600A6C3 RID: 42691 RVA: 0x002FC240 File Offset: 0x002FA440
		public bool TryGetArg<T>(int index, out T arg)
		{
			NamedArgument namedArgument;
			if (this.TryGetArg(index, out namedArgument))
			{
				object arg2 = namedArgument.arg;
				if (arg2 is T)
				{
					T t = (T)((object)arg2);
					arg = t;
					return true;
				}
			}
			arg = default(T);
			return false;
		}

		// Token: 0x0600A6C4 RID: 42692 RVA: 0x002FC284 File Offset: 0x002FA484
		public bool TryGetArg<T>(string name, out T arg)
		{
			NamedArgument namedArgument;
			if (!this.TryGetArg(name, out namedArgument) || !(namedArgument.arg is T))
			{
				arg = default(T);
				return false;
			}
			arg = (T)((object)namedArgument.arg);
			return true;
		}

		// Token: 0x0600A6C5 RID: 42693 RVA: 0x002FC2C4 File Offset: 0x002FA4C4
		public NamedArgument GetArg(int index)
		{
			NamedArgument result;
			if (this.TryGetArg(index, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x0600A6C6 RID: 42694 RVA: 0x002FC2E8 File Offset: 0x002FA4E8
		public NamedArgument GetArg(string name)
		{
			NamedArgument result;
			if (this.TryGetArg(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name);
		}

		// Token: 0x0600A6C7 RID: 42695 RVA: 0x002FC314 File Offset: 0x002FA514
		public T GetArg<T>(string name)
		{
			T result;
			if (this.TryGetArg<T>(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name + " of type " + typeof(T).Name);
		}

		// Token: 0x0600A6C8 RID: 42696 RVA: 0x002FC354 File Offset: 0x002FA554
		public TaggedString GetFormattedText(TaggedString text)
		{
			if (this.count == 0)
			{
				return text.Formatted(Array.Empty<NamedArgument>());
			}
			if (this.args != null)
			{
				return text.Formatted(this.args);
			}
			if (this.count == 1)
			{
				return text.Formatted(this.arg1);
			}
			if (this.count == 2)
			{
				return text.Formatted(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return text.Formatted(this.arg1, this.arg2, this.arg3);
			}
			return text.Formatted(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x0600A6C9 RID: 42697 RVA: 0x002FC3FC File Offset: 0x002FA5FC
		public TaggedString GetTranslatedText(string textKey)
		{
			if (this.count == 0)
			{
				return textKey.Translate();
			}
			if (this.args != null)
			{
				return textKey.Translate(this.args);
			}
			if (this.count == 1)
			{
				return textKey.Translate(this.arg1);
			}
			if (this.count == 2)
			{
				return textKey.Translate(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return textKey.Translate(this.arg1, this.arg2, this.arg3);
			}
			return textKey.Translate(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x0600A6CA RID: 42698 RVA: 0x002FC4A0 File Offset: 0x002FA6A0
		public void Add(NamedArgument arg)
		{
			if (this.args != null)
			{
				NamedArgument[] array = new NamedArgument[this.args.Length + 1];
				for (int i = 0; i < this.args.Length; i++)
				{
					array[i] = this.args[i];
				}
				array[array.Length - 1] = arg;
				this.args = array;
				this.count = this.args.Length;
				return;
			}
			if (this.count == 0)
			{
				this.arg1 = arg;
			}
			else if (this.count == 1)
			{
				this.arg2 = arg;
			}
			else if (this.count == 2)
			{
				this.arg3 = arg;
			}
			else if (this.count == 3)
			{
				this.arg4 = arg;
			}
			else
			{
				this.args = new NamedArgument[5];
				this.args[0] = this.arg1;
				this.args[1] = this.arg2;
				this.args[2] = this.arg3;
				this.args[3] = this.arg4;
				this.args[4] = arg;
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.count++;
		}

		// Token: 0x0600A6CB RID: 42699 RVA: 0x002FC5FF File Offset: 0x002FA7FF
		public void Add(NamedArgument arg1, NamedArgument arg2)
		{
			this.Add(arg1);
			this.Add(arg2);
		}

		// Token: 0x0600A6CC RID: 42700 RVA: 0x002FC60F File Offset: 0x002FA80F
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
		}

		// Token: 0x0600A6CD RID: 42701 RVA: 0x002FC626 File Offset: 0x002FA826
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
			this.Add(arg4);
		}

		// Token: 0x0600A6CE RID: 42702 RVA: 0x002FC648 File Offset: 0x002FA848
		public void Add(params NamedArgument[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				this.Add(args[i]);
			}
		}

		// Token: 0x0600A6CF RID: 42703 RVA: 0x002FC670 File Offset: 0x002FA870
		public void Add(SignalArgs args)
		{
			if (args.count == 0)
			{
				return;
			}
			if (args.args != null)
			{
				for (int i = 0; i < args.args.Length; i++)
				{
					this.Add(args.args[i]);
				}
				return;
			}
			if (args.count >= 1)
			{
				this.Add(args.arg1);
			}
			if (args.count >= 2)
			{
				this.Add(args.arg2);
			}
			if (args.count >= 3)
			{
				this.Add(args.arg3);
			}
			if (args.count >= 4)
			{
				this.Add(args.arg4);
			}
		}

		// Token: 0x04007365 RID: 29541
		private int count;

		// Token: 0x04007366 RID: 29542
		private NamedArgument arg1;

		// Token: 0x04007367 RID: 29543
		private NamedArgument arg2;

		// Token: 0x04007368 RID: 29544
		private NamedArgument arg3;

		// Token: 0x04007369 RID: 29545
		private NamedArgument arg4;

		// Token: 0x0400736A RID: 29546
		private NamedArgument[] args;
	}
}
