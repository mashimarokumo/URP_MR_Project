using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InteractableUnityEventWrapper))]
public class CameraSwitchSpot : MonoBehaviour
{
    [Tooltip("移动后相机的具体位置、旋转和焦距的参照")]
    public Camera cameraData;
    [Tooltip("移动后镜头控制延迟的时间（秒数）")]
    public float moveDelay;

    private void Start()
    {
        if(cameraData == null)
        {
            Debug.LogError("需要给定相机参数！");
        }
        cameraData.enabled = false;
        InteractableUnityEventWrapper interact = GetComponent<InteractableUnityEventWrapper>();
        interact.WhenSelect.AddListener(() => {
            Player.Instance.SwitchCameraSpot(this);
        });
        interact.WhenHover.AddListener(() =>
        {
            Player.Instance.curSelecting = this;
        });
        interact.WhenUnhover.AddListener(() =>
        {
            if(Player.Instance.curSelecting == transform)
            {
                Player.Instance.curSelecting = null;
            }
        });
    }
}
