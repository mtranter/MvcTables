using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MvcTables.Tests
{
    [TestClass]
    public class InstanceToIndexExpressionMaker_Tests
    {
        [TestMethod]
        public void ExpressionReturnsSelf()
        {
            var newExp = InstanceToIndexExpressionMaker<Model>.Replace(0, m => m);
            var func = newExp.Compile();
            var retval = func(new[] { new Model() { Name = "Mark" } });

            Assert.AreEqual("Mark", retval.Name);
        }

        [TestMethod]
        public void ExpressionReturnsProperty()
        {
            var newExp = InstanceToIndexExpressionMaker<Model>.Replace(0, m => m.Name);
            var func = newExp.Compile();
            var retval = func(new[] {new Model() {Name = "Mark"}});

            Assert.AreEqual("Mark", retval);
        }

        [TestMethod]
        public void ExpressionReturnsMethodCall()
        {
            var newExp = InstanceToIndexExpressionMaker<Model>.Replace(0, m => m.DoSomething());
            var func = newExp.Compile();
            var retval = func(new[] { new Model() });

            Assert.AreEqual("Call", retval);
        }

        [TestMethod]
        public void ExpressionReturnsChildProperty()
        {
            var newExp = InstanceToIndexExpressionMaker<Model>.Replace(0, m => m.Child.Name);
            var func = newExp.Compile();
            var retval = func(new[] { new Model(){Child = new ChildModel(){ Name = "Chris"}} });

            Assert.AreEqual("Chris", retval);
        }


        [TestMethod]
        public void ExpressionUsesParentService()
        {
            var service = new SomeService();
            var newExp = InstanceToIndexExpressionMaker<Model>.Replace(0, m => service.GetSomething(m));
            var func = newExp.Compile();
            var retval = func(new[] { new Model() { Id = 7 } });

            Assert.AreEqual(7, retval);
        }
    }

    interface ISomeService
    {
        int GetSomething(Model model);
    }   

    class SomeService : ISomeService
    {
        public int GetSomething(Model model)
        {
            return model.Id;
        }
    }


    class Model
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public ChildModel Child { get; set; }

        public string DoSomething()
        {
            return "Call";
        }
    
    }

    public class ChildModel
    {
        public string Name { get; set; }

        public int[] Years { get; set; }
    }

    
}
