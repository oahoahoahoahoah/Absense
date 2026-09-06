using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public static class RemoveWindBarrier
{
	// Token: 0x06000157 RID: 343 RVA: 0x00019068 File Offset: 0x00017268
	public static void Update()
	{
		bool flag = Settings.RemoveWindBarrierEnabled == RemoveWindBarrier.applied;
		if (!flag)
		{
			RemoveWindBarrier.applied = Settings.RemoveWindBarrierEnabled;
			bool flag2 = RemoveWindBarrier.applied;
			if (flag2)
			{
				RemoveWindBarrier.HideBarrier();
			}
			else
			{
				RemoveWindBarrier.ShowBarrier();
			}
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x000190B0 File Offset: 0x000172B0
	private static void SetBarrierActive(bool active)
	{
		try
		{
			string[] paths = new string[] { "Environment Objects/LocalObjects_Prefab/ForestToHoverboard/TurnOnInForestAndHoverboard/ForestDome_CollisionOnly/TutorialMapDomeCap_FBX/Tutorial_DomeBlock_Collision", "Environment Objects/LocalObjects_Prefab/ForestToHoverboard/TurnOnInForestAndHoverboard/ForestDome_CollisionOnly/TutorialMapDomeCap_FBX/Tutorial_DomeBlock", "Environment Objects/LocalObjects_Prefab/Forest/Environment/Forest_ForceVolumes/LevelBoundaryForceVolume/Capsule" };
			foreach (string path in paths)
			{
				GameObject obj = GameObject.Find(path);
				bool flag = obj != null;
				if (flag)
				{
					obj.SetActive(active);
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00019130 File Offset: 0x00017330
	private static void HideBarrier()
	{
		RemoveWindBarrier.SetBarrierActive(false);
	}

	// Token: 0x0600015A RID: 346 RVA: 0x00019139 File Offset: 0x00017339
	private static void ShowBarrier()
	{
		RemoveWindBarrier.SetBarrierActive(true);
	}

	// Token: 0x0400028B RID: 651
	private static bool applied;
}
