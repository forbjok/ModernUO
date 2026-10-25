using System;
using ModernUO.CodeGeneratedEvents;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.VeteranRewards
{
    public static class ForbSystem
    {
        [OnEvent(nameof(PlayerMobile.PlayerLoginEvent))]
        public static void OnLogin(PlayerMobile pm)
        {
            if (!pm.Alive)
                return;

            // Increase total skill cap to 10000
            pm.SkillsCap = 100000;

            // Increase total stat cap to 400
            pm.StatCap = 400;
        }
    }
}
