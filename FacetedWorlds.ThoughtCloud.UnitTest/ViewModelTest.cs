using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;
using FacetedWorlds.ThoughtCloud.Model;
using FacetedWorlds.ThoughtCloud.ViewModel;

namespace FacetedWorlds.ThoughtCloud.UnitTest
{
    [TestClass]
    public class ViewModelTest : SilverlightTest
    {
        private Community _community;
        private Identity _identity;
        private CloudViewModel _cloudViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _community = new Community(new MemoryStorageStrategy())
                .Register<Model.CorrespondenceModel>();

            _identity = _community.AddFact(new Identity("mike"));
            _cloudViewModel = new CloudViewModel();
        }

        [TestMethod]
        public void InitialThoughtIsMyThought()
        {
            ThoughtViewModel thought = _cloudViewModel.Thoughts.Single();
            Assert.AreEqual("My thought", thought.Text);
        }
    }
}