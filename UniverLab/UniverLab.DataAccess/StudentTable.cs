using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverLab.Models;

namespace UniverLab.DataAccess
{
    public class StudentTable
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _providerFactory;

        public StudentTable()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["appConnection"]
                .ConnectionString;
            _providerFactory = DbProviderFactories
                .GetFactory(ConfigurationManager
                .ConnectionStrings["appConnection"]
                .ProviderName);
        }

        public List<Student> GetAll()
        {
            var data = new List<Student>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    command.CommandText = "select * from Students";

                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string fullName = dataReader["FullName"].ToString();
                        int groupId = (int)dataReader["GroupId"];

                        data.Add(new Student
                        {
                            Id = id,
                            FullName = fullName,
                            GroupId = groupId
                        });
                    }
                    dataReader.Close();
                }
                catch(DbException exeption)
                {
                    Console.WriteLine($"Ошибка: {exeption.Message}");
                    throw;
                }
                catch (Exception exeption)
                {
                    Console.WriteLine($"Ошибка: {exeption.Message}");
                    throw;
                }
            }
            return data;
        }

        public void Add(Student student)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                DbTransaction transaction = null;

                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.Transaction = transaction;
                    command.CommandText = $"insert into Students values (@fullName, @groupId)";

                    DbParameter fullNameParametr = command.CreateParameter();
                    fullNameParametr.ParameterName = "@fullName";
                    fullNameParametr.DbType = System.Data.DbType.String;
                    fullNameParametr.Value = student.FullName;

                    DbParameter groupIdParametr = command.CreateParameter();
                    groupIdParametr.ParameterName = "@groupId";
                    groupIdParametr.DbType = System.Data.DbType.Int32;
                    groupIdParametr.Value = student.GroupId;

                    command.Parameters.AddRange(new DbParameter[] { fullNameParametr, groupIdParametr });

                    var affectedRows = command.ExecuteNonQuery();
                    if (affectedRows < 1)
                        throw new Exception("Вставка не удалась");
                    transaction.Commit();
                }
                catch (DbException exeption)
                {
                    Console.WriteLine($"Ошибка: {exeption.Message}");
                    transaction?.Rollback();
                    throw;
                }
                catch (Exception exeption)
                {
                    Console.WriteLine($"Ошибка: {exeption.Message}");
                    transaction?.Rollback();
                    throw;
                }
                finally
                {
                    transaction?.Dispose();
                }
            }
        }

        public void Delete(Guid studentId)
        {
            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                DbTransaction transaction = null;

                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    command.Transaction = transaction;
                    command.CommandText = $"delete from Student where id = @studentId";

                    DbParameter studentIdParametr = command.CreateParameter();
                    studentIdParametr.ParameterName = "@studentId";
                    studentIdParametr.DbType = System.Data.DbType.String;
                    studentIdParametr.Value = studentId;

                    command.Parameters.AddRange(new DbParameter[] { studentIdParametr});

                    var affectedRows = command.ExecuteNonQuery();
                    if (affectedRows < 1)
                        throw new Exception("Ошибка при удалении");
                    transaction.Commit();
                }
                catch (DbException exeption)
                {
                    Console.WriteLine($"Ошибка: {exeption.Message}");
                    transaction?.Rollback();
                    throw;
                }
                catch (Exception exeption)
                {
                    Console.WriteLine($"Ошибка: {exeption.Message}");
                    transaction?.Rollback();
                    throw;
                }
                finally
                {
                    transaction?.Dispose();
                }
            }
        }
    }
}
