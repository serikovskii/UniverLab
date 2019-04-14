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
    public class ProfessorTable
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _providerFactory;

        public ProfessorTable()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["appConnection"]
                .ConnectionString;
            _providerFactory = DbProviderFactories
                .GetFactory(ConfigurationManager
                .ConnectionStrings["appConnection"]
                .ProviderName);
        }

        public List<Professor> GetAll()
        {
            var data = new List<Professor>();

            using (var connection = _providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = _connectionString;
                    connection.Open();

                    command.CommandText = "select * from Professors";

                    var dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int id = (int)dataReader["Id"];
                        string fullName = dataReader["FullName"].ToString();

                        data.Add(new Professor
                        {
                            Id = id,
                            FullName = fullName
                        });
                    }
                    dataReader.Close();
                }
                catch (DbException exeption)
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

        public void Add(Professor professor)
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
                    command.CommandText = $"insert into Professors values (@fullName)";

                    DbParameter fullNameParametr = command.CreateParameter();
                    fullNameParametr.ParameterName = "@fullName";
                    fullNameParametr.DbType = System.Data.DbType.String;
                    fullNameParametr.Value = professor.FullName;
                    
                    command.Parameters.AddRange(new DbParameter[] { fullNameParametr });

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

        public void Delete(Guid professorId)
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
                    command.CommandText = $"delete from Professor where id = @professorId";

                    DbParameter professorIdParametr = command.CreateParameter();
                    professorIdParametr.ParameterName = "@professorId";
                    professorIdParametr.DbType = System.Data.DbType.String;
                    professorIdParametr.Value = professorId;

                    command.Parameters.AddRange(new DbParameter[] { professorIdParametr });

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
