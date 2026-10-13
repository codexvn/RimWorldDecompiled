using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002351 RID: 9041
	public class NameBank
	{
		// Token: 0x17001DDF RID: 7647
		// (get) Token: 0x0600C434 RID: 50228 RVA: 0x00390862 File Offset: 0x0038EA62
		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				int num;
				for (int i = 0; i < NameBank.numGenders; i = num + 1)
				{
					for (int j = 0; j < NameBank.numSlots; j = num + 1)
					{
						yield return this.names[i, j];
						num = j;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600C435 RID: 50229 RVA: 0x00390874 File Offset: 0x0038EA74
		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		// Token: 0x0600C436 RID: 50230 RVA: 0x003908D8 File Offset: 0x0038EAD8
		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				foreach (string str in (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>())
				{
					Log.Error("Duplicated name: " + str);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]");
					}
				}
			}
		}

		// Token: 0x0600C437 RID: 50231 RVA: 0x00390A5C File Offset: 0x0038EC5C
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		// Token: 0x0600C438 RID: 50232 RVA: 0x00390A6C File Offset: 0x0038EC6C
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		// Token: 0x0600C439 RID: 50233 RVA: 0x00390ABC File Offset: 0x0038ECBC
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		// Token: 0x0600C43A RID: 50234 RVA: 0x00390AD8 File Offset: 0x0038ECD8
		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new string[]
				{
					"Name list for gender=",
					gender.ToString(),
					" slot=",
					slot.ToString(),
					" is empty."
				}));
				return "Errorname";
			}
			string text;
			for (;;)
			{
				text = list.RandomElement<string>();
				if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
				{
					break;
				}
				num++;
				if (num > 50)
				{
					return text;
				}
			}
			return text;
		}

		// Token: 0x040085C8 RID: 34248
		public PawnNameCategory nameType;

		// Token: 0x040085C9 RID: 34249
		private List<string>[,] names;

		// Token: 0x040085CA RID: 34250
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		// Token: 0x040085CB RID: 34251
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;
	}
}
