import React, { useState } from 'react';

const CASOS_CLINICOS = [
  {
    titulo: 'Caso 1: Emergencia Vital (Código Rojo)',
    datos: {
      temperatura: 37.0,
      frecuenciaCardiaca: 135,
      presionArterial: '150/100',
      saturacionOxigeno: 87,
      dolorToracico: true,
      dificultadRespiratoria: true,
      sintomasPrincipales: 'Dolor retroesternal punzante con falta de aire aguda',
      observacionesClinicas: 'Paciente sudoroso y pálido en sala de espera'
    }
  },
  {
    titulo: 'Caso 2: Fiebre Crítica (Código Naranja)',
    datos: {
      temperatura: 39.8,
      frecuenciaCardiaca: 128,
      presionArterial: '130/85',
      saturacionOxigeno: 94,
      dolorToracico: false,
      dificultadRespiratoria: false,
      sintomasPrincipales: 'Fiebre persistente muy alta y desorientación leve',
      observacionesClinicas: 'Adulto mayor con deshidratación'
    }
  },
  {
    titulo: 'Caso 3: Cuadro Febril Moderado (Código Amarillo)',
    datos: {
      temperatura: 38.4,
      frecuenciaCardiaca: 105,
      presionArterial: '125/82',
      saturacionOxigeno: 97,
      dolorToracico: false,
      dificultadRespiratoria: false,
      sintomasPrincipales: 'Fiebre Elevada (> 38.5 °C), Cefalea Moderada',
      observacionesClinicas: 'Escalofríos desde hace 12 horas'
    }
  },
  {
    titulo: 'Caso 4: Consulta Menor (Código Verde / Azul)',
    datos: {
      temperatura: 36.6,
      frecuenciaCardiaca: 74,
      presionArterial: '120/80',
      saturacionOxigeno: 99,
      dolorToracico: false,
      dificultadRespiratoria: false,
      sintomasPrincipales: 'Molestia nasal y dolor de garganta leve',
      observacionesClinicas: 'Paciente camina sin apoyo y hemodinámicamente estable'
    }
  }
];

export default function TriageEvaluationForm({
  pacientes,
  sintomas,
  onEvaluar,
  onQuickCreatePaciente
}) {
  const [form, setForm] = useState({
    pacienteId: pacientes[0]?.id || 1,
    temperatura: 36.8,
    frecuenciaCardiaca: 78,
    presionArterial: '120/80',
    saturacionOxigeno: 98,
    dolorToracico: false,
    dificultadRespiratoria: false,
    sintomasPrincipales: '',
    observacionesClinicas: ''
  });

  const [ultimoResultado, setUltimoResultado] = useState(null);
  const [cargando, setCargando] = useState(false);
  const [error, setError] = useState('');
  const [mostrarNuevoPaciente, setMostrarNuevoPaciente] = useState(false);
  const [nuevoPac, setNuevoPac] = useState({
    dni: '',
    nombreCompleto: '',
    edad: 30,
    genero: 'Masculino',
    telefono: ''
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const agregarSintomaCatalogo = (sintoma) => {
    setForm((prev) => {
      const actual = prev.sintomasPrincipales.trim();
      const nuevoTexto = actual
        ? actual.includes(sintoma.nombre)
          ? actual
          : `${actual}, ${sintoma.nombre}`
        : sintoma.nombre;

      return {
        ...prev,
        sintomasPrincipales: nuevoTexto,
        dolorToracico: prev.dolorToracico || sintoma.codigo === 'SIN-001',
        dificultadRespiratoria:
          prev.dificultadRespiratoria || sintoma.codigo === 'SIN-002'
      };
    });
  };

  const cargarCaso = (caso) => {
    setError('');
    setForm((prev) => ({
      ...prev,
      ...caso.datos
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setCargando(true);
    try {
      const payload = {
        ...form,
        pacienteId: Number(form.pacienteId),
        temperatura: Number(form.temperatura),
        frecuenciaCardiaca: Number(form.frecuenciaCardiaca),
        saturacionOxigeno: Number(form.saturacionOxigeno)
      };
      const res = await onEvaluar(payload);
      setUltimoResultado(res);
    } catch (err) {
      setError(err.message || 'Error al procesar evaluación de triage.');
    } finally {
      setCargando(false);
    }
  };

  const handleGuardarPacienteRapido = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const creado = await onQuickCreatePaciente(nuevoPac);
      setForm((prev) => ({ ...prev, pacienteId: creado.id }));
      setMostrarNuevoPaciente(false);
      setNuevoPac({
        dni: '',
        nombreCompleto: '',
        edad: 30,
        genero: 'Masculino',
        telefono: ''
      });
    } catch (err) {
      setError(err.message || 'No se pudo registrar el paciente.');
    }
  };

  return (
    <div>
      <div className="page-title-bar">
        <h1>Motor de Inferencia Clínica (Triage Manchester)</h1>
        <button
          type="button"
          className="btn btn-default"
          onClick={() => setMostrarNuevoPaciente(!mostrarNuevoPaciente)}
        >
          {mostrarNuevoPaciente ? 'Cancelar Registro' : 'Nuevo Paciente Rápido'}
        </button>
      </div>

      {error && (
        <div className="alert-banner error">
          <span>Error de Validación: {error}</span>
          <button
            type="button"
            className="btn btn-default btn-sm"
            onClick={() => setError('')}
          >
            Cerrar
          </button>
        </div>
      )}

      {mostrarNuevoPaciente && (
        <form
          onSubmit={handleGuardarPacienteRapido}
          className="panel"
          style={{ backgroundColor: '#f9f9f9' }}
        >
          <div className="panel-header">
            <h2>Registro Rápido de Filiación de Paciente</h2>
          </div>
          <div className="form-grid">
            <div className="form-group">
              <label>DNI (0000-0000-00000)</label>
              <input
                className="form-input"
                placeholder="0801-1990-12345"
                value={nuevoPac.dni}
                onChange={(e) =>
                  setNuevoPac({ ...nuevoPac, dni: e.target.value })
                }
                required
              />
            </div>
            <div className="form-group">
              <label>Nombre Completo</label>
              <input
                className="form-input"
                placeholder="Ej. Roberto Carlos Gómez"
                value={nuevoPac.nombreCompleto}
                onChange={(e) =>
                  setNuevoPac({ ...nuevoPac, nombreCompleto: e.target.value })
                }
                required
              />
            </div>
            <div className="form-group">
              <label>Edad (Años)</label>
              <input
                type="number"
                className="form-input"
                value={nuevoPac.edad}
                onChange={(e) =>
                  setNuevoPac({ ...nuevoPac, edad: e.target.value })
                }
                required
              />
            </div>
            <div className="form-group">
              <label>Género</label>
              <select
                className="form-select"
                value={nuevoPac.genero}
                onChange={(e) =>
                  setNuevoPac({ ...nuevoPac, genero: e.target.value })
                }
              >
                <option value="Masculino">Masculino</option>
                <option value="Femenino">Femenino</option>
              </select>
            </div>
          </div>
          <button type="submit" className="btn btn-primary">
            Guardar y Seleccionar Paciente
          </button>
        </form>
      )}

      <div className="two-col-grid">
        {/* Columna Izquierda: Formulario de Signos Vitales */}
        <div className="panel">
          <div className="panel-header">
            <h2>Registro de Signos Vitales y Cuadro Clínico</h2>
          </div>

          <div style={{ marginBottom: '15px' }}>
            <label
              style={{
                display: 'block',
                fontSize: '12px',
                color: '#777777',
                marginBottom: '6px',
                fontWeight: 700
              }}
            >
              PLANTILLAS DE PRUEBA DEL MOTOR EXPERTO:
            </label>
            <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6px' }}>
              {CASOS_CLINICOS.map((c, idx) => (
                <button
                  key={idx}
                  type="button"
                  className="btn btn-default btn-sm"
                  onClick={() => cargarCaso(c)}
                >
                  {c.titulo}
                </button>
              ))}
            </div>
          </div>

          <form onSubmit={handleSubmit}>
            <div className="form-group" style={{ marginBottom: '14px' }}>
              <label>Paciente Ingresado</label>
              <select
                name="pacienteId"
                className="form-select"
                value={form.pacienteId}
                onChange={handleChange}
                required
              >
                {pacientes.map((p) => (
                  <option key={p.id} value={p.id}>
                    #{p.id} - {p.nombreCompleto} (DNI: {p.dni} | {p.edad} años)
                  </option>
                ))}
              </select>
            </div>

            <div className="form-grid">
              <div className="form-group">
                <label>Temperatura (°C)</label>
                <input
                  type="number"
                  step="0.1"
                  name="temperatura"
                  className="form-input"
                  value={form.temperatura}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="form-group">
                <label>Frecuencia Cardíaca (lpm)</label>
                <input
                  type="number"
                  name="frecuenciaCardiaca"
                  className="form-input"
                  value={form.frecuenciaCardiaca}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="form-group">
                <label>Presión Arterial (mmHg)</label>
                <input
                  type="text"
                  name="presionArterial"
                  className="form-input"
                  placeholder="120/80"
                  value={form.presionArterial}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="form-group">
                <label>Saturación SpO2 (%)</label>
                <input
                  type="number"
                  name="saturacionOxigeno"
                  className="form-input"
                  value={form.saturacionOxigeno}
                  onChange={handleChange}
                  required
                />
              </div>
            </div>

            <label
              style={{ fontSize: '13px', fontWeight: 700, color: '#333333' }}
            >
              Banderas Rojas Críticas (Signos de Alarma Inmediata)
            </label>
            <div className="checkbox-row">
              <label
                className={`alarm-toggle ${form.dolorToracico ? 'checked' : ''}`}
              >
                <input
                  type="checkbox"
                  name="dolorToracico"
                  checked={form.dolorToracico}
                  onChange={handleChange}
                />
                Dolor Torácico Opresivo
              </label>

              <label
                className={`alarm-toggle ${
                  form.dificultadRespiratoria ? 'checked' : ''
                }`}
              >
                <input
                  type="checkbox"
                  name="dificultadRespiratoria"
                  checked={form.dificultadRespiratoria}
                  onChange={handleChange}
                />
                Dificultad Respiratoria Severa (Disnea)
              </label>
            </div>

            <div className="form-group" style={{ marginBottom: '12px' }}>
              <label>
                Síntomas Principales (Ingrese texto o seleccione del catálogo)
              </label>
              <textarea
                name="sintomasPrincipales"
                rows={2}
                className="form-textarea"
                placeholder="Ej. Dolor retroesternal, fiebre, cefalea..."
                value={form.sintomasPrincipales}
                onChange={handleChange}
                required
              />
              <div className="symptom-tags">
                {sintomas.map((s) => (
                  <span
                    key={s.id}
                    className={`symptom-tag ${
                      s.esSignoAlarma ? 'red-flag' : ''
                    }`}
                    onClick={() => agregarSintomaCatalogo(s)}
                    title={s.descripcion}
                  >
                    {s.esSignoAlarma ? '[ALARMA] ' : '+ '}
                    {s.nombre}
                  </span>
                ))}
              </div>
            </div>

            <div className="form-group" style={{ marginBottom: '16px' }}>
              <label>Observaciones Clínicas de Enfermería</label>
              <input
                type="text"
                name="observacionesClinicas"
                className="form-input"
                placeholder="Estado general del paciente, antecedentes relevantes..."
                value={form.observacionesClinicas}
                onChange={handleChange}
              />
            </div>

            <button
              type="submit"
              className="btn btn-primary"
              style={{ width: '100%', padding: '9px 14px', fontSize: '14px' }}
              disabled={cargando}
            >
              {cargando
                ? 'Procesando Reglas de Inferencia...'
                : 'Ejecutar Inferencia y Clasificar Paciente'}
            </button>
          </form>
        </div>

        {/* Columna Derecha: Dictamen del Motor Experto */}
        <div className="panel">
          <div className="panel-header">
            <h2>Resultado de Clasificación Manchester (MTS)</h2>
          </div>

          {!ultimoResultado ? (
            <div
              style={{
                textAlign: 'center',
                padding: '45px 20px',
                color: '#777777',
                border: '1px dashed #cccccc',
                borderRadius: '4px',
                backgroundColor: '#fafafa'
              }}
            >
              <p
                style={{
                  fontWeight: 700,
                  color: '#333333',
                  marginBottom: '6px',
                  fontSize: '15px'
                }}
              >
                Pendiente de Evaluación Clínica
              </p>
              <p style={{ fontSize: '13px' }}>
                Complete los parámetros fisiológicos a la izquierda o seleccione
                una plantilla de caso clínico para obtener el dictamen del
                sistema experto.
              </p>
            </div>
          ) : (
            <div className={`eval-card border-${ultimoResultado.codigoColor}`}>
              <div className="eval-top">
                <div>
                  <span className="patient-name">
                    {ultimoResultado.nombrePaciente}
                  </span>
                  <p style={{ fontSize: '12.5px', color: '#777777' }}>
                    Edad: {ultimoResultado.edadPaciente} años | Registro #
                    {ultimoResultado.id}
                  </p>
                </div>
                <span
                  className={`badge-mts badge-${ultimoResultado.codigoColor}`}
                >
                  Código {ultimoResultado.codigoColor} —{' '}
                  {ultimoResultado.nombrePrioridad}
                </span>
              </div>

              <div className="vitals-row">
                <span
                  className={`vital-chip ${
                    ultimoResultado.temperatura >= 38.0 ? 'alert' : ''
                  }`}
                >
                  Temp: {ultimoResultado.temperatura} °C
                </span>
                <span
                  className={`vital-chip ${
                    ultimoResultado.frecuenciaCardiaca > 100 ? 'alert' : ''
                  }`}
                >
                  FC: {ultimoResultado.frecuenciaCardiaca} lpm
                </span>
                <span
                  className={`vital-chip ${
                    ultimoResultado.saturacionOxigeno < 94 ? 'alert' : ''
                  }`}
                >
                  SpO2: {ultimoResultado.saturacionOxigeno}%
                </span>
                <span className="vital-chip">
                  PA: {ultimoResultado.presionArterial}
                </span>
              </div>

              <div
                style={{
                  display: 'grid',
                  gridTemplateColumns: '1fr 1fr',
                  gap: '12px',
                  background: '#f5f5f5',
                  padding: '12px',
                  borderRadius: '4px',
                  border: '1px solid #dddddd'
                }}
              >
                <div>
                  <span
                    style={{
                      fontSize: '11px',
                      color: '#777777',
                      display: 'block',
                      fontWeight: 700
                    }}
                  >
                    TIEMPO MÁXIMO DE ESPERA
                  </span>
                  <strong style={{ fontSize: '15px', color: '#333333' }}>
                    {ultimoResultado.tiempoMaximoEsperaMinutos === 0
                      ? '0 min (Atención Inmediata)'
                      : `${ultimoResultado.tiempoMaximoEsperaMinutos} minutos`}
                  </strong>
                </div>
                <div>
                  <span
                    style={{
                      fontSize: '11px',
                      color: '#777777',
                      display: 'block',
                      fontWeight: 700
                    }}
                  >
                    ÁREA DE DERIVACIÓN
                  </span>
                  <strong style={{ fontSize: '13.5px', color: '#428bca' }}>
                    {ultimoResultado.areaDerivacion}
                  </strong>
                </div>
              </div>

              <div className="rule-box">
                <strong
                  style={{
                    display: 'block',
                    marginBottom: '4px',
                    color: '#333333'
                  }}
                >
                  Justificación de la Regla Inferida:
                </strong>
                {ultimoResultado.justificacionRegla}
              </div>

              {ultimoResultado.accionInmediata && (
                <div style={{ fontSize: '13px', color: '#555555' }}>
                  <strong>Protocolo Clínico:</strong>{' '}
                  {ultimoResultado.accionInmediata}
                </div>
              )}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
