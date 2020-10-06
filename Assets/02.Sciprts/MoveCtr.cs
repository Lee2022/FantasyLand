using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCtr : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform[] points;
    public enum MoveType
    {
        WAP_POINT,
        LOOK_AT,
        GEAR_VR
    }
    public MoveType moveType = MoveType.WAP_POINT;
    public float speed = 1.0f;
    public float damping = 3.0f;

    private Transform tr;
    private CharacterController cc;
    private Transform camTr;
    private int nextIdx = 1;
    void Start()
    {
        tr = GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
        camTr = Camera.main.GetComponent<Transform>();
        GameObject wayPointGroup = GameObject.Find("WayPointGroup");
        if(wayPointGroup != null)
        {
            points = wayPointGroup.GetComponentsInChildren<Transform>();
        
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveType)
        {
            case MoveType.WAP_POINT:
                MoveWayPoint();
                break;
            case MoveType.LOOK_AT:
                MoveLookAt(1);
                break;
            case MoveType.GEAR_VR:
                break;

        }
    }
    void MoveWayPoint()
    {
        Vector3 direction = points[nextIdx].position - tr.position;
        Quaternion rot = Quaternion.LookRotation(direction);
        tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        tr.Translate(Vector3.forward * Time.deltaTime * speed);

    }
    void MoveLookAt(int facing)
    {
        Vector3 heading = camTr.forward;
        heading.y = 0.0f;
        Debug.DrawRay(tr.position, heading.normalized * 0.1f, Color.red);
        cc.SimpleMove(heading * speed * facing);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("WAY_POINT"))
        {
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    }
}
