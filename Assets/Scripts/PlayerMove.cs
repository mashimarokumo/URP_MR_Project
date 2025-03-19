using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using DG.Tweening;
using Oculus.Interaction.Input;
using static OVRInput;

public class PlayerMove : MonoBehaviour
{
    public RayInteractor interactor;
    public float speed;
    Vector3 pos;


    bool startDraw = false;

    //public void OnHover()
    //{
    //    startDraw = true;
    //}

    //public void UnHover()
    //{
    //    startDraw = false;
    //}

    public void Move()
    {
        Vector3[] points = new Vector3[20];
        lineRenderer.GetPositions(points);
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < points.Length; i++)
        {
            sequence.Append(transform.DOMove(points[i], 0.1f));
        }
        sequence.Play();
        //Player.Instance.transform.DOMove(interactor.End, 2f);
    }

    private void Update()
    {
        startDraw = false;
        foreach (var item in Physics.RaycastAll(transform.position, Player.Instance.playerCam.transform.forward))
        {
            if (item.collider.gameObject.CompareTag("ground"))
            {
                pos = item.point;
                startDraw = true;
            }
        } 

        if(startDraw && OVRInput.Get(Button.SecondaryIndexTrigger))
        {
            DrawQuadraticBezierCurve(transform.position, pos);
        }
        else if(startDraw && OVRInput.GetUp(Button.SecondaryIndexTrigger))
        {
            Move();
            lineRenderer.positionCount = 0;
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    public float height = 1f;
    public LineRenderer lineRenderer;
    // 曲线分段数，分段越多，曲线越平滑
    public int segments = 20;

    // 绘制二次贝塞尔曲线
    private void DrawQuadraticBezierCurve(Vector3 pointA, Vector3 pointB)
    {
        lineRenderer.positionCount = segments;
        Vector3 midPoint = (pointA + pointB) / 2f;
        Vector3 controlPoint = midPoint + Vector3.up * height;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (segments - 1.0f);
            Vector3 position = CalculateQuadraticBezierPoint(t, pointA, controlPoint, pointB);
            lineRenderer.SetPosition(i, position);
        }
    }

    // 计算二次贝塞尔曲线上的点
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }
}
