IF DB_ID('tienda_electrodomesticos2') IS NULL
BEGIN
    CREATE DATABASE tienda_electrodomesticos2;
END
GO

USE tienda_electrodomesticos2;
GO

-- TABLA CATEGORIAS
CREATE TABLE categorias (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    imagen_nombre VARCHAR(255),
    is_active BIT DEFAULT 1
);
GO

-- TABLA USUARIOS (simplificada)
CREATE TABLE usuarios (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    passwordhash VARCHAR(255) NOT NULL,
    profile_image VARCHAR(150),
    rol VARCHAR(50) NOT NULL,
    is_enable BIT NOT NULL DEFAULT 1
);
GO

-- TABLA PRODUCTOS
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

-- TABLA CARRITO
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

-- INSERTAR CATEGORIAS INICIALES
INSERT INTO categorias (nombre, imagen_nombre, is_active) VALUES
('Computadoras de Escritorio', 'computadoras_escritorio.webp', 1),
('All-in-One', 'all_in_one.webp', 1),
('Laptops', 'laptops.webp', 1),
('Tablets', 'tablets.webp', 1),
('Monitores', 'monitores.webp', 1);
GO

-- INSERTAR PRODUCTOS INICIALES
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

INSERT INTO usuarios (nombre, apellido, email, passwordhash, profile_image, rol, is_enable)
VALUES
('Juan', 'Perez', 'juan@correo.com', 'hash123', NULL, 'ROLE_USER', 1),
('Maria', 'Lopez', 'maria@correo.com', 'hash123', NULL, 'ROLE_USER', 1);
GO

INSERT INTO carrito (usuario_id, producto_id, cantidad)
VALUES
(1, 1, 1),
(1, 2, 2),
(1, 3, 1),
(2, 2, 1),
(2, 4, 3);
GO




-- PROCEDIMIENTOS ALMACENADOS CATEGORIA
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

CREATE PROCEDURE sp_actualizar_categoria
    @id INT,
    @nombre VARCHAR(100),
    @imagen_nombre VARCHAR(255),
    @is_active BIT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM categorias WHERE id = @id)
    BEGIN
        RAISERROR('La categoría no existe.', 16, 1);
        RETURN;
    END

    UPDATE categorias
    SET
        nombre = @nombre,
        imagen_nombre = @imagen_nombre,
        is_active = @is_active
    WHERE id = @id;
END;
GO


CREATE or alter PROCEDURE sp_listar_categorias_combo
AS
BEGIN
    SELECT
        id,
        nombre,
        imagen_nombre
    FROM categorias
    WHERE is_active = 1
    ORDER BY nombre;
END;
GO

CREATE PROCEDURE sp_eliminar_categoria
    @categoria_id INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM categorias WHERE id = @categoria_id)
    BEGIN
        RAISERROR('La categoría no existe.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM productos WHERE categoria_id = @categoria_id)
    BEGIN
        RAISERROR('No se puede eliminar la categoría porque tiene productos asociados.', 16, 1);
        RETURN;
    END

    DELETE FROM categorias
    WHERE id = @categoria_id;
END;
GO

CREATE PROCEDURE sp_existe_categoria_por_nombre
    @nombre VARCHAR(100)
AS
BEGIN
    SELECT COUNT(*)
    FROM categorias
    WHERE LOWER(nombre) = LOWER(@nombre);
END;
GO

CREATE PROCEDURE sp_obtener_categoria_por_id
    @id INT
AS
BEGIN
    SELECT
        id,
        nombre,
        imagen_nombre,
        is_active
    FROM categorias
    WHERE id = @id;
END;
GO



-- PROCEDIMIENTOS ALMACENADOS PRODUCTO
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

CREATE OR ALTER PROCEDURE sp_listar_productos
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
        p.categoria_id,  -- <--- agregar este campo
        c.nombre AS categoria
    FROM productos p
    INNER JOIN categorias c ON p.categoria_id = c.id;
END;
GO

CREATE PROCEDURE sp_eliminar_producto
    @producto_id INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM productos WHERE id = @producto_id)
    BEGIN
        RAISERROR('El producto no existe.', 16, 1);
        RETURN;
    END

    DELETE FROM productos
    WHERE id = @producto_id;
END;
GO

select * from categorias;

CREATE PROCEDURE sp_actualizar_producto
    @id INT,
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
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM productos WHERE id = @id)
    BEGIN
        RAISERROR('El producto no existe.', 16, 1);
        RETURN;
    END

    UPDATE productos
    SET
        titulo = @titulo,
        descripcion = @descripcion,
        categoria_id = @categoria_id,
        precio = @precio,
        stock = @stock,
        imagen = @imagen,
        descuento = @descuento,
        precio_con_descuento = @precio_con_descuento,
        is_active = @is_active
    WHERE id = @id;
END;
GO


-- PROCEDIMIENTOS ALMACENADOS CArrito

CREATE PROCEDURE sp_buscar_carrito_producto_usuario
    @producto_id INT,
    @usuario_id INT
AS
BEGIN
    SELECT *
    FROM carrito
    WHERE producto_id = @producto_id
      AND usuario_id = @usuario_id;
END;
GO


CREATE PROCEDURE sp_insertar_carrito
    @usuario_id INT,
    @producto_id INT,
    @cantidad INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM carrito WHERE usuario_id = @usuario_id AND producto_id = @producto_id)
    BEGIN
        UPDATE carrito
        SET cantidad = cantidad + @cantidad
        WHERE usuario_id = @usuario_id AND producto_id = @producto_id;
    END
    ELSE
    BEGIN
        INSERT INTO carrito (usuario_id, producto_id, cantidad)
        VALUES (@usuario_id, @producto_id, @cantidad);
    END
END;



CREATE PROCEDURE sp_contar_carrito_usuario
    @usuario_id INT
AS
BEGIN
    SELECT COUNT(*) 
    FROM carrito 
    WHERE usuario_id = @usuario_id;
END;
GO


CREATE PROCEDURE sp_listar_carrito_usuario
    @usuario_id INT
AS
BEGIN
    SELECT *
    FROM carrito
    WHERE usuario_id = @usuario_id;
END;
GO


---- USUARIO

CREATE PROCEDURE sp_obtener_usuario_por_id
    @id INT
AS
BEGIN
    SELECT id, nombre, apellido, email, passwordhash, profile_image, rol, is_enable
    FROM usuarios
    WHERE id = @id;
END;
GO



-- INSERTAR USUARIO
CREATE PROCEDURE sp_insertar_usuario
    @nombre VARCHAR(100),
    @apellido VARCHAR(100),
    @email VARCHAR(100),
    @passwordhash VARCHAR(255),
    @rol VARCHAR(50),
    @profile_image VARCHAR(150),
    @is_enable BIT
AS
BEGIN
    INSERT INTO usuarios (nombre, apellido, email, passwordhash, rol, profile_image, is_enable)
    VALUES (@nombre, @apellido, @email, @passwordhash, @rol, @profile_image, @is_enable);
END;
GO



-- BUSCAR USUARIO POR EMAIL
CREATE PROCEDURE sp_buscar_usuario_por_email
    @email VARCHAR(100)
AS
BEGIN
    SELECT id, nombre, apellido, email, passwordhash, rol, profile_image, is_enable
    FROM usuarios
    WHERE email = @email;
END;
GO

-- LISTAR USUARIOS POR ROL
CREATE PROCEDURE sp_listar_usuarios_por_rol
    @rol VARCHAR(50)
AS
BEGIN
    SELECT id, nombre, apellido, email, passwordhash, rol, profile_image, is_enable
    FROM usuarios
    WHERE rol = @rol;
END;
GO

-- ACTUALIZAR USUARIO
CREATE PROCEDURE sp_actualizar_usuario
    @id INT,
    @nombre VARCHAR(100),
    @apellido VARCHAR(100),
    @email VARCHAR(100),
    @rol VARCHAR(50),
    @profile_image VARCHAR(150),
    @is_enable BIT
AS
BEGIN
    UPDATE usuarios
    SET nombre = @nombre,
        apellido = @apellido,
        email = @email,
        rol = @rol,
        profile_image = @profile_image,
        is_enable = @is_enable
    WHERE id = @id;
END;
GO

-- ELIMINAR USUARIO
CREATE PROCEDURE sp_eliminar_usuario
    @id INT
AS
BEGIN
    DELETE FROM usuarios
    WHERE id = @id;
END;
GO

select * from usuarios;