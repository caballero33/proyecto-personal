const API_BASE_URL = import.meta.env.VITE_API_URL || '/api';
const FALLBACK_DIRECT_API_URL = 'http://localhost:5000/api';

// Datos semilla de respaldo (idénticos a TriageDocDbContext / init.sql) por si el backend .NET no está encendido aún
const INITIAL_PROTOCOLOS = [
  {
    id: 1,
    nivelPrioridad: 1,
    codigoColor: 'Rojo',
    nombreNivel: 'Nivel 1 - Resucitación',
    tiempoMaximoEsperaMinutos: 0,
    areaDerivacion: 'Sala de Shock Trauma / Box de Reanimación',
    accionInmediata: 'Atención médica y de enfermería inmediata continua. Monitoreo invasivo y soporte vital.'
  },
  {
    id: 2,
    nivelPrioridad: 2,
    codigoColor: 'Naranja',
    nombreNivel: 'Nivel 2 - Emergencia',
    tiempoMaximoEsperaMinutos: 10,
    areaDerivacion: 'Urgencias Médicas / Observación Inmediata',
    accionInmediata: 'Evaluación médica en menos de 10 minutos. Monitorización de signos vitales cada 15 min.'
  },
  {
    id: 3,
    nivelPrioridad: 3,
    codigoColor: 'Amarillo',
    nombreNivel: 'Nivel 3 - Urgencia',
    tiempoMaximoEsperaMinutos: 60,
    areaDerivacion: 'Boxes de Consulta Médica Rápida',
    accionInmediata: 'Atención en menos de 60 minutos. Reevaluación periódica de signos si la espera supera 30 min.'
  },
  {
    id: 4,
    nivelPrioridad: 4,
    codigoColor: 'Verde',
    nombreNivel: 'Nivel 4 - Poco Urgente',
    tiempoMaximoEsperaMinutos: 120,
    areaDerivacion: 'Consulta Externa / Triage Secundario',
    accionInmediata: 'Atención ambulatoria antes de 120 minutos. Tratamiento sintomático general.'
  },
  {
    id: 5,
    nivelPrioridad: 5,
    codigoColor: 'Azul',
    nombreNivel: 'Nivel 5 - No Urgente',
    tiempoMaximoEsperaMinutos: 240,
    areaDerivacion: 'Centro de Salud / Consulta Programada',
    accionInmediata: 'Atención diferida o derivación a consulta externa comunitaria.'
  }
];

const INITIAL_SINTOMAS = [
  {
    id: 1,
    codigo: 'SIN-001',
    nombre: 'Dolor Torácico Opresivo',
    descripcion: 'Dolor retroesternal opresivo con irradiación a mandíbula o extremidad izquierda',
    esSignoAlarma: true,
    categoria: 'Cardiovascular',
    nivelGravedadBase: 5
  },
  {
    id: 2,
    codigo: 'SIN-002',
    nombre: 'Dificultad Respiratoria Severa (Disnea)',
    descripcion: 'Dificultad severa para ventilar en reposo, estridor o tiraje intercostal',
    esSignoAlarma: true,
    categoria: 'Respiratorio',
    nivelGravedadBase: 5
  },
  {
    id: 3,
    codigo: 'SIN-003',
    nombre: 'Pérdida Súbita del Estado de Alerta',
    descripcion: 'Síncope, confusión aguda o escala de Glasgow menor a 13',
    esSignoAlarma: true,
    categoria: 'Neurológico',
    nivelGravedadBase: 5
  },
  {
    id: 4,
    codigo: 'SIN-004',
    nombre: 'Fiebre Elevada (> 38.5 °C)',
    descripcion: 'Registro térmico axilar mayor a 38.5 °C con escalofríos',
    esSignoAlarma: false,
    categoria: 'Infeccioso',
    nivelGravedadBase: 3
  },
  {
    id: 5,
    codigo: 'SIN-005',
    nombre: 'Cefalea Moderada',
    descripcion: 'Dolor craneal sin signos meníngeos ni focalización motora',
    esSignoAlarma: false,
    categoria: 'Neurológico',
    nivelGravedadBase: 2
  },
  {
    id: 6,
    codigo: 'SIN-006',
    nombre: 'Trauma Menor en Extremidad',
    descripcion: 'Contusión o esguince en brazo o pierna sin deformidad ósea evidente',
    esSignoAlarma: false,
    categoria: 'Traumatología',
    nivelGravedadBase: 2
  }
];

const INITIAL_PACIENTES = [
  {
    id: 1,
    dni: '0801-1995-12345',
    nombreCompleto: 'Juan Carlos Rodríguez',
    edad: 45,
    genero: 'Masculino',
    telefono: '+504 9876-5432',
    fechaRegistro: '2026-01-15T08:30:00Z',
    totalEvaluaciones: 1
  },
  {
    id: 2,
    dni: '0801-1950-67890',
    nombreCompleto: 'María Elena Castro',
    edad: 76,
    genero: 'Femenino',
    telefono: '+504 9911-2233',
    fechaRegistro: '2026-01-16T09:15:00Z',
    totalEvaluaciones: 0
  },
  {
    id: 3,
    dni: '0801-2002-33445',
    nombreCompleto: 'Andrés Fernando López',
    edad: 24,
    genero: 'Masculino',
    telefono: '+504 9455-6677',
    fechaRegistro: '2026-01-17T11:00:00Z',
    totalEvaluaciones: 0
  }
];

const INITIAL_EVALUACIONES = [
  {
    id: 1,
    pacienteId: 1,
    nombrePaciente: 'Juan Carlos Rodríguez',
    edadPaciente: 45,
    fechaRegistro: '2026-01-15T08:45:00Z',
    temperatura: 37.2,
    frecuenciaCardiaca: 115,
    presionArterial: '140/90',
    saturacionOxigeno: 88,
    dolorToracico: true,
    dificultadRespiratoria: true,
    sintomasPrincipales: 'Dolor Torácico Opresivo, Dificultad Respiratoria Severa',
    prioridadAsignada: 1,
    codigoColor: 'Rojo',
    nombrePrioridad: 'Nivel 1 - Resucitación',
    tiempoMaximoEsperaMinutos: 0,
    areaDerivacion: 'Sala de Shock Trauma / Box de Reanimación',
    accionInmediata: 'Atención médica y de enfermería inmediata continua. Monitoreo invasivo y soporte vital.',
    justificacionRegla: 'REGLA-01: Dolor torácico + Disnea con SpO2 crítica (<90%). Código Rojo.',
    requiereAtencionInmediata: true,
    observacionesClinicas: 'Paciente remitido inmediatamente a Box de Reanimación.'
  }
];

// Estado local de respaldo en memoria
let localState = {
  protocolos: [...INITIAL_PROTOCOLOS],
  sintomas: [...INITIAL_SINTOMAS],
  pacientes: [...INITIAL_PACIENTES],
  evaluaciones: [...INITIAL_EVALUACIONES]
};

// Réplica exacta de TriageService.cs para modo offline/demo si la API no responde
function procesarInferenciaLocal(paciente, dto, protocolos) {
  let prioridad = 5;
  let justificacion = '';
  let requiereInmediata = false;

  const temp = Number(dto.temperatura);
  const fc = Number(dto.frecuenciaCardiaca);
  const spo2 = Number(dto.saturacionOxigeno);
  const edad = Number(paciente.edad);

  if ((dto.dolorToracico && dto.dificultadRespiratoria) || spo2 < 90 || fc > 150 || fc < 40) {
    prioridad = 1;
    requiereInmediata = true;
    justificacion =
      'REGLA-01 [NIVEL 1]: Paciente con signos de paro inminente, hipoxemia severa (SpO2 < 90%) o compromiso hemodinámico extremo. Atención inmediata en Shock Trauma.';
  } else if (
    dto.dificultadRespiratoria ||
    dto.dolorToracico ||
    temp >= 39.5 ||
    (spo2 >= 90 && spo2 <= 93) ||
    fc > 125
  ) {
    prioridad = 2;
    requiereInmediata = true;
    justificacion =
      'REGLA-02 [NIVEL 2]: Dificultad respiratoria detectada, dolor torácico aislado, fiebre muy alta (>39.5°C) o taquicardia severa. Atención en menos de 10 min.';
  } else if (temp >= 38.0 || fc > 100 || edad >= 75 || edad <= 1) {
    prioridad = 3;
    requiereInmediata = false;
    justificacion = `REGLA-03 [NIVEL 3]: Síndrome febril (Temp: ${temp}°C) o paciente en grupo de riesgo etario (${edad} años). Atención en menos de 60 min.`;
  } else if (
    (dto.sintomasPrincipales && dto.sintomasPrincipales.toLowerCase().includes('leve')) ||
    temp > 37.5
  ) {
    prioridad = 4;
    requiereInmediata = false;
    justificacion =
      'REGLA-04 [NIVEL 4]: Signos vitales normales con síntomas agudos leves a moderados sin bandera roja. Atención en menos de 120 min.';
  } else {
    prioridad = 5;
    requiereInmediata = false;
    justificacion =
      'REGLA-05 [NIVEL 5]: Parámetros fisiológicos estables sin banderas rojas identificadas. Atención en menos de 240 min.';
  }

  const protocolo = protocolos.find((p) => p.nivelPrioridad === prioridad) || protocolos[4];

  return {
    id: Date.now(),
    pacienteId: paciente.id,
    nombrePaciente: paciente.nombreCompleto,
    edadPaciente: paciente.edad,
    fechaRegistro: new Date().toISOString(),
    temperatura: temp,
    frecuenciaCardiaca: fc,
    presionArterial: dto.presionArterial,
    saturacionOxigeno: spo2,
    dolorToracico: Boolean(dto.dolorToracico),
    dificultadRespiratoria: Boolean(dto.dificultadRespiratoria),
    sintomasPrincipales: dto.sintomasPrincipales,
    prioridadAsignada: prioridad,
    codigoColor: protocolo.codigoColor,
    nombrePrioridad: protocolo.nombreNivel,
    tiempoMaximoEsperaMinutos: protocolo.tiempoMaximoEsperaMinutos,
    areaDerivacion: protocolo.areaDerivacion,
    accionInmediata: protocolo.accionInmediata,
    justificacionRegla: justificacion,
    requiereAtencionInmediata: requiereInmediata,
    observacionesClinicas: dto.observacionesClinicas || ''
  };
}

async function request(endpoint, options = {}) {
  const fetchOptions = {
    headers: {
      'Content-Type': 'application/json',
      ...(options.headers || {})
    },
    ...options
  };

  let response;
  try {
    response = await fetch(`${API_BASE_URL}${endpoint}`, fetchOptions);
    const contentType = response.headers.get('content-type') || '';
    if (response.status !== 204 && !contentType.includes('application/json') && API_BASE_URL !== FALLBACK_DIRECT_API_URL) {
      response = await fetch(`${FALLBACK_DIRECT_API_URL}${endpoint}`, fetchOptions);
    }
  } catch {
    if (API_BASE_URL !== FALLBACK_DIRECT_API_URL) {
      response = await fetch(`${FALLBACK_DIRECT_API_URL}${endpoint}`, fetchOptions);
    } else {
      throw new Error('Error de red al conectar con el servidor API.');
    }
  }

  if (!response.ok) {
    let errorBody = null;
    try {
      errorBody = await response.json();
    } catch {
      // ignore json parse error
    }
    const msg =
      errorBody?.errores?.join(' | ') ||
      errorBody?.mensaje ||
      `Error HTTP ${response.status}`;
    throw new Error(msg);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}

export const triageApi = {
  baseUrl: API_BASE_URL,

  // 1. Estado del Motor Experto
  async getEstadoMotor() {
    try {
      const data = await request('/triage/estado');
      return { online: true, data };
    } catch {
      return {
        online: false,
        data: {
          estado: 'Motor Experto TriageDoc v2.0 - Sistema Manchester 5 Niveles (Modo Simulación Local)',
          motor: 'InferenceEngine-Manchester-v2',
          timestamp: new Date().toISOString(),
          tablasCore: ['Pacientes', 'Sintomas', 'ProtocolosAtencion', 'EvaluacionesTriage']
        }
      };
    }
  },

  // 2. Evaluaciones de Triage
  async getEvaluaciones(online = true) {
    if (!online) return [...localState.evaluaciones].sort((a, b) => a.prioridadAsignada - b.prioridadAsignada);
    return request('/evaluaciones');
  },

  async getEvaluacionesByPaciente(pacienteId, online = true) {
    if (!online) {
      return localState.evaluaciones.filter((e) => e.pacienteId === Number(pacienteId));
    }
    return request(`/evaluaciones/paciente/${pacienteId}`);
  },

  async createEvaluacion(dto, online = true) {
    if (!online) {
      const paciente = localState.pacientes.find((p) => p.id === Number(dto.pacienteId));
      if (!paciente) throw new Error('Paciente no encontrado.');
      const nueva = procesarInferenciaLocal(paciente, dto, localState.protocolos);
      localState.evaluaciones = [nueva, ...localState.evaluaciones];
      paciente.totalEvaluaciones = (paciente.totalEvaluaciones || 0) + 1;
      return nueva;
    }
    return request('/evaluaciones', {
      method: 'POST',
      body: JSON.stringify(dto)
    });
  },

  async deleteEvaluacion(id, online = true) {
    if (!online) {
      localState.evaluaciones = localState.evaluaciones.filter((e) => e.id !== id);
      return null;
    }
    return request(`/evaluaciones/${id}`, { method: 'DELETE' });
  },

  // 3. Pacientes (CRUD Completo)
  async getPacientes(online = true) {
    if (!online) return [...localState.pacientes];
    return request('/pacientes');
  },

  async createPaciente(dto, online = true) {
    if (!online) {
      const nuevo = {
        id: Date.now(),
        dni: dto.dni,
        nombreCompleto: dto.nombreCompleto,
        edad: Number(dto.edad),
        genero: dto.genero,
        telefono: dto.telefono || '',
        fechaRegistro: new Date().toISOString(),
        totalEvaluaciones: 0
      };
      localState.pacientes = [...localState.pacientes, nuevo];
      return nuevo;
    }
    return request('/pacientes', {
      method: 'POST',
      body: JSON.stringify(dto)
    });
  },

  async updatePaciente(id, dto, online = true) {
    if (!online) {
      localState.pacientes = localState.pacientes.map((p) =>
        p.id === id ? { ...p, ...dto, edad: Number(dto.edad) } : p
      );
      return null;
    }
    return request(`/pacientes/${id}`, {
      method: 'PUT',
      body: JSON.stringify(dto)
    });
  },

  async deletePaciente(id, online = true) {
    if (!online) {
      localState.pacientes = localState.pacientes.filter((p) => p.id !== id);
      return null;
    }
    return request(`/pacientes/${id}`, { method: 'DELETE' });
  },

  // 4. Síntomas (CRUD + Filtro Banderas Rojas)
  async getSintomas(soloAlarmas = false, online = true) {
    if (!online) {
      return soloAlarmas
        ? localState.sintomas.filter((s) => s.esSignoAlarma)
        : [...localState.sintomas];
    }
    return request(soloAlarmas ? '/sintomas/alarmas' : '/sintomas');
  },

  async createSintoma(dto, online = true) {
    if (!online) {
      const nuevo = {
        id: Date.now(),
        ...dto,
        nivelGravedadBase: Number(dto.nivelGravedadBase)
      };
      localState.sintomas = [...localState.sintomas, nuevo];
      return nuevo;
    }
    return request('/sintomas', {
      method: 'POST',
      body: JSON.stringify(dto)
    });
  },

  async updateSintoma(id, dto, online = true) {
    if (!online) {
      localState.sintomas = localState.sintomas.map((s) =>
        s.id === id ? { ...s, ...dto, nivelGravedadBase: Number(dto.nivelGravedadBase) } : s
      );
      return null;
    }
    return request(`/sintomas/${id}`, {
      method: 'PUT',
      body: JSON.stringify(dto)
    });
  },

  async deleteSintoma(id, online = true) {
    if (!online) {
      localState.sintomas = localState.sintomas.filter((s) => s.id !== id);
      return null;
    }
    return request(`/sintomas/${id}`, { method: 'DELETE' });
  },

  // 5. Protocolos de Atención Manchester
  async getProtocolos(online = true) {
    if (!online) return [...localState.protocolos];
    return request('/protocolos');
  },

  async updateProtocolo(id, dto, online = true) {
    if (!online) {
      localState.protocolos = localState.protocolos.map((p) =>
        p.id === id
          ? {
              ...p,
              ...dto,
              tiempoMaximoEsperaMinutos: Number(dto.tiempoMaximoEsperaMinutos)
            }
          : p
      );
      return null;
    }
    return request(`/protocolos/${id}`, {
      method: 'PUT',
      body: JSON.stringify(dto)
    });
  }
};
