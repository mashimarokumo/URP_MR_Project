using DG.Tweening;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public InteractableUnityEventWrapper panel;
    public Camera playerCam;
    public Camera staticCam;
    [HideInInspector]
    public CameraSwitchSpot curSelecting;
    OVRManager ovr;
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
    }

    public void SwitchCameraSpot(CameraSwitchSpot cam)
    {
        transform.DOShakeRotation(0.5f, 1, 10).onComplete += () =>
        {
            transform.SetParent(cam.transform);
            playerCam.gameObject.SetActive(false);
            staticCam.gameObject.SetActive(true);
            
            //playerCam.focalLength = cam.cameraData.focalLength;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            //staticCam.focalLength = cam.cameraData.focalLength;

            Vector3 offset = cam.transform.position - playerCam.transform.position;
            transform.position += offset;

            Quaternion rot = Quaternion.FromToRotation(playerCam.transform.forward, cam.transform.forward);
            Quaternion targetRot = rot * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 20);

            staticCam.transform.position = playerCam.transform.position;
            staticCam.transform.rotation = playerCam.transform.rotation;

            TimerManager.instance.CreateAndStartTimer(cam.moveDelay, 1, () =>
            {
                Quaternion rot = Quaternion.FromToRotation(playerCam.transform.forward, staticCam.transform.forward);
                Quaternion targetRot = rot * transform.rotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 180);

                staticCam.gameObject.SetActive(false);
                playerCam.gameObject.SetActive(true);
            });
        };
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (curSelecting != null)
            {
                //Vector3 dir = (curSelecting.transform.position - transform.position).normalized;
                //panel.transform.position = dir * 0.2f + transform.position + Vector3.up;
                //panel.transform.rotation = Quaternion.LookRotation(dir);
                panel.gameObject.SetActive(true);
            }
        }

        if (curSelecting == null)
        {
            panel.gameObject.SetActive(false);
        }


        TimerManager.instance.Loop(Time.deltaTime);
    }
}
