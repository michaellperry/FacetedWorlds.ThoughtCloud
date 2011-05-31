
namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Identity
    {
        public Cloud NewCloud()
        {
            Cloud cloud = Community.AddFact(new Cloud(this));
            NewShare(cloud);
            return cloud;
        }

        public void NewShare(Cloud cloud)
        {
            Community.AddFact(new Share(this, cloud));
        }
    }
}
