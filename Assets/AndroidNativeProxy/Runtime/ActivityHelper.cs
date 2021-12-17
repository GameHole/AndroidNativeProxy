using System;
using System.Collections.Generic;
using UnityEngine;
namespace AndroidNativeProxy
{
	public class ActivityHelper
	{
        static ActivityCreated created = new ActivityCreated();
        static AndroidJavaClass empAct = new AndroidJavaClass("com.ad.nativeview.EmptyActivity");
        public static void CreateEmpty(Action<AndroidJavaObject> action)
        {
            created.action = action;
            empAct.CallStatic("New", ActivityGeter.GetActivity(), created);
        }
	}
}
