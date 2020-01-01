using System;
using GameEngine;

namespace MenuSystem
{
    public class MenuItem
    {
        private string _description = default!;

        public string Description
        {
            get => _description;
            set => _description = Validate(value, 1, 100, false);
        }
        
        public Func<string>? CommandToExecute { get; set; }

        public static string Validate(string item, int minLength, int maxLength, bool toUpper)
        {
            item = item.Trim();
            if (toUpper)
            {
                item = item.ToUpper();
            }

            if (item.Length < minLength  || item.Length > maxLength)
            {
                if (minLength != maxLength)
                {
                    throw new ArgumentException(
                        $"String is not correct length (" +
                        $"{minLength}-{maxLength} symbols)! Got " +
                        $"{item.Length} characters.");
                }
                else
                {
                    throw new ArgumentException(
                        $"String is not correct length (" +
                        $"{minLength} symbol(s))! Got " +
                        $"{item.Length} characters.");
                }
            }
            
            return item;
        }

        public override string ToString()
        {
            return Description;
        }
    }
}