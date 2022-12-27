using System;
using System.Collections;
using System.Collections.Generic;
using TofAr.V0.Hand;
using UnityEngine;

public class Mant : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private int[] indices;
    private Vector3[] vertices = new Vector3[4];

    private int bothDetected = 0;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        indices = new int[] {0, 1, 2, 0, 1, 3, 1, 0, 2, 1, 0, 3, 2, 3, 0, 2, 3, 1, 3, 2, 0, 3, 2, 1};
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = GetComponent<MeshRenderer>();

        TofArHandManager.OnFrameArrived += FrameArrived;
    }

    private void FrameArrived(object sender)
    {
        HandData handData = TofArHandManager.Instance.HandData;
        HandStatus handStatus = handData.Data.handStatus;

        if (handStatus == HandStatus.BothHands)
        {
            Vector3[] left = handData.Data.featurePointsLeft;
            vertices[0] = left[(int)HandPointIndex.IndexTip];
            vertices[1] = left[(int)HandPointIndex.ThumbTip];

            Vector3[] right = handData.Data.featurePointsRight;
            vertices[2] = right[(int)HandPointIndex.IndexTip];
            vertices[3] = right[(int)HandPointIndex.ThumbTip];

            bothDetected = 10;

            Quaternion rot = Quaternion.Euler(0, 0, -90);
            for (int i = 0; i < vertices.Length; i++)
            {
                // rotation
                vertices[i] = rot * vertices[i];

                // set z to 0.1f
                vertices[i] = vertices[i] / vertices[i].z * 0.1f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bothDetected > 0)
        {
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.RecalculateBounds();
            meshRenderer.enabled = true;
            bothDetected--;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
