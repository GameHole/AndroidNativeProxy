using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AndroidNativeProxy
{
    public static class TransformHelper
    {
        public static Rect TransformToScreenRect(this RectTransform transform)
        {
            var localRect = transform.rect;
            Rect screenRect = new Rect();
            screenRect.max = transform.ToScreen((Vector2)transform.localPosition + localRect.max);
            screenRect.min = transform.ToScreen((Vector2)transform.localPosition + localRect.min);
            return screenRect;
        }
        static Vector3 ToScreen(this RectTransform rectTransform, Vector3 localPos)
        {
            var pos = rectTransform.parent.TransformPoint(localPos);
            var canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas)
            {
                var cam = canvas.worldCamera;
                if (cam != null)
                {
                    pos = cam.WorldToScreenPoint(pos);
                }
            }
            return pos;
        }
    }
}

