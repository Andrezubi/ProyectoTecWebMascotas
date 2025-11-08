using Dapper;
using ProyectoMascotas.Core.Enum;
using ProyectoMascotas.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMascotas.Infrastructure.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IDbConnectionFactory _connFactory;
        //private readonly IUnitOfWork? _unitOfWork; // opcional

        // Ambient connection per async flow (no compartida entre requests)
        private static readonly AsyncLocal<(IDbConnection? Conn, IDbTransaction? Tx)> _ambient =
            new();

        public DatabaseProvider Provider => _connFactory.Provider;

        public DapperContext(IDbConnectionFactory connFactory
            //IUnitOfWork? unitOfWork
            )
        {
            _connFactory = connFactory;
            //_unitOfWork = unitOfWork; // si el UoW inició una transacción, lo reutilizamos
        }

        // UnitOfWork llamará esto al iniciar transacción
        public void SetAmbientConnection(IDbConnection conn, IDbTransaction? tx)
        {
            _ambient.Value = (conn, tx);
        }

        // UnitOfWork llamará esto al finalizar/rollback
        public void ClearAmbientConnection()
        {
            _ambient.Value = (null, null);
        }

        /// <summary>
        /// Si el UnitOfWork está activo:
        /// Usa la misma conexión y transacción. Si no hay UnitOfWork:
        /// Crea una conexión nueva desde IDbConnectionFactory.
        /// ownsConnection indica si la conexión es propiedad de este contexto (y debe cerrarla) o si la comparte con UnitOfWork(y no debe cerrarla).
        /// </summary>
        /// <returns></returns>
        private (IDbConnection conn, IDbTransaction? tx, bool ownsConnection) GetConnAndTx()
        {
            var ambient = _ambient.Value;
            if (ambient.Conn != null)
                return (ambient.Conn, ambient.Tx, false);

            var conn = _connFactory.CreateConnection();
            return (conn, null, true);
        }

        /// <summary>
        /// Antes de ejecutar algo, asegura que la conexión esté abierta.
        /// Esto evita errores tipo "Invalid operation: connection is closed".
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private async Task OpenIfNeededAsync(IDbConnection conn)
        {
            if (conn is DbConnection dbConn && dbConn.State == ConnectionState.Closed)
                await dbConn.OpenAsync();
        }

        /// <summary>
        /// Ejecuta un SELECT que devuelve múltiples filas.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                return await conn.QueryAsync<T>(new CommandDefinition(sql, param, tx, commandType: commandType));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn && dbConn.State != ConnectionState.Closed) await dbConn.CloseAsync();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Devuelve solo una fila (o null).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                return await conn.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, param, tx, commandType: commandType));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn && dbConn.State != ConnectionState.Closed) await dbConn.CloseAsync();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Ejecuta comandos que no devuelven datos (INSERT, UPDATE, DELETE).
        /// Devuelve el número de filas afectadas.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                return await conn.ExecuteAsync(new CommandDefinition(sql, param, tx, commandType: commandType));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn && dbConn.State != ConnectionState.Closed) await dbConn.CloseAsync();
                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Ejecuta un query y devuelve un valor escalar (ejemplo: el último ID insertado).
        /// Hace Convert.ChangeType para castear el valor al tipo genérico T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();
            try
            {
                await OpenIfNeededAsync(conn);
                var res = await conn.ExecuteScalarAsync(new CommandDefinition(sql, param, tx, commandType: commandType));
                if (res == null || res == DBNull.Value) return default!;
                return (T)Convert.ChangeType(res, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn && dbConn.State != ConnectionState.Closed) await dbConn.CloseAsync();
                    conn.Dispose();
                }
            }
        }
    }

}
