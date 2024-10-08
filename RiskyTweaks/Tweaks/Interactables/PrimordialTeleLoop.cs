﻿using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace RiskyTweaks.Tweaks.Interactables
{
    public class PrimordialTeleLoop : TweakBase<PrimordialTeleLoop>
    {
        public override string ConfigCategoryString => "Interactables";

        public override string ConfigOptionName => "(Server-Side) Primordial Teleporter After Loop";

        public override string ConfigDescriptionString => "Replaces the Teleporter with the Primordial Teleporter after looping, like in RoR1.";

        private static InteractableSpawnCard teleCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/Teleporters/iscTeleporter.asset").WaitForCompletion();
        private static InteractableSpawnCard primordialTeleCard = Addressables.LoadAssetAsync<InteractableSpawnCard>("RoR2/Base/Teleporters/iscLunarTeleporter.asset").WaitForCompletion();
        private static bool isNaturalLunar = false;
        private static bool firstIdleToActive = true;

        protected override void ApplyChanges()
        {
            LegacyResourcesAPI.Load<GameObject>("prefabs/networkedobjects/teleporters/LunarTeleporterProngs").AddComponent<PrimordialTeleInitialState>();
            IL.RoR2.SceneDirector.PlaceTeleporter += SceneDirector_PlaceTeleporter;
            On.RoR2.Stage.Start += Stage_Start;
            On.EntityStates.LunarTeleporter.IdleToActive.OnExit += IdleToActive_OnExit;
        }

        private void IdleToActive_OnExit(On.EntityStates.LunarTeleporter.IdleToActive.orig_OnExit orig, EntityStates.LunarTeleporter.IdleToActive self)
        {
            if (ModCompat.RiskyArtifactsCompat.IsPrimacyActive())
            {
                orig(self);
                return;
            }

            if (!firstIdleToActive || isNaturalLunar)
            {
                firstIdleToActive = false;
                orig(self);
            }
            else
            {
                firstIdleToActive = false;
            }
        }

        private System.Collections.IEnumerator Stage_Start(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            firstIdleToActive = true;
            return orig(self);
        }

        private void SceneDirector_PlaceTeleporter(MonoMod.Cil.ILContext il)
        {
            bool error = true;
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                 x => x.MatchLdfld<SceneDirector>("teleporterSpawnCard")
                ))
            {
                if (c.TryGotoNext(
                     x => x.MatchLdfld<SceneDirector>("teleporterSpawnCard")
                    ))
                {
                    c.Index++;
                    c.EmitDelegate<Func<SpawnCard, SpawnCard>>(origTeleporter =>
                    {
                        if (ModCompat.RiskyArtifactsCompat.IsPrimacyActive()) return origTeleporter;
                        bool isNormalTele = origTeleporter == teleCard;
                        isNaturalLunar = !isNormalTele;

                        if ((Run.instance.stageClearCount >= 5) && isNormalTele)
                        {
                            origTeleporter = primordialTeleCard;
                        }
                        return origTeleporter;
                    });
                    error = false;
                }
            }

            if (error)
            {
                Debug.LogError("RiskyTweaks: PrimordialTeleLoop IL hook failed.");
            }
        }

        public class PrimordialTeleInitialState : MonoBehaviour
        {
            public void Start()
            {
                if (ModCompat.RiskyArtifactsCompat.IsPrimacyActive())
                {
                    Destroy(this);
                    return;
                }

                if (NetworkServer.active)
                {
                    if (!PrimordialTeleLoop.isNaturalLunar)
                    {
                        EntityStateMachine esm = base.GetComponent<EntityStateMachine>();
                        esm.SetNextState(new EntityStates.LunarTeleporter.Idle());
                    }
                }
                Destroy(this);
            }
        }
    }
}
