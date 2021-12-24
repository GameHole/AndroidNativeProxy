using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AndroidNativeProxy
{
    public static class DialogHelper
    {
        class ClickProxy : AndroidJavaProxy
        {
            public Action<ButtonType> _onClick;
            public ClickProxy() : base("android.content.DialogInterface$OnClickListener")
            {
            }
            public void onClick(AndroidJavaObject dialog, int which)
            {
                _onClick?.Invoke((ButtonType)which);
                dialog.Call("dismiss");
            }
        }
        static readonly ClickProxy proxy = new ClickProxy();
        public enum ButtonType
        {
            OK=-1,CANCEL=-2
        }
        public static string ConvertUrl(string inputUrl)
        {
            if (inputUrl.StartsWith("http"))
                return inputUrl;
            else
                return "file:///android_asset/" + inputUrl;
        }
        public static void ShowPrivacyPolicyDialog(string url, Action<ButtonType> onClick, int gravity = 80)
        {
            proxy._onClick = onClick;
            var act = ActivityGeter.GetActivity();
            if (act == null) return;
            var display = act.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay");
            using (var bdr = new AndroidJavaObject("android.app.AlertDialog$Builder", act))
            {
                bdr.Call("setTitle", "隐私协议");
                using (var webview = new AndroidJavaObject("android.webkit.WebView", act))
                {
                    var setting = webview.Call<AndroidJavaObject>("getSettings");
                    setting.Call("setUseWideViewPort", true);
                    setting.Call("setLoadWithOverviewMode", true);
                    setting.Call("setSupportZoom", true);
                    webview.Call("setLayoutParams",
                        new AndroidJavaObject("android.view.ViewGroup$LayoutParams", display.Call<int>("getWidth"), WHParams.WRAP_CONTENT));
                    webview.Call("setWebViewClient", new AndroidJavaObject("android.webkit.WebViewClient"));
                    webview.Call("loadUrl", ConvertUrl(url));
                    bdr.Call<AndroidJavaObject>("setView", webview);
                    bdr.Call<AndroidJavaObject>("setCancelable", false);
                    bdr.Call<AndroidJavaObject>("setPositiveButton", "同意", proxy);
                    bdr.Call<AndroidJavaObject>("setNegativeButton", "退出", proxy);
                    var dialog = bdr.Call<AndroidJavaObject>("create");
                    var win = dialog.Call<AndroidJavaObject>("getWindow");
                    win.Call("setGravity", gravity);
                    var pama = win.Call<AndroidJavaObject>("getAttributes");
                    pama.Set("width", display.Call<int>("getWidth"));
                    win.Call<AndroidJavaObject>("setAttributes", pama);
                    dialog.Call("show");
                }
            }
        }
    }
}

