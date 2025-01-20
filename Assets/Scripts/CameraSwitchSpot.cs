using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InteractableUnityEventWrapper))]
public class CameraSwitchSpot : MonoBehaviour
{
    [Tooltip("�ƶ�������ľ���λ�á���ת�ͽ���Ĳ���")]
    public Camera cameraData;
    [Tooltip("�ƶ���ͷ�����ӳٵ�ʱ�䣨������")]
    public float moveDelay;

    private void Start()
    {
        if(cameraData == null)
        {
            Debug.LogError("��Ҫ�������������");
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
