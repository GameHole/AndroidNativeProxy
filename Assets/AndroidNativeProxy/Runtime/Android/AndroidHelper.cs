using System;
using System.Collections.Generic;
using UnityEngine;
using MiniGameSDK;
namespace AndroidNativeProxy
{
	public static class AndroidHelper
    {
        private static AndroidJavaObject handler;
        public static bool PostToAndroidUIThread(Action action)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                var runnable = new AndroidJavaRunnable(() =>
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                });
                var act = ActivityGeter.GetActivity();
                if (act != null)
                {
                    act.Call("runOnUiThread", runnable);
                }
                return true;
                //if (handler == null)
                //{
                //    handler = new AndroidJavaObject("android.os.Handler", new AndroidJavaObject("android.os.Looper")
                //      .CallStatic<AndroidJavaObject>("getMainLooper"));
                //}
                //return handler.Call<bool>("post", runnable);
            }
            if (PlatfotmHelper.isEditor())
            {
                action?.Invoke();
                return true;
            }
            return false;
        }
    }
}
