using System.Collections.Generic;
using Core.Data.Models;
using Core.Interfaces;
using SQLite;
using UnityEngine;

namespace Core.GameSystems.Managers
{
    public class DbManager : ISystem
    {
        private static DbManager Instance;
        private static string Path;
        private static SQLiteConnection Connection;

        private DbManager(){}

        public static DbManager CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new DbManager();
            }
            return Instance;
        }
        public void Initialize()
        {
            if (Connection == null)
            {
                Path = Application.persistentDataPath + "/ManagementGame.db";
                Connection = new SQLiteConnection(Path);
                //Connection.CreateTable<Player>();
            }
        }

        public void Shutdown()
        {
            Connection.Close();
        }

        public void DeletePlayer(Player player)
        {
            Connection.Delete<Player>(player.Id);
        }
        public List<Player> GetPlayers()
        {
            return Connection.Table<Player>().ToList();
        }

        public Player GetPlayer(int id)
        {
            return Connection.Table<Player>().Where(p => p.Id == id).FirstOrDefault();
        }
    }
    
}