-- Script de creación de tablas en SQLite
CREATE TABLE IF NOT EXISTS Clientes (
    ClienteID INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Telefono TEXT,
    Correo TEXT
);

CREATE TABLE IF NOT EXISTS Mascotas (
    MascotaID INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL,
    Especie TEXT NOT NULL,
    Raza TEXT,
    Edad INTEGER,
    ClienteID INTEGER,
    FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
);