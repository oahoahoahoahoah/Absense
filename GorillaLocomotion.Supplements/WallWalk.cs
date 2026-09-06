using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000041 RID: 65
public static class WallWalk
{
	// Token: 0x060001E7 RID: 487 RVA: 0x000206E4 File Offset: 0x0001E8E4
	public static void Apply()
	{
		bool flag = !Settings.WallWalkEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = instance == null;
				if (!flag2)
				{
					Rigidbody rigidbody = GorillaTagger.Instance.rigidbody;
					LayerMask locomotionEnabledLayers = instance.locomotionEnabledLayers;
					bool wallWalkPerHandBinds = Settings.WallWalkPerHandBinds;
					if (wallWalkPerHandBinds)
					{
						bool flag3 = Settings.WallWalkLeftHand && Settings.IsPressed(Settings.WallWalkLeftKeybind);
						if (flag3)
						{
							Vector3 position = instance.LeftHand.controllerTransform.position;
							WallWalk.StickToWall(rigidbody, position, locomotionEnabledLayers);
						}
						bool flag4 = Settings.WallWalkRightHand && Settings.IsPressed(Settings.WallWalkRightKeybind);
						if (flag4)
						{
							Vector3 position2 = instance.RightHand.controllerTransform.position;
							WallWalk.StickToWall(rigidbody, position2, locomotionEnabledLayers);
						}
					}
					else
					{
						bool flag5 = Settings.IsPressed(Settings.WallWalkKeybind);
						if (flag5)
						{
							bool wallWalkLeftHand = Settings.WallWalkLeftHand;
							if (wallWalkLeftHand)
							{
								Vector3 position3 = instance.LeftHand.controllerTransform.position;
								WallWalk.StickToWall(rigidbody, position3, locomotionEnabledLayers);
							}
							bool wallWalkRightHand = Settings.WallWalkRightHand;
							if (wallWalkRightHand)
							{
								Vector3 position4 = instance.RightHand.controllerTransform.position;
								WallWalk.StickToWall(rigidbody, position4, locomotionEnabledLayers);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00020838 File Offset: 0x0001EA38
	private static bool ProbeSurface(Vector3 origin, float distance, LayerMask mask, out float hitDistance, out Vector3 hitPoint, out Vector3 hitNormal)
	{
		hitDistance = float.MaxValue;
		hitPoint = Vector3.zero;
		hitNormal = Vector3.zero;
		bool result = false;
		RaycastHit val = default(RaycastHit);
		for (int i = 0; i < WallWalk.probeDirections.Length; i++)
		{
			bool flag4 = Physics.Raycast(origin, WallWalk.probeDirections[i], ref val, distance, mask) && Mathf.Abs(Vector3.Dot(val.normal, Vector3.up)) < 0.5f;
			if (flag4)
			{
				string text = ((val.collider != null) ? val.collider.name.ToLower() : "");
				string text2 = ((!(val.collider != null) || !(val.collider.sharedMaterial != null)) ? "" : val.collider.sharedMaterial.name.ToLower());
				bool flag = text.Contains("tree") || text.Contains("branch") || text.Contains("log");
				bool flag2 = text.Contains("slip") || text.Contains("ice") || text.Contains("pit") || text2.Contains("slip") || text2.Contains("ice");
				bool flag3 = !flag && !flag2;
				bool flag5 = (Settings.WallWalkIncludeTrees || flag2) && (!flag || !Settings.WallWalkBlacklistTrees) && (!flag3 || !Settings.WallWalkBlacklistWalls) && (!flag2 || !Settings.WallWalkBlacklistSlippery) && val.distance < hitDistance;
				if (flag5)
				{
					hitDistance = val.distance;
					hitPoint = val.normal;
					hitNormal = val.point;
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00020A34 File Offset: 0x0001EC34
	private static void StickToWall(Rigidbody body, Vector3 origin, LayerMask mask)
	{
		Vector3 hitPoint = default(Vector3);
		float num;
		Vector3 vector;
		bool flag = WallWalk.ProbeSurface(origin, Settings.WallWalkDistance, mask, out num, out hitPoint, out vector);
		if (flag)
		{
			Vector3 val = hitPoint * (0f - Settings.WallWalkPower);
			body.AddForce(val, 5);
		}
	}

	// Token: 0x060001EA RID: 490 RVA: 0x00020A84 File Offset: 0x0001EC84
	static WallWalk()
	{
		Vector3[] array2 = new Vector3[8];
		array2[0] = Vector3.forward;
		array2[1] = Vector3.back;
		array2[2] = Vector3.left;
		array2[3] = Vector3.right;
		Vector3[] array = (Vector3[])array2;
		array[4] = (Vector3.forward + Vector3.right).normalized;
		array[5] = (Vector3.forward + Vector3.left).normalized;
		array[6] = (Vector3.back + Vector3.right).normalized;
		array[7] = (Vector3.back + Vector3.left).normalized;
		WallWalk.probeDirections = array;
	}

	// Token: 0x040004A0 RID: 1184
	private static readonly Vector3[] probeDirections;
}
