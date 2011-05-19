using System.Linq;
using FacetedWorlds.ThoughtCloud.Model;
using FacetedWorlds.ThoughtCloud.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;

namespace FacetedWorlds.ThoughtCloud.UnitTest
{
    [TestClass]
    public class HomeViewModelTest
    {
        private Community _community;
        private Identity _identity;
        private HomeViewModel _viewModel;

        [TestInitialize]
        public void Initialize()
        {
            _community = new Community(new MemoryStorageStrategy())
                .Register<CorrespondenceModel>();

            _identity = _community.AddFact(new Identity("mike"));
            _viewModel = new HomeViewModel(_identity);
        }

        [TestMethod]
        public void CanAddACloud()
        {
            _viewModel.AddCloud.Execute(null);
            Assert.AreEqual(1, _viewModel.Clouds.Count());
        }
    }
}
