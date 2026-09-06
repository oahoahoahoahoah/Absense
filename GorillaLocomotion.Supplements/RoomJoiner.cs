using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000030 RID: 48
public static class RoomJoiner
{
	// Token: 0x0600015B RID: 347 RVA: 0x00019144 File Offset: 0x00017344
	private static void FindPhotonType()
	{
		bool flag = RoomJoiner.searched;
		if (!flag)
		{
			RoomJoiner.searched = true;
			try
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Assembly[] array = assemblies;
				foreach (Assembly assembly in array)
				{
					try
					{
						RoomJoiner.photonType = assembly.GetTypes().FirstOrDefault<Type>(delegate(Type photonType)
						{
							string name = photonType.Name;
							return name == "PhotonNetworkController";
						});
						bool flag2 = RoomJoiner.photonType != null;
						if (flag2)
						{
							break;
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x00019204 File Offset: 0x00017404
	private static object GetPhotonInstance()
	{
		RoomJoiner.FindPhotonType();
		bool flag = RoomJoiner.photonType == null;
		object obj;
		if (flag)
		{
			obj = null;
		}
		else
		{
			try
			{
				PropertyInfo property = RoomJoiner.photonType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				bool flag2 = property != null;
				if (flag2)
				{
					return property.GetValue(null);
				}
				FieldInfo field = RoomJoiner.photonType.GetField("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				bool flag3 = field != null;
				if (flag3)
				{
					return field.GetValue(null);
				}
				MethodInfo method = typeof(Object).GetMethod("FindObjectOfType", new Type[] { typeof(Type) });
				bool flag4 = method != null;
				if (flag4)
				{
					return method.Invoke(null, new object[] { RoomJoiner.photonType });
				}
			}
			catch
			{
			}
			obj = null;
		}
		return obj;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x000192F8 File Offset: 0x000174F8
	public static void JoinRoom()
	{
		RoomJoiner.JoinStateMachine stateMachine = default(RoomJoiner.JoinStateMachine);
		stateMachine._003C_003Et__builder = AsyncVoidMethodBuilder.Create();
		stateMachine._003C_003E1__state = -1;
		stateMachine._003C_003Et__builder.Start<RoomJoiner.JoinStateMachine>(ref stateMachine);
	}

	// Token: 0x0600015E RID: 350 RVA: 0x00019334 File Offset: 0x00017534
	public static void LeaveRoom()
	{
		try
		{
			bool inRoom = PhotonNetwork.InRoom;
			if (inRoom)
			{
				PhotonNetwork.Disconnect();
				RoomJoiner.JoinStatus = "Disconnected";
			}
			else
			{
				RoomJoiner.JoinStatus = "Not in a room";
			}
		}
		catch (Exception ex)
		{
			RoomJoiner.JoinStatus = "Error: " + ex.Message;
		}
	}

	// Token: 0x0400028C RID: 652
	public static string RoomCode = "";

	// Token: 0x0400028D RID: 653
	public static bool IsJoining = false;

	// Token: 0x0400028E RID: 654
	public static string JoinStatus = "";

	// Token: 0x0400028F RID: 655
	private static object cachedInstance = null;

	// Token: 0x04000290 RID: 656
	private static MethodInfo joinMethod = null;

	// Token: 0x04000291 RID: 657
	private static Type photonType = null;

	// Token: 0x04000292 RID: 658
	private static bool searched = false;

	// Token: 0x02000079 RID: 121
	[CompilerGenerated]
	[Serializable]
	private sealed class Class1
	{
		// Token: 0x06000280 RID: 640 RVA: 0x00023994 File Offset: 0x00021B94
		internal bool method_0(Type photonType)
		{
			string name = photonType.Name;
			return name == "PhotonNetworkController";
		}

		// Token: 0x04000538 RID: 1336
		public static readonly RoomJoiner.Class1 _003C_003E9 = new RoomJoiner.Class1();

		// Token: 0x04000539 RID: 1337
		public static Func<Type, bool> _003C_003E9__7_0;
	}

	// Token: 0x0200007A RID: 122
	private struct JoinStateMachine : IAsyncStateMachine
	{
		// Token: 0x06000283 RID: 643 RVA: 0x000239CD File Offset: 0x00021BCD
		public void MoveNext()
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000239D0 File Offset: 0x00021BD0
		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			this._003C_003Et__builder.SetStateMachine(stateMachine);
		}

		// Token: 0x0400053A RID: 1338
		public int _003C_003E1__state;

		// Token: 0x0400053B RID: 1339
		public AsyncVoidMethodBuilder _003C_003Et__builder;
	}
}
