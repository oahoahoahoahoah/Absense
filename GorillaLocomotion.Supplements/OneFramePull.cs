using System;
using System.Reflection;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000027 RID: 39
internal class OneFramePull
{
	// Token: 0x06000124 RID: 292 RVA: 0x00015B34 File Offset: 0x00013D34
	private static float GetPullStrength()
	{
		float oneFramePullStrength = Settings.OneFramePullStrength;
		return Mathf.Max(0.01f, oneFramePullStrength + Random.Range(0f - Settings.PullRandomiseMin, Settings.PullRandomiseMax));
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00015B70 File Offset: 0x00013D70
	private static bool ShouldPull()
	{
		bool flag = GTPlayer.Instance != null;
		return flag && GTPlayer.Instance.disableMovement;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00015BA0 File Offset: 0x00013DA0
	private static bool HasValidTarget(GTPlayer player)
	{
		int int_ = -2101770653;
		int num = 0;
		Transform val = null;
		int num2 = 0;
		RaycastHit val2 = default(RaycastHit);
		string text = null;
		uint num11;
		for (;;)
		{
			num++;
			int num3 = ObfConst.Decode(int_) % 10;
			switch (num3 / 4)
			{
			case 0:
				switch (num3 % 4)
				{
				case 0:
				{
					bool flag = Physics.Raycast(player.transform.position + Vector3.up * 0.3f, Vector3.down, ref val2, 1.5f);
					if (flag)
					{
						int_ = -2101770628 ^ ((num * (num + 1)) & 1);
						continue;
					}
					int num4 = num;
					int num5 = num;
					int_ = -2101770636 ^ ((num4 * (num5 + 1)) & 1);
					continue;
				}
				case 1:
					val = val2.transform;
					int_ = -2101770629 ^ ((num * (num + 1)) & 1);
					continue;
				case 2:
					text = val.name;
					num2 = 0;
					int_ = -2101770651 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
					int_ = ((!(text == OneFramePull.targetNames[num2])) ? (-2101770656 ^ ((num * (num + 1)) & 1)) : (-2101770649 ^ ((num * (num + 1)) & 1)));
					continue;
				}
				break;
			case 1:
				switch (num3 % 4)
				{
				case 0:
					goto IL_014D;
				case 1:
					num2++;
					int_ = -2101770651 ^ ((num * (num + 1)) & 1);
					continue;
				case 2:
				{
					bool flag2 = num2 >= OneFramePull.targetNames.Length;
					if (flag2)
					{
						int_ = -2101770650 ^ ((num * (num + 1)) & 1);
						continue;
					}
					int num6 = num;
					int num7 = num;
					int_ = -2101770654 ^ ((num6 * (num7 + 1)) & 1);
					continue;
				}
				case 3:
					val = val.parent;
					int_ = -2101770629 ^ ((num * (num + 1)) & 1);
					continue;
				}
				break;
			case 2:
			{
				uint num10 = (uint)(num3 % 4);
				num11 = num10;
				if (num11 != 0U)
				{
					goto Block_2;
				}
				bool flag3 = !(val != null);
				if (flag3)
				{
					int_ = -2101770636 ^ ((num * (num + 1)) & 1);
					continue;
				}
				int num8 = num;
				int num9 = num;
				int_ = -2101770655 ^ ((num8 * (num9 + 1)) & 1);
				continue;
			}
			}
			break;
		}
		goto IL_024A;
		Block_2:
		if (num11 == 1U)
		{
			return false;
		}
		goto IL_024A;
		IL_014D:
		return true;
		IL_024A:
		throw null;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00015E08 File Offset: 0x00014008
	public static void Update()
	{
		bool flag2 = !Settings.OneFramePullEnabled;
		if (!flag2)
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag3 = instance == null;
			if (!flag3)
			{
				bool oneFramePullToggleMode = Settings.OneFramePullToggleMode;
				if (oneFramePullToggleMode)
				{
					bool flag = Settings.IsPressed(Settings.OneFramePullActivation);
					bool flag4 = flag && !OneFramePull.rightWasColliding;
					if (flag4)
					{
						Settings.OneFramePullToggleActive = !Settings.OneFramePullToggleActive;
					}
					OneFramePull.rightWasColliding = flag;
					bool flag5 = !Settings.OneFramePullToggleActive;
					if (flag5)
					{
						OneFramePull.toggleActive = instance.LeftHand.wasColliding;
						OneFramePull.leftWasColliding = instance.RightHand.wasColliding;
						OneFramePull.toggleHeld = false;
						return;
					}
				}
				bool flag6 = Settings.OneFramePullToggleMode || Settings.IsPressed(Settings.OneFramePullActivation);
				if (flag6)
				{
					bool wasColliding = instance.LeftHand.wasColliding;
					bool wasColliding2 = instance.RightHand.wasColliding;
					bool flag7 = ((OneFramePull.toggleActive && !wasColliding) || (OneFramePull.leftWasColliding && !wasColliding2)) && !OneFramePull.toggleHeld && !OneFramePull.HasValidTarget(instance);
					if (flag7)
					{
						Rigidbody val = OneFramePull.GetRigidbody(instance);
						bool flag8 = val != null;
						if (flag8)
						{
							Vector3 vector3_ = OneFramePull.GetAveragedVelocity(val);
							float num = 6.5f * Settings.OneFramePullThreshold;
							bool flag9 = vector3_.magnitude / instance.scale > num;
							if (flag9)
							{
								Vector3 val2 = OneFramePull.ClampByObstacles(instance, vector3_);
								bool flag10 = val2.sqrMagnitude > 0.001f;
								if (flag10)
								{
									OneFramePull.toggleHeld = true;
									OneFramePull.lastPullTime = 0f;
									OneFramePull.appliedOffset = val2;
								}
							}
						}
					}
					bool flag11 = OneFramePull.toggleHeld;
					if (flag11)
					{
						OneFramePull.lastPullTime += Time.deltaTime;
						bool flag12 = OneFramePull.lastPullTime < 0.02f;
						if (flag12)
						{
							float num2 = Time.deltaTime / 0.02f;
							Transform transform = instance.transform;
							transform.position += OneFramePull.appliedOffset * num2;
						}
						else
						{
							OneFramePull.toggleHeld = false;
						}
					}
				}
				else
				{
					OneFramePull.toggleHeld = false;
				}
				OneFramePull.toggleActive = instance.LeftHand.wasColliding;
				OneFramePull.leftWasColliding = instance.RightHand.wasColliding;
			}
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00016040 File Offset: 0x00014240
	private static Vector3 ClampByObstacles(GTPlayer player, Vector3 origin)
	{
		Vector3 val = default(Vector3);
		Vector3 val2 = default(Vector3);
		RaycastHit val3 = default(RaycastHit);
		Vector3 val4 = default(Vector3);
		val2 = player.transform.position;
		val = origin * OneFramePull.GetPullStrength() * 0.044f;
		bool flag = !Physics.Raycast(val2 + Vector3.up * 0.5f, Vector3.down, ref val3, 2f);
		if (!flag)
		{
			Vector3 normal = val3.normal;
			val4 = val - Vector3.Dot(val, normal) * normal;
			bool flag2 = normal.y > 0.5f;
			if (flag2)
			{
				val = val4;
			}
		}
		float num4 = Mathf.Sqrt(val.x * val.x + val.z * val.z);
		bool flag3 = val.y < (0f - num4) * 0.5f;
		Vector3 vector;
		if (flag3)
		{
			bool flag4 = val.y <= num4 * 1.5f;
			if (flag4)
			{
			}
			Vector3[] array = (Vector3[])new Vector3[5];
			int num5 = 0;
			for (;;)
			{
				int num6 = num5;
				bool flag5 = num6 < 5;
				if (!flag5)
				{
					break;
				}
				int num7 = num5;
				float num8 = (float)(num7 + 1) / 5f;
				array[num5] = val * num8;
				num5++;
			}
			int num9 = 4;
			for (;;)
			{
				int num10 = num9;
				bool flag6 = num10 < 0;
				if (flag6)
				{
					break;
				}
				bool flag7 = !OneFramePull.HasLineOfSight(val2, array[num9]);
				if (flag7)
				{
					goto Block_6;
				}
				num9--;
			}
			return Vector3.zero;
			Block_6:
			vector = array[num9];
		}
		else
		{
			bool flag8 = val.y <= num4 * 1.5f;
			if (flag8)
			{
			}
			Vector3[] array = (Vector3[])new Vector3[5];
			int num5 = 0;
			for (;;)
			{
				int num6 = num5;
				bool flag9 = num6 < 5;
				if (!flag9)
				{
					break;
				}
				int num7 = num5;
				float num8 = (float)(num7 + 1) / 5f;
				array[num5] = val * num8;
				num5++;
			}
			int num9 = 4;
			for (;;)
			{
				int num10 = num9;
				bool flag10 = num10 < 0;
				if (flag10)
				{
					break;
				}
				bool flag11 = !OneFramePull.HasLineOfSight(val2, array[num9]);
				if (flag11)
				{
					goto Block_9;
				}
				num9--;
			}
			return Vector3.zero;
			Block_9:
			vector = array[num9];
		}
		return vector;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000162C0 File Offset: 0x000144C0
	private static bool HasLineOfSight(Vector3 origin, Vector3 target)
	{
		RaycastHit val = default(RaycastHit);
		float num2 = target.magnitude;
		bool flag = num2 < 0.01f;
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			Vector3 val2 = target / num2;
			bool flag3 = Physics.SphereCast(origin, 0.1f, val2, ref val, num2);
			if (flag3)
			{
				bool flag4 = val.distance >= num2;
				if (flag4)
				{
					bool flag5 = Physics.CheckSphere(origin + target, 0.2f);
					flag2 = flag5;
				}
				else
				{
					flag2 = true;
				}
			}
			else
			{
				bool flag6 = Physics.CheckSphere(origin + target, 0.2f);
				flag2 = flag6;
			}
		}
		return flag2;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00016374 File Offset: 0x00014574
	private static Rigidbody GetRigidbody(GTPlayer player)
	{
		bool flag = !(OneFramePull.cachedRigidbody == null);
		if (!flag)
		{
			OneFramePull.cachedRigidbody = player.GetComponent<Rigidbody>();
		}
		return OneFramePull.cachedRigidbody;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000163B0 File Offset: 0x000145B0
	private static Vector3 GetAveragedVelocity(Rigidbody body)
	{
		bool flag = OneFramePull.ShouldPull();
		Vector3 vector;
		if (flag)
		{
			vector = GTPlayer.Instance.AveragedVelocity;
		}
		else
		{
			vector = OneFramePull.GetVelocity(body);
		}
		return vector;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x000163E0 File Offset: 0x000145E0
	private static Vector3 GetVelocity(Rigidbody body)
	{
		bool flag = body == null;
		Vector3 vector;
		if (flag)
		{
			vector = Vector3.zero;
		}
		else
		{
			bool flag2 = !OneFramePull.velocityReflectionReady;
			if (flag2)
			{
				OneFramePull.velocityReflectionReady = true;
				Type typeFromHandle = typeof(Rigidbody);
				OneFramePull.velocityProperty = typeFromHandle.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public);
				bool flag3 = OneFramePull.velocityProperty == null;
				if (flag3)
				{
					Type typeFromHandle2 = typeof(Rigidbody);
					OneFramePull.velocityProperty = typeFromHandle2.GetProperty("velocity", BindingFlags.Instance | BindingFlags.Public);
				}
			}
			bool flag4 = OneFramePull.velocityProperty != null;
			if (flag4)
			{
				try
				{
					return (Vector3)OneFramePull.velocityProperty.GetValue(body);
				}
				catch
				{
				}
			}
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x04000244 RID: 580
	private static bool toggleActive;

	// Token: 0x04000245 RID: 581
	private static bool leftWasColliding;

	// Token: 0x04000246 RID: 582
	private static bool rightWasColliding;

	// Token: 0x04000247 RID: 583
	private static PropertyInfo velocityProperty = null;

	// Token: 0x04000248 RID: 584
	private static bool velocityReflectionReady = false;

	// Token: 0x04000249 RID: 585
	private static Rigidbody cachedRigidbody = null;

	// Token: 0x0400024A RID: 586
	private const float SmoothTime = 0.2f;

	// Token: 0x0400024B RID: 587
	private const int StopBackSteps = 5;

	// Token: 0x0400024C RID: 588
	private static bool toggleHeld = false;

	// Token: 0x0400024D RID: 589
	private static float lastPullTime = 0f;

	// Token: 0x0400024E RID: 590
	private static Vector3 appliedOffset = Vector3.zero;

	// Token: 0x0400024F RID: 591
	private static readonly string[] targetNames = new string[] { "hoverboardentranceslide", "concrete slab" };
}
