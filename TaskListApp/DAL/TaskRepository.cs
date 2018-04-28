using MongoDB.Bson;
using MongoDB.Driver;
using TaskListApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;

namespace TaskListApp
{
    public class TaskRepository : IDisposable
    {
        private bool disposed = false;
  
        private readonly string userName;
        private readonly string host;
        private readonly string password;
        
        private const string dbName = "Tasks";
        private const string collectionName = "TasksList";
    
        public TaskRepository()
        {
            var appSettings = ConfigurationManager.AppSettings;
            userName = appSettings["cosmoUserName"];
            host = appSettings["cosmoHost"];
            password = appSettings["cosmoPassword"];

        }

        // Gets all Task items from the MongoDB server.        
        public List<MyTask> GetAllTasks()
        {
            try
            {
                var collection = GetTasksCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<MyTask>();
            }
        }

        // Creates a Task and inserts it into the collection in MongoDB.
        public void CreateTask(MyTask task)
        {
            var collection = GetTasksCollectionForEdit();
            try
            {
                collection.InsertOne(task);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        private IMongoCollection<MyTask> GetTasksCollection()
        {
            var client = new MongoClient(GetMongoClientSettings());
            var database = client.GetDatabase(dbName);

            var todoTaskCollection = database.GetCollection<MyTask>(collectionName);
            return todoTaskCollection;
        }

        private IMongoCollection<MyTask> GetTasksCollectionForEdit()
        {
            var client = new MongoClient(GetMongoClientSettings());
            var database = client.GetDatabase(dbName);

            var todoTaskCollection = database.GetCollection<MyTask>(collectionName);
            return todoTaskCollection;
        }

        private MongoClientSettings GetMongoClientSettings()
        {
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(host, 10255),
                UseSsl = true,
                SslSettings = new SslSettings
                {
                    EnabledSslProtocols = SslProtocols.Tls12
                }
            };

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            return settings;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }

            disposed = true;
        }

        # endregion
    }
}