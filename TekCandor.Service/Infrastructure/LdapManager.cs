using System.DirectoryServices;

namespace TekCandor.Service.Infrastructure
{
    public interface ILdapValidator
    {
        bool Validate(string userId, string password);
    }

    public class LdapManager : ILdapValidator
    {
        public readonly string DomainName;
        public readonly int PortNumber;

        public LdapManager(string domainName, int port = 389)
        {
            DomainName = domainName;
            PortNumber = port;
        }

        public bool Validate(string userId, string password)
        {
            try
            {
                string path = LdapPath();
                DirectoryEntry de = new DirectoryEntry(path, userId, password, AuthenticationTypes.Secure);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch (DirectoryServicesCOMException)
            {
                return false;
            }
        }

        public string LdapPath()
        {
            return string.Format(@"LDAP://{0}:{1}", DomainName, PortNumber);
        }
    }
}
