using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200002E RID: 46
public static class RemoveLeaves
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000152 RID: 338 RVA: 0x00018D14 File Offset: 0x00016F14
	public static string LeafPrefix
	{
		get
		{
			bool flag = RemoveLeaves.leafPrefixCache == null;
			if (flag)
			{
				GameObject forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
				bool flag2 = forest == null;
				if (flag2)
				{
					return "UnityTempFile";
				}
				GameObject val = forest;
				IGrouping<string, Transform> grouping = (from igrouping_0 in (from transform_0 in val.GetComponentsInChildren<Transform>(true).Where<Transform>(delegate(Transform transform_0)
						{
							bool flag5 = !transform_0.name.StartsWith("UnityTempFile");
							bool flag6;
							if (flag5)
							{
								flag6 = false;
							}
							else
							{
								bool flag7 = !(transform_0.parent != null);
								flag6 = !flag7 && transform_0.parent == forest.transform;
							}
							return flag6;
						})
						group transform_0 by transform_0.name).Where<IGrouping<string, Transform>>(delegate(IGrouping<string, Transform> igrouping_0)
					{
						int num = igrouping_0.Count<Transform>();
						return num == 3;
					})
					orderby igrouping_0.First<Transform>().GetSiblingIndex() descending
					select igrouping_0).FirstOrDefault<IGrouping<string, Transform>>();
				bool flag3 = grouping == null;
				object obj;
				if (!flag3)
				{
					obj = grouping.Key;
					bool flag4 = obj != null;
					if (flag4)
					{
						goto IL_010A;
					}
				}
				obj = "UnityTempFile";
				IL_010A:
				RemoveLeaves.leafPrefixCache = (string)obj;
			}
			return RemoveLeaves.leafPrefixCache;
		}
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00018E48 File Offset: 0x00017048
	public static void Update()
	{
		bool removeLeavesEnabled = Settings.RemoveLeavesEnabled;
		if (removeLeavesEnabled)
		{
			bool flag = !RemoveLeaves.hidden;
			if (flag)
			{
				RemoveLeaves.HideLeaves();
				RemoveLeaves.hidden = true;
			}
		}
		else
		{
			bool flag2 = !RemoveLeaves.hidden;
			if (!flag2)
			{
				RemoveLeaves.RestoreLeaves();
				RemoveLeaves.hidden = false;
			}
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00018EA0 File Offset: 0x000170A0
	public static void HideLeaves()
	{
		GameObject val3 = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
		bool flag = !(val3 != null);
		if (!flag)
		{
			int num3 = 0;
			for (;;)
			{
				bool flag2 = num3 < val3.transform.childCount;
				if (!flag2)
				{
					break;
				}
				GameObject val4 = val3.transform.GetChild(num3).gameObject;
				bool flag3 = !val4.name.Contains(RemoveLeaves.LeafPrefix);
				if (!flag3)
				{
					GameObject val5 = val4;
					val5.SetActive(false);
					RemoveLeaves.leaves.Add(val4);
				}
				num3++;
			}
		}
		GameObject val6 = GameObject.Find("RankedMain/Ranked_Layout/Ranked_Forest_prefab");
		bool flag4 = !(val6 != null);
		if (!flag4)
		{
			int num4 = 0;
			for (;;)
			{
				bool flag5 = num4 >= val6.transform.childCount;
				if (flag5)
				{
					break;
				}
				GameObject val7 = val6.transform.GetChild(num4).gameObject;
				bool flag6 = val7.name.Contains(RemoveLeaves.LeafPrefix);
				if (flag6)
				{
					GameObject val8 = val7;
					val8.SetActive(false);
					RemoveLeaves.leaves.Add(val7);
					num4++;
				}
				else
				{
					num4++;
				}
			}
		}
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00018FEC File Offset: 0x000171EC
	public static void RestoreLeaves()
	{
		foreach (GameObject leaf in RemoveLeaves.leaves)
		{
			bool flag = leaf != null;
			if (flag)
			{
				leaf.SetActive(true);
			}
		}
		RemoveLeaves.leaves.Clear();
	}

	// Token: 0x04000288 RID: 648
	private static string leafPrefixCache;

	// Token: 0x04000289 RID: 649
	public static readonly List<GameObject> leaves = new List<GameObject>();

	// Token: 0x0400028A RID: 650
	private static bool hidden;

	// Token: 0x02000075 RID: 117
	[CompilerGenerated]
	[Serializable]
	private sealed class Class10
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00023810 File Offset: 0x00021A10
		internal string method_0(Transform transform_0)
		{
			return transform_0.name;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00023828 File Offset: 0x00021A28
		internal bool method_1(IGrouping<string, Transform> igrouping_0)
		{
			int num = igrouping_0.Count<Transform>();
			return num == 3;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00023848 File Offset: 0x00021A48
		internal int method_2(IGrouping<string, Transform> igrouping_0)
		{
			return igrouping_0.First<Transform>().GetSiblingIndex();
		}

		// Token: 0x0400052E RID: 1326
		public static readonly RemoveLeaves.Class10 _003C_003E9 = new RemoveLeaves.Class10();

		// Token: 0x0400052F RID: 1327
		public static Func<Transform, string> _003C_003E9__2_1;

		// Token: 0x04000530 RID: 1328
		public static Func<IGrouping<string, Transform>, bool> _003C_003E9__2_2;

		// Token: 0x04000531 RID: 1329
		public static Func<IGrouping<string, Transform>, int> _003C_003E9__2_3;
	}

	// Token: 0x02000076 RID: 118
	[CompilerGenerated]
	private sealed class Class11
	{
		// Token: 0x06000277 RID: 631 RVA: 0x0002387C File Offset: 0x00021A7C
		internal bool method_0(Transform transform_0)
		{
			bool flag = !transform_0.name.StartsWith("UnityTempFile");
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = !(transform_0.parent != null);
				flag2 = !flag3 && transform_0.parent == this.forest.transform;
			}
			return flag2;
		}

		// Token: 0x04000532 RID: 1330
		public GameObject forest;
	}
}
