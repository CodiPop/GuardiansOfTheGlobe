CREATE PROCEDURE InsertarNuevoHeroe
    @nombre VARCHAR(50),
    @edad INT,
    @habilidades VARCHAR(255),
    @debilidades VARCHAR(255),
    @relaciones_personales VARCHAR(255)
AS
BEGIN

    INSERT INTO heroes (nombre, edad, habilidades, debilidades, relaciones_personales)
    VALUES (@nombre, @edad, @habilidades, @debilidades, @relaciones_personales);
    
END;
go

CREATE PROCEDURE InsertarNuevoVillano
    @nombre VARCHAR(50),
    @edad INT,
    @habilidades VARCHAR(255),
    @origen VARCHAR(255),
    @debilidades VARCHAR(255),
    @poder VARCHAR(255)
AS
BEGIN
    INSERT INTO villanos (nombre, edad, habilidades, origen, debilidades, poder)
    VALUES (@nombre, @edad, @habilidades, @origen, @debilidades, @poder);
END

go
CREATE PROCEDURE InsertarNuevaPelea
    @id_heroe INT,
    @id_villano INT,
    @resultado VARCHAR(50)
AS
BEGIN
    INSERT INTO peleas (id_heroe, id_villano, resultado)
    VALUES (@id_heroe, @id_villano, @resultado);
END

go
CREATE PROCEDURE InsertarNuevoPatrocinador
    @id_heroe INT,
    @nombre VARCHAR(50),
    @monto FLOAT,
    @origen_dinero VARCHAR(255)
AS
BEGIN
    INSERT INTO patrocinadores (id_heroe, nombre, monto, origen_dinero)
    VALUES (@id_heroe, @nombre, @monto, @origen_dinero);
END
go
CREATE PROCEDURE InsertarNuevoEventoEnAgenda
    @id_heroe INT,
    @fecha DATE,
    @evento VARCHAR(255)
AS
BEGIN
    INSERT INTO agenda (id_heroe, fecha, evento)
    VALUES (@id_heroe, @fecha, @evento);
END

go

CREATE PROCEDURE ObtenerHeroesConMasVictorias
AS
BEGIN
    SELECT TOP 3
        heroes.nombre AS nombre,
        COUNT(*) AS victorias
    FROM
        peleas
        JOIN heroes ON peleas.id_heroe = heroes.id
    WHERE
        peleas.resultado = 'Victoria'
    GROUP BY
        heroes.nombre
    ORDER BY
        victorias DESC;
END;

go

CREATE PROCEDURE ObtenerVillanoConMasPeleas
    @id_heroe INT
AS
BEGIN
    SELECT TOP 1
        v.nombre AS nombre_villano,
        COUNT(*) AS cantidad_de_peleas
    FROM
        peleas p
        INNER JOIN villanos v ON p.id_villano = v.id
    WHERE
        p.id_heroe = @id_heroe
    GROUP BY
        v.nombre
    ORDER BY
        cantidad_de_peleas DESC;
END;

go

CREATE PROCEDURE ObtenerMyG
   
AS
BEGIN
SELECT heroes.nombre AS nombre_heroe, villanos.nombre AS nombre_villano, peleas.resultado
FROM peleas
JOIN heroes ON peleas.id_heroe = heroes.id
JOIN villanos ON peleas.id_villano = villanos.id
WHERE heroes.nombre = 'The Guardians' OR heroes.nombre = 'Mark';
END

EXEC InsertarNuevoHeroe @nombre = 'Superman', @edad = 35, @habilidades = 'Vuelo, Fuerza sobrehumana', @debilidades = 'Criptonita', @relaciones_personales = 'Lois Lane';
EXEC InsertarNuevoHeroe @nombre = 'Spider-Man', @edad = 20, @habilidades = 'Agilidad, Sentido arácnido', @debilidades = 'Despreocupación, Amenazas personales', @relaciones_personales = 'Tía May';
EXEC InsertarNuevoHeroe @nombre = 'Wonder Woman', @edad = 30, @habilidades = 'Fuerza, Lazo de la verdad', @debilidades = 'Pérdida de sus poderes', @relaciones_personales = 'Steve Trevor';
EXEC InsertarNuevoHeroe @nombre = 'Black Widow', @edad = 35, @habilidades = 'Espionaje, Combate cuerpo a cuerpo', @debilidades = 'Su pasado', @relaciones_personales = 'Hawkeye';
EXEC InsertarNuevoHeroe @nombre = 'Flash', @edad = 28, @habilidades = 'Velocidad sobrehumana, Viaje en el tiempo', @debilidades = 'Falta de control', @relaciones_personales = 'Iris West';
EXEC InsertarNuevoHeroe @nombre = 'Captain Marvel', @edad = 30, @habilidades = 'Superfuerza, Vuelo, Disparo de energía', @debilidades = 'Manipulación mental', @relaciones_personales = 'Nick Fury';
EXEC InsertarNuevoHeroe @nombre = 'Santiago', @edad = 2, @habilidades = 'Ego', @debilidades = 'Procastinar', @relaciones_personales = 'Mi ex';
EXEC InsertarNuevoHeroe @nombre = 'The Guardians', @edad = 0, @habilidades = 'Trabajo en equipo', @debilidades = 'Coordinacion', @relaciones_personales = 'Nick Fury';
EXEC InsertarNuevoHeroe @nombre = 'Mark', @edad = 45, @habilidades = 'Omnipotente', @debilidades = 'Ninguna', @relaciones_personales = 'The Guardians';


EXEC InsertarNuevoVillano @nombre = 'Joker', @edad = 40, @habilidades = 'Mente brillante, Manipulación', @origen = 'Gotham City', @debilidades = 'Su obsesión con Batman', @poder = 'Caos';
EXEC InsertarNuevoVillano @nombre = 'Magneto', @edad = 55, @habilidades = 'Control del magnetismo', @origen = 'Genosha', @debilidades = 'Armas anti-mutantes', @poder = 'Manipulación de metales';
EXEC InsertarNuevoVillano @nombre = 'Loki', @edad = 1000, @habilidades = 'Ilusiones, Cambio de forma', @origen = 'Asgard', @debilidades = 'Orgullo', @poder = 'Magia';
EXEC InsertarNuevoVillano @nombre = 'Venom', @edad = 35, @habilidades = 'Simbionte alienígena, Fuerza sobrehumana', @origen = 'Planeta Klyntar', @debilidades = 'Sonido y fuego', @poder = 'Symbiote Rage';
EXEC InsertarNuevoVillano @nombre = 'Darkseid', @edad = 1000, @habilidades = 'Superfuerza, Inmortalidad, Omega Beams', @origen = 'Apokolips', @debilidades = 'Ecuación Anti-Vida', @poder = 'Omega Effect';
EXEC InsertarNuevoVillano @nombre = 'Red Skull', @edad = 60, @habilidades = 'Genio estratégico, Manipulación', @origen = 'Alemania', @debilidades = 'Su obsesión por la superioridad', @poder = 'Cosmic Cube';


EXEC InsertarNuevoPatrocinador @id_heroe = 1, @nombre = 'Stark Industries', @monto = 100000, @origen_dinero = 'Industria tecnológica';
EXEC InsertarNuevoPatrocinador @id_heroe = 2, @nombre = 'Wayne Enterprises', @monto = 150000, @origen_dinero = 'Industria financiera';
EXEC InsertarNuevoPatrocinador @id_heroe = 3, @nombre = 'Oscorp', @monto = 80000, @origen_dinero = 'Investigación científica';
EXEC InsertarNuevoPatrocinador @id_heroe = 4, @nombre = 'Stark Industries', @monto = 120000, @origen_dinero = 'Industria tecnológica';
EXEC InsertarNuevoPatrocinador @id_heroe = 5, @nombre = 'LexCorp', @monto = 90000, @origen_dinero = 'Industria tecnológica';
EXEC InsertarNuevoPatrocinador @id_heroe = 6, @nombre = 'Daily Bugle', @monto = 75000, @origen_dinero = 'Medios de comunicación';

EXEC InsertarNuevoEventoEnAgenda @id_heroe = 1, @fecha = '2023-05-10', @evento = 'Reunión de equipo';
EXEC InsertarNuevoEventoEnAgenda @id_heroe = 2, @fecha = '2023-05-12', @evento = 'Entrenamiento';
EXEC InsertarNuevoEventoEnAgenda @id_heroe = 3, @fecha = '2023-05-15', @evento = 'Misión en Themyscira';
EXEC InsertarNuevoEventoEnAgenda @id_heroe = 4, @fecha = '2023-05-18', @evento = 'Investigación encubierta';
EXEC InsertarNuevoEventoEnAgenda @id_heroe = 5, @fecha = '2023-05-20', @evento = 'Entrenamiento en el Sanctum Sanctorum';
EXEC InsertarNuevoEventoEnAgenda @id_heroe = 6, @fecha = '2023-05-22', @evento = 'Defensa de la Torre de los Vengadores';

EXEC InsertarNuevaPelea @id_heroe = 1, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 2, @id_villano = 3, @resultado = 'Derrota';
EXEC InsertarNuevaPelea @id_heroe = 3, @id_villano = 1, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 4, @id_villano = 3, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 5, @id_villano = 1, @resultado = 'Derrota';
EXEC InsertarNuevaPelea @id_heroe = 6, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 6, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 5, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 1, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 4, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 6, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 6, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 5, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 8, @id_villano = 1, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 4, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 6, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 5, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 1, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 4, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 2, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 6, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 5, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 1, @resultado = 'Victoria';
EXEC InsertarNuevaPelea @id_heroe = 9, @id_villano = 4, @resultado = 'Victoria';