using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Exceptions;

namespace AdvancedDevSample.Test.Domain.Entities
{
    public class SupplierTests
    {
        [Fact]
        public void Constructor_Should_Throw_When_Name_Is_Empty()
        {
            Assert.Throws<DomainException>(() => new Supplier(Guid.NewGuid(), ""));
        }

        [Fact]
        public void Rename_Should_Update_Name_When_Valid()
        {
            var supplier = new Supplier(Guid.NewGuid(), "Old Name");

            supplier.Rename("New Name");

            Assert.Equal("New Name", supplier.Name);
        }

        [Fact]
        public void Deactivate_And_Activate_Should_Toggle_IsActive()
        {
            var supplier = new Supplier(Guid.NewGuid(), "Fournisseur", true);

            supplier.Deactivate();
            Assert.False(supplier.IsActive);

            supplier.Activate();
            Assert.True(supplier.IsActive);
        }
    }
}

