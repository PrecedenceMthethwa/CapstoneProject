using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using StudentTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Azure;
using System.Threading.Tasks;

namespace StudentBussines
{
    public class StudentBusiness
    {
        public List<StudentEntity>GetAllStudents(string tableName)
        {
            var table = GetTableReference(tableName);

            //var returnList = new List<CustomerEntity>();

            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<StudentEntity> tableQuery = new TableQuery<StudentEntity>();

            // Retrieve a segment (up to 1,000 entities).
            var tableQueryResult = table.ExecuteQuery(tableQuery);

            List<StudentEntity> returnlist = new List<StudentEntity>(tableQueryResult);

            return returnlist;
        }

        //Only return specific properties
        public List<string> GetStudentEmails(string tableName)
        {
            CloudTable table = GetTableReference(tableName);
            // Define the query, and select only the Email property. 

            //Specify you just want a property called email
            TableQuery<DynamicTableEntity> projectionQuery = new TableQuery<DynamicTableEntity>().Select(new string[] { "FirstName, PhoneNumber" });

            //Run a query for specific data
            //TableQuery<DynamicTableEntity> projectionQuery = new TableQuery<DynamicTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Cassim"));

            // Define an entity resolver to work with the entity after retrieval. 
            // Used for converting directly from table entity data to a client object type without requiring a separate table entity class type that deserializes every property individually
            // This is a delegate function.
            EntityResolver<string> resolver = (pk, rk, ts, props, etag) =>
                $"{props["FirstName"].StringValue},{props["PhoneNumber"].StringValue}";

            var returnlist = table.ExecuteQuery(projectionQuery, resolver, null, null);

            return new List<string>(returnlist);
        }
        //Search customer by name
        public List<StudentEntity> GetStudentByName(string tableName, string name)
        {

            var table = GetTableReference(tableName);

            var query = (from client in table.CreateQuery<StudentEntity>().Execute()
                         where client.Email.Contains(name)
                         select client);


            return new List<StudentEntity>(query);
        }


        //Get all customers but run Async
        public async Task<List<StudentEntity>> GetAllCustomersAsync(string tableName)
        {
            var table = GetTableReference(tableName);

            var returnList = new List<StudentEntity>();

            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<StudentEntity> tableQuery = new TableQuery<StudentEntity>();

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                TableQuerySegment<StudentEntity> tableQueryResult =
                    await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                List<StudentEntity> alist = new List<StudentEntity>(tableQueryResult.Results.ToArray());
                returnList.AddRange(alist);

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);

            return await Task.FromResult(returnList);
        }

        public void InsertCustomer(string tableName, StudentEntity studentEntity)
        {
            var table = GetTableReference(tableName);

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(studentEntity);

            // Execute the insert operation.
            table.Execute(insertOperation);
        }

        private static CloudTable GetTableReference(string tablename)
        {
            CloudTableClient tableClient = GetContext();

            // Retrieve reference to a previously created container.
            var table = tableClient.GetTableReference(tablename);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            return table;
        }

        private static CloudTableClient GetContext()
        {
           
            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            return tableClient;
        }
    }
}
