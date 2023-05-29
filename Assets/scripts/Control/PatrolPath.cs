using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control { 
    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 1f;
        Transform nextChild;
        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {

                nextChild = transform.GetChild(getNext(i));
               Gizmos.color = Color.white;
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
                Gizmos.DrawLine(transform.GetChild(i).position, nextChild.position);

            }
        }

        public int getNext(int i) {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 getCurrentWaypoint(int i)
        {

            return transform.GetChild(i).position;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
