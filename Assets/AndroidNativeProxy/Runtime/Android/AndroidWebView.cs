using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AndroidNativeProxy
{
    public class AndroidWebView
    {
        public Vector2 position;
        public Vector2Int size;
        public bool useWideViewPort;
        public bool isLoadWithOverviewMode;
        public bool isSupportZoom;
        public int textScale = 100;
        public string url;
        FrameLayout layout;
        public AndroidWebView()
        {
            layout = new FrameLayout();
            layout.JoinToRenderCurrentActivity(new FrameLayout.LayoutParams(WHParams.MATCH_PARENT, WHParams.MATCH_PARENT));
        }
        public void Show()
        {
            ShowInternal(this);
        }
        //static FrameLayout layout;
        static void ShowInternal(AndroidWebView view)
        {
            var act = ActivityGeter.GetActivity();
            if (act!=null)
            {
                AndroidHelper.PostToAndroidUIThread(() =>
                {
                    using (var webview = new AndroidJavaObject("android.webkit.WebView", act))
                    {
                        var setting = webview.Call<AndroidJavaObject>("getSettings");
                        setting.Call("setUseWideViewPort", view.useWideViewPort);
                        setting.Call("setLoadWithOverviewMode", view.isLoadWithOverviewMode);
                        setting.Call("setSupportZoom", view.isSupportZoom);
                        setting.Call("setTextZoom", view.textScale);
                        webview.Call("setLayoutParams",
                            new AndroidJavaObject("android.view.ViewGroup$LayoutParams", view.size.x, view.size.y));
                        webview.Call("setWebViewClient", new AndroidJavaObject("android.webkit.WebViewClient"));
                        webview.Call("loadUrl",DialogHelper.ConvertUrl(view.url));
                        view.layout.RemoveAllViews();
                        view.layout.AddView(webview);
                        webview.Call("setX", view.position.x);
                        webview.Call("setY", view.position.y);
                    }
                });
            }
        }
        
        public void Hide()
        {
            layout.RemoveAllViews();
        }
    }
}

