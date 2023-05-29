using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float dwellingTime = 3f;
        [SerializeField] Vector3 guardPosition;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] int waypointPositionIndex =0;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        float timeSinceLastSawPlayer ;
        float dwellingTimeSpent;
        // Start is called before the first frame update
        void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
            health = GetComponent<Health>();
            guardPosition = transform.position;
            mover = GetComponent<Mover>();
            timeSinceLastSawPlayer = Mathf.Infinity;
            dwellingTimeSpent = Mathf.Infinity;
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) {
                return;
            }
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                attackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspicionTime)
            {
                suspectBehaviour();
                
            }
            else {
                PatrolBehaviour();
            }
            UpdateTimers();
            
        }

        public void attackBehaviour()
        {
            fighter.Attack(player);
            timeSinceLastSawPlayer = 0;

        }

        public void suspectBehaviour()
        {
            fighter.Cancel();
        }

        public void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            fighter.Cancel();
            if (patrolPath != null)
            {
                
                
                if (atWaypoint())
                {
                    
                        dwellingTimeSpent = 0;
                        cycleWayPoint();
                    
                }
                nextPosition = getCurrentWaypoint();
            }
            if (dwellingTimeSpent > dwellingTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            dwellingTimeSpent += Time.deltaTime;
        }
        private bool atWaypoint()
        {
            //print("distance is: " + Vector3.Distance(transform.position, getCurrentWaypoint()));
            return Vector3.Distance(transform.position, getCurrentWaypoint()) < waypointTolerance;
        }

        private void cycleWayPoint()
        {
            waypointPositionIndex = patrolPath.getNext(waypointPositionIndex);

        }
        private Vector3 getCurrentWaypoint()
        {

            return patrolPath.getCurrentWaypoint(waypointPositionIndex);
        }


        private bool InAttackRangeOfPlayer() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        //called by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position ,chaseDistance);
        }
    }
}
