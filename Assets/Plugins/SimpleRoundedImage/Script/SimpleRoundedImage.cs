using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Plugins.SimpleRoundedImage.Script
{
    public class SimpleRoundedImage : Image
    {

        //コーナーごとの三角形の最大数、通常は5～8個で良好な丸め効果が得られますが、
        //不要なパフォーマンスの浪費を防ぐためにMaxを設定してください。
        private const int MaxTriangleNum = 20;
        private const int MinTriangleNum = 1;

        public float radius;
        //いくつかの三角形を使って、各コーナーの4分の1を円で埋める。
        [Range(MinTriangleNum, MaxTriangleNum)]
        public int triangleNum;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var v = GetDrawingDimensions(false);
            var uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();
            //半径の値に制限を設け、小さい方の辺の0～1/2の範囲にします。
            var rad = radius;
            if (rad > (v.z - v.x) / 2) rad = (v.z - v.x) / 2;
            if (rad > (v.w - v.y) / 2) rad = (v.w - v.y) / 2;
            if (rad < 0) rad = 0;
            //uvの半径の値に対応する座標軸の半径を計算する
            var uvRadiusX = rad / (v.z - v.x);
            var uvRadiusY = rad / (v.w - v.y);

            //0，1
            vh.AddVert(new Vector3(v.x, v.w - rad), color32, new Vector2(uv.x, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.x, v.y + rad), color32, new Vector2(uv.x, uv.y + uvRadiusY));

            //2，3，4，5
            vh.AddVert(new Vector3(v.x + rad, v.w), color32, new Vector2(uv.x + uvRadiusX, uv.w));
            vh.AddVert(new Vector3(v.x + rad, v.w - rad), color32, new Vector2(uv.x + uvRadiusX, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.x + rad, v.y + rad), color32, new Vector2(uv.x + uvRadiusX, uv.y + uvRadiusY));
            vh.AddVert(new Vector3(v.x + rad, v.y), color32, new Vector2(uv.x + uvRadiusX, uv.y));

            //6，7，8，9
            vh.AddVert(new Vector3(v.z - rad, v.w), color32, new Vector2(uv.z - uvRadiusX, uv.w));
            vh.AddVert(new Vector3(v.z - rad, v.w - rad), color32, new Vector2(uv.z - uvRadiusX, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.z - rad, v.y + rad), color32, new Vector2(uv.z - uvRadiusX, uv.y + uvRadiusY));
            vh.AddVert(new Vector3(v.z - rad, v.y), color32, new Vector2(uv.z - uvRadiusX, uv.y));

            //10，11
            vh.AddVert(new Vector3(v.z, v.w - rad), color32, new Vector2(uv.z, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.z, v.y + rad), color32, new Vector2(uv.z, uv.y + uvRadiusY));

            //左側の長方形
            vh.AddTriangle(1, 0, 3);
            vh.AddTriangle(1, 3, 4);
            //中間の長方形
            vh.AddTriangle(5, 2, 6);
            vh.AddTriangle(5, 6, 9);
            //右側の長方形
            vh.AddTriangle(8, 7, 10);
            vh.AddTriangle(8, 10, 11);

            //四隅の組み立てを始める
            var vCenterList = new List<Vector2>();
            var uvCenterList = new List<Vector2>();
            var vCenterVertList = new List<int>();

            //右上の円の中心
            vCenterList.Add(new Vector2(v.z - rad, v.w - rad));
            uvCenterList.Add(new Vector2(uv.z - uvRadiusX, uv.w - uvRadiusY));
            vCenterVertList.Add(7);

            //左上の円の中心
            vCenterList.Add(new Vector2(v.x + rad, v.w - rad));
            uvCenterList.Add(new Vector2(uv.x + uvRadiusX, uv.w - uvRadiusY));
            vCenterVertList.Add(3);

            //左下の円の中心
            vCenterList.Add(new Vector2(v.x + rad, v.y + rad));
            uvCenterList.Add(new Vector2(uv.x + uvRadiusX, uv.y + uvRadiusY));
            vCenterVertList.Add(4);

            //右下の円の中心
            vCenterList.Add(new Vector2(v.z - rad, v.y + rad));
            uvCenterList.Add(new Vector2(uv.z - uvRadiusX, uv.y + uvRadiusY));
            vCenterVertList.Add(8);

            //各三角形の頂角
            var degreeDelta = (Mathf.PI / 2 / triangleNum);
            //現在の状況
            float curDegree = 0;

            for (var i = 0; i < vCenterVertList.Count; i++)
            {
                var preVertNum = vh.currentVertCount;
                for (var j = 0; j <= triangleNum; j++)
                {
                    var cosA = Mathf.Cos(curDegree);
                    var sinA = Mathf.Sin(curDegree);
                    var vPosition = new Vector3(vCenterList[i].x + cosA * rad, vCenterList[i].y + sinA * rad);
                    var uvPosition = new Vector2(uvCenterList[i].x + cosA * uvRadiusX, uvCenterList[i].y + sinA * uvRadiusY);
                    vh.AddVert(vPosition, color32, uvPosition);
                    curDegree += degreeDelta;
                }
                curDegree -= degreeDelta;
                for (var j = 0; j <= triangleNum - 1; j++)
                {
                    vh.AddTriangle(vCenterVertList[i], preVertNum + j + 1, preVertNum + j);
                }
            }
        }

        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var padding = overrideSprite == null ? Vector4.zero : DataUtility.GetPadding(overrideSprite);
            var r = GetPixelAdjustedRect();
            var size = overrideSprite == null ? new Vector2(r.width, r.height) : new Vector2(overrideSprite.rect.width, overrideSprite.rect.height);
            //Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

            var spriteW = Mathf.RoundToInt(size.x);
            var spriteH = Mathf.RoundToInt(size.y);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                var spriteRatio = size.x / size.y;
                var rectRatio = r.width / r.height;

                if (spriteRatio > rectRatio)
                {
                    var oldHeight = r.height;
                    r.height = r.width * (1.0f / spriteRatio);
                    r.y += (oldHeight - r.height) * rectTransform.pivot.y;
                }
                else
                {
                    var oldWidth = r.width;
                    r.width = r.height * spriteRatio;
                    r.x += (oldWidth - r.width) * rectTransform.pivot.x;
                }
            }

            var v = new Vector4(
                    padding.x / spriteW,
                    padding.y / spriteH,
                    (spriteW - padding.z) / spriteW,
                    (spriteH - padding.w) / spriteH);

            v = new Vector4(
                    r.x + r.width * v.x,
                    r.y + r.height * v.y,
                    r.x + r.width * v.z,
                    r.y + r.height * v.w
                    );

            return v;
        }
    }
}
