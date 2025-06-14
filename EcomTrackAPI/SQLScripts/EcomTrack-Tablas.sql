CREATE DATABASE EcomTrack;
GO

USE EcomTrack;
GO


-- Tabla de roles de usuario
CREATE TABLE Rol (
    Id INT NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    PRIMARY KEY (Id)
);

-- Tabla de usuarios del sistema
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    Pwd VARCHAR(50) NOT NULL,
    Rol INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NULL DEFAULT GETDATE(),
    PRIMARY KEY (Id),
    UNIQUE (Correo),
    FOREIGN KEY (Rol) REFERENCES Rol(Id)
);

-- Tabla de administradores del sistema
CREATE TABLE Administrador (
    Id INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (Id) REFERENCES Usuario(Id) ON DELETE CASCADE
);

-- Tabla de clientes
CREATE TABLE Cliente (
    Id INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NULL,
    TotalCompras INT NULL DEFAULT 0,
    TotalGastado DECIMAL(10, 2) NULL DEFAULT 0,
    PRIMARY KEY (Id),
    FOREIGN KEY (Id) REFERENCES Usuario(Id) ON DELETE CASCADE
);

-- Tabla de categorías de productos
CREATE TABLE Categoria (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    CantidadProducto INT NULL DEFAULT 0,
    Estado BIT NULL DEFAULT 1,
    PRIMARY KEY (Id)
);

-- Tabla de proveedores
CREATE TABLE Proveedor (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NULL,
    Correo VARCHAR(100) NULL,
    Telefono VARCHAR(15) NULL,
    Estado BIT NULL DEFAULT 1,
    PRIMARY KEY (Id)
);

-- Tabla de productos
CREATE TABLE Producto (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Categoria INT NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Proveedor INT NOT NULL,
    UrlImagen NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME NULL DEFAULT GETDATE(),
    Estado BIT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (Categoria) REFERENCES Categoria(Id),
    FOREIGN KEY (Proveedor) REFERENCES Proveedor(Id)
);

-- Tabla de carritos de compra
CREATE TABLE Carrito (
    Id INT IDENTITY(1,1) NOT NULL,
    Cliente INT NOT NULL,
    Activo BIT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (Cliente) REFERENCES Cliente(Id)
);

-- Tabla de detalles del carrito
CREATE TABLE DetalleCarrito (
    Id INT IDENTITY(1,1) NOT NULL,
    Carrito INT NOT NULL,
    Producto INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Subtotal AS (Cantidad * Precio) PERSISTED,
    PRIMARY KEY (Id),
    FOREIGN KEY (Carrito) REFERENCES Carrito(Id),
    FOREIGN KEY (Producto) REFERENCES Producto(Id)
);

-- Tabla de estados de pedidos
CREATE TABLE Estado (
    Id INT IDENTITY(1,1) NOT NULL,
    Descripcion VARCHAR(20) NULL,
    PRIMARY KEY (Id)
);

-- Tabla de tipos de dirección
CREATE TABLE TipoDireccion (
    Id INT IDENTITY(1,1) NOT NULL,
    Descripcion VARCHAR(20) NULL,
    PRIMARY KEY (Id)
);

-- Tabla de estados/regiones
CREATE TABLE Estados (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    PRIMARY KEY (Id)
);

-- Tabla de direcciones de clientes
CREATE TABLE Direccion (
    Id INT IDENTITY(1,1) NOT NULL,
    IdCliente INT NOT NULL,
    TipoDireccion INT NOT NULL,
    EsPredeterminado BIT NULL DEFAULT 1,
    Calle VARCHAR(100) NOT NULL,
    Numero VARCHAR(10) NOT NULL,
    EntreCalles VARCHAR(200) NOT NULL,
    Colonia VARCHAR(100) NOT NULL,
    Ciudad VARCHAR(100) NOT NULL,
    IdEstado INT NOT NULL,
    CodigoPostal VARCHAR(10) NOT NULL,
    EstaActiva BIT NOT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(Id),
    FOREIGN KEY (TipoDireccion) REFERENCES TipoDireccion(Id),
    FOREIGN KEY (IdEstado) REFERENCES Estados(Id)
);

-- Tabla de pedidos
CREATE TABLE Pedido (
    Id INT IDENTITY(1,1) NOT NULL,
    IdCliente INT NULL,
    FechaPedido DATETIME NULL DEFAULT GETDATE(),
    FechaPedidoINT INT NULL,
    Importe DECIMAL(10, 2) NULL,
    IdEstado INT NULL,
    IdDireccion INT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdCliente) REFERENCES Cliente(Id),
    FOREIGN KEY (IdEstado) REFERENCES Estado(Id),
    FOREIGN KEY (IdDireccion) REFERENCES Direccion(Id)
);

-- Tabla de detalles de pedido
CREATE TABLE DetallePedido (
    Id INT IDENTITY(1,1) NOT NULL,
    IdPedido INT NULL,
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(10, 2) NOT NULL,
    Subtotal DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdPedido) REFERENCES Pedido(Id),
    FOREIGN KEY (IdProducto) REFERENCES Producto(Id)
);

-- Tabla de métodos de pago
CREATE TABLE MetodoPago (
    Id INT IDENTITY(1,1) NOT NULL,
    Descripcion VARCHAR(20) NULL,
    PRIMARY KEY (Id)
);

-- Tabla de tarjetas de clientes
CREATE TABLE Tarjeta (
    Id INT IDENTITY(1,1) NOT NULL,
    Cliente INT NULL,
    NumeroTarjeta VARCHAR(16) NULL,
    MetodoDePago INT NULL,
    MarcaTarjeta VARCHAR(100) NULL,
    Activa BIT NULL,
    Predeterminada BIT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (Cliente) REFERENCES Cliente(Id),
    FOREIGN KEY (MetodoDePago) REFERENCES MetodoPago(Id)
);

-- Tabla de pagos
CREATE TABLE Pago (
    Id INT IDENTITY(1,1) NOT NULL,
    IdPedido INT NULL,
    IdTarjeta INT NULL,
    ImporteAbonado DECIMAL(10, 2) NULL,
    FechaPago DATETIME NULL DEFAULT GETDATE(),
    PRIMARY KEY (Id),
    FOREIGN KEY (IdPedido) REFERENCES Pedido(Id),
    FOREIGN KEY (IdTarjeta) REFERENCES Tarjeta(Id)
);

-- Tabla de transportistas
CREATE TABLE Transportista (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(50) NULL,
    Correo VARCHAR(100) NULL,
    Telefono VARCHAR(15) NULL,
    Estado BIT NULL DEFAULT 1,
    PRIMARY KEY (Id)
);

-- Tabla de envíos
CREATE TABLE Envio (
    Id INT IDENTITY(1,1) NOT NULL,
    FechaEnvio DATETIME NULL,
    FechaEntrega DATETIME NULL,
    IdPedido INT NULL,
    IdTransportista INT NULL,
    IdEstado INT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdEstado) REFERENCES Estado(Id),
    FOREIGN KEY (IdPedido) REFERENCES Pedido(Id),
    FOREIGN KEY (IdTransportista) REFERENCES Transportista(Id)
);