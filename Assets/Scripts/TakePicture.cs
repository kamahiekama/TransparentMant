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

    private Texture2D tex;

    /// <summary>
    /// 撮影した画像データ
    /// </summary>
    //public RenderTexture buffer;
    public Texture2D buffer;

    /// <summary>
    /// 撮影した画像データを反映させるマテリアル
    /// </summary>
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        // コールバックメソッドの登録
        TofArColorManager.OnFrameArrived += FrameArrived;
    }

    public void FrameArrived(object sender)
    {
        TofArColorManager mgr = (TofArColorManager)sender;
        if (mgr.ColorTexture != null)
        {
            tex = mgr.ColorTexture;
        }
        else
        {
            Debug.Log("null");
        }
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    public void TakePhoto()
    {
        trigger = true;
    }
}
