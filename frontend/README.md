# Frontend - TriageDoc (React + Vite)

Aplicación cliente en **React 18 + Vite** para **TriageDoc**, diseñada bajo arquitectura **API-First** que consume los endpoints de `TriageDoc.API` (.NET 8) y cuenta con modo de simulación local automático cuando el backend no está activo.

## Módulos Implementados
1. **Panel Médico de Clasificación Manchester (`MedicalDashboard.jsx`):**
   - Tarjetas interactivas por código de color (**Rojo, Naranja, Amarillo, Verde, Azul**).
   - Filtrado por nivel de prioridad y buscador clínico en tiempo real.
   - Visualización de signos vitales, banderas rojas y la justificación de la regla inferida por el sistema experto.
2. **Formulario de Autoevaluación y Motor Experto (`TriageEvaluationForm.jsx`):**
   - Selección o alta rápida de pacientes.
   - Carga rápida de casos clínicos de prueba (Código Rojo, Naranja, Amarillo, Verde/Azul).
   - Selección interactiva de síntomas del catálogo y banderas rojas.
   - Tarjeta de dictamen inmediato con nivel Manchester, tiempo máximo de espera y área de derivación.
3. **Gestión CRUD de Pacientes (`PacientesManager.jsx`):**
   - Creación (`POST`), consulta (`GET`), actualización (`PUT`), eliminación (`DELETE`) y consulta de historial de evaluaciones por paciente (`GET /api/evaluaciones/paciente/{id}`).
4. **Base de Conocimiento (`KnowledgeBaseManager.jsx`):**
   - Gestión del catálogo de síntomas y filtro exclusivo de signos de alarma (`GET /api/sintomas/alarmas`).
   - Visualización y edición de los 5 Protocolos Oficiales Manchester (`PUT /api/protocolos/{id}`).

## Ejecución
```bash
cd TriageDoc/frontend
npm install
npm run dev
```
La aplicación estará disponible en `http://localhost:5173`.
