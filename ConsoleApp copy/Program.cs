using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using ConsoleUI;
using DAL;
using GameEngine;
using MenuSystem;

namespace ConsoleApp
{
    class Program
    {
        private static GameSettings _settings = new GameSettings();
     
        
        private CellState[,] Board { get; set; }
        static void Main(string[] args)
        {
            
            SetDefaultSettings();
            Console.Clear();
            //_settings = GameConfigHandler.LoadConfig();
            
            Console.WriteLine($"Hello to {_settings.GameName}!");
            Console.WriteLine();
            
            var savedGamesMenu = new Menu(1)
            {
                Title = $"Choose the {_settings.GameName} game to continue with",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
            };
            
            List<string> savedGames = new List<string>();
            
            using (var ctx = new AppDbContext())
            {
                foreach (var gameSave in ctx.GameSettingses
                    .Where(g => g.GameSettingsId != 1))
                {
                    if (gameSave.SaveName != null) {savedGames.Add(gameSave.SaveName);}
                }
            };

            int i = 1;
            foreach (var save in savedGames)
            {
                savedGamesMenu.MenuItemsDictionary.Add($"{i}", new MenuItem() { Title = save, CommandToExecute = () 
                        =>
                    {
                        _settings = LoadSavedGame(save);
                        return TestGame();
                    }
                });
                i++;
            }

            var gameMenu = new Menu(1)
            {
                Title = $"Start a new game of {_settings.GameName}",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Start the game",
                            CommandToExecute = NormalRun
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Title = "Start from Saved Settings",
                            //CommandToExecute = RunFromFile
                            CommandToExecute = savedGamesMenu.Run
                        }
                    },
                    {
                        "3", new MenuItem()
                        {
                            Title = "Add custom game settings",
                            CommandToExecute = SaveSettings
                        }
                    }

                }
            };
            
            var menu0 = new Menu(0)
            {
                Title = $"{_settings.GameName} Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "S", new MenuItem()
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    }
                }
            };
            
            menu0.Run();
        }
        
        static string SaveSettings()
        {
            Console.Clear();
                
            var boardWidth = 0;
            var boardHeight = 0;
            var userCanc = false;
            string s;

            (boardWidth, userCanc, s) = Game.GetUserIntInput("Enter board width", 3, 20, 0);
            if (userCanc) return "";

            (boardHeight, userCanc, s) = Game.GetUserIntInput("Enter board height", 3, 20, 0);
            if (userCanc) return "";


            _settings.GameSettingsId = 1;
            _settings.BoardWidth = boardWidth;
            _settings.BoardHeight = boardHeight;

            using (var ctx = new AppDbContext())
            {
                InsertDataToDb(ctx);
            }
            
            //GameConfigHandler.SaveConfig(Settings);
            
            return "";
        }
        static void InsertDataToDb(AppDbContext ctx)
        {
            if (_settings.GameSettingsId == 1)
            {
                var defSettings = ctx.GameSettingses.First(item => item.GameSettingsId == 1);
                defSettings.BoardHeight = _settings.BoardHeight;
                defSettings.BoardWidth = _settings.BoardWidth;
                ctx.GameSettingses.Update(defSettings);
            }
            else
            {
                ctx.GameSettingses.Add(new GameSettings()
                {
                    BoardHeight = _settings.BoardHeight,
                    BoardWidth = _settings.BoardWidth,
                    SaveName = _settings.SaveName,
                    SerializedBoard = _settings.SerializedBoard
                });
            }
            
            ctx.SaveChanges();
        }
        static void SetDefaultSettings()
        {
            using (var ctx = new AppDbContext())
            {
                if (!ctx.GameSettingses.Any(item => item.GameSettingsId == 1))
                {
                    ctx.Add(new GameSettings()
                    {
                        BoardHeight = 4,
                        BoardWidth = 4,
                        GameName = "Connect4"
                    });
                    ctx.SaveChanges();
                }
                
                _settings = ctx.GameSettingses.First(item => item.GameSettingsId == 1);
            }
        }
       /* static string RunFromFile()
        {
             Game game = new Game(_settings);
             var board = game.GetBoard();
             int i = 0, j = 0;
             if (System.IO.File.Exists("test.json"))
                {
                    var jsonString = System.IO.File.ReadAllText("test.json");
                    var options = new JsonDocumentOptions
                    {
                        AllowTrailingCommas = true
                    };
                    using (JsonDocument document = JsonDocument.Parse(jsonString, options))
                    {
                        foreach (JsonElement element in document.RootElement.EnumerateArray())
                        {
                            string val = element.GetProperty("value").ToString();
                            switch (val)
                            {
                                case "Y":
                                    board[i, j] = CellState.Y;
                                    break;
                                case "R":
                                    board[i, j] = CellState.R;
                                    break;
                                case "Empty":
                                    board[i, j] = CellState.Empty;
                                    break;
                                default:
                                    board[i, j] = CellState.Empty;
                                    break;
                            }
                            if(j == game.BoardWidth - 1)
                            {
                                i++;
                                j = 0;
                            }
                            else
                            {
                                j++;
                            }
                        }
                    }
                    var done = false;
                    game.Board = board;
                    TestGame();
                    return "Do Test";
                }
             return "";
        }*/
        static string NormalRun()
        {
            Console.Clear();
            var boardWidth = 0;
            var boardHeight = 0;
            var userCanc = false;
            var gameName = "";
            var nameEntered = false;
            string s;

            (boardWidth, userCanc, s) = Game.GetUserIntInput("Enter board width", 3, 20, 0);
            if (userCanc) return "";

            (boardHeight, userCanc, s) = Game.GetUserIntInput("Enter board height", 3, 20, 0);
            if (userCanc) return "";

            do
            {
                Console.WriteLine("Enter Game Name");
                gameName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(gameName))
                {
                    _settings.GameName = gameName;
                    nameEntered = true;
                }
                else
                {
                    Console.WriteLine("Game name can not be empty!");
                }

            } while (nameEntered == false);
          
            _settings.BoardHeight = boardHeight;
            _settings.BoardWidth = boardWidth;
            GameConfigHandler.SaveConfig(_settings);
            var game = new Game(_settings);
            TestGame();
            return "";
        }
        static string TestGame()
        {
            var game = new Game(_settings);
            var done = false;
            do
            {
                Console.Clear();
                GameUI.PrintBoard(game);

                var userXint = 0;
                var Yint = _settings.BoardHeight - 1;

                var userCanceled = false;
                string save = "";
                (userXint, userCanceled, save) = Game.GetUserIntInput("Enter X coordinate or enter S to save", 1, _settings.BoardWidth, 0);
                
                if (userCanceled)
                {
                    done = true;
                }
                else
                {
                    if (save == "s")
                    {
                        done = false;
                        System.IO.File.CreateText("test.json").Write("");
                        Translate(game);
                    }
                    else
                    {
                        if (userXint > _settings.BoardWidth || userXint < 0)
                        {
                            Console.WriteLine("X is not valid");
                            
                        }
                        else
                        {
                            done = game.Move(Yint, userXint - 1);
                        }
                    }
                    
                }
            } while (!done);
            return "GAME OVER!!";
        }

        static void Translate(Game game)
        {
            var board = game.GetBoard();
            OurData[,] arrayOfObj = new OurData[game.BoardWidth, game.BoardHeight];
            
            for (var i = 0; i < game.BoardWidth; i++)
            {
                for (var j = 0; j < game.BoardHeight; j++)
                {
                    arrayOfObj[i, j] = new OurData();
                    arrayOfObj[i, j].xPos = i;
                    arrayOfObj[i, j].yPos = j;
                    
                    arrayOfObj[i, j].value = board[i, j].ToString();
                    GameUI.WriteToFile(arrayOfObj[i, j], game);
                }
            }
        }
        public static GameSettings LoadSavedGame(string gameName)
        {
            GameSettings res;

            using (var ctx = new AppDbContext())
            {
                var gameSettings = ctx.GameSettingses.First(item => item.SaveName == gameName);
                
                if (gameSettings == null)
                {
                    res = ctx.GameSettingses.First(item => item.GameSettingsId == 1);
                    res.SaveName = "No game found with the following name. The new game is started";
                }
                else
                {
                    gameSettings.StartingBoard = JsonConvert.DeserializeObject<CellState[,]>(gameSettings.SerializedBoard);
                    res = gameSettings;
                }
            }

            return res;
        }
    }
}