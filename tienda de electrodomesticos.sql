IF DB_ID('tienda_electrodomesticos') IS NULL
BEGIN
    CREATE DATABASE tienda_electrodomesticos;
END
GO

USE tienda_electrodomesticos;
GO

CREATE TABLE categorias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    imagen_nombre VARCHAR(255),
    is_active BIT DEFAULT 1
);
GO

CREATE TABLE usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    password VARCHAR(100) NOT NULL,
    profile_image VARCHAR(150) NOT NULL,
    rol VARCHAR(50) NOT NULL,
    is_enable BIT NOT NULL DEFAULT 1,
    cuenta_no_bloqueada BIT DEFAULT 1,
    intento_fallido INT DEFAULT 0,
    lock_time DATETIME2,
    reset_token VARCHAR(255)
);
GO

CREATE TABLE productos (
    id INT IDENTITY(1,1) PRIMARY KEY,
    titulo VARCHAR(500) NOT NULL,
    descripcion TEXT NOT NULL,
    categoria_id INT NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    imagen VARCHAR(255),
    descuento INT NOT NULL DEFAULT 0,
    precio_con_descuento DECIMAL(10,2),
    is_active BIT DEFAULT 1,

    CONSTRAINT FK_productos_categorias
        FOREIGN KEY (categoria_id)
        REFERENCES categorias(id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
);
GO

CREATE TABLE carrito (
    id INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id INT NOT NULL,
    producto_id INT NOT NULL,
    cantidad INT NOT NULL CHECK (cantidad >= 1),

    CONSTRAINT FK_carrito_usuarios
        FOREIGN KEY (usuario_id)
        REFERENCES usuarios(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,

    CONSTRAINT FK_carrito_productos
        FOREIGN KEY (producto_id)
        REFERENCES productos(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
GO

INSERT INTO categorias (nombre, imagen_nombre, is_active) VALUES
('Computadoras de Escritorio', 'computadoras_escritorio.webp', 1),
('All-in-One', 'all_in_one.webp', 1),
('Laptops', 'laptops.webp', 1),
('Tablets', 'tablets.webp', 1),
('Monitores', 'monitores.webp', 1);
GO

INSERT INTO productos
(titulo, descripcion, categoria_id, precio, stock, imagen, descuento, precio_con_descuento, is_active)
VALUES
('Computadora de Escritorio Gamer',
 'PC de escritorio con procesador Intel i7, 16GB RAM, SSD 1TB y tarjeta gráfica RTX 3060.',
 1, 1800.00, 8, 'pc_gamer.webp', 10, 1620.00, 1),

('All-in-One 27"',
 'All-in-One de 27 pulgadas con pantalla 4K, procesador Intel i5, 16GB RAM y 512GB SSD.',
 2, 1500.00, 5, 'aio27.webp', 5, 1425.00, 1),

('Laptop Ultrabook i7',
 'Laptop ultraligera con procesador Intel i7, 16GB RAM, SSD 512GB, ideal para profesionales.',
 3, 2200.00, 10, 'laptop_ultrabook.webp', 10, 1980.00, 1),

('Tablet Pro 12.9"',
 'Tablet de 12.9 pulgadas con pantalla Retina, 256GB almacenamiento y soporte para lápiz digital.',
 4, 1200.00, 12, 'tablet_pro.webp', 0, 1200.00, 1),

('Monitor 27" 4K',
 'Monitor 27 pulgadas 4K UHD, con alta frecuencia de actualización y soporte ajustable.',
 5, 450.00, 15, 'monitor_27.webp', 0, 450.00, 1);
GO

CREATE PROCEDURE sp_insertar_categoria
    @nombre VARCHAR(100),
    @imagen_nombre VARCHAR(255),
    @is_active BIT
AS
BEGIN
    INSERT INTO categorias (nombre, imagen_nombre, is_active)
    VALUES (@nombre, @imagen_nombre, @is_active);
END;
GO

CREATE PROCEDURE sp_insertar_producto
    @titulo VARCHAR(500),
    @descripcion TEXT,
    @categoria_id INT,
    @precio DECIMAL(10,2),
    @stock INT,
    @imagen VARCHAR(255),
    @descuento INT,
    @precio_con_descuento DECIMAL(10,2),
    @is_active BIT
AS
BEGIN
    INSERT INTO productos
    (titulo, descripcion, categoria_id, precio, stock, imagen, descuento, precio_con_descuento, is_active)
    VALUES
    (@titulo, @descripcion, @categoria_id, @precio, @stock, @imagen, @descuento, @precio_con_descuento, @is_active);
END;
GO

CREATE PROCEDURE sp_listar_productos
AS
BEGIN
    SELECT
        p.id,
        p.titulo,
        p.descripcion,
        p.precio,
        p.stock,
        p.descuento,
        p.precio_con_descuento,
        p.imagen,
        p.is_active,
        c.nombre AS categoria
    FROM productos p
    INNER JOIN categorias c ON p.categoria_id = c.id;
END;
GO

CREATE PROCEDURE sp_listar_categorias_combo
AS
BEGIN
    SELECT
        id,
        nombre
    FROM categorias
    WHERE is_active = 1
    ORDER BY nombre;
END;
GO

CREATE PROCEDURE sp_eliminar_categoria
    @categoria_id INT
AS
BEGIN
    -- Verificar si la categoría existe
    IF NOT EXISTS (SELECT 1 FROM categorias WHERE id = @categoria_id)
    BEGIN
        RAISERROR('La categoría no existe.', 16, 1);
        RETURN;
    END

    -- Verificar si tiene productos asociados
    IF EXISTS (SELECT 1 FROM productos WHERE categoria_id = @categoria_id)
    BEGIN
        RAISERROR('No se puede eliminar la categoría porque tiene productos asociados.', 16, 1);
        RETURN;
    END

    -- Eliminar categoría
    DELETE FROM categorias
    WHERE id = @categoria_id;
END;
GO

CREATE PROCEDURE sp_eliminar_producto
    @producto_id INT
AS
BEGIN
    -- Verificar si el producto existe
    IF NOT EXISTS (SELECT 1 FROM productos WHERE id = @producto_id)
    BEGIN
        RAISERROR('El producto no existe.', 16, 1);
        RETURN;
    END

    DELETE FROM productos
    WHERE id = @producto_id;
END;
GO

