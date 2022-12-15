using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    public pickUpObjCode objCode;
    
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Controller")    )    {

            if (objCode == pickUpObjCode.TORCH)
            {
                ObjManager.instance.intro.SetActive(false);
            }
            transform.SetParent(other.gameObject.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            BaseServer.instance.SendToClient(new Net_ObjInteraction_MSg(objCode, true));
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
            ObjManager.instance.endVibrationInvoke();
            ObjManager.instance.add(this.gameObject);
            this.enabled = false;
        }

    }
}
