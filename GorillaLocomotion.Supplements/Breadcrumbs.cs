using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000007 RID: 7
public static class Breadcrumbs
{
	// Token: 0x06000009 RID: 9 RVA: 0x00002364 File Offset: 0x00000564
	public static void Update()
	{
		bool flag = !Settings.BreadcrumbsEnabled;
		if (flag)
		{
			bool flag2 = Breadcrumbs.active;
			if (flag2)
			{
				Breadcrumbs.Clear();
				Breadcrumbs.active = false;
			}
		}
		else
		{
			bool flag3 = GorillaTagger.Instance == null || VRRigCache.ActiveRigs == null;
			if (!flag3)
			{
				bool flag4 = Breadcrumbs.markerShader == null;
				if (flag4)
				{
					Shader val = Shader.Find("Unlit/Color");
					bool flag5 = val == null;
					if (flag5)
					{
						val = Shader.Find("GUI/Text Shader");
					}
					Breadcrumbs.markerShader = val;
				}
				Color color_ = Color.HSVToRGB(Settings.BreadcrumbColorHue, Settings.BreadcrumbColorSat, Settings.BreadcrumbColorVal);
				HashSet<int> hashSet = new HashSet<int>();
				foreach (VRRig activeRig in VRRigCache.ActiveRigs)
				{
					bool flag6 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig);
					if (flag6)
					{
						int instanceID = activeRig.GetInstanceID();
						hashSet.Add(instanceID);
						Vector3 position = activeRig.transform.position;
						bool flag7 = !Breadcrumbs.lastPositions.ContainsKey(instanceID);
						if (flag7)
						{
							Breadcrumbs.lastPositions[instanceID] = position;
						}
						else
						{
							bool flag8 = Vector3.Distance(position, Breadcrumbs.lastPositions[instanceID]) >= Settings.BreadcrumbSpacing;
							if (flag8)
							{
								Breadcrumbs.SpawnMarker(instanceID, position, color_);
								Breadcrumbs.lastPositions[instanceID] = position;
							}
						}
					}
				}
				List<int> list = new List<int>();
				foreach (int key in Breadcrumbs.trails.Keys)
				{
					bool flag9 = !hashSet.Contains(key);
					if (flag9)
					{
						list.Add(key);
					}
				}
				foreach (int item in list)
				{
					foreach (GameObject item2 in Breadcrumbs.trails[item])
					{
						bool flag10 = item2 != null;
						if (flag10)
						{
							Object.Destroy(item2);
						}
					}
					Breadcrumbs.trails.Remove(item);
					Breadcrumbs.lastPositions.Remove(item);
				}
				Breadcrumbs.active = true;
			}
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00002644 File Offset: 0x00000844
	private static void SpawnMarker(int rigId, Vector3 position, Color color)
	{
		bool flag = !Breadcrumbs.trails.ContainsKey(rigId);
		if (flag)
		{
			Breadcrumbs.trails[rigId] = new List<GameObject>();
		}
		List<GameObject> list = Breadcrumbs.trails[rigId];
		int count = list.Count;
		bool flag2 = count < 20;
		if (!flag2)
		{
			List<GameObject> list2 = list;
			bool flag3 = !(list2[0] != null);
			if (!flag3)
			{
				List<GameObject> list3 = list;
				Object.Destroy(list3[0]);
			}
			List<GameObject> list4 = list;
			list4.RemoveAt(0);
		}
		GameObject val = GameObject.CreatePrimitive(0);
		Object.Destroy(val.GetComponent<Collider>());
		bool flag4 = Breadcrumbs.markerShader != null;
		if (flag4)
		{
			list.Add(val);
		}
		else
		{
			list.Add(val);
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002718 File Offset: 0x00000918
	public static void Clear()
	{
		foreach (KeyValuePair<int, List<GameObject>> item in Breadcrumbs.trails)
		{
			foreach (GameObject item2 in item.Value)
			{
				bool flag = item2 != null;
				if (flag)
				{
					Object.Destroy(item2);
				}
			}
		}
		Breadcrumbs.trails.Clear();
		Breadcrumbs.lastPositions.Clear();
	}

	// Token: 0x0400001C RID: 28
	private static readonly Dictionary<int, List<GameObject>> trails = new Dictionary<int, List<GameObject>>();

	// Token: 0x0400001D RID: 29
	private static readonly Dictionary<int, Vector3> lastPositions = new Dictionary<int, Vector3>();

	// Token: 0x0400001E RID: 30
	private static bool active = false;

	// Token: 0x0400001F RID: 31
	private static Shader markerShader;

	// Token: 0x04000020 RID: 32
	private const int MaxMarkers = 20;
}
