using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AndroidNativeProxy
{
    public class WebView : MonoBehaviour
    {
        RectTransform rectTransform;
        public int textScale = 100;
        public string url;
        AndroidWebView view;
        private void Awake()
        {
            view = new AndroidWebView();
            view.textScale = textScale;
            view.url = url;
            view.isSupportZoom = view.isLoadWithOverviewMode = view.useWideViewPort = true;
            rectTransform = GetComponent<RectTransform>();
        }
        private void OnEnable()
        {
            if (rectTransform)
            {
                Rect screenRect = rectTransform.TransformToScreenRect();
                var position = new Vector2(screenRect.min.x, Screen.height - screenRect.max.y);
                Debug.Log(position);
                var size = new Vector2Int((int)screenRect.size.x, (int)screenRect.size.y);
                Debug.Log(size);
                view.size = size;
                view.position = position;
                view.Show();
            }
        }
        
        private void OnDisable()
        {
            view.Hide();
        }
    }
}
