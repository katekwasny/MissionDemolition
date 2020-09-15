using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    [Header("Set in Inspector")]
    public GameObject cloud;
    public int numCloudMin = 6;
    public int numCloudMax = 10;
    public Vector3 cloudOffsetScale = new Vector3(5, 2, 1);
    public Vector2 cloudScaleRangeX = new Vector2(4, 8);
    public Vector2 cloudScaleRangeY = new Vector2(3, 1);
    public Vector2 cloudScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;

    private List<GameObject> clouds;

    // Start is called before the first frame update
    void Start()
    {
        clouds = new List<GameObject>();

        int num = Random.Range(numCloudMin, numCloudMax);
        for(int i = 0; i < num; i++ )
        {
            GameObject cl = Instantiate<GameObject>(cloud);
            clouds.Add(cl);
            Transform clTrans = cl.transform;
            clTrans.SetParent(this.transform);

            //randomly assign a position

            Vector3 offset = Random.insideUnitSphere;
            offset.x *= cloudOffsetScale.x;
            offset.y *= cloudOffsetScale.y;
            offset.z *= cloudOffsetScale.z;
            clTrans.localPosition = offset;

            //randomly assign scale

            Vector3 scale = Vector3.one;
            scale.x = Random.Range(cloudScaleRangeX.x, cloudScaleRangeX.y);
            scale.y = Random.Range(cloudScaleRangeY.x, cloudScaleRangeY.y);
            scale.z = Random.Range(cloudScaleRangeZ.x, cloudScaleRangeZ.y);

            //adjusy y scale by x distance from core
            scale.y *= 1 - (Mathf.Abs(offset.x) / cloudOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            clTrans.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Restart();
        //}
    }

    void Restart()
    {
        //clear out old clouds
        foreach(GameObject cl in clouds)
        {
            Destroy(cl);
        }

        Start();
    }
}
