using System;
using Sprinterviews;
using BepInEx;
using MonoMod.RuntimeDetour.HookGen;
using UnityEngine.Networking;
using UnityEngine;
using System.Runtime.CompilerServices;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Sprinterviews.Patches
{
    public class SprintPatch
    {


        internal static void Init()
        {

            On.PlayerController.Start += On_PlayerController_Start;
            On.PlayerController.Update  += On_PlayerController_Update;
            IL.PlayerController.Update += PlayerController_Update;

        }

        private static void On_PlayerController_Start(On.PlayerController.orig_Start orig, PlayerController self)
        {
            // Call the Trampoline for the Original method or another method in the Detour Chain if any exist
            orig(self);

            // Set maxStamina, sprintMultiplier, and staminaReActivationThreshold to match config values
            self.maxStamina                     = Config.MaxStamina.Value;
            self.sprintMultiplier               = Config.SprintMultiplier.Value;
            self.staminaReActivationThreshold   = Config.StaminaReactivationThreshold.Value * Config.MaxStamina.Value;

            // Force currentStamina to match config value
            Player.localPlayer.data.currentStamina = Config.MaxStamina.Value;
        }

        private static void On_PlayerController_Update(On.PlayerController.orig_Update orig, PlayerController self)
        {
            // Call the Trampoline for the Original method or another method in the Detour Chain if any exist
            orig(self);

            // Force staminaRegRate and canSprintInAnyDirection to match config value
            self.staminaRegRate = Config.StaminaRegen.Value;
            self.canSprintInAnyDirection = Config.CanSprintAnyDirection.Value;
        }

        /*
         * Injects the stamina regen code inPlayerController.Update with the unused staminaRegRate field
         * so that we can manipulate it from elsewhere
         */
        private static void PlayerController_Update(ILContext il)
        {

            // Create ILCursor to find the correct IL statements in the original method
            ILCursor c = new (il);

            // Search for the unique IF statement that directly precedes stamina regen logic
            c.GotoNext(
                MoveType.After,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<PlayerController>("player"),
                x => x.MatchLdfld<Player>("data"),
                x => x.MatchLdfld<Player.PlayerData>("sinceSprint")
                );
            
            // Jump cursor ahead so we can inject new Opcodes AFTER Time.deltaTime()
            c.Index += 12;

            // Add `this.staminaRegRate` to the stack
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldfld, typeof(PlayerController).GetField("staminaRegRate"));

            // Add Mul Opcode so that `this.staminaRegRate` and `Time.deltaTime()` are multiplied together
            c.Emit(
                OpCodes.Mul);

        }
    }
}
