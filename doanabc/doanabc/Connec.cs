using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace doanabc
{
    class Connec
    {     
        static String connS = "mongodb+srv://tuanphan:01692158894pvt%40@cluster0.mvsug.mongodb.net/admin?retryWrites=true&w=majority";
        static String dbname = "dichvuchamsoc";
        public static IMongoCollection<BsonDocument> getconnectdv()
        {
            var client = new MongoClient(connS);
            var server = client.GetDatabase(dbname);
            var collection = server.GetCollection<BsonDocument>("dv");
            return collection;
        }
        public static IMongoCollection<BsonDocument> getconnectcdv()
        {
            var client = new MongoClient(connS);
            var server = client.GetDatabase(dbname);
            var collection = server.GetCollection<BsonDocument>("cdv");
            return collection;
        }
        public static IMongoCollection<BsonDocument> getconnectnv()
        {
            var client = new MongoClient(connS);
            var server = client.GetDatabase(dbname);
            var collection = server.GetCollection<BsonDocument>("nv");
            return collection;
        }
    }
}
