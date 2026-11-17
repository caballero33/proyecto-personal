import React, { useState } from 'react';
import { IconSearch, IconPatient, IconMed, IconTriage } from './Icons';

const NIVELES_MTS = [
  { color: 'Rojo', nombre: 'Nivel 1 - Resucitación', espera: '0 min (Inmediato)' },
  { color: 'Naranja', nombre: 'Nivel 2 - Emergencia', espera: 'Máx. 10 min' },
  { color: 'Amarillo', nombre: 'Nivel 3 - Urgencia', espera: 'Máx. 60 min' },
  { color: 'Verde', nombre: 'Nivel 4 - Poco Urgente', espera: 'Máx. 120 min' },
  { color: 'Azul', nombre: 'Nivel 5 - No Urgente', espera: 'Máx. 240 min' }
];

export default function MedicalDashboard({
  evaluaciones,
  pacientes = [],
  onDeleteEvaluacion,
  onGoToNewTriage
}) {
  const [filtroColor, setFiltroColor] = useState('TODOS');
  const [filtroPaciente, setFiltroPaciente] = useState('TODOS');
  const [filtroFecha, setFiltroFecha] = useState('');
  const [busqueda, setBusqueda] = useState('');
  const [detalleSeleccionado, setDetalleSeleccionado] = useState(null);

  const conteoPorColor = (color) =>
    evaluaciones.filter(
      (e) => (e.codigoColor || '').toLowerCase() === color.toLowerCase()
    ).length;

  const evaluacionesFiltradas = evaluaciones.filter((e) => {
    const pasaColor =
      filtroColor === 'TODOS' ||
      (e.codigoColor || '').toLowerCase() === filtroColor.toLowerCase();
    const pasaPaciente =
      filtroPaciente === 'TODOS' ||
      String(e.pacienteId) === String(filtroPaciente);
    const pasaFecha =
      !filtroFecha ||
      (e.fechaRegistro && e.fechaRegistro.startsWith(filtroFecha));
    const texto = `${e.nombrePaciente} ${e.sintomasPrincipales} ${e.justificacionRegla} ${e.areaDerivacion}`.toLowerCase();
    const pasaBusqueda =
      !busqueda.trim() || texto.includes(busqueda.toLowerCase());
    return pasaColor && pasaPaciente && pasaFecha && pasaBusqueda;
  });

  const limpiarFiltros = () => {
    setFiltroColor('TODOS');
    setFiltroPaciente('TODOS');
    setFiltroFecha('');
    setBusqueda('');
  };

  return (
    <div>
      {/* Cabecera estilo BookMedik */}
      <div className="page-title-bar">
        <h1>Evaluaciones de Triage</h1>
        <div style={{ display: 'flex', gap: '8px' }}>
          {(filtroColor !== 'TODOS' ||
            filtroPaciente !== 'TODOS' ||
            filtroFecha ||
            busqueda) && (
            <button
              type="button"
              className="btn btn-default"
              onClick={limpiarFiltros}
            >
              Restablecer Filtros
            </button>
          )}
          <button
            type="button"
            className="btn btn-default"
            onClick={onGoToNewTriage}
          >
            Nueva Evaluación Clínica
          </button>
        </div>
      </div>

      {/* Barra de Búsqueda con Input-Group Addons (Idéntica a la referencia) */}
      <div className="filter-row">
        <div className="input-group">
          <span className="input-group-addon">
            <IconSearch />
          </span>
          <input
            type="text"
            className="form-input"
            placeholder="Palabra clave o síntoma"
            value={busqueda}
            onChange={(e) => setBusqueda(e.target.value)}
          />
        </div>

        <div className="input-group">
          <span className="input-group-addon">
            <IconPatient />
          </span>
          <select
            className="form-select"
            value={filtroPaciente}
            onChange={(e) => setFiltroPaciente(e.target.value)}
          >
            <option value="TODOS">PACIENTE (TODOS)</option>
            {pacientes.map((p) => (
              <option key={p.id} value={p.id}>
                {p.nombreCompleto}
              </option>
            ))}
          </select>
        </div>

        <div className="input-group">
          <span className="input-group-addon">
            <IconMed />
          </span>
          <select
            className="form-select"
            value={filtroColor}
            onChange={(e) => setFiltroColor(e.target.value)}
          >
            <option value="TODOS">PRIORIDAD MTS (TODAS)</option>
            <option value="Rojo">Código Rojo (Nivel 1)</option>
            <option value="Naranja">Código Naranja (Nivel 2)</option>
            <option value="Amarillo">Código Amarillo (Nivel 3)</option>
            <option value="Verde">Código Verde (Nivel 4)</option>
            <option value="Azul">Código Azul (Nivel 5)</option>
          </select>
        </div>

        <div className="input-group">
          <span className="input-group-addon">
            <IconTriage />
          </span>
          <input
            type="date"
            className="form-input"
            value={filtroFecha}
            onChange={(e) => setFiltroFecha(e.target.value)}
          />
        </div>

        <button
          type="button"
          className="btn btn-primary"
          style={{ width: '100%', height: '34px' }}
          onClick={() => {}}
        >
          Buscar
        </button>
      </div>

      {/* Indicadores de Prioridad Manchester */}
      <div className="mts-grid">
        {NIVELES_MTS.map((nivel) => {
          const activo = filtroColor === nivel.color;
          const total = conteoPorColor(nivel.color);
          return (
            <div
              key={nivel.color}
              className={`mts-card color-${nivel.color} ${
                activo ? 'active-filter' : ''
              }`}
              onClick={() => setFiltroColor(activo ? 'TODOS' : nivel.color)}
            >
              <div className="mts-card-header">
                <span className="mts-card-title">Código {nivel.color}</span>
                <span className="mts-count">{total}</span>
              </div>
              <div style={{ fontSize: '12.5px', fontWeight: 600 }}>
                {nivel.nombre}
              </div>
              <div className="mts-meta">Tiempo objetivo: {nivel.espera}</div>
            </div>
          );
        })}
      </div>

      {/* Detalle Expandido de Regla Clínica (cuando se selecciona una fila) */}
      {detalleSeleccionado && (
        <div
          className={`eval-card border-${
            detalleSeleccionado.codigoColor || 'Azul'
          }`}
          style={{ marginBottom: '18px' }}
        >
          <div className="eval-top">
            <div>
              <span className="patient-name">
                Dictamen Clínico #{detalleSeleccionado.id} —{' '}
                {detalleSeleccionado.nombrePaciente} (
                {detalleSeleccionado.edadPaciente} años)
              </span>
              <div style={{ fontSize: '12.5px', color: '#777777' }}>
                Área de Derivación: {detalleSeleccionado.areaDerivacion} | Tiempo
                Máximo de Espera: {detalleSeleccionado.tiempoMaximoEsperaMinutos}{' '}
                minutos
              </div>
            </div>
            <div style={{ display: 'flex', gap: '8px', alignItems: 'center' }}>
              <span
                className={`badge-mts badge-${
                  detalleSeleccionado.codigoColor || 'Azul'
                }`}
              >
                Código {detalleSeleccionado.codigoColor} —{' '}
                {detalleSeleccionado.nombrePrioridad}
              </span>
              <button
                type="button"
                className="btn btn-default btn-sm"
                onClick={() => setDetalleSeleccionado(null)}
              >
                Cerrar Detalle
              </button>
            </div>
          </div>

          <div className="vitals-row">
            <span
              className={`vital-chip ${
                detalleSeleccionado.temperatura >= 38.0 ? 'alert' : ''
              }`}
            >
              Temperatura: {detalleSeleccionado.temperatura} °C
            </span>
            <span
              className={`vital-chip ${
                detalleSeleccionado.frecuenciaCardiaca > 100 ||
                detalleSeleccionado.frecuenciaCardiaca < 50
                  ? 'alert'
                  : ''
              }`}
            >
              Frecuencia Cardíaca: {detalleSeleccionado.frecuenciaCardiaca} lpm
            </span>
            <span
              className={`vital-chip ${
                detalleSeleccionado.saturacionOxigeno < 94 ? 'alert' : ''
              }`}
            >
              Saturación SpO2: {detalleSeleccionado.saturacionOxigeno}%
            </span>
            <span className="vital-chip">
              Presión Arterial: {detalleSeleccionado.presionArterial}
            </span>
            {detalleSeleccionado.dolorToracico && (
              <span className="vital-chip alert">Dolor Torácico: Sí</span>
            )}
            {detalleSeleccionado.dificultadRespiratoria && (
              <span className="vital-chip alert">
                Dificultad Respiratoria: Sí
              </span>
            )}
          </div>

          <div className="rule-box">
            <strong>Justificación del Motor Experto: </strong>
            {detalleSeleccionado.justificacionRegla}
          </div>

          {detalleSeleccionado.accionInmediata && (
            <div style={{ fontSize: '12.5px', color: '#555555' }}>
              <strong>Acción Inmediata Protocolizada: </strong>
              {detalleSeleccionado.accionInmediata}
            </div>
          )}
        </div>
      )}

      {/* Tabla Principal con Bordes Limpios (Estilo BookMedik) */}
      <div className="table-wrap">
        <table className="data-table">
          <thead>
            <tr>
              <th>Motivo / Síntomas</th>
              <th>Paciente</th>
              <th>Área de Derivación</th>
              <th>Signos Vitales</th>
              <th>Prioridad MTS</th>
              <th>Fecha</th>
              <th style={{ width: '165px' }}>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {evaluacionesFiltradas.length === 0 ? (
              <tr>
                <td
                  colSpan={7}
                  style={{ textAlign: 'center', color: '#777777', padding: '24px' }}
                >
                  No se encontraron registros que coincidan con los criterios de
                  búsqueda.
                </td>
              </tr>
            ) : (
              evaluacionesFiltradas.map((ev) => (
                <tr key={ev.id}>
                  <td>
                    <strong>{ev.sintomasPrincipales}</strong>
                    {ev.requiereAtencionInmediata && (
                      <div
                        style={{
                          fontSize: '11px',
                          color: '#d9534f',
                          fontWeight: 700
                        }}
                      >
                        ATENCIÓN INMEDIATA REQUERIDA
                      </div>
                    )}
                  </td>
                  <td>
                    {ev.nombrePaciente}
                    <div style={{ fontSize: '12px', color: '#777777' }}>
                      {ev.edadPaciente} años
                    </div>
                  </td>
                  <td>
                    {ev.areaDerivacion}
                    <div style={{ fontSize: '12px', color: '#777777' }}>
                      Espera máx: {ev.tiempoMaximoEsperaMinutos} min
                    </div>
                  </td>
                  <td>
                    <span style={{ fontSize: '12.5px' }}>
                      T: {ev.temperatura}°C | FC: {ev.frecuenciaCardiaca} | SpO2:{' '}
                      {ev.saturacionOxigeno}% | PA: {ev.presionArterial}
                    </span>
                  </td>
                  <td>
                    <span className={`badge-mts badge-${ev.codigoColor || 'Azul'}`}>
                      {ev.codigoColor} (Nivel {ev.prioridadAsignada})
                    </span>
                  </td>
                  <td>
                    {new Date(ev.fechaRegistro)
                      .toISOString()
                      .replace('T', ' ')
                      .substring(0, 16)}
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '5px' }}>
                      <button
                        type="button"
                        className="btn btn-warning btn-sm"
                        onClick={() => setDetalleSeleccionado(ev)}
                      >
                        Dictamen
                      </button>
                      <button
                        type="button"
                        className="btn btn-danger btn-sm"
                        onClick={() => {
                          if (detalleSeleccionado?.id === ev.id) {
                            setDetalleSeleccionado(null);
                          }
                          onDeleteEvaluacion(ev.id);
                        }}
                      >
                        Eliminar
                      </button>
                    </div>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
