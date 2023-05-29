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
        Health target;
        Mover mover;
        ActionScheduler scheduler;

        [SerializeField] float timeBetweenAttack = 1f;
        [SerializeField] float weaponDamage = 5f;
        float timeSinceLastAttack = Mathf.Infinity;

        public void Start()
        {
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
        }
        // Start is called before the first frame update
        public void Attack( GameObject combatTarget)
        {
            scheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
            print("I am gonna hit you in your ball");
        }

        // Update is called once per frame
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;

            if (target.IsDead()) { 
                return;
            }

            if ( !GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
                //mover.StartMoveAction(target.position);
                //scheduler.StartAction( this);
            }
            else
            {
                    AttackBehaviour();
            }
        }
        private void AttackBehaviour()
        {
            GetComponent<Mover>().Cancel();
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttack)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
                    //this will trigger the Hit() event
            }
        }
        //animation event
        private void Hit()
        {
            if (target == null) { return; }
            target.TakeDamage(weaponDamage);
        }
        private bool GetIsInRange()
        {
            // otra opcion Vector3.Distance(transform, target) <weaponRange
            //(transform.position - target.position).sqrMagnitude > weaponRange
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) 
            { 
                return false; 
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
            
        }

        public void Cancel()
        {
            StopAttack();
            print("cancel method called from figther");
            target = null;
            GetComponent<Mover>().Cancel();
        }


    }

}
