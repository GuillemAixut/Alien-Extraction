using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YmirEngine;


enum EnemyState
{
    Idle,
    Moving,
    Attacking,
    Dead
}

public enum WanderState
{
    REACHED,
    GOING,
    CHASING,
    ATTACK,
    HIT,
    STOPED
}

public class Enemy: YmirComponent 
{

        protected PathFinding agent;
        public GameObject player = null;

        public Health healthScript;
        public float movementSpeed;

        protected WanderState wanderState;
        public float life = 100f;

        //This may change depending on enemy rarity
        public float armor = 0;

        //0 = Common, 1 = Rare, 2 = Elite
        public int rarity = 0;

        public float wanderRange = 10f;

        public float detectionRadius = 60f;

        public float xSpeed = 0, ySpeed = 0;


    public void LookAt(Vector3 pointToLook)
        {

            Vector3 direction = pointToLook - gameObject.transform.globalPosition;
            direction = direction.normalized;
            float angle = (float)Math.Atan2(direction.x, direction.z);

            //Debug.Log("Desired angle: " + (angle * Mathf.Rad2Deg).ToString());

            if (Math.Abs(angle * Mathf.Rad2Deg) < 1.0f)
                return;

            Quaternion dir = Quaternion.RotateAroundAxis(Vector3.up, angle);

            float rotationSpeed = Time.deltaTime * agent.angularSpeed;


            Quaternion desiredRotation = Quaternion.Slerp(gameObject.transform.localRotation, dir, rotationSpeed);

            gameObject.SetRotation(desiredRotation);
        }


        public void MoveToCalculatedPos(float speed)
        {
            Vector3 pos = gameObject.transform.globalPosition;
            Vector3 destination = agent.GetDestination();
            Vector3 direction = destination - pos;

            gameObject.SetVelocity(direction.normalized * speed);
        }

        public bool CheckDistance(Vector3 first, Vector3 second, float checkRadius)
        {
            float deltaX = Math.Abs(first.x - second.x);
            float deltaY = Math.Abs(first.y - second.y);
            float deltaZ = Math.Abs(first.z - second.z);

            return deltaX <= checkRadius && deltaY <= checkRadius && deltaZ <= checkRadius;
        }
    public void DestroyEnemy()
    {
        Audio.PlayAudio(gameObject, "FH_Death");
        InternalCalls.Destroy(gameObject);
    }

    public void IsReached(Vector3 position, Vector3 destintion)
    {
        Vector3 roundedPosition = new Vector3(Mathf.Round(position.x),
                                                                    0,
                                                Mathf.Round(position.z));

        Vector3 roundedDestination = new Vector3(Mathf.Round(destintion.x),
                                                                            0,
                                                    Mathf.Round(destintion.z));


        if ((roundedPosition.x == roundedDestination.x) && (roundedPosition.y == roundedDestination.y) && (roundedPosition.z == roundedDestination.z))
        {
            wanderState = WanderState.REACHED;  
            Debug.Log("Reached!!!!");

        }
    }
        

}

