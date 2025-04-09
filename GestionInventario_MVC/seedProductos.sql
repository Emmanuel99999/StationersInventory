SET IDENTITY_INSERT Products ON;

INSERT INTO Products (Id, Name, Category, Stock, Barcode) VALUES
(1, 'Cuaderno universitario', 'Papelería', 50, '750000000001'),
(2, 'Lápiz HB', 'Escritura', 100, '750000000002'),
(3, 'Bolígrafo azul', 'Escritura', 75, '750000000003'),
(4, 'Goma de borrar', 'Accesorios', 60, '750000000004'),
(5, 'Sacapuntas metálico', 'Accesorios', 40, '750000000005'),
(6, 'Marcador permanente', 'Escritura', 30, '750000000006'),
(7, 'Resaltador amarillo', 'Escritura', 45, '750000000007'),
(8, 'Papel bond A4', 'Papelería', 25, '750000000008'),
(9, 'Carpeta plástica', 'Organización', 35, '750000000009'),
(10, 'Pegamento en barra', 'Accesorios', 55, '750000000010'),
(11, 'Tijeras escolares', 'Accesorios', 20, '750000000011'),
(12, 'Regla de 30 cm', 'Accesorios', 50, '750000000012'),
(13, 'Cartulina blanca', 'Papelería', 15, '750000000013'),
(14, 'Cartulina de colores', 'Papelería', 20, '750000000014'),
(15, 'Cuaderno de dibujo', 'Papelería', 30, '750000000015'),
(16, 'Bloc de notas', 'Organización', 40, '750000000016'),
(17, 'Portaminas', 'Escritura', 35, '750000000017'),
(18, 'Grapadora pequeña', 'Accesorios', 10, '750000000018'),
(19, 'Caja de clips', 'Accesorios', 60, '750000000019'),
(20, 'Corrector líquido', 'Accesorios', 25, '750000000020');

SET IDENTITY_INSERT Products OFF;
