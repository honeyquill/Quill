using System;
using ChatCommands;

namespace Quill
{
    public class ChangeTeam : ChatCommand
    {
        public ChangeTeam() : base("team", "Toggles which team you are on", ChangeTeamExecute, 0)
        {
        }

        public static void ChangeTeamExecute(string[] args, string player)
        {
            var actor = BeetleUtils.GetActorByName(player);
            actor._team.Value = (int)actor.Team == (int)Il2Cpp.TeamType.Blue ? (int)Il2Cpp.TeamType.Red : (int)Il2Cpp.TeamType.Blue;
        }
    }
}