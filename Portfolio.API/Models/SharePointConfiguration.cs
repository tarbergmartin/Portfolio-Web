using Microsoft.SharePoint.Client;

namespace Portfolio.API.Models
{
    public class SharePointConfiguration
    {
        public SharePointOnlineCredentials Credentials { get; set; }
        public string TargetSite { get; set; }
    }
}
