using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using BepInEx.Configuration;

namespace Sprinterviews;

public class Config
{
    public static ConfigFile Configurations = new ConfigFile(Paths.ConfigPath + "\\" + MyPluginInfo.PLUGIN_GUID + ".cfg", true);

    public static ConfigEntry<float>    SprintMultiplier;
    public static ConfigEntry<float>    MaxStamina;
    public static ConfigEntry<float>    StaminaRegen;
    public static ConfigEntry<float>    StaminaReactivationThreshold;
    public static ConfigEntry<bool>     CanSprintAnyDirection;

    public void LoadFile()
    {
        SprintMultiplier                = Configurations.Bind("General", "SprintSpeed", 2f, "Sets the speed bonus sprinting gives you.");
        MaxStamina                      = Configurations.Bind("General", "MaxStamina", 10f, "Sets the maximum amount of stamina you have.");
        StaminaRegen                    = Configurations.Bind("General", "StaminaRegen", 1f, "Sets the stamina regeneration speed. Number is a multiplier. Number can only be an positive number.");
        StaminaReactivationThreshold    = Configurations.Bind("General", "StaminaReactivationThreshold", 0.3f, "Percentage of stamina that must be reached before the player can sprint again.");
        CanSprintAnyDirection           = Configurations.Bind("General", "CanSprintAnyDirection", false, "Sets whether the player can sprint in any direction or not.");

        SprinterviewsCore.Logger.LogInfo($"SprintMultiplier: {SprintMultiplier.Value}");
        SprinterviewsCore.Logger.LogInfo($"MaxStamina: {MaxStamina.Value}");
        SprinterviewsCore.Logger.LogInfo($"StaminaRegen: {StaminaRegen.Value}");
        SprinterviewsCore.Logger.LogInfo($"StaminaReactivationThreshold: {StaminaReactivationThreshold.Value}");
        SprinterviewsCore.Logger.LogInfo($"CanSprintAnyDirection: {CanSprintAnyDirection.Value}");

        Configurations.Save();
    }
}
