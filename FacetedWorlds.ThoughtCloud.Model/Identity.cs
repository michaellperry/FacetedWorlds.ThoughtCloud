
namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Identity
    {
        public Thought NewThought()
        {
            return Community.AddFact(new Thought(this));
        }

        public Cloud NewCloud()
        {
            return Community.AddFact(new Cloud(this));
        }
    }
}
