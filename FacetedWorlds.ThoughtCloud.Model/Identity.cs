
namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Identity
    {
        public Cloud NewCloud()
        {
            return Community.AddFact(new Cloud(this));
        }

        public void NewShare(Cloud cloud)
        {
            Community.AddFact(new Share(this, cloud));
        }
    }
}
