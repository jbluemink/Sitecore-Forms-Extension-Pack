using NUnit.Framework;
using Sitecore.Data;
using Sitecore.FakeDb;
using Stockpick.Forms.Feature.ExperienceForms.Model.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockpick.Forms.Feature.ExperienceForms.Tests.Models
{
    [TestFixture]
    public class HiddenViewModelTests : HiddenViewModel
    {

        [Test]
        public void InitItemProperties_ValidItem_SetsValue()
        {

            var hiddenViewModel = new HiddenViewModelTests();

            var id = ID.NewID;
            var hiddenFieldDbItem = new DbItem("Hidden", id);
            hiddenFieldDbItem.Add(new DbField("Value", Constants.Templates.Content.Hidden.Fields.Value) { Value = "RandomValue" });

            using (var db = new Db { hiddenFieldDbItem })
            {
                var hiddenItem = db.Database.GetItem(id);
                hiddenViewModel.InitItemProperties(hiddenItem);
            }

            Assert.That(hiddenViewModel.Value, Is.SameAs("RandomValue"));

        }
    }
}
