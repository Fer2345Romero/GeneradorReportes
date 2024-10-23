using DataAccessLayer.DbConnection;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository
    {
        private readonly SqlDataAccess _dbConnection;

        public ProductRepository()
        {
            _dbConnection = new SqlDataAccess();
        }

        public DataTable GetProducts()
        {
            DataTable productTable = new DataTable();

            using ( var connection = _dbConnection.GetConnection())
            {
                string query = @"SELECT TOP 20 ProductID, ProductName, QuantityPerUnit, UnitPrice, UnitsInStock
                             FROM Products";

                SqlCommand commad = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = commad.ExecuteReader();
                productTable.Load(reader);

            }

            return productTable;
        }
    }
}
