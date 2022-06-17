namespace SimAlpha
{
    internal class Program
    {
        static void Main()
        {
            
            if (AuthUser.AuthToken() == true) { DataGen.Data(); }
        }
    }
}
