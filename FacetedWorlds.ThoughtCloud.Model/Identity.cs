
namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Identity
    {
        public Cloud NewCloud()
        {
            return Community.AddFact(new Cloud(this));
        }
    }
}
