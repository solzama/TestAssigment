using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DAL;
using GameEngine;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UserInterface;

namespace Connect4
{
    class Program
    {
        private static GameSettings _settings = new GameSettings();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            SetDefaultSettings();
            
            Console.Clear();
            Console.WriteLine($"Welcome to the {_settings.GameName} game!");
            Console.WriteLine();

            var savedGamesMenu = new Menu(1)
            {
                MenuTitle = $"Choose the {_settings.GameName} game to continue with",
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
                savedGamesMenu.MenuItemsDictionary.Add($"{i}", new MenuItem() { Description = save, CommandToExecute = () 
                    =>
                    {
                        _settings = LoadSavedGame(save);
                        return GameRun();
                    }
                });
                i++;
            }

            var gameOptionMenu = new Menu(1)
            {
                // All game options lead to the same game mode for now
                MenuTitle = $"Choose a {_settings.GameName} game mode",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Description = "Computer starts",
                            CommandToExecute = GameRun
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Description = "Human starts",
                            CommandToExecute = GameRun
                        }
                    },
                    {
                        "3", new MenuItem()
                        {
                            Description = "Human vs. Human",
                            CommandToExecute = GameRun
                        }
                    },
                }
            };

            
            var mainMenu = new Menu(0)
            {
                MenuTitle = $"{_settings.GameName} - Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Description = "Start a new game",
                            CommandToExecute = gameOptionMenu.Run
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Description = "Continue with a saved game",
                            CommandToExecute = savedGamesMenu.Run
                        }
                    },
                    {
                        "3", new MenuItem()
                        {
                            Description = "Add custom game settings",
                            CommandToExecute = SaveSettings
                        }
                    }
                }
            };
            
            mainMenu.Run();
        }

        // Add default GameSettings record into DB with GameSettingsId = 1, if none is present
        // Take Settings = record with GameSettingsId = 1
        static void SetDefaultSettings()
        {
            using (var ctx = new AppDbContext())
            {
                if (!ctx.GameSettingses.Any(item => item.GameSettingsId == 1))
                {
                    ctx.Add(new GameSettings()
                    {
                        BoardHeight = 6,
                        BoardWidth = 7,
                        GameName = "Connect4"
                    });
                    ctx.SaveChanges();
                }
                
                _settings = ctx.GameSettingses.First(item => item.GameSettingsId == 1);
            }
        }

        static string SaveGame(Game game)
        {
            Console.Clear();

            string gameName;
            var exit = false;

            do
            {
                bool userCancelled;
                
                (gameName, userCancelled) =
                    Game.GetUserStringInput("Please input the name for the game to be saved:", 0);

                if (userCancelled) return "";

                if (gameName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) == -1)
                {
                    var time = (DateTime.Now);
                    var timeStamp = time.ToString("yy-MM-dd HH-mm");
                    var jsonBoard = JsonConvert.SerializeObject(game.GetBoard());

                    GameSettings newSave = new GameSettings()
                    {
                        SerializedBoard = jsonBoard,
                        SaveName = gameName + $" {timeStamp}",
                        BoardHeight = _settings.BoardHeight,
                        BoardWidth = _settings.BoardWidth
                    };

                    _settings = newSave;

                    using (var ctx = new AppDbContext())
                    {
                        InsertDataToDb(ctx);
                    }

                    //GameConfigHandler.SaveGame(newSave);
                    exit = true;
                }
            } while (!exit);

            return $"The game {gameName} was successfully saved.";
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

        static string SaveSettings()
        {
            Console.Clear();
                
            int boardWidth;
            int boardHeight;
            bool userCanceled;
            bool menuCall;
                
            (boardWidth, userCanceled, menuCall) = Game.GetUserIntInput("Enter board width", 4, 20, 0);
            if (userCanceled || menuCall) return "";

            (boardHeight, userCanceled, menuCall) = Game.GetUserIntInput("Enter board height", 4, 20, 0);
            if (userCanceled || menuCall) return "";

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
        
        static string GameRun()
        {
            var game = new Game(_settings);
            var colFull = false;

            var done = false;
            do
            {
                Console.Clear();
                GameUI.PrintBoard(game);

                int xIndexInput;
                var endGame = false;
                var menuCall = false;

                if (colFull)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine();
                    Console.WriteLine("This column is already full! Please try again.");
                    Console.ResetColor();
                    colFull = false;
                }
                
                (xIndexInput, endGame, menuCall) = Game.GetUserIntInput("Enter X coordinate:", 1, game.BoardWidth, 0, "S");

                if (endGame)
                {
                    done = true;
                    SetDefaultSettings();
                }
                
                else if (menuCall)
                {
                    Console.WriteLine(SaveGame(game));
                }
                else
                {
                    (endGame, colFull) = game.Move(xIndexInput - 1);
                    if (endGame)
                    {
                        done = true;
                        SetDefaultSettings();
                    }
                }

            } while (!done);
            
            Console.WriteLine();
            Console.WriteLine("***** Game Over! *****");
            
            return "";
        }
    }
}