import React, { useState } from 'react';

export default function KnowledgeBaseManager({
  vistaActiva = 'conocimiento',
  sintomas,
  protocolos,
  onCreateSintoma,
  onDeleteSintoma,
  onUpdateProtocolo
}) {
  const [soloAlarmas, setSoloAlarmas] = useState(false);
  const [mostrarFormSintoma, setMostrarFormSintoma] = useState(false);
  const [nuevoSintoma, setNuevoSintoma] = useState({
    codigo: 'SIN-007',
    nombre: '',
    descripcion: '',
    esSignoAlarma: true,
    categoria: 'Neurológico',
    nivelGravedadBase: 5
  });
  const [protocoloEdit, setProtocoloEdit] = useState(null);
  const [error, setError] = useState('');
  const [mensaje, setMensaje] = useState('');

  const sintomasMostrados = soloAlarmas
    ? sintomas.filter((s) => s.esSignoAlarma)
    : sintomas;

  const handleCrearSintoma = async (e) => {
    e.preventDefault();
    setError('');
    setMensaje('');
    try {
      await onCreateSintoma({
        ...nuevoSintoma,
        nivelGravedadBase: Number(nuevoSintoma.nivelGravedadBase)
      });
      setMensaje('Síntoma agregado al catálogo clínico.');
      setMostrarFormSintoma(false);
      setNuevoSintoma({
        codigo: `SIN-00${sintomas.length + 2}`,
        nombre: '',
        descripcion: '',
        esSignoAlarma: false,
        categoria: 'General',
        nivelGravedadBase: 3
      });
    } catch (err) {
      setError(err.message || 'Error al registrar síntoma.');
    }
  };

  const handleGuardarProtocolo = async (e) => {
    e.preventDefault();
    setError('');
    setMensaje('');
    try {
      await onUpdateProtocolo(protocoloEdit.id, {
        ...protocoloEdit,
        tiempoMaximoEsperaMinutos: Number(
          protocoloEdit.tiempoMaximoEsperaMinutos
        )
      });
      setMensaje(`Protocolo ${protocoloEdit.nombreNivel} actualizado.`);
      setProtocoloEdit(null);
    } catch (err) {
      setError(err.message || 'Error al actualizar protocolo.');
    }
  };

  if (vistaActiva === 'protocolos') {
    return (
      <div>
        <div className="page-title-bar">
          <h1>Áreas Médicas y Protocolos Manchester</h1>
        </div>

        {error && <div className="alert-banner error">{error}</div>}
        {mensaje && <div className="alert-banner success">{mensaje}</div>}

        {protocoloEdit && (
          <form
            onSubmit={handleGuardarProtocolo}
            className="panel"
            style={{ backgroundColor: '#f9f9f9' }}
          >
            <div className="panel-header">
              <h2>Editando {protocoloEdit.nombreNivel}</h2>
            </div>
            <div className="form-grid">
              <div className="form-group">
                <label>Tiempo Máximo de Espera (Minutos)</label>
                <input
                  type="number"
                  className="form-input"
                  value={protocoloEdit.tiempoMaximoEsperaMinutos}
                  onChange={(e) =>
                    setProtocoloEdit({
                      ...protocoloEdit,
                      tiempoMaximoEsperaMinutos: e.target.value
                    })
                  }
                  required
                />
              </div>
              <div className="form-group">
                <label>Área Física de Derivación</label>
                <input
                  className="form-input"
                  value={protocoloEdit.areaDerivacion}
                  onChange={(e) =>
                    setProtocoloEdit({
                      ...protocoloEdit,
                      areaDerivacion: e.target.value
                    })
                  }
                  required
                />
              </div>
            </div>
            <div className="form-group" style={{ marginBottom: '12px' }}>
              <label>Directriz / Acción Clínica Inmediata</label>
              <input
                className="form-input"
                value={protocoloEdit.accionInmediata}
                onChange={(e) =>
                  setProtocoloEdit({
                    ...protocoloEdit,
                    accionInmediata: e.target.value
                  })
                }
                required
              />
            </div>
            <div style={{ display: 'flex', gap: '8px' }}>
              <button type="submit" className="btn btn-primary">
                Guardar Cambios
              </button>
              <button
                type="button"
                className="btn btn-default"
                onClick={() => setProtocoloEdit(null)}
              >
                Cancelar
              </button>
            </div>
          </form>
        )}

        <div className="table-wrap">
          <table className="data-table">
            <thead>
              <tr>
                <th>Nivel</th>
                <th>Color MTS</th>
                <th>Espera Máxima</th>
                <th>Área Médica de Derivación</th>
                <th>Acción Inmediata</th>
                <th style={{ width: '90px' }}></th>
              </tr>
            </thead>
            <tbody>
              {protocolos.map((pr) => (
                <tr key={pr.id}>
                  <td>
                    <strong>{pr.nombreNivel}</strong>
                  </td>
                  <td>
                    <span className={`badge-mts badge-${pr.codigoColor}`}>
                      {pr.codigoColor}
                    </span>
                  </td>
                  <td>
                    {pr.tiempoMaximoEsperaMinutos === 0
                      ? '0 min (Inmediato)'
                      : `${pr.tiempoMaximoEsperaMinutos} min`}
                  </td>
                  <td>{pr.areaDerivacion}</td>
                  <td>{pr.accionInmediata}</td>
                  <td>
                    <button
                      type="button"
                      className="btn btn-warning btn-sm"
                      onClick={() => setProtocoloEdit(pr)}
                    >
                      Editar
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    );
  }

  return (
    <div>
      <div className="page-title-bar">
        <h1>Catálogo de Síntomas y Signos de Alarma</h1>
        <div style={{ display: 'flex', gap: '8px' }}>
          <button
            type="button"
            className={`btn ${soloAlarmas ? 'btn-primary' : 'btn-default'}`}
            onClick={() => setSoloAlarmas(!soloAlarmas)}
          >
            {soloAlarmas
              ? 'Ver Catálogo Completo'
              : 'Filtrar Solo Signos de Alarma'}
          </button>
          <button
            type="button"
            className="btn btn-default"
            onClick={() => setMostrarFormSintoma(!mostrarFormSintoma)}
          >
            {mostrarFormSintoma ? 'Cerrar Formulario' : 'Nuevo Síntoma'}
          </button>
        </div>
      </div>

      {error && <div className="alert-banner error">{error}</div>}
      {mensaje && <div className="alert-banner success">{mensaje}</div>}

      {mostrarFormSintoma && (
        <form
          onSubmit={handleCrearSintoma}
          className="panel"
          style={{ backgroundColor: '#f9f9f9' }}
        >
          <div className="panel-header">
            <h2>Registrar Síntoma en la Base de Conocimiento</h2>
          </div>
          <div className="form-grid">
            <div className="form-group">
              <label>Código</label>
              <input
                className="form-input"
                value={nuevoSintoma.codigo}
                onChange={(e) =>
                  setNuevoSintoma({ ...nuevoSintoma, codigo: e.target.value })
                }
                required
              />
            </div>
            <div className="form-group">
              <label>Nombre del Síntoma</label>
              <input
                className="form-input"
                placeholder="Ej. Convulsiones Tónico-Clónicas"
                value={nuevoSintoma.nombre}
                onChange={(e) =>
                  setNuevoSintoma({ ...nuevoSintoma, nombre: e.target.value })
                }
                required
              />
            </div>
            <div className="form-group">
              <label>Categoría Clínica</label>
              <select
                className="form-select"
                value={nuevoSintoma.categoria}
                onChange={(e) =>
                  setNuevoSintoma({
                    ...nuevoSintoma,
                    categoria: e.target.value
                  })
                }
              >
                <option value="Cardiovascular">Cardiovascular</option>
                <option value="Respiratorio">Respiratorio</option>
                <option value="Neurológico">Neurológico</option>
                <option value="Infeccioso">Infeccioso</option>
                <option value="Traumatología">Traumatología</option>
                <option value="General">General</option>
              </select>
            </div>
            <div className="form-group">
              <label>Gravedad Base (1-5)</label>
              <input
                type="number"
                min={1}
                max={5}
                className="form-input"
                value={nuevoSintoma.nivelGravedadBase}
                onChange={(e) =>
                  setNuevoSintoma({
                    ...nuevoSintoma,
                    nivelGravedadBase: e.target.value
                  })
                }
                required
              />
            </div>
          </div>

          <div className="form-group" style={{ marginBottom: '12px' }}>
            <label>Descripción Semiológica</label>
            <input
              className="form-input"
              placeholder="Descripción clínica detallada..."
              value={nuevoSintoma.descripcion}
              onChange={(e) =>
                setNuevoSintoma({
                  ...nuevoSintoma,
                  descripcion: e.target.value
                })
              }
              required
            />
          </div>

          <div
            style={{
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center'
            }}
          >
            <label
              style={{
                display: 'flex',
                alignItems: 'center',
                gap: '8px',
                fontWeight: 700,
                cursor: 'pointer'
              }}
            >
              <input
                type="checkbox"
                checked={nuevoSintoma.esSignoAlarma}
                onChange={(e) =>
                  setNuevoSintoma({
                    ...nuevoSintoma,
                    esSignoAlarma: e.target.checked
                  })
                }
              />
              Clasificar como Signo de Alarma (Bandera Roja)
            </label>

            <button type="submit" className="btn btn-primary">
              Guardar Síntoma
            </button>
          </div>
        </form>
      )}

      <div className="table-wrap">
        <table className="data-table">
          <thead>
            <tr>
              <th>Código</th>
              <th>Nombre del Síntoma</th>
              <th>Descripción</th>
              <th>Categoría</th>
              <th>Clasificación</th>
              <th style={{ width: '90px' }}></th>
            </tr>
          </thead>
          <tbody>
            {sintomasMostrados.map((s) => (
              <tr key={s.id}>
                <td>
                  <strong>{s.codigo}</strong>
                </td>
                <td>{s.nombre}</td>
                <td>{s.descripcion}</td>
                <td>{s.categoria}</td>
                <td>
                  {s.esSignoAlarma ? (
                    <span className="badge-mts badge-Rojo">
                      Signo de Alarma
                    </span>
                  ) : (
                    <span>Gravedad Nivel {s.nivelGravedadBase}</span>
                  )}
                </td>
                <td>
                  <button
                    type="button"
                    className="btn btn-danger btn-sm"
                    onClick={() => onDeleteSintoma(s.id)}
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
