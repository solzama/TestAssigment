using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Payment_Validation.Models;

namespace Payment_Validation.Controllers
{
    public class ValidationController : Controller
    {
        
        public string error_message { get; private set; }
        bool success = true;
        
      //todo: API should return credit card type in case of success: Master Card, Visa or American Express,
      //todo: API should return all validation errors in case of failure.
        // GET
        
        
        [HttpPost]
        public ActionResult Card(string owner, string cardNumber, string issueDateMonth, string issueDateYear, string expiryDateMonth,string expiryDateYear , string cvc)
        {
            success = MainCheck(owner, cardNumber, issueDateMonth, issueDateYear, expiryDateMonth, expiryDateYear, cvc);
            if (success) //API should return credit card type in case of success: Master Card, Visa or American Express (I also add user name and card number)
            {
                var cardType = CardTypeCheck(cardNumber); //deriving card type 
                var myCard = new Card
                {
                    Owner = owner,
                    CardNumber = cardNumber,
                    IssueMonth = issueDateMonth,
                    IssueYear = issueDateYear,
                    ExpiryMonth = expiryDateMonth,
                    ExpiryYear = expiryDateYear,
                    CVC = cvc,
                    CardType = cardType
                };
                return View("Card", myCard); 
            }
            var error = new Error //API should return all validation errors in case of failure. 
            {
                Message = error_message
            };
            return View("Card_Error", error);
        }

        public bool MainCheck(string owner, string cardNumber, string issueDateMonth, string issueDateYear,
            string expiryDateMonth, string expiryDateYear, string cvc)
        {
            success = CheckValuesNotNull(owner, cardNumber, issueDateMonth, issueDateYear, expiryDateMonth, expiryDateYear, cvc);
            success = CheckOwnerString(owner);
            success = CheckDates(expiryDateMonth, expiryDateYear, issueDateMonth, issueDateYear);
            if (success)
            {
                success = CheckExpiryDate(expiryDateMonth, expiryDateYear);
            }
            var cardType = CardTypeCheck(cardNumber);
            if (cardType != "")
            {
                success = cvcCheck(cvc, cardType);
            }
            else
            {
                error_message = error_message + " Invalid card type was provided.";
                success = false;
            }

            return success;
        }
        public bool CheckOwnerString(string owner) // card owner field does not have credit card numbers (any numbers in my case)
        {
            if (!Regex.IsMatch(owner, @"^[a-zA-Z\s]+$"))
            {
                error_message = error_message + " Invalid owner name was provided.";
                return false;
            }
            return success;
        }
        public bool cvcCheck(string cvc, string card_type) //cvc is valid for particular card type
        {
            if (card_type == "American Express") //cvc for American Express consists of 4 digits 
            {
                if (!Regex.IsMatch(cvc, @"^\d{4}$"))
                {
                    error_message = error_message + " Invalid CVC was provided.";
                    return false;
                }
            }
            else
            {
                if (!Regex.IsMatch(cvc, @"^\d{3}$")) //cvc for Visa and Mastercard consists of 3 digits
                {
                    error_message = error_message + " Invalid CVC was provided.";
                    return false;
                }    
            }

            return success;
        }
        
        public string CardTypeCheck(string card_number) //card number belongs to one of 3 card types
        {
     
        /*American Express cards start with either 34 or 37. 15 digits
         Mastercard numbers begin with 51–55. 16 digits
         Visa cards – Begin with a 4 and have 13 or 16 digits */
        
        card_number = card_number.Replace( " ", "" );
        if (!Regex.IsMatch(card_number, @"^[0-9]*$"))
        {
            return "";
        }
        if ((card_number.StartsWith("34") || card_number.StartsWith("37")) && (card_number.Length == 15))
        {
            return "American Express";
        }
        if ((card_number.StartsWith("51") || card_number.StartsWith("52") || card_number.StartsWith("53")|| card_number.StartsWith("54")|| card_number.StartsWith("55")) && (card_number.Length == 16))
        {
            return "Mastercard";
        }
        if ((card_number.StartsWith("4") && ((card_number.Length == 13) ||card_number.Length == 16) ))
        {
            return "Visa";
        }

        return "";
        }

        public bool CheckDates(string expiration_date_month, string expiration_date_year, string issue_date_month, string issue_date_year) //make sure user provided valid dates
        {
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            if (!monthCheck.IsMatch(expiration_date_month) 
                || !yearCheck.IsMatch(expiration_date_year)
                || !monthCheck.IsMatch(issue_date_month)
                || !yearCheck.IsMatch(issue_date_year))
            {
                error_message = error_message + " Invalid dates were provided.";
                return false;
            }//check date format is valid as "MM/yyyy"
            return success;
        }
        public bool CheckExpiryDate(string expiry_date_month,string expiry_date_year ) //as far as it was not possible to derive whether the card is still valid operating only with issue date//(at least Google could not provide me with answer),// I made a decision to ask user for the expiry date as well  
        {
            var year = int.Parse(expiry_date_year);
            var month = int.Parse(expiry_date_month);            
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get actual expiry date
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);
            if (cardExpiry < DateTime.Now)
            {
                error_message = error_message + " Card expired.";
                return false;
            }
            return success;
        }
       public bool CheckValuesNotNull(string Owner, string cardNumber, string issue_date_month, string issue_date_year, string expiration_date_month,string expiration_date_year , string CVC) //all fields are provided
        {
            if (String.IsNullOrEmpty(Owner) || String.IsNullOrEmpty(cardNumber) 
                                            || String.IsNullOrEmpty(issue_date_month)
                                            || String.IsNullOrEmpty(issue_date_year) 
                                            || String.IsNullOrEmpty(expiration_date_month)
                                            || String.IsNullOrEmpty(expiration_date_year)
                                            || String.IsNullOrEmpty(CVC))
            {
                error_message = error_message + "Empty fields are not allowed.";
                return false;
            }
            return success;
        }
        
    }
}