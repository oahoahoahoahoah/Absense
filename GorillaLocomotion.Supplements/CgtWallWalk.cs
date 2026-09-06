using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000008 RID: 8
public static class CgtWallWalk
{
	// Token: 0x0600000D RID: 13 RVA: 0x000027F0 File Offset: 0x000009F0
	public static void Apply()
	{
		bool flag = !Settings.CGTWallWalkEnabled;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = !(instance == null) && Settings.IsPressed(Settings.CGTWallWalkKeybind);
				if (flag2)
				{
					Rigidbody rigidbody = GorillaTagger.Instance.rigidbody;
					LayerMask locomotionEnabledLayers = instance.locomotionEnabledLayers;
					Vector3 position = instance.LeftHand.controllerTransform.position;
					CgtWallWalk.StickToWall(rigidbody, position, locomotionEnabledLayers);
					Vector3 position2 = instance.RightHand.controllerTransform.position;
					CgtWallWalk.StickToWall(rigidbody, position2, locomotionEnabledLayers);
				}
			}
			catch (Exception)
			{
			}
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002898 File Offset: 0x00000A98
	private static bool ProbeSurface(Vector3 origin, float distance, LayerMask mask, out float hitDistance, out Vector3 hitPoint, out Vector3 hitNormal)
	{
		hitDistance = float.MaxValue;
		hitPoint = Vector3.zero;
		hitNormal = Vector3.zero;
		bool result = false;
		int i = 0;
		RaycastHit val = default(RaycastHit);
		while (i < CgtWallWalk.probeDirections.Length)
		{
			bool flag = Physics.Raycast(origin, CgtWallWalk.probeDirections[i], ref val, distance, mask) && Mathf.Abs(Vector3.Dot(val.normal, Vector3.up)) < 0.5f && val.distance < hitDistance;
			if (flag)
			{
				hitDistance = val.distance;
				hitPoint = val.normal;
				hitNormal = val.point;
				result = true;
			}
			i++;
		}
		return result;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002968 File Offset: 0x00000B68
	private static void StickToWall(Rigidbody body, Vector3 origin, LayerMask mask)
	{
		float float_ = 0f;
		Vector3 hitPoint = default(Vector3);
		float num3 = Settings.CGTWallWalkDistance;
		float num4 = Mathf.Max(0.01f, Settings.CGTWallWalkReach);
		float float_2 = num3 + num4;
		Vector3 vector;
		bool flag = CgtWallWalk.ProbeSurface(origin, float_2, mask, out float_, out hitPoint, out vector);
		if (flag)
		{
			bool flag2 = float_ > num3;
			if (flag2)
			{
				float num5 = Mathf.Clamp01((float_ - num3) / num4);
				Vector3 val = -hitPoint;
				float num6 = Settings.CGTWallWalkPower * (num5 * num5);
				body.AddForce(val * num6, 5);
			}
		}
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002A08 File Offset: 0x00000C08
	static CgtWallWalk()
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
		CgtWallWalk.probeDirections = array;
	}

	// Token: 0x04000021 RID: 33
	private static readonly Vector3[] probeDirections;
}
