namespace Gossip.Helpers
{
    public static class GroupValidator
    {
        public static bool ValidateGroup(int sender, int receiver)
        {
            if (sender > receiver)
                return true;
            return false;
        }
    }
}