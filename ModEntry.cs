using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace FestivalFeedback
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        public List<Item> ffGrangeItems = new List<Item>();

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayEnding += this.OnDayEnded;
            helper.Events.Display.MenuChanged += this.MenuChanged;
            helper.Events.Player.Warped += this.Warped;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised when player inventory changes. Records grange items to ffGrangeItems list for later retrieval.</summary>
        private void MenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (Game1.CurrentEvent != null && Game1.CurrentEvent.isSpecificFestival("fall16"))
            {
                if (Game1.CurrentEvent.grangeScore == -1000) // Grange has not yet been judged
                {
                    this.ffGrangeItems.Clear();
                    foreach (Item g in Game1.player.team.grangeDisplay)
                    {
                        if (g != null)
                        {
                            //this.Monitor.Log($"Grange Display item {g.ItemId}", LogLevel.Debug);
                            this.ffGrangeItems.Add(g);
                        }
                    }
                }
            }
        }

        /// <summary>Raised when a new in-game day ends. Sets activeDialogueEvents based on luauIngredients or ffGrangeItems.</summary>
        private void OnDayEnded(object sender, DayEndingEventArgs e)
        {
            string CurrentYear = Game1.year.ToString();
            if (Game1.IsSummer && Game1.dayOfMonth == 11)  // Luau
            {
                foreach (Item item in Game1.player.team.luauIngredients)
                {
                    if (item != null)
                    {
                        string luauTopic = $"FestivalFeedback_Y{CurrentYear}_LuauIngredient_{item.ItemId}";
                        string luauTopicWithQuality = $"FestivalFeedback_Y{CurrentYear}_LuauIngredient_{item.ItemId}_Q{item.Quality}";
                        if (!Game1.player.hasSeenActiveDialogueEvent(luauTopic))
                        {
                            Game1.player.activeDialogueEvents.Add(luauTopic, 2);
                        }
                        if (!Game1.player.hasSeenActiveDialogueEvent(luauTopicWithQuality))
                        {
                            Game1.player.activeDialogueEvents.Add(luauTopicWithQuality, 2);
                        }
                    }
                }
            }
            if (Game1.IsFall && Game1.dayOfMonth == 16)  // Fair
            {
                foreach (Item gitem in this.ffGrangeItems)
                {
                    if (gitem != null)
                    {
                        string grangeTopic = $"FestivalFeedback_Y{CurrentYear}_GrangeDisplay_{gitem.ItemId}";
                        string grangeTopicWithQuality = $"FestivalFeedback_Y{CurrentYear}_GrangeDisplay_{gitem.ItemId}_Q{gitem.Quality}";
                        if (!Game1.player.hasSeenActiveDialogueEvent(grangeTopic))
                        {
                            Game1.player.activeDialogueEvents.Add(grangeTopic, 2);
                        }
                        if (!Game1.player.hasSeenActiveDialogueEvent(grangeTopicWithQuality))
                        {
                            Game1.player.activeDialogueEvents.Add(grangeTopicWithQuality, 2);
                        }
                    }
                }
                this.ffGrangeItems.Clear();
            }
        }

        /// <summary>
        /// Raised when player warps to new location. Debugger to verify that ffGrangeItems has been populated properly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Warped(object sender, WarpedEventArgs e)
        {
            // Restrict to only proc when player warps from Festival to Farm on festival days
            if (Game1.IsSummer && Game1.dayOfMonth == 11 && e.OldLocation.Name == "Temp" && e.NewLocation.Name == "Farm")
            {
                string lidList = string.Join(", ", Game1.player.team.luauIngredients.Select(l => l.itemId));
                this.Monitor.Log($"Recorded Luau items {lidList}", LogLevel.Debug);
            }
            if (Game1.IsFall && Game1.dayOfMonth == 16 && e.OldLocation.Name == "Temp" && e.NewLocation.Name == "Farm")
            {
                string gidList = string.Join(", ", this.ffGrangeItems.Select(g => g.ItemId));
                this.Monitor.Log($"Recorded Grange items {gidList}", LogLevel.Debug);
            }
        }
    }
}