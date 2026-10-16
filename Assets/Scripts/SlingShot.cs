using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject prefabProjectile;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public float bandWidth = 0.15f;
    public Material bandMat;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    private Transform rightArm;
    private Transform leftArm;
    // private LineRenderer leftBand;
    // private LineRenderer rightBand;

    private LineRenderer slingshotBand;
    

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");

        launchPoint = launchPointTrans.gameObject;    
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        leftArm = transform.Find("LeftArm");
        rightArm = transform.Find("RightArm");

        slingshotBand = gameObject.AddComponent<LineRenderer>();
        slingshotBand.material = bandMat;
        slingshotBand.startWidth = slingshotBand.endWidth = bandWidth;

        slingshotBand.numCapVertices = 4;
        slingshotBand.useWorldSpace = true;
        slingshotBand.positionCount = 3;

        // leftBand = MakeBandSegment("LeftBand", bandMat);
        // rightBand = MakeBandSegment("RightBand", bandMat);
        UpdateBand(launchPos);
    }

    Vector3 ArmTip(Transform arm) {
      return arm.position + arm.up * arm.lossyScale.y;
    }

    void UpdateBand(Vector3 projPos) {
      slingshotBand.SetPosition(0, ArmTip(leftArm));
      slingshotBand.SetPosition(1, projPos);
      slingshotBand.SetPosition(2, ArmTip(rightArm));
    }

    void Update()
    {
      if (!aimingMode)
      {
        return;
      }

      Vector3 mousePos2D = Input.mousePosition;
      mousePos2D.z = -Camera.main.transform.position.z;
      Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

      Vector3 mouseDelta = mousePos3D - launchPos;

      float maxMagnitude = this.GetComponent<SphereCollider>().radius;

      if (mouseDelta.magnitude > maxMagnitude)
      {
        mouseDelta.Normalize();
        mouseDelta *= maxMagnitude;
      }

      Vector3 projPos = launchPos + mouseDelta;
      projectile.transform.position = projPos;
      UpdateBand(projPos);

      if (Input.GetMouseButtonUp(0))
      {
        aimingMode = false;
        projectileRigidbody.isKinematic = false;
        projectileRigidbody.velocity = -mouseDelta * velocityMult;
        // Switch to slingshot view immediately before setting POI
        FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
        FollowCam.POI = projectile;
        // add ProjectileLine to Projectile
        Instantiate<GameObject>(projLinePrefab, projectile.transform);
        projectile = null;
        MissionDemolition.SHOT_FIRED();
        UpdateBand(launchPos);
      }
    }

    void OnMouseEnter()
    {
      launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
      launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;

        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }
}
