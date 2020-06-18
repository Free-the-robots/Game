using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Weapon
{
    public class Abilities
    {
        protected bool running;
        protected bool runningEnd;
        protected float timeRunning;
        protected float timeEnding;

        protected float tActual;
        protected float tActualEnd;
        public Abilities(float t)
        {
            timeRunning = t;
            timeEnding = 0f;
            tActual = 0f;
            tActualEnd = 0f;
            runningEnd = false;
            running = false;
        }
        public Abilities(float t, float tEnd)
        {
            timeRunning = t;
            timeEnding = tEnd;
            tActual = 0f;
            tActualEnd = 0f;
            runningEnd = false;
            running = false;
        }

        public virtual void RunAbility(Transform transform)
        {
            if(!running && !runningEnd)
            {
                tActual = 0f;
                tActualEnd = 0f;
                running = true;
                runningEnd = false;
            }
        }
        public void Update()
        {
            if (running)
            {
                tActual += Time.deltaTime;
                AbilityUpdate();


                if (tActual > timeRunning)
                {
                    running = false;
                    runningEnd = true;
                    AbilityEnd();
                }
            }

            if (runningEnd)
            {
                tActualEnd += Time.deltaTime;
                AbilityEndUpdate();

                if (tActualEnd > timeEnding)
                {
                    running = false;
                    runningEnd = false;
                    AbilityEndEnd();
                }
            }

        }

        /// <summary>
        /// Update when ability is running
        /// order : AbilityUpdate (Update) -> AbilityEnd -> AbilityEndUpdate (Update) -> AbilityEndEnd
        /// </summary>
        protected virtual void AbilityUpdate()
        {

        }

        /// <summary>
        /// End when ability has finished running
        /// order : AbilityUpdate (Update) -> AbilityEnd -> AbilityEndUpdate (Update) -> AbilityEndEnd
        /// </summary>
        protected virtual void AbilityEnd()
        {

        }

        /// <summary>
        /// Update when ability has finished
        /// order : AbilityUpdate (Update) -> AbilityEnd -> AbilityEndUpdate (Update) -> AbilityEndEnd
        /// </summary>
        protected virtual void AbilityEndUpdate()
        {

        }

        /// <summary>
        /// End when ability has finished the finishing update
        /// order : AbilityUpdate (Update) -> AbilityEnd -> AbilityEndUpdate (Update) -> AbilityEndEnd
        /// </summary>
        protected virtual void AbilityEndEnd()
        {

        }
    }

    public class CircleDeath : Abilities
    {
        public CircleDeath() : base(0f)
        {

        }
        public override void RunAbility(Transform transform)
        {
            base.RunAbility(transform);

            List<GameObject> enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (Vector3.Distance(enemies[i].transform.position, transform.position) < 10f)
                    enemies[i].GetComponent<Spaceship>().loseHealth(1000);
            }
        }
    }

    public class SlowMo : Abilities
    {
        float slowTime = 0.3f;
        float timeSlowing = 1f;
        public SlowMo() : base(5f)
        {

        }

        protected override void AbilityUpdate()
        {
            if (tActual < timeSlowing)
                Time.timeScale -= Mathf.SmoothStep(1f, slowTime, tActual / timeSlowing);
            else
                Time.timeScale = slowTime;
        }

        protected override void AbilityEndUpdate()
        {
            if (tActualEnd < timeSlowing)
                Time.timeScale -= Mathf.SmoothStep(slowTime, 1f, tActualEnd / timeSlowing);
            else
                Time.timeScale = 1f;
        }

        protected override void AbilityEndEnd()
        {
            Time.timeScale = 1f;
        }
    }
}
