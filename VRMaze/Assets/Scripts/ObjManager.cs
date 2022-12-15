using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ObjManager instance;
    public List<GameObject> objs = new List<GameObject>();
    public GameObject controllerModel;
    public GameObject intro;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void add(GameObject newObj)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
        objs.Add(newObj);
        controllerModel.SetActive(false);
    }
    public void Reset()
    {
        foreach (GameObject obj in objs)
        {
            Destroy(obj);
        }
        objs = new List<GameObject>();
        controllerModel.SetActive(true);
        intro.SetActive(true);

    }
    public void use(pickUpObjCode useObj)
    {
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<PickUpObj>().objCode == useObj)
                obj.SetActive(true);
            else
                obj.SetActive(false);

        }
    }
    public void endVibrationInvoke()
    {
        Invoke("endVibration", 0.2f);

    }
     void endVibration()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);

    }
}
