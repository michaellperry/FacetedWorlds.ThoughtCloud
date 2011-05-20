
namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Cloud
    {
        public Thought NewThought()
        {
            return Community.AddFact(new Thought(this));
        }
    }
}
