using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace ClinicaVeterinaria
{
    public class MascotaRepository
    {
        // Ruta de la base de datos SQLite
        private readonly string _connectionString = "Data Source=veterinaria.db;";

        public MascotaRepository()
        {
            InicializarBaseDeDatos();
        }

        // Crea las tablas automáticamente si no existen al iniciar
        private void InicializarBaseDeDatos()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Mascotas (
                        MascotaID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Especie TEXT NOT NULL,
                        Raza TEXT,
                        Edad INTEGER,
                        ClienteID INTEGER
                    );";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // 1. CREATE (Insertar)
        public void AgregarMascota(Mascota mascota)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Mascotas (Nombre, Especie, Raza, Edad, ClienteID) VALUES (@nombre, @especie, @raza, @edad, @clienteId)";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", mascota.Nombre);
                    command.Parameters.AddWithValue("@especie", mascota.Especie);
                    command.Parameters.AddWithValue("@raza", mascota.Raza ?? "");
                    command.Parameters.AddWithValue("@edad", mascota.Edad);
                    command.Parameters.AddWithValue("@clienteId", mascota.ClienteID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 2. READ (Consultar todas las mascotas)
        public List<Mascota> ObtenerTodas()
        {
            var lista = new List<Mascota>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT MascotaID, Nombre, Especie, Raza, Edad, ClienteID FROM Mascotas";
                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Mascota
                        {
                            MascotaID = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Especie = reader.GetString(2),
                            Raza = reader.IsDBNull(3) ? "" : reader.GetString(3),
                            Edad = reader.GetInt32(4),
                            ClienteID = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                        });
                    }
                }
            }
            return lista;
        }

        // 3. UPDATE (Actualizar datos)
        public void ActualizarMascota(Mascota mascota)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Mascotas SET Nombre=@nombre, Especie=@especie, Raza=@raza, Edad=@edad WHERE MascotaID=@id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", mascota.Nombre);
                    command.Parameters.AddWithValue("@especie", mascota.Especie);
                    command.Parameters.AddWithValue("@raza", mascota.Raza ?? "");
                    command.Parameters.AddWithValue("@edad", mascota.Edad);
                    command.Parameters.AddWithValue("@id", mascota.MascotaID);
                    command.ExecuteNonQuery();
                }
            }
        }

        // 4. DELETE (Eliminar registro)
        public void EliminarMascota(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Mascotas WHERE MascotaID = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}