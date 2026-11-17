-- Script de inicialización de Base de Datos para TriageDoc (PostgreSQL 16)
-- Modelo Relacional Normalizado (3FN) con 4 Tablas

-- 1. Tabla de Pacientes
CREATE TABLE IF NOT EXISTS pacientes (
    id SERIAL PRIMARY KEY,
    dni VARCHAR(20) NOT NULL UNIQUE,
    nombre_completo VARCHAR(150) NOT NULL,
    edad INT NOT NULL,
    genero VARCHAR(20) DEFAULT 'No Especificado',
    telefono VARCHAR(25),
    fecha_registro TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

-- 2. Tabla de Catálogo de Síntomas
CREATE TABLE IF NOT EXISTS sintomas (
    id SERIAL PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(120) NOT NULL,
    descripcion VARCHAR(300),
    es_signo_alarma BOOLEAN DEFAULT FALSE,
    categoria VARCHAR(50) DEFAULT 'General',
    nivel_gravedad_base INT DEFAULT 3
);

-- 3. Tabla de Protocolos de Atención (Manchester)
CREATE TABLE IF NOT EXISTS protocolos_atencion (
    id SERIAL PRIMARY KEY,
    nivel_prioridad INT NOT NULL,
    codigo_color VARCHAR(30) NOT NULL,
    nombre_nivel VARCHAR(50) NOT NULL,
    tiempo_maximo_espera_minutos INT NOT NULL,
    area_derivacion VARCHAR(120) NOT NULL,
    accion_inmediata VARCHAR(300) NOT NULL
);

-- 4. Tabla de Evaluaciones de Triage (Transaccional)
CREATE TABLE IF NOT EXISTS evaluaciones_triage (
    id SERIAL PRIMARY KEY,
    paciente_id INT NOT NULL REFERENCES pacientes(id) ON DELETE RESTRICT,
    fecha_registro TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    temperatura NUMERIC(4,1) NOT NULL,
    frecuencia_cardiaca INT NOT NULL,
    presion_arterial VARCHAR(20),
    saturacion_oxigeno INT NOT NULL,
    dolor_toracico BOOLEAN DEFAULT FALSE,
    dificultad_respiratoria BOOLEAN DEFAULT FALSE,
    sintomas_principales TEXT,
    prioridad_asignada INT NOT NULL,
    protocolo_atencion_id INT REFERENCES protocolos_atencion(id) ON DELETE SET NULL,
    justificacion_regla VARCHAR(500),
    requiere_atencion_inmediata BOOLEAN DEFAULT FALSE,
    observaciones_clinicas VARCHAR(500)
);

-- Inserción de Datos Iniciales (Seed Data)

INSERT INTO protocolos_atencion (id, nivel_prioridad, codigo_color, nombre_nivel, tiempo_maximo_espera_minutos, area_derivacion, accion_inmediata)
VALUES
(1, 1, 'Rojo', 'Nivel 1 - Resucitación', 0, 'Sala de Shock Trauma / Box de Reanimación', 'Atención inmediata continua. Soporte vital y monitoreo invasivo.'),
(2, 2, 'Naranja', 'Nivel 2 - Emergencia', 10, 'Urgencias Médicas / Observación Inmediata', 'Evaluación médica en menos de 10 minutos. Monitoreo constante.'),
(3, 3, 'Amarillo', 'Nivel 3 - Urgencia', 60, 'Boxes de Consulta Rápida', 'Atención antes de 60 minutos con reevaluación periódica.'),
(4, 4, 'Verde', 'Nivel 4 - Poco Urgente', 120, 'Consulta Externa / Triage Secundario', 'Atención ambulatoria antes de 120 minutos.'),
(5, 5, 'Azul', 'Nivel 5 - No Urgente', 240, 'Consulta General / Centro de Salud', 'Atención diferida sin riesgo vital.')
ON CONFLICT (id) DO NOTHING;

INSERT INTO sintomas (id, codigo, nombre, descripcion, es_signo_alarma, categoria, nivel_gravedad_base)
VALUES
(1, 'SIN-001', 'Dolor Torácico Opresivo', 'Dolor opresivo retroesternal con irradiación', TRUE, 'Cardiovascular', 5),
(2, 'SIN-002', 'Dificultad Respiratoria Severa', 'Disnea aguda o estridor respiratorio en reposo', TRUE, 'Respiratorio', 5),
(3, 'SIN-003', 'Pérdida del Estado de Alerta', 'Síncope o Glasgow < 13', TRUE, 'Neurológico', 5),
(4, 'SIN-004', 'Fiebre Elevada (> 38.5 °C)', 'Temperatura axilar elevada con diaforesis', FALSE, 'Infeccioso', 3),
(5, 'SIN-005', 'Cefalea Moderada', 'Cefalea tensional sin déficit neurológico', FALSE, 'Neurológico', 2)
ON CONFLICT (id) DO NOTHING;

INSERT INTO pacientes (id, dni, nombre_completo, edad, genero, telefono)
VALUES
(1, '0801-1995-12345', 'Juan Carlos Rodríguez', 45, 'Masculino', '+504 9876-5432'),
(2, '0801-1950-67890', 'María Elena Castro', 76, 'Femenino', '+504 9911-2233')
ON CONFLICT (id) DO NOTHING;

INSERT INTO evaluaciones_triage (id, paciente_id, temperatura, frecuencia_cardiaca, presion_arterial, saturacion_oxigeno, dolor_toracico, dificultad_respiratoria, sintomas_principales, prioridad_asignada, protocolo_atencion_id, justificacion_regla, requiere_atencion_inmediata, observaciones_clinicas)
VALUES
(1, 1, 37.0, 130, '140/90', 88, TRUE, TRUE, 'Dolor Torácico Opresivo, Disnea', 1, 1, 'REGLA-01: Dolor de pecho + Dificultad respiratoria con SpO2 crítica (<90%). Código Rojo.', TRUE, 'Paciente trasladado a sala de reanimación.')
ON CONFLICT (id) DO NOTHING;

-- Sincronización de secuencias SERIAL tras inserción de IDs explícitos
SELECT setval(pg_get_serial_sequence('protocolos_atencion', 'id'), COALESCE((SELECT MAX(id) FROM protocolos_atencion), 1));
SELECT setval(pg_get_serial_sequence('sintomas', 'id'), COALESCE((SELECT MAX(id) FROM sintomas), 1));
SELECT setval(pg_get_serial_sequence('pacientes', 'id'), COALESCE((SELECT MAX(id) FROM pacientes), 1));
SELECT setval(pg_get_serial_sequence('evaluaciones_triage', 'id'), COALESCE((SELECT MAX(id) FROM evaluaciones_triage), 1));
