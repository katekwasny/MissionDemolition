using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{

    [Header("Set in Inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; //minimum scale of each cloud
    public float cloudScaleMax = 3; //max scale of each cloud
    public float cloudSpeedMult = 0.5f; //adjust speed of clouds

    private GameObject[] cloudInstances;

    private void Awake()
    {
        //make an array large enough to hold all the cloud instances
        cloudInstances = new GameObject[numClouds];

        //find the cloudanchor parent gameobject
        GameObject anchor = GameObject.Find("CloudAnchor");

        //iterate through and make clouds
        GameObject cloud;
        for(int i = 0; i < numClouds; i++)
        {
            //make an instance of cloudprefab
            cloud = Instantiate<GameObject>(cloudPrefab);

            //position cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            //scale cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            //smaller clouds (with smaller scaleU) should be nearer the ground
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            //smaller clouds should be farther away
            cPos.z = 100 - 90 * scaleU;

            //apply these transformations to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            //make cloud a chilg of the anchor
            cloud.transform.SetParent(anchor.transform);

            //add the cloud to cloudInstances
            cloudInstances[i] = cloud;
      
           
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //iterate over each cloud that was created
           foreach(GameObject cloud in cloudInstances)
        {
            //get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            //move larger clouds faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            //if a cloud has moved too far to the left
            if(cPos.x <= cloudPosMin.x)
            {
                //move it to the far right
                cPos.x = cloudPosMax.x;
            }

            //apply the new position to cloud
            cloud.transform.position = cPos;
        }
    }
}
