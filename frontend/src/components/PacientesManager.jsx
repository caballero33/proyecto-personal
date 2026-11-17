import React, { useState } from 'react';
import { IconSearch } from './Icons';

export default function PacientesManager({
  pacientes,
  onCreate,
  onUpdate,
  onDelete,
  onFetchHistorial
}) {
  const [editandoId, setEditandoId] = useState(null);
  const [mostrarFormulario, setMostrarFormulario] = useState(false);
  const [busqueda, setBusqueda] = useState('');
  const [form, setForm] = useState({
    dni: '',
    nombreCompleto: '',
    edad: 35,
    genero: 'Masculino',
    telefono: '+504 '
  });
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');
  const [historialPaciente, setHistorialPaciente] = useState(null);
  const [listaHistorial, setListaHistorial] = useState([]);

  const iniciarEdicion = (p) => {
    setError('');
    setMensaje('');
    setEditandoId(p.id);
    setMostrarFormulario(true);
    setForm({
      dni: p.dni,
      nombreCompleto: p.nombreCompleto,
      edad: p.edad,
      genero: p.genero,
      telefono: p.telefono || ''
    });
  };

  const cancelarEdicion = () => {
    setEditandoId(null);
    setMostrarFormulario(false);
    setForm({
      dni: '',
      nombreCompleto: '',
      edad: 35,
      genero: 'Masculino',
      telefono: '+504 '
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setMensaje('');
    try {
      if (editandoId) {
        await onUpdate(editandoId, { ...form, edad: Number(form.edad) });
        setMensaje(`Paciente #${editandoId} actualizado correctamente.`);
        cancelarEdicion();
      } else {
        await onCreate({ ...form, edad: Number(form.edad) });
        setMensaje('Paciente registrado exitosamente.');
        cancelarEdicion();
      }
    } catch (err) {
      setError(err.message || 'Error al guardar datos del paciente.');
    }
  };

  const verHistorial = async (paciente) => {
    setHistorialPaciente(paciente);
    const items = await onFetchHistorial(paciente.id);
    setListaHistorial(items);
  };

  const pacientesFiltrados = pacientes.filter((p) => {
    const t = `${p.dni} ${p.nombreCompleto} ${p.telefono || ''}`.toLowerCase();
    return !busqueda.trim() || t.includes(busqueda.toLowerCase());
  });

  return (
    <div>
      <div className="page-title-bar">
        <h1>Pacientes</h1>
        <button
          type="button"
          className="btn btn-default"
          onClick={() => {
            if (mostrarFormulario) {
              cancelarEdicion();
            } else {
              setMostrarFormulario(true);
            }
          }}
        >
          {mostrarFormulario ? 'Cerrar Formulario' : 'Nuevo Paciente'}
        </button>
      </div>

      {error && <div className="alert-banner error">{error}</div>}
      {mensaje && <div className="alert-banner success">{mensaje}</div>}

      {/* Formulario Desplegable de Alta / Edición */}
      {mostrarFormulario && (
        <div className="panel" style={{ backgroundColor: '#f9f9f9' }}>
          <div className="panel-header">
            <h2>
              {editandoId
                ? `Editar Paciente #${editandoId}`
                : 'Registrar Nuevo Paciente'}
            </h2>
          </div>

          <form onSubmit={handleSubmit}>
            <div className="form-grid">
              <div className="form-group">
                <label>DNI / Identificación</label>
                <input
                  className="form-input"
                  placeholder="0801-1990-99887"
                  value={form.dni}
                  onChange={(e) => setForm({ ...form, dni: e.target.value })}
                  required
                />
              </div>

              <div className="form-group">
                <label>Nombre Completo</label>
                <input
                  className="form-input"
                  placeholder="Carlos Roberto Mendoza"
                  value={form.nombreCompleto}
                  onChange={(e) =>
                    setForm({ ...form, nombreCompleto: e.target.value })
                  }
                  required
                />
              </div>

              <div className="form-group">
                <label>Edad (Años)</label>
                <input
                  type="number"
                  className="form-input"
                  value={form.edad}
                  onChange={(e) => setForm({ ...form, edad: e.target.value })}
                  required
                />
              </div>

              <div className="form-group">
                <label>Género</label>
                <select
                  className="form-select"
                  value={form.genero}
                  onChange={(e) => setForm({ ...form, genero: e.target.value })}
                >
                  <option value="Masculino">Masculino</option>
                  <option value="Femenino">Femenino</option>
                  <option value="No Especificado">No Especificado</option>
                </select>
              </div>

              <div className="form-group">
                <label>Teléfono de Contacto</label>
                <input
                  className="form-input"
                  placeholder="+504 9555-4433"
                  value={form.telefono}
                  onChange={(e) =>
                    setForm({ ...form, telefono: e.target.value })
                  }
                />
              </div>
            </div>

            <div style={{ display: 'flex', gap: '8px' }}>
              <button type="submit" className="btn btn-primary">
                {editandoId ? 'Actualizar Paciente' : 'Guardar Paciente'}
              </button>
              <button
                type="button"
                className="btn btn-default"
                onClick={cancelarEdicion}
              >
                Cancelar
              </button>
            </div>
          </form>
        </div>
      )}

      {/* Barra de Búsqueda */}
      <div
        style={{
          display: 'grid',
          gridTemplateColumns: '1fr 160px',
          gap: '12px',
          marginBottom: '18px'
        }}
      >
        <div className="input-group">
          <span className="input-group-addon">
            <IconSearch />
          </span>
          <input
            type="text"
            className="form-input"
            placeholder="Buscar por DNI, nombre o teléfono..."
            value={busqueda}
            onChange={(e) => setBusqueda(e.target.value)}
          />
        </div>
        <button type="button" className="btn btn-primary">
          Buscar
        </button>
      </div>

      {/* Historial Clínico Seleccionado */}
      {historialPaciente && (
        <div className="panel" style={{ marginBottom: '18px' }}>
          <div className="panel-header">
            <h2>
              Historial de Evaluaciones: {historialPaciente.nombreCompleto} (DNI:{' '}
              {historialPaciente.dni})
            </h2>
            <button
              type="button"
              className="btn btn-default btn-sm"
              onClick={() => setHistorialPaciente(null)}
            >
              Cerrar Historial
            </button>
          </div>

          {listaHistorial.length === 0 ? (
            <p style={{ color: '#777777' }}>
              Este paciente no registra evaluaciones de triage previas.
            </p>
          ) : (
            <div className="eval-list">
              {listaHistorial.map((h) => (
                <div
                  key={h.id}
                  className={`eval-card border-${h.codigoColor || 'Azul'}`}
                >
                  <div className="eval-top">
                    <strong>
                      Fecha: {new Date(h.fechaRegistro).toLocaleString()}
                    </strong>
                    <span className={`badge-mts badge-${h.codigoColor}`}>
                      Código {h.codigoColor} — {h.nombrePrioridad}
                    </span>
                  </div>
                  <div>
                    <strong>Síntomas:</strong> {h.sintomasPrincipales}
                  </div>
                  <div className="rule-box">{h.justificacionRegla}</div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      {/* Tabla de Pacientes con botones Editar (Naranja) y Eliminar (Rojo) */}
      <div className="table-wrap">
        <table className="data-table">
          <thead>
            <tr>
              <th>DNI</th>
              <th>Nombre Completo</th>
              <th>Edad</th>
              <th>Género</th>
              <th>Teléfono</th>
              <th>Fecha Registro</th>
              <th style={{ width: '210px' }}></th>
            </tr>
          </thead>
          <tbody>
            {pacientesFiltrados.map((p) => (
              <tr key={p.id}>
                <td>{p.dni}</td>
                <td>{p.nombreCompleto}</td>
                <td>{p.edad} años</td>
                <td>{p.genero}</td>
                <td>{p.telefono || 'No registrado'}</td>
                <td>
                  {new Date(p.fechaRegistro)
                    .toISOString()
                    .replace('T', ' ')
                    .substring(0, 16)}
                </td>
                <td>
                  <div style={{ display: 'flex', gap: '5px' }}>
                    <button
                      type="button"
                      className="btn btn-default btn-sm"
                      onClick={() => verHistorial(p)}
                    >
                      Historial
                    </button>
                    <button
                      type="button"
                      className="btn btn-warning btn-sm"
                      onClick={() => iniciarEdicion(p)}
                    >
                      Editar
                    </button>
                    <button
                      type="button"
                      className="btn btn-danger btn-sm"
                      onClick={async () => {
                        setError('');
                        try {
                          await onDelete(p.id);
                        } catch (err) {
                          setError(err.message);
                        }
                      }}
                    >
                      Eliminar
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
