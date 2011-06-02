using UpdateControls.Correspondence.POXClient;

namespace FacetedWorlds.ThoughtCloud
{
    public class POXConfigurationProvider : IPOXConfigurationProvider
    {
        public POXConfiguration Configuration
        {
            get
            {
                string address = "http://api.facetedworlds.com/correspondence_server_web/pox";
                string apiKey = "B22E33EB0ABD46FE9161BF4FB8748A65";
                return new POXConfiguration(address, "FacetedWorlds.ThoughtCloud", apiKey, 30);
            }
        }
    }
}
