using System.Collections.Generic;
using AutoMoq;
using Moq;
using Xunit;

namespace BFFTDD.xUnit.Tests
{
    public class CustomerServiceTests
    {
        //“As an API user, I want to have the ability to update an existing customer so I can always have the most up-to-date customer information in the system”
        //Acceptance Criteria:
        //An error should be thrown if there are duplicate records
        //If no customer exists for a given ID, customer record should be created
        //    Validation rules
        //    No customers under age of 18
        //Update following fields
        //    First name
        //    Last name
        //    Age

        private readonly CustomerService _testObject;
        private readonly Mock<ICustomerRepository> _repo;

        public CustomerServiceTests()
        {
            //Arrange
            var moqer = new AutoMoqer();

            _testObject = moqer.Create<CustomerService>();
            _repo = moqer.GetMock<ICustomerRepository>();
        }

        [Fact]
        public void Update_Throws_ProcessException_If_Age_Under18()
        {
            //Arrange

            //Act 
            var ex = Assert.Throws<CustomerProcessException>(() => _testObject.Update(new Customer()));

            //Assert
            Assert.NotNull(ex);
            Assert.Equal("Customer is under 18.", ex.Message);
        }

        [Fact]
        public void Update_Sends_Customer_To_Repository()
        {
            var expected = new Customer
            {
                Id = 1,
                Age = 19,
                FirstName = "Foo",
                LastName = "Bar"
            };

            //Arrange
            Customer customerCallback = null;

            _repo.Setup(x => x.UpdateCustomer(It.IsAny<Customer>()))
                .Callback<Customer>(c => customerCallback = c)
                .Returns(true);

            _repo.Setup(x => x.GetCustomer(1)).Returns(new List<Customer> {expected});

         
            //Act
            var result = _testObject.Update(expected);

            //Assert
            Assert.True(result);
            Assert.NotNull(customerCallback);
            Assert.Equal(expected.Id, customerCallback.Id);
            Assert.Equal(expected.FirstName, customerCallback.FirstName);
            Assert.Equal(expected.LastName, customerCallback.LastName);
            Assert.Equal(expected.Age, customerCallback.Age);
        }

        [Fact]
        public void Update_Throws_Exception_On_Duplicate_Record()
        {
            //Arrange
            var customer = new Customer { Id = 1, Age = 19};
            _repo.Setup(x => x.GetCustomer(1))
                .Returns(new List<Customer> {customer, new Customer {Age = 20}});

            //Act
            var ex = Assert.Throws<CustomerProcessException>(() => _testObject.Update(customer));
            
            //Assert
            Assert.NotNull(ex);
            Assert.Equal("Duplicate record found.", ex.Message);
        }
    }
}
