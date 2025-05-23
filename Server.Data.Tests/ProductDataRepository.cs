﻿using Server.Data.API;
using Server.ObjectModels.Data.API;

namespace Server.Data.Tests
{
    [TestClass]
    public class ItemDataRepositoryTests : DataRepositoryTestBase
    {
        [TestMethod]
        public void Add_ShouldAddItemToRepository()
        {
            DummyProduct item = new DummyProduct("TV", 100, 5);

            _repository.AddItem(item);

            Assert.IsTrue(_mockContext.Items.ContainsKey(item.Id));
            Assert.AreEqual(item.Name, _mockContext.Items[item.Id].Name);
        }

        [TestMethod]
        public void Get_ShouldReturnItem_WhenItemExists()
        {
            Guid itemId = Guid.NewGuid();
            DummyProduct item = new DummyProduct(itemId, "TV", 100, 5);
            _mockContext.Items[itemId] = item;

            IProduct? result = _repository.GetItem(itemId);

            Assert.IsNotNull(result);
            Assert.AreEqual(item.Name, result.Name);
        }

        [TestMethod]
        public void Get_ShouldReturnNull_WhenItemDoesNotExist()
        {
            Guid itemId = Guid.NewGuid();

            IProduct? result = _repository.GetItem(itemId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Remove_ShouldRemoveItem_WhenItemExists()
        {
            Guid itemId = Guid.NewGuid();
            DummyProduct item = new DummyProduct(itemId, "TV", 100, 5);
            _mockContext.Items[itemId] = item;

            bool result = _repository.RemoveItem(item);

            Assert.IsTrue(result);
            Assert.IsFalse(_mockContext.Items.ContainsKey(itemId));
        }

        [TestMethod]
        public void Remove_ShouldReturnFalse_WhenItemDoesNotExist()
        {
            DummyProduct item = new DummyProduct(Guid.NewGuid(), "TV", 100, 5);

            bool result = _repository.RemoveItem(item);

            Assert.IsFalse(result);
        }
    }
}
