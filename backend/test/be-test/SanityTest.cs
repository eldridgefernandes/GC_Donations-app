using System.Threading.Tasks;
using NUnit.Framework.Legacy;
using Microsoft.Data.Sqlite;

namespace BackendTesting
{
    public class SanityTest
    {
        #region Private Variables

        private SqliteConnection _connection;
        private HttpClient _httpClient;

        #endregion

        #region Constants

        const string BASE_URL = "http://localhost:5050";
        const int DB_ID = 1;
        const string DB_NAME = "John Doe";
        const double DB_AMOUNT = 100.50;
        const string DB_DATE = "2025-09-05";

        #endregion

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BASE_URL);

            _connection = new SqliteConnection("Data Source=../../../../../db/mydatabase.db");
        }

        [Test, Order(1)]
        public async Task Test_PingCheck()
        {
            ClassicAssert.NotNull(await ApiHelper.SendGetRequest(_httpClient, "/ping"));
        }

        [Test, Order(2)]
        public void Test_InsertToDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                _connection.Open();

                string sql = "INSERT INTO donations (id, donor_name, amount, date) VALUES (@id, @donorName, @amount, @date)";
                var command = new SqliteCommand(sql, _connection);

                command.Parameters.AddWithValue("@id", DB_ID);
                command.Parameters.AddWithValue("@donorName", DB_NAME);
                command.Parameters.AddWithValue("@amount", DB_AMOUNT);
                command.Parameters.AddWithValue("@date", DB_DATE);

                int insertedRows = command.ExecuteNonQuery();
                Assert.That(insertedRows == 1);
            });
        }

        [Test, Order(3)]
        public void Test_ReadFromDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                _connection.Open();

                var command = _connection.CreateCommand();
                command.CommandText = "SELECT id, donor_name, amount, date FROM donations WHERE id = " + DB_ID;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        double amount = reader.GetDouble(2);
                        string date = reader.GetString(3);

                        Assert.That(id == DB_ID);
                        Assert.That(name == DB_NAME);
                        Assert.That(amount == DB_AMOUNT);
                        Assert.That(date == DB_DATE);
                    }
                }
            });
        }

        [Test, Order(4)]
        public void Test_DeleteFromDatabase()
        {
            Assert.DoesNotThrow(() =>
            {
                _connection.Open();

                string sql = "DELETE FROM donations WHERE id=@id AND donor_name=@donorName AND amount=@amount AND date=@date";
                var command = new SqliteCommand(sql, _connection);

                command.Parameters.AddWithValue("@id", DB_ID);
                command.Parameters.AddWithValue("@donorName", DB_NAME);
                command.Parameters.AddWithValue("@amount", DB_AMOUNT);
                command.Parameters.AddWithValue("@date", DB_DATE);

                int deletedRows = command.ExecuteNonQuery();
                Assert.That(deletedRows == 1);
            });
        }

        [TearDown]
        public void TearDown()
        {
            _connection?.Dispose();
            _httpClient?.Dispose();
        }
    }
}