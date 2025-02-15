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
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayEnding += this.OnDayEnded;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised when a new in-game day ends.</summary>
        private void OnDayEnded(object sender, DayEndingEventArgs e)
        {
            string CurrentYear = Game1.year.ToString();
            foreach (Item item in Game1.player.team.luauIngredients)
            {
                string luauTopic = $"Y{CurrentYear}_LuauIngredient_{item.ItemId}";
                Game1.player.activeDialogueEvents.Add(luauTopic, 2);
            }
            foreach (Item item in Game1.player.team.grangeDisplay)
            {
                string grangeTopic = $"Y{CurrentYear}_GrangeDisplay_{item.ItemId}";
                Game1.player.activeDialogueEvents.Add(grangeTopic, 2);
            }
        }
    }
}