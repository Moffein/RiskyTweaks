using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Enemies.Bell
{
    public class FullStun : TweakBase<FullStun>
    {
        public override string ConfigCategoryString => "Enemies - Brass Contraption";

        public override string ConfigOptionName => "(Server-Side) Allow Attack Interrupt";

        public override string ConfigDescriptionString => "Stuns can interrupt the attack chargeup.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            GameObject enemyObject = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/bellbody");

            EntityStateMachine weapon = null;
            EntityStateMachine[] stateMachines = enemyObject.GetComponents<EntityStateMachine>();
            foreach (EntityStateMachine esm in stateMachines)
            {
                switch (esm.customName)
                {
                    case "Weapon":
                        weapon = esm;
                        break;
                    default:
                        break;
                }
            }

            SetStateOnHurt ssoh = enemyObject.GetComponent<SetStateOnHurt>();
            ssoh.idleStateMachine = new EntityStateMachine[] { weapon };
        }
    }
}
