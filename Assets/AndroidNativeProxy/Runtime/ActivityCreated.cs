using System;
using System.Collections.Generic;
using UnityEngine;
namespace AndroidNativeProxy
{
    class ActivityCreated : AndroidJavaProxy
    {
        internal Action<AndroidJavaObject> action;
        internal ActivityCreated() : this(null)
        {
        }
        public ActivityCreated(Action<AndroidJavaObject> action) : base("com.ad.nativeview.EmptyActivity$IOnActivityCreated")
        {
            this.action = action;
        }
        public void onCreate(AndroidJavaObject activity)
        {
            action?.Invoke(activity);
        }
    }
}
