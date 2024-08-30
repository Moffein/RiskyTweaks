using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RiskyTweaks.Tweaks.Equipment
{
    public class BFGTendrils : TweakBase<BFGTendrils>
    {
        public override string ConfigCategoryString => "Equipment";

        public override string ConfigOptionName => "(Server-Side) Preon Accumulator - Band QoL";

        public override string ConfigDescriptionString => "Preon Accumulator tendrils deal slightly less damage so they don't proc bands.";

        protected override void ApplyChanges()
        {
            //The description isn't even accurate so changing damage here is fine.
            GameObject projectilePrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/BeamSphere");//.InstantiateClone("RiskyMod_BFG", true);
            ProjectileProximityBeamController pbc = projectilePrefab.GetComponent<ProjectileProximityBeamController>();
            pbc.damageCoefficient = 1.9f;
        }
    }
}
