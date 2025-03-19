using DG.Tweening;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public InteractableUnityEventWrapper panel;
    public Camera playerCam;
    public Camera staticCam;
    [HideInInspector]
    public CameraSwitchSpot curSelecting;
    OVRManager ovr;
    public Material skyMat;

    public Image cover;
    public Volume globalVolume;
    MotionBlur motionBlur;
    LensDistortion lensDistortion;

    enum Transition
    {
        None,

    }



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        panel.WhenSelect.AddListener(() => { if (curSelecting != null) SwitchCameraSpot(curSelecting); });
        ovr = GetComponent<OVRManager>();
        globalVolume.profile.TryGet(out motionBlur);
        globalVolume.profile.TryGet(out lensDistortion);
        motionBlur.active = false;
    }

    public void SwitchCameraSpot(CameraSwitchSpot cam)
    {
        //transform.DOShakeRotation(0.5f, 1, 10).onComplete += () =>
        //{
            StartCoroutine(ControlVariable());
            cover.DOColor(new Color(0, 0, 0, 0.9f), 0.4f);
            cover.DOColor(new Color(0, 0, 0, 0f), 0.1f).SetDelay(1.2f);
            //TimerManager.instance.CreateAndStartTimer(2, 1, () => { cover.DOColor(new Color(0, 0, 0, 0f), 1); });
            
            motionBlur.active = true;
            TimerManager.instance.CreateAndStartTimer(1, 1, () => { motionBlur.active = false; });
            
            //TimerManager.instance.CreateAndStartTimer(0.1f, 6, () => { lensDistortion.intensity.Override(lensDistortion.intensity.value - 0.1f); });
            //TimerManager.instance.CreateAndStartTimer(1.4f, 1, () =>
            //{
            //    TimerManager.instance.CreateAndStartTimer(0.1f, 6, () => { lensDistortion.intensity.Override(lensDistortion.intensity.value - 0.1f); });
            //});
            //Debug.Log(cam.cameraData.transform.position);
            transform.DOMove(cam.cameraData.transform.position, 1.4f).SetEase(Ease.OutCirc);
            //transform.DOLookAt(cam.transform.forward, 2f, AxisConstraint.None, cam.transform.up);

            //transform.SetParent(cam.transform);
            //playerCam.gameObject.SetActive(false);
            //staticCam.gameObject.SetActive(true);

            ////playerCam.focalLength = cam.cameraData.focalLength;
            //transform.localPosition = Vector3.zero;
            //transform.localRotation = Quaternion.identity;

            ////staticCam.focalLength = cam.cameraData.focalLength;

            //Vector3 offset = cam.transform.position - playerCam.transform.position;
            //transform.position += offset;

            //Quaternion rot = Quaternion.FromToRotation(playerCam.transform.forward, cam.transform.forward);
            //Quaternion targetRot = rot * transform.rotation;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 20);

            //staticCam.transform.position = playerCam.transform.position;
            //staticCam.transform.rotation = playerCam.transform.rotation;

            //TimerManager.instance.CreateAndStartTimer(cam.moveDelay, 1, () =>
            //{
            //    Quaternion rot = Quaternion.FromToRotation(playerCam.transform.forward, staticCam.transform.forward);
            //    Quaternion targetRot = rot * transform.rotation;
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 180);

            //    staticCam.gameObject.SetActive(false);
            //    playerCam.gameObject.SetActive(true);
            //});
        //};
    }
    private IEnumerator ControlVariable()
    {
        float timer = 0f;
        float totalDuration = 0.6f;
        float increaseDuration = 0.6f; // 快速增加的时长

        while (timer < totalDuration)
        {
            timer += Time.deltaTime;
            if (timer <= increaseDuration)
            {
                // 前面0.5秒内按线性关系快速增加
                lensDistortion.intensity.Override(Mathf.Lerp(0f, -1f, timer / increaseDuration));
            }
            else
            {
                //// 剩下的时间内平滑降低到0
                //float t = (timer - increaseDuration) / (totalDuration - increaseDuration);
                //lensDistortion.intensity.Override(Mathf.Lerp(-1, 0f, t));
            }

            // 可以在这里根据 currentValue 执行其他逻辑，例如更新 UI

            yield return new WaitForEndOfFrame();
        }

        // 确保在结束后 currentValue 为0
        lensDistortion.intensity.Override(0);
    }
    private void Update()
    {
        //    if (OVRInput.GetDown(OVRInput.Button.One))
        //    {
        //        if (curSelecting != null)
        //        {
        //            //Vector3 dir = (curSelecting.transform.position - transform.position).normalized;
        //            //panel.transform.position = dir * 0.2f + transform.position + Vector3.up;
        //            //panel.transform.rotation = Quaternion.LookRotation(dir);
        //            panel.gameObject.SetActive(true);
        //        }
        //    }

        //    if (curSelecting == null)
        //    {
        //        panel.gameObject.SetActive(false);
        //    }

        skyMat.SetFloat("_Rotation", skyMat.GetFloat("_Rotation") + Time.deltaTime);
        TimerManager.instance.Loop(Time.deltaTime);
    }
}
