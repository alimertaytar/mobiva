using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.mobiva
{
    public class ContextApiHelper
    {
        private readonly DbContext _context;
        private readonly string _logDirectory;

        public ContextApiHelper(DbContext context, string logDirectory = "Logs")
        {
            _context = context;
            _logDirectory = logDirectory;
            Directory.CreateDirectory(_logDirectory);
        }

        public async Task<OperationResult<int>> SaveAsync<T>(T entity) where T : class
        {
            try
            {
                var dbSet = _context.Set<T>();
                var idProp = typeof(T).GetProperty("Id");
                if (idProp == null)
                    return OperationResult<int>.Fail("Entity'nin 'Id' property'si yok.");

                var idValue = (int)idProp.GetValue(entity);
                var changes = new List<ChangeLogEntry>();
                string operation;

                if (idValue == 0)
                {
                    dbSet.Add(entity);
                    operation = "INSERT";
                }
                else
                {
                    var existingEntity = await dbSet.AsNoTracking()
                        .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == idValue);

                    if (existingEntity == null)
                        return OperationResult<int>.Fail("Güncellenecek kayıt bulunamadı.");

                    dbSet.Attach(entity);
                    operation = "UPDATE";

                    foreach (var prop in typeof(T).GetProperties())
                    {
                        if (!prop.CanWrite || (!prop.PropertyType.IsValueType && prop.PropertyType != typeof(string)))
                            continue;

                        var oldValue = prop.GetValue(existingEntity);
                        var newValue = prop.GetValue(entity);

                        if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                        {
                            var dt = (DateTime?)newValue;
                            if (!dt.HasValue || dt.Value < new DateTime(1753, 1, 1))
                                continue;
                        }

                        if (!Equals(oldValue, newValue))
                        {
                            _context.Entry(entity).Property(prop.Name).IsModified = true;

                            changes.Add(new ChangeLogEntry
                            {
                                Property = prop.Name,
                                OldValue = oldValue?.ToString(),
                                NewValue = newValue?.ToString()
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();

                var updatedId = (int)idProp.GetValue(entity);

                var log = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = operation,
                    Entity = typeof(T).Name,
                    EntityId = updatedId,
                    Changes = changes
                };

                await LogToJsonFileAsync(log);

                return OperationResult<int>.Ok(updatedId, "Kayıt işlemi başarılı.");
            }
            catch (Exception ex)
            {
                var errorLog = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = "ERROR",
                    Entity = typeof(T).Name,
                    Error = ex.Message
                };

                await LogToJsonFileAsync(errorLog);
                return OperationResult<int>.Fail("Hata: " + ex.Message);
            }
        }

        public async Task<OperationResult<bool>> DeleteAsync<T>(int id) where T : class
        {
            try
            {
                var dbSet = _context.Set<T>();
                var entity = await dbSet.FindAsync(id);

                if (entity == null)
                    return OperationResult<bool>.Fail("Silinecek kayıt bulunamadı.");

                dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                var log = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = "DELETE",
                    Entity = typeof(T).Name,
                    EntityId = id
                };

                await LogToJsonFileAsync(log);

                return OperationResult<bool>.Ok(true, "Kayıt silindi.");
            }
            catch (Exception ex)
            {
                await LogToJsonFileAsync(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Operation = "ERROR",
                    Entity = typeof(T).Name,
                    Error = ex.Message
                });

                return OperationResult<bool>.Fail("Silme hatası: " + ex.Message);
            }
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<List<T>> GetAllAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            return await _context.Set<T>().Where(filter).ToListAsync();
        }
        private async Task LogToJsonFileAsync(LogEntry logEntry)
        {
            var fileName = Path.Combine(_logDirectory, $"log-{DateTime.Now:yyyy-MM-dd}.json");

            List<LogEntry> existingLogs = new List<LogEntry>();

            if (File.Exists(fileName))
            {
                var json = await File.ReadAllTextAsync(fileName);
                if (!string.IsNullOrWhiteSpace(json))
                    existingLogs = JsonSerializer.Deserialize<List<LogEntry>>(json) ?? new List<LogEntry>();
            }

            existingLogs.Add(logEntry);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(existingLogs, options);
            await File.WriteAllTextAsync(fileName, updatedJson);
        }

        // İçerik tipleri
        public class OperationResult<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }

            public static OperationResult<T> Ok(T data, string message = "İşlem başarılı.") =>
                new OperationResult<T> { Success = true, Message = message, Data = data };

            public static OperationResult<T> Fail(string message) =>
                new OperationResult<T> { Success = false, Message = message };
        }

        public class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string Operation { get; set; }
            public string Entity { get; set; }
            public int EntityId { get; set; }
            public List<ChangeLogEntry> Changes { get; set; }
            public string Error { get; set; }
        }

        public class ChangeLogEntry
        {
            public string Property { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }
    }

}
