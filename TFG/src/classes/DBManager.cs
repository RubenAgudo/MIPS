using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG.Properties;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TFG.src.classes
{
	public class DBManager
	{

		private static DBManager myDBManager;
		private MongoClient theClient;
		private MongoServer theServer;

		public String ConnectionString
		{
			get 
			{
				return Settings.Default.ConnectionString;
			}
		}

		private DBManager()
		{
			theClient = new MongoClient(ConnectionString);
			theServer = theClient.GetServer();
		}

		public static DBManager getDBManager()
		{
			if (myDBManager == null)
			{
				myDBManager = new DBManager();
			}

			return myDBManager;
		}

		public void addDocument(BsonDocument aDocument, string theDatabase, string theCollection)
		{
			MongoDatabase myDatabase = theServer.GetDatabase(theDatabase);
			MongoCollection<BsonDocument> aCollection = 
				myDatabase.GetCollection<BsonDocument>(theCollection);
			aCollection.Insert(aDocument);
		} 

	}
}
