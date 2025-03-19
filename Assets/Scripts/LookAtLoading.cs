using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtLoading : MonoBehaviour
{
    public MeshRenderer lookAtLoading;
    public Transform spot;
    public bool isCD = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCD) return;
        Physics.SphereCast(transform.position, 1f, transform.forward, out RaycastHit hit, 10000, LayerMask.GetMask("tele"));
        if(hit.collider != null && hit.transform.CompareTag("telespot"))
        {
            lookAtLoading.gameObject.SetActive(true);
            //Debug.LogWarning(hit.transform.position - transform.position);
            Vector3 des = (transform.position - hit.point).normalized * 0.2f;
            //des.z = 0.2f;
            lookAtLoading.transform.position = hit.point + des;
            if(hit.transform  == spot)
            {
                lookAtLoading.material.SetFloat("_Progress", lookAtLoading.material.GetFloat("_Progress") + Time.deltaTime);
                if(lookAtLoading.material.GetFloat("_Progress") >= 1)
                {
                    Player.Instance.SwitchCameraSpot(hit.transform.GetComponent<CameraSwitchSpot>());
                    lookAtLoading.material.SetFloat("_Progress", 0);
                    spot = null;
                    isCD = true;
                    TimerManager.instance.CreateAndStartTimer(3, 1, () => { isCD = false; });
                }
            }
            else
            {
                lookAtLoading.material.SetFloat("_Progress", 0);
                spot = hit.transform;
            }
        }
        else
        {
            lookAtLoading.gameObject.SetActive(false);
            lookAtLoading.material.SetFloat("_Progress", 0);
            spot = null;
        }
    }
}
