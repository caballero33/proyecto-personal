import React, { useEffect, useState, useCallback } from 'react';
import { triageApi } from './services/api';
import MedicalDashboard from './components/MedicalDashboard';
import TriageEvaluationForm from './components/TriageEvaluationForm';
import PacientesManager from './components/PacientesManager';
import KnowledgeBaseManager from './components/KnowledgeBaseManager';
import {
  IconHome,
  IconTriage,
  IconPatient,
  IconMed,
  IconList,
  IconPlus,
  IconRefresh
} from './components/Icons';

export default function App() {
  const [pestana, setPestana] = useState('dashboard');
  const [apiOnline, setApiOnline] = useState(false);
  const [estadoMotor, setEstadoMotor] = useState('Verificando motor experto...');

  const [evaluaciones, setEvaluaciones] = useState([]);
  const [pacientes, setPacientes] = useState([]);
  const [sintomas, setSintomas] = useState([]);
  const [protocolos, setProtocolos] = useState([]);

  const cargarTodo = useCallback(async () => {
    const status = await triageApi.getEstadoMotor();
    setApiOnline(status.online);
    setEstadoMotor(status.data.estado);

    const [evals, pacs, sints, prots] = await Promise.all([
      triageApi.getEvaluaciones(status.online),
      triageApi.getPacientes(status.online),
      triageApi.getSintomas(false, status.online),
      triageApi.getProtocolos(status.online)
    ]);

    setEvaluaciones(evals);
    setPacientes(pacs);
    setSintomas(sints);
    setProtocolos(prots);
  }, []);

  useEffect(() => {
    cargarTodo();
  }, [cargarTodo]);

  // Handlers de Evaluaciones
  const handleEvaluarPaciente = async (dto) => {
    const resultado = await triageApi.createEvaluacion(dto, apiOnline);
    const actualizadas = await triageApi.getEvaluaciones(apiOnline);
    setEvaluaciones(actualizadas);
    return resultado;
  };

  const handleEliminarEvaluacion = async (id) => {
    await triageApi.deleteEvaluacion(id, apiOnline);
    setEvaluaciones((prev) => prev.filter((e) => e.id !== id));
  };

  // Handlers de Pacientes
  const handleCrearPaciente = async (dto) => {
    const creado = await triageApi.createPaciente(dto, apiOnline);
    const actualizados = await triageApi.getPacientes(apiOnline);
    setPacientes(actualizados);
    return creado;
  };

  const handleActualizarPaciente = async (id, dto) => {
    await triageApi.updatePaciente(id, dto, apiOnline);
    const actualizados = await triageApi.getPacientes(apiOnline);
    setPacientes(actualizados);
  };

  const handleEliminarPaciente = async (id) => {
    await triageApi.deletePaciente(id, apiOnline);
    setPacientes((prev) => prev.filter((p) => p.id !== id));
  };

  const handleFetchHistorialPaciente = async (pacienteId) => {
    return triageApi.getEvaluacionesByPaciente(pacienteId, apiOnline);
  };

  // Handlers de Síntomas y Protocolos
  const handleCrearSintoma = async (dto) => {
    await triageApi.createSintoma(dto, apiOnline);
    const actualizados = await triageApi.getSintomas(false, apiOnline);
    setSintomas(actualizados);
  };

  const handleEliminarSintoma = async (id) => {
    await triageApi.deleteSintoma(id, apiOnline);
    setSintomas((prev) => prev.filter((s) => s.id !== id));
  };

  const handleActualizarProtocolo = async (id, dto) => {
    await triageApi.updateProtocolo(id, dto, apiOnline);
    const actualizados = await triageApi.getProtocolos(apiOnline);
    setProtocolos(actualizados);
  };

  return (
    <div className="bookmedik-wrapper">
      {/* Barra Superior Oscura (#222222) */}
      <header className="bm-topbar">
        <div className="bm-topbar-left">
          <div className="bm-brand" onClick={() => setPestana('dashboard')}>
            <span>TriageDoc</span>
            <span className="bm-version-badge">v2.0</span>
          </div>

          <button
            type="button"
            className="bm-quick-link"
            onClick={() => setPestana('triage')}
          >
            <IconPlus />
            <span>Nueva Evaluación</span>
          </button>
        </div>

        <div className="bm-topbar-right">
          <div className="status-pill" title={estadoMotor}>
            <span
              className={`status-dot ${apiOnline ? 'online' : 'offline'}`}
            />
            <span>
              {apiOnline
                ? 'API Conectada (.NET + PostgreSQL)'
                : 'Simulación Local Activa'}
            </span>
          </div>
          <button
            type="button"
            className="bm-quick-link"
            onClick={cargarTodo}
            title="Sincronizar con el servidor backend"
          >
            <IconRefresh />
            <span>Sincronizar</span>
          </button>
        </div>
      </header>

      {/* Contenedor Lateral Oscuro + Lienzo Principal Blanco */}
      <div className="bm-body">
        <aside className="bm-sidebar">
          <ul className="bm-nav-list">
            <li>
              <button
                type="button"
                className={`bm-nav-item ${
                  pestana === 'dashboard' ? 'active' : ''
                }`}
                onClick={() => setPestana('dashboard')}
              >
                <IconHome />
                <span>Evaluaciones Triage</span>
              </button>
            </li>
            <li>
              <button
                type="button"
                className={`bm-nav-item ${
                  pestana === 'triage' ? 'active' : ''
                }`}
                onClick={() => setPestana('triage')}
              >
                <IconTriage />
                <span>Motor de Inferencia</span>
              </button>
            </li>
            <li>
              <button
                type="button"
                className={`bm-nav-item ${
                  pestana === 'pacientes' ? 'active' : ''
                }`}
                onClick={() => setPestana('pacientes')}
              >
                <IconPatient />
                <span>Pacientes</span>
              </button>
            </li>
            <li>
              <button
                type="button"
                className={`bm-nav-item ${
                  pestana === 'conocimiento' ? 'active' : ''
                }`}
                onClick={() => setPestana('conocimiento')}
              >
                <IconMed />
                <span>Síntomas y Alarmas</span>
              </button>
            </li>
            <li>
              <button
                type="button"
                className={`bm-nav-item ${
                  pestana === 'protocolos' ? 'active' : ''
                }`}
                onClick={() => setPestana('protocolos')}
              >
                <IconList />
                <span>Áreas y Protocolos</span>
              </button>
            </li>
          </ul>
        </aside>

        <main className="bm-content">
          {pestana === 'dashboard' && (
            <MedicalDashboard
              evaluaciones={evaluaciones}
              pacientes={pacientes}
              protocolos={protocolos}
              onDeleteEvaluacion={handleEliminarEvaluacion}
              onGoToNewTriage={() => setPestana('triage')}
            />
          )}

          {pestana === 'triage' && (
            <TriageEvaluationForm
              pacientes={pacientes}
              sintomas={sintomas}
              onEvaluar={handleEvaluarPaciente}
              onQuickCreatePaciente={handleCrearPaciente}
            />
          )}

          {pestana === 'pacientes' && (
            <PacientesManager
              pacientes={pacientes}
              onCreate={handleCrearPaciente}
              onUpdate={handleActualizarPaciente}
              onDelete={handleEliminarPaciente}
              onFetchHistorial={handleFetchHistorialPaciente}
            />
          )}

          {(pestana === 'conocimiento' || pestana === 'protocolos') && (
            <KnowledgeBaseManager
              vistaActiva={pestana}
              sintomas={sintomas}
              protocolos={protocolos}
              onCreateSintoma={handleCrearSintoma}
              onDeleteSintoma={handleEliminarSintoma}
              onUpdateProtocolo={handleActualizarProtocolo}
            />
          )}
        </main>
      </div>
    </div>
  );
}
