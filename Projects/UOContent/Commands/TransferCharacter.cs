using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Targeting;

namespace ModernUO.Scripts;

public static class TransferCharacter
{
    public static void Initialize()
    {
        CommandSystem.Register("TransferCharacter", AccessLevel.Administrator, new CommandEventHandler(TransferCharacter_OnCommand));
    }

    [Usage("TransferCharacter <AccountName>")]
    [Description("Transfers the targeted character to the specified account")]
    private static void TransferCharacter_OnCommand(CommandEventArgs e)
    {
        var dstAccountName = e.GetString(0);
        var dstAccount = Accounts.GetAccount(dstAccountName);

        if (dstAccount == null)
        {
            e.Mobile.SendMessage($"Destination account '{dstAccountName}' does not exist.");
            return;
        }

        e.Mobile.SendMessage("Target the player character you wish to transfer.");
        e.Mobile.Target = new TransferTarget(dstAccount);
    }

    private class TransferTarget : Target
    {
        private readonly IAccount _dstAccount;

        public TransferTarget(IAccount dstAccount) : base(-1, false, TargetFlags.None)
        {
            _dstAccount = dstAccount;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is PlayerMobile pm)
            {
                var srcAccount = pm.Account;
                int? srcIndex = null;
                int? dstIndex = null;

                for (int i = 0; i < srcAccount.Length; ++i) {
                    if (srcAccount[i] == pm) {
                        srcIndex = i;
                        break;
                    }
                }

                if (srcIndex is null) {
                    from.SendMessage("Character not found on source account.");
                    return;
                }

                for (int i = 0; i < _dstAccount.Length; ++i) {
                    if (_dstAccount[i] is null) {
                        dstIndex = i;
                        break;
                    }
                }

                if (dstIndex is null) {
                    from.SendMessage("No empty slots on destination account.");
                    return;
                }

                // Update account on character
                pm.Account = _dstAccount;

                // Update account character lists
                _dstAccount[dstIndex.Value] = pm;
                srcAccount[srcIndex.Value] = null;

                from.SendMessage($"Success! Moved {pm.Name} over to account: {_dstAccount.Username}.");
                pm.SendMessage($"Your character has been moved to account: {_dstAccount.Username}.");
            }
            else
            {
                from.SendMessage("Invalid target. You must select a player character.");
            }
        }
    }
}
