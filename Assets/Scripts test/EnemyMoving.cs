using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    public Path pathToFollow;
    public int currentWayPointID = 0;
    public float speed = 2;
    public float reachDistance = 0.4f;
    public float rotationSpeed = 5f;

    float distance;
    public bool useBezier;

    public enum EnemyStates
    {
        ON_PATH,
        FLY_IN,
        IDLE
    }

    public EnemyStates enemyStates;

    public int enemyID;
    public Formation formation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(enemyStates)
        {
            case EnemyStates.ON_PATH:
                MoveOnThePath(pathToFollow);
                break;
            case EnemyStates.FLY_IN:
                MoveToFormation();
                break;
        }
        
    }

    void MoveToFormation()
    {
        transform.position = Vector3.MoveTowards(transform.position, formation.GetVector(enemyID), speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, formation.GetVector(enemyID)) <= 0.0001f)
        {
            transform.SetParent(formation.gameObject.transform);
            transform.eulerAngles = Vector3.zero;
            enemyStates = EnemyStates.IDLE;
        }
    }
    void MoveOnThePath(Path path)
    {
        if (useBezier)
        {
            distance = Vector3.Distance(path.bezierObjList[currentWayPointID], transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path.bezierObjList[currentWayPointID], speed * Time.deltaTime);
            /*
            rotation enemy
            var direction = path.bezierObjList[currentWayPointID] - transform.position;
            if (direction != Vector3.zero)
            {
                direction.y = 0;
                direction = direction.normalized;
                var rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }

            var direction = path.bezierObjList[currentWayPointID] - transform.position;
            if (direction != Vector3.zero)
            {
                direction.z = 0f;
                direction = direction.normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            */
        }
        else
        {
            distance = Vector3.Distance(path.pathObjList[currentWayPointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path.pathObjList[currentWayPointID].position, speed * Time.deltaTime);
        }
        
        if(useBezier)
        {
            if (distance <= reachDistance)
            {
                currentWayPointID++;
            }

            if (currentWayPointID >= path.bezierObjList.Count)
            {
                currentWayPointID = 0;
                enemyStates = EnemyStates.FLY_IN;
            }
        }
        else
        {
            if(distance <= reachDistance)
            {
                currentWayPointID++;
            }

            if(currentWayPointID >= path.pathObjList.Count)
            {
                currentWayPointID = 0;
                enemyStates = EnemyStates.FLY_IN;
            }
        }
    }

    public void SpawnSetup(Path path, int ID, Formation _formation)
    {
        pathToFollow = path;
        enemyID = ID;
        formation = _formation;
    }
}
