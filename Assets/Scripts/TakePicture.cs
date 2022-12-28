using System;
using System.Collections;
using System.Collections.Generic;
using TofAr.V0.Color;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    /// <summary>
    /// 撮影ボタンが押されたら true になり、次の画像フレームを取得する
    /// </summary>
    private bool trigger = false;

    /// <summary>
    /// Y, UV テクスチャをコピーする元
    /// </summary>
    public Material MatSrc;

    private Texture2D YTex;
    private Texture2D UVTex;

    /// <summary>
    /// 撮影した画像データを反映させるマテリアル
    /// </summary>
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        // コールバックメソッドの登録
        TofArColorManager.OnFrameArrived += FrameArrived;

        ResolutionProperty config = TofArColorManager.Instance.GetProperty<ResolutionProperty>();

        //texWidth = config.width;
        //texHeight = config.height;
    }

    public void FrameArrived(object sender)
    {
        // YUV420 設定だと mgr.ColorTexture が null になってしまうため、ここからは映像取得できない。
        /*
        TofArColorManager mgr = (TofArColorManager)sender;
        if (mgr.ColorTexture != null)
        {
            tex = mgr.ColorTexture;
        }
        else
        {
            Debug.Log("null");
        }
        //*/
    }

    // Update is called once per frame
    void Update()
    {
        // ColorTexture からコピーするやり方
        /*
        if (trigger && tex != null)
        {
            if (buffer == null)
            {
                buffer = new Texture2D(tex.width, tex.height, tex.format, false);
            }
            byte[] byteArray = tex.GetRawTextureData();
            buffer.LoadRawTextureData(byteArray);
            buffer.Apply();

            material.mainTexture = buffer;

            trigger = false;
        }
        //*/
        // Y, UV Texture をコピーする
        if (trigger)
        {
            Texture2D ytex = (Texture2D)MatSrc.GetTexture("_YTex");
            Texture2D uvtex = (Texture2D)MatSrc.GetTexture("_UVTex");

            if (YTex == null)
            {
                YTex = new Texture2D(ytex.width, ytex.height, ytex.format, false);
                UVTex = new Texture2D(uvtex.width, uvtex.height, uvtex.format, false);
            }

            YTex.LoadRawTextureData(ytex.GetRawTextureData());
            YTex.Apply();
            UVTex.LoadRawTextureData(uvtex.GetRawTextureData());
            UVTex.Apply();

            material.SetTexture("_YTex", YTex);
            material.SetTexture("_UVTex", UVTex);

            trigger = false;
        }
    }

    public void TakePhoto()
    {
        trigger = true;
    }
}
