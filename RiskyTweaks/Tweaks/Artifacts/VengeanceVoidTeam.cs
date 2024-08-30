using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace RiskyTweaks.Tweaks.Artifacts
{
    public class VengeanceVoidTeam : TweakBase<VengeanceVoidTeam>
    {
        public override string ConfigCategoryString => "Artifacts - Vengeance";

        public override string ConfigOptionName => "(Server-Side) Void Team";

        public override string ConfigDescriptionString => "Vengeance Umbras are a part of the Void team.";

        public override bool StopLoadOnConfigDisable => true;

        protected override void ApplyChanges()
        {
            IL.RoR2.Artifacts.DoppelgangerInvasionManager.CreateDoppelganger += DoppelgangerInvasionManager_CreateDoppelganger;
        }

        private void DoppelgangerInvasionManager_CreateDoppelganger(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchCallvirt<DirectorCore>("TrySpawnObject")
                ))
            {
                c.EmitDelegate<Func<DirectorSpawnRequest, DirectorSpawnRequest>>(spawnRequest =>
                {
                    spawnRequest.teamIndexOverride = new TeamIndex?(TeamIndex.Void);
                    return spawnRequest;
                });
            }
            else
            {
                UnityEngine.Debug.LogError("RiskyTweaks: VengeanceVoidTeam IL Hook failed");
            }
        }
    }
}
