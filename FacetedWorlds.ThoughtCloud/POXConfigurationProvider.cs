using System.Linq;
using UpdateControls.Fields;
using UpdateControls.Correspondence.POXClient;
using FacetedWorlds.ThoughtCloud.Models;

namespace FacetedWorlds.ThoughtCloud
{
    public class POXConfigurationProvider : IPOXConfigurationProvider
    {
        public POXConfiguration Configuration
        {
            get
            {
                string address = "https://api.facetedworlds.com:9443/correspondence_server_web/pox";
                string apiKey = "B22E33EB0ABD46FE9161BF4FB8748A65";
                return new POXConfiguration(address, "FacetedWorlds.ThoughtCloud", apiKey);
            }
        }
    }
}
