using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{


    public class Fighter : MonoBehaviour , IAction
    {
        [SerializeField] float weaponRange = 2f;
        Transform target;
        Mover mover;
        ActionScheduler scheduler;

        public void Start()
        {
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
        }
        // Start is called before the first frame update
        public void Attack( CombatTarget combatTarget)
        {
            scheduler.StartAction(this);
            target = combatTarget.transform;
            print("I am gonna hit you in your ball");
        }

        // Update is called once per frame
        private void Update()
        {
            if (target == null) return;

            if ( !GetIsInRange())
            {
                mover.MoveTo(target.position);
                //mover.StartMoveAction(target.position);
                //scheduler.StartAction( this);
            }
            else
            {
                GetComponent<Mover>().Cancel();
            }
        }

        private bool GetIsInRange()
        {
            // otra opcion Vector3.Distance(transform, target) <weaponRange
            //(transform.position - target.position).sqrMagnitude > weaponRange
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Cancel()
        {
            print("cancel method called from figther");
            target = null;
        }
    }

}
