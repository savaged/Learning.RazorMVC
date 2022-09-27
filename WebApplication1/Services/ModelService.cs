using Dapper;
using System.Data;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ModelService : IModelService
    {
        private readonly IDbConnection _connection;

        public ModelService(IDbConnection dbConnection)
        {
            _connection = dbConnection ??
                throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<T> Create<T>()
            where T : IModel, new()
        {
            await Task.CompletedTask;
            return new T();
        }

        public async Task<T> Show<T>(int id)
            where T : IModel
        {
            T? model = default;
            _connection.Open();
            try
            {
                model = await _connection.QuerySingleOrDefaultAsync<T>(
                    $"SELECT * FROM {typeof(T).Name} WHERE {nameof(IModel.Id)} = {id};");
            }
            finally
            {
                _connection.Close();
            }
            return model;
        }

        public async Task<IEnumerable<T>> Index<T>()
            where T : IModel
        {
            IEnumerable<T> data = new List<T>();
            _connection.Open();
            try
            {
                data = await _connection.QueryAsync<T>($"SELECT * FROM {typeof(T).Name};");
            }
            finally
            {
                _connection.Close();
            }
            return data;
        }

        public async Task<T> Store<T>(T model)
            where T : IModel
        {
            var sql = $"INSERT INTO {typeof(T).Name} ({GetColumns(model)}) VALUES ({GetParameterNames(model)});";
            _connection.Open();
            try
            {
                await _connection.ExecuteAsync(sql, model);
                model.Id = await _connection.ExecuteScalarAsync<int>(
                    $"SELECT MAX({nameof(IModel.Id)}) FROM {typeof(T).Name};");
            }
            finally
            {
                _connection.Close();
            }
            return model;
        }

        public async Task<T> Edit<T>(int id)
            where T : IModel
        {
            return await Show<T>(id);
        }

        public async Task Update<T>(T model)
            where T : IModel
        {
            var sb = new StringBuilder($"UPDATE {typeof(T).Name} SET ");
            var names = GetPropertyNames(model, true);
            var separator = string.Empty;
            foreach (var name in names)
            {
                sb.Append(separator);
                sb.Append($"{name} = @{name}");
                separator = ", ";
            }
            sb.Append($" WHERE {nameof(IModel.Id)} = {model.Id};");
            var sql = sb.ToString();
            _connection.Open();
            try
            {
                await _connection.ExecuteAsync(sql, model);
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task Destroy<T>(int id)
            where T : IModel
        {
            _connection.Open();
            try
            {
                await _connection.QueryAsync<T>(
                    $"DELETE FROM {typeof(T).Name} WHERE {nameof(IModel.Id)} = {id};");
            }
            finally
            {
                _connection.Close();
            }
        }

        private string GetColumns(IModel model, bool excludeId = true)
        {
            return GetDelimitedPropertyNames(GetPropertyNames(model, excludeId), ", ");
        }

        private string GetParameterNames(IModel model, bool excludeId = true)
        {
            return $"@{GetDelimitedPropertyNames(GetPropertyNames(model, excludeId), ", @")}";
        }

        private string GetDelimitedPropertyNames(
            IList<string> propertyNames, string delimiter)
        {
            var sb = new StringBuilder();
            var separator = string.Empty;
            foreach (var name in propertyNames)
            {
                sb.Append(separator);
                sb.Append(name);
                separator = delimiter;
            }
            return sb.ToString();
        }

        private IList<string> GetPropertyNames(IModel model, bool excludeId)
        {
            IList<string> names = new List<string>();
            foreach (var p in model.GetType().GetProperties())
            {
                if (excludeId && p.Name == nameof(IModel.Id)) continue;
                names.Add(p.Name);
            }
            return names;
        }

    }
}
