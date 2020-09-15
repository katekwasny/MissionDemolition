using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot s;

    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8.0f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (s == null) return Vector3.zero;
            return s.launchPos;
        }
    }

     void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);

    }

    void Awake()
    {
        s = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;         
    }
    void OnMouseDown()
    {
        //Player has pressed the mouse button while over slingshot
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        //start the projectile at launch point
        projectile.transform.position = launchPos;
        //set projectile to isKinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    void Update()
    {
        //if slingshot is not in aiming Mode don't run this code
        if (!aimingMode) return;

        //get the current mouse poistion in 2d screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        //Limit mouseDelta to the radius of the slingshot spherecollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0))
        {
            //the mouse button has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;

            FollowCam.POI = projectile;

            projectile = null;

            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }

    }


}
