using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
namespace RPG.Movement
{

    public class Mover : MonoBehaviour , IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed =5f;


        NavMeshAgent navMeshAgent;
        Ray lastRay;
        ActionScheduler scheduler;
        Health health;
        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            scheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {            //Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
            navMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            scheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01( speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
            print("mover cancel action called");
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);

        }
    }

}