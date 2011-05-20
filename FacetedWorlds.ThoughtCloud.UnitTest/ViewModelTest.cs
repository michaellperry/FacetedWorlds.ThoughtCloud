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
            Cloud cloud = _community.AddFact(new Cloud(_identity));
            Thought thought = _community.AddFact(new Thought(cloud));
            cloud.CentralThought = thought;
            _cloudViewModel = new CloudViewModel(cloud);
        }

        [TestMethod]
        public void InitialThoughtIsMyThought()
        {
            ThoughtViewModel thought = _cloudViewModel.Thoughts.Single();
            Assert.AreEqual("My thought", thought.Text);
        }

        [TestMethod]
        public void CanChangeThoughtText()
        {
            ThoughtViewModel thought = _cloudViewModel.Thoughts.Single();
            thought.Text = "New thought";
            Assert.AreEqual("New thought", thought.Text);
        }

        [TestMethod]
        public void CanCreateANewThought()
        {
            _cloudViewModel.NewThought.Execute(null);
            Assert.AreEqual(2, _cloudViewModel.Thoughts.Count());
        }
    }
}