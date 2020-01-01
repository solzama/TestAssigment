using System;
using Xunit;
using Payment_Validation.Controllers;


/* 
all fields are provided DONE
card owner field does not have credit card numbers DONE
credit card is not expired DONE
number is valid for specified credit card type DONE
CVC is valid for specified credit card type. DONE
API should return credit card type in case of success DONE
API should return all validation errors in case of failure.
        */
namespace UnitTests
{
    public class ValidationControllerTests
    {
        private readonly ValidationController _validationController;
        
        public ValidationControllerTests()
        {
            _validationController = new ValidationController();
        }
     
        [Fact]
        public void false_is_returned_in_case_of_empty_fields()
        {
            var tOwner = "Sasha";
            var tCardNumber = "";
            var tIssueDateMonth = "12";
            var tIssueDateYear = "2017";
            var tExpiryDateMonth = "12";
            var tExpiryDateYear = "2021";
            var tCVC = "123";

            var result = _validationController.MainCheck(tOwner, tCardNumber, tIssueDateMonth, tIssueDateYear, tExpiryDateMonth,
                tExpiryDateYear, tCVC);
            Assert.Equal(false, result);
        }
        
        [Fact]
        public void correct_error_message_is_returned_in_case_of_empty_card_number_field()
        {
            var tOwner = "Sasha";
            var tCardNumber = "";
            var tIssueDateMonth = "12";
            var tIssueDateYear = "2017";
            var tExpiryDateMonth = "12";
            var tExpiryDateYear = "2021";
            var tCVC = "123";

            var result = _validationController.MainCheck(tOwner, tCardNumber, tIssueDateMonth, tIssueDateYear, tExpiryDateMonth,
                tExpiryDateYear, tCVC);
            Assert.Equal(" Empty fields are not allowed. Invalid card type was provided.", _validationController.error_message);

        }
        
        [Fact]
        public void card_owner_string_has_to_contain_only_letters()
        {
            var tOwner = "Sasha4050";
            var result = _validationController.CheckOwnerString(tOwner);
            Assert.Equal(false, result);
        }
        
        [Fact]
        public void three_digit_cvc_for_American_Express_returns_false()
        {
            var tCVC = "454";
            var tCardType = "American Express";
            var result = _validationController.cvcCheck(tCVC, tCardType);
            Assert.Equal(false, result);
        }

        [Fact]
        public void error_message_is_generated_if_card_expired()
        {
            var tExpiryMonth = "12";
            var tExpiryYear = "2018";
            _validationController.CheckExpiryDate(tExpiryMonth, tExpiryYear);
            Assert.Equal(" Card expired.", _validationController.error_message);

        }
        
        [Fact]
        public void method_returns_card_type_if_card_number_is_valid()
        {
            var tCardNumber = "3400 0000 0000 009";
            var result = _validationController.CardTypeCheck(tCardNumber);
            Assert.Equal("American Express", result);
        }
        
        [Fact]
        public void method_returns_empty_string_if_card_number_is_not_valid()
        {
            var tCardNumber = "1200 0000 0000 009";
            var result = _validationController.CardTypeCheck(tCardNumber);
            Assert.Equal("", result);
        }
        
        [Fact]
        public void all_errors_returned_in_case_of_failure()
        {
            var tOwner = "Sasha123";
            var tIssueDateMonth = "12a";
            var tIssueDateYear = "";
            var tExpiryDateMonth = "12";
            var tExpiryDateYear = "2018";
            var tCVC = "12344";
            var tCardNumber = "1200 0000 0000 009";
            var result = _validationController.MainCheck(tOwner, tCardNumber, tIssueDateMonth, tIssueDateYear, tExpiryDateMonth,
                tExpiryDateYear, tCVC);
            Assert.Equal("Empty fields are not allowed. Invalid owner name was provided. Invalid dates were provided. Invalid card type was provided.", _validationController.error_message);
        }
        
        [Fact]
        public void method_returns_empty_string_if_card_number_contains_non_digit_characters() //though spaces are allowed, they are removed
        {
            var tCardNumber = "4111 1111 1111 11aa";
            var result = _validationController.CardTypeCheck(tCardNumber);
            Assert.Equal("", result);
        }
    }
}