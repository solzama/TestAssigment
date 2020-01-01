﻿using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public class Menu
    {
        private const string MenuCommandExit = "X";
        private const string MenuCommandReturnToPrevious = "R";
        private const string MenuCommandReturnToMain = "M";

        private int _menuLevel;
        
        public Menu(int menuLevel = 0)
        {
            _menuLevel = menuLevel;
        }

        private Dictionary<string, MenuItem> _menuItemsDictionary = new Dictionary<string, MenuItem>();
        
        public Dictionary<string, MenuItem> MenuItemsDictionary
        {
            get => _menuItemsDictionary;
            set
            {
                _menuItemsDictionary = value;
                if (_menuLevel >= 2)
                {
                    _menuItemsDictionary.Add(MenuCommandReturnToPrevious, 
                        new MenuItem(){ Description = "Return to the Previous Menu" });
                }
                if (_menuLevel >= 1)
                {
                    _menuItemsDictionary.Add(MenuCommandReturnToMain, 
                        new MenuItem(){ Description = "Return to the Main Menu" });
                }
                _menuItemsDictionary.Add(MenuCommandExit, 
                    new MenuItem(){ Description = "Exit" });
            }
        }

        public string MenuTitle { get; set; } = default!;

        public string Run()
        {
            var command = "";
            do
            {
                Console.WriteLine(MenuTitle);
                Console.WriteLine("============================");

                foreach (var menuItem in MenuItemsDictionary)
                {
                    Console.Write(menuItem.Key);
                    Console.Write(" ");
                    Console.WriteLine(menuItem.Value);
                }
                
                Console.WriteLine("-----------------------------");
                Console.Write(">");

                command = Console.ReadLine()?.Trim().ToUpper() ?? "";

                var returnCommand = "";

                if (MenuItemsDictionary.ContainsKey(command))
                {
                    var menuItem = MenuItemsDictionary[command];
                    if (menuItem.CommandToExecute != null)
                    {
                        returnCommand = menuItem.CommandToExecute();
                    }
                }

                switch (returnCommand)
                {
                    case MenuCommandExit:
                        command = MenuCommandExit;
                        break;
                    case MenuCommandReturnToMain:
                    {
                        if (_menuLevel != 0) { command = MenuCommandReturnToMain; }
                        break;
                    }
                }
            } while (command != MenuCommandExit && 
                     command != MenuCommandReturnToMain && 
                     command != MenuCommandReturnToPrevious);
            
            return command;
        }

    }
}