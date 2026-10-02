
/*
 * Proyecto: Agente_AI
 * Formulario: FrmAgenteIA
 * Propósito:
 *     Conectar el formulario de Windows Forms con Ollama,
 *     utilizando el modelo local qwen2.5:3b.
 *
 * Características:
 *     - Conexión local con Ollama.
 *     - Creación de un agente con problema e instrucciones.
 *     - Conversación con memoria durante la sesión.
 *     - Inicio de una nueva sesión.
 *     - Herramienta local para consultar fecha y hora.
 *     - Manejo de errores.
 *
 * Importante:
 *     Este archivo utiliza los controles que ya existen
 *     en el formulario. No reemplaza el diseñador.
 */

// Importa las clases básicas de C#.
using System;

// Importa las colecciones para guardar el historial.
using System.Collections.Generic;

// Importa las clases para realizar solicitudes HTTP.
using System.Net.Http;

// Importa las clases para trabajar con texto.
using System.Text;

// Importa las herramientas para convertir JSON.
using System.Text.Json;

// Importa las clases para trabajar con formularios.
using System.Drawing;
using System.Windows.Forms;

// Define el espacio de nombres del proyecto.
namespace Agente_AI
{
    /// <summary>
    /// Formulario principal del agente de inteligencia artificial.
    /// Utiliza Ollama para consultar un modelo local.
    /// </summary>
    public partial class FrmAgenteIA : Form
    {
        // Dirección de la API local de Ollama.
        private const string UrlOllama =
            "http://localhost:11434/api/chat";

        // Nombre del modelo que se descargó anteriormente.
        private const string ModeloPredeterminado = "qwen2.5:3b";

        // Cliente HTTP compartido para comunicarse con Ollama.
        private static readonly HttpClient ClienteHttp = new HttpClient()
        {
            // Permite más tiempo para generar respuestas largas.
            Timeout = TimeSpan.FromMinutes(3)
        };

        // Opciones para convertir correctamente las propiedades a JSON.
        private static readonly JsonSerializerOptions OpcionesJson =
            new JsonSerializerOptions()
            {
                // Convierte nombres como Role en role.
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

        // Guarda los mensajes de la conversación actual.
        private readonly List<MensajeOllama> _historial =
            new List<MensajeOllama>();

        // Guarda las instrucciones que debe seguir el agente.
        private string _instruccionesSistema = string.Empty;

        // Indica si el agente ya fue configurado.
        private bool _agenteCreado = false;

        // Guarda el modelo seleccionado.
        private string _modeloActual = ModeloPredeterminado;

        // Indica si Ollama respondió correctamente a la comprobación.
        private bool _ollamaConectado = false;

        // Utiliza el campo de mensaje existente como campo de consulta.
        private TextBox txtConsulta => txtMensaje;

        // Utiliza el historial de chat existente.
        private RichTextBox rtbConversacion => rtbChat;

        // Selector del proveedor de inteligencia artificial.
        private ComboBox cmbProveedor = new ComboBox();

        // Campo para describir el problema que resolverá el agente.
        private TextBox txtProblema = new TextBox();

        // Campo para establecer las instrucciones del agente.
        private TextBox txtInstrucciones = new TextBox();

        // Botón para crear el agente.
        private Button btnCrearAgente = new Button();

        // Botón para iniciar una conversación nueva.
        private Button btnNuevaSesion = new Button();

        // Etiqueta que muestra el estado de la aplicación.
        private Label lblEstado = new Label();

        /// <summary>
        /// Inicializa el formulario y configura los controles.
        /// </summary>
        public FrmAgenteIA()
        {
            // Carga los controles que ya existen en el diseñador.
            InitializeComponent();

            // Configura la interfaz sin reemplazar el formulario.
            ConfigurarInterfazAsignacion();

            // Oculta el campo de clave API porque Ollama no la necesita.
            txtApikey.Visible = false;

            // Oculta también la etiqueta de la clave API.
            lblApiKey.Visible = false;

            // Selecciona Ollama como proveedor.
            cmbProveedor.SelectedIndex = 0;

            // Selecciona el modelo local.
            cmbModelo.SelectedIndex = 0;

            // Evita que el botón Enviar tenga el evento duplicado.
            btnEnviar.Click -= btnEnviar_Click;

            // Conecta el botón Enviar con su método correspondiente.
            btnEnviar.Click += btnEnviar_Click;

            // Muestra el mensaje inicial en el historial.
            AgregarMensaje(
                "Sistema",
                "Bienvenido. Comprueba Ollama y configura tu agente para comenzar."
            );

            // Informa que todavía no se ha configurado el agente.
            ActualizarEstado("Agente sin crear.");
        }

        /// <summary>
        /// Configura la interfaz y conserva los controles existentes.
        /// </summary>
        private void ConfigurarInterfazAsignacion()
        {
            // Permite desplazar el panel si no hay suficiente espacio vertical.
            pnlConfiguración.AutoScroll = true;

            // Establece el ancho del panel de configuración.
            pnlConfiguración.Width = 360;

            // Crea la etiqueta del proveedor.
            Label lblProveedor = new Label()
            {
                Name = "lblProveedor",
                Text = "Proveedor de IA",
                AutoSize = true,
                Location = new Point(18, 65),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Configura el selector del proveedor.
            cmbProveedor.Name = "cmbProveedor";
            cmbProveedor.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProveedor.Items.Clear();
            cmbProveedor.Items.Add("Ollama (local)");
            cmbProveedor.Location = new Point(18, 90);
            cmbProveedor.Size = new Size(300, 28);

            // Actualiza la etiqueta del modelo.
            lblModelo.Text = "Modelo local";
            lblModelo.Location = new Point(18, 130);

            // Configura el selector del modelo.
            cmbModelo.Location = new Point(18, 155);
            cmbModelo.Size = new Size(300, 28);
            cmbModelo.Items.Clear();

            // Agrega el modelo que ya está instalado en el equipo.
            cmbModelo.Items.Add(ModeloPredeterminado);
            cmbModelo.DropDownStyle = ComboBoxStyle.DropDownList;

            // Selecciona el modelo local predeterminado.
            cmbModelo.SelectedIndex = 0;

            // Oculta el campo de clave API de Anthropic.
            lblApiKey.Visible = false;
            txtApikey.Visible = false;

            // Reutiliza el botón existente para comprobar Ollama.
            btnConectar.Text = "Comprobar Ollama";
            btnConectar.Location = new Point(18, 195);
            btnConectar.Size = new Size(300, 38);

            // Evita registrar el evento de conexión más de una vez.
            btnConectar.Click -= btnConectar_Click;

            // Conecta el botón con el método de comprobación local.
            btnConectar.Click += btnConectar_Click;

            // Crea la etiqueta del problema.
            Label lblProblema = new Label()
            {
                Name = "lblProblema",
                Text = "Problema que debe resolver",
                AutoSize = true,
                Location = new Point(18, 250),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Configura el campo del problema.
            txtProblema.Name = "txtProblema";
            txtProblema.Location = new Point(18, 275);
            txtProblema.Size = new Size(300, 75);
            txtProblema.Multiline = true;
            txtProblema.ScrollBars = ScrollBars.Vertical;
            txtProblema.PlaceholderText =
                "Describe el problema que resolverá el agente...";

            // Crea la etiqueta de las instrucciones.
            Label lblInstrucciones = new Label()
            {
                Name = "lblInstrucciones",
                Text = "Instrucciones del agente",
                AutoSize = true,
                Location = new Point(18, 360),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            // Configura el campo de las instrucciones.
            txtInstrucciones.Name = "txtInstrucciones";
            txtInstrucciones.Location = new Point(18, 385);
            txtInstrucciones.Size = new Size(300, 110);
            txtInstrucciones.Multiline = true;
            txtInstrucciones.ScrollBars = ScrollBars.Vertical;

            // Establece las instrucciones iniciales del agente.
            txtInstrucciones.Text =
                "Eres un asistente útil. Responde en español, " +
                "analiza los problemas paso a paso y explica tus respuestas. " +
                "Si falta información, pregunta antes de asumir. " +
                "No inventes datos.";

            // Configura el botón para crear el agente.
            btnCrearAgente.Name = "btnCrearAgente";
            btnCrearAgente.Text = "Crear agente";
            btnCrearAgente.Location = new Point(18, 510);
            btnCrearAgente.Size = new Size(300, 40);

            // Evita registrar el evento dos veces.
            btnCrearAgente.Click -= btnCrearAgente_Click;

            // Conecta el botón con el método de creación del agente.
            btnCrearAgente.Click += btnCrearAgente_Click;

            // Agrega los controles nuevos al panel existente.
            pnlConfiguración.Controls.Add(lblProveedor);
            pnlConfiguración.Controls.Add(cmbProveedor);
            pnlConfiguración.Controls.Add(lblProblema);
            pnlConfiguración.Controls.Add(txtProblema);
            pnlConfiguración.Controls.Add(lblInstrucciones);
            pnlConfiguración.Controls.Add(txtInstrucciones);
            pnlConfiguración.Controls.Add(btnCrearAgente);

            // Configura el título del chat.
            lblChat.Text = "Conversación con el agente";
            lblChat.Location = new Point(20, 18);

            // Configura el botón para crear una nueva sesión.
            btnNuevaSesion.Name = "btnNuevaSesion";
            btnNuevaSesion.Text = "Nueva sesión";
            btnNuevaSesion.Size = new Size(130, 34);
            btnNuevaSesion.Anchor =
                AnchorStyles.Top | AnchorStyles.Right;

            // Ubica el botón en la esquina superior derecha.
            btnNuevaSesion.Location =
                new Point(pnlChat.ClientSize.Width - 150, 18);

            // Conecta el botón con su método correspondiente.
            btnNuevaSesion.Click += btnNuevaSesion_Click;

            // Agrega el botón al panel del chat.
            pnlChat.Controls.Add(btnNuevaSesion);

            // Configura la etiqueta de estado.
            lblEstado.Name = "lblEstado";
            lblEstado.AutoSize = true;
            lblEstado.Text = "Estado: esperando conexión";
            lblEstado.Location = new Point(20, 48);
            lblEstado.ForeColor = Color.DarkSlateGray;

            // Agrega el estado al panel del chat.
            pnlChat.Controls.Add(lblEstado);

            // Configura el historial de conversación existente.
            rtbConversacion.Location = new Point(16, 75);
            rtbConversacion.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // Ajusta el tamaño inicial del historial.
            rtbConversacion.Size = new Size(
                pnlChat.ClientSize.Width - 32,
                pnlChat.ClientSize.Height - 175
            );

            // Impide que el usuario modifique manualmente el historial.
            rtbConversacion.ReadOnly = true;

            // Configura el campo para escribir las consultas.
            txtConsulta.Location = new Point(
                16,
                pnlChat.ClientSize.Height - 80
            );

            // Ajusta el ancho del campo de consulta.
            txtConsulta.Size = new Size(
                pnlChat.ClientSize.Width - 185,
                48
            );

            // Mantiene el campo en la parte inferior del panel.
            txtConsulta.Anchor =
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            // Permite escribir mensajes en varias líneas.
            txtConsulta.Multiline = true;

            // Limpia el mensaje inicial del campo de consulta.
            txtConsulta.Text = string.Empty;

            // Muestra una indicación cuando el campo está vacío.
            txtConsulta.PlaceholderText = "Escribe tu consulta aquí...";

            // Configura el botón Enviar.
            btnEnviar.Location = new Point(
                pnlChat.ClientSize.Width - 155,
                pnlChat.ClientSize.Height - 80
            );

            // Ajusta el tamaño del botón.
            btnEnviar.Size = new Size(139, 48);

            // Mantiene el botón en la esquina inferior derecha.
            btnEnviar.Anchor =
                AnchorStyles.Bottom | AnchorStyles.Right;

            // Coloca los controles principales al frente.
            lblEstado.BringToFront();
            btnNuevaSesion.BringToFront();

            // Reorganiza los controles cuando cambia el tamaño del panel.
            pnlChat.Resize += (sender, args) =>
            {
                // Reubica el botón de nueva sesión.
                btnNuevaSesion.Location = new Point(
                    pnlChat.ClientSize.Width - 150,
                    18
                );

                // Ajusta el tamaño del historial.
                rtbConversacion.Size = new Size(
                    pnlChat.ClientSize.Width - 32,
                    pnlChat.ClientSize.Height - 175
                );

                // Ajusta la ubicación del campo de consulta.
                txtConsulta.Location = new Point(
                    16,
                    pnlChat.ClientSize.Height - 80
                );

                // Ajusta el ancho del campo de consulta.
                txtConsulta.Size = new Size(
                    pnlChat.ClientSize.Width - 185,
                    48
                );

                // Reubica el botón Enviar.
                btnEnviar.Location = new Point(
                    pnlChat.ClientSize.Width - 155,
                    pnlChat.ClientSize.Height - 80
                );
            };
        }

        /// <summary>
        /// Comprueba si Ollama responde y si el modelo está instalado.
        /// </summary>
        private async void btnConectar_Click(object? sender, EventArgs e)
        {
            // Evita repetir la comprobación mientras hay otra en curso.
            btnConectar.Enabled = false;

            try
            {
                // Informa que se está comprobando el servicio local.
                ActualizarEstado("Comprobando conexión con Ollama...");

                // Consulta la lista de modelos instalados.
                using HttpResponseMessage respuesta = await ClienteHttp.GetAsync(
                    "http://localhost:11434/api/tags"
                );

                // Comprueba que Ollama haya respondido correctamente.
                respuesta.EnsureSuccessStatusCode();

                // Lee el contenido de la respuesta.
                string contenido = await respuesta.Content.ReadAsStringAsync();

                // Convierte el contenido recibido en un documento JSON.
                using JsonDocument documento = JsonDocument.Parse(contenido);

                // Obtiene la lista de modelos instalados.
                JsonElement modelos = documento.RootElement.GetProperty("models");

                // Inicializa la variable que indica si el modelo existe.
                bool modeloEncontrado = false;

                // Recorre todos los modelos disponibles en Ollama.
                foreach (JsonElement modelo in modelos.EnumerateArray())
                {
                    // Lee el nombre del modelo actual.
                    string nombre = modelo.GetProperty("name").GetString()
                        ?? string.Empty;

                    // Comprueba si coincide con el modelo seleccionado.
                    if (nombre.Equals(
                        _modeloActual,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        // Indica que el modelo está instalado.
                        modeloEncontrado = true;

                        // Finaliza la búsqueda.
                        break;
                    }
                }

                // Guarda el resultado de la comprobación.
                _ollamaConectado = modeloEncontrado;

                // Informa el resultado al usuario.
                if (_ollamaConectado)
                {
                    ActualizarEstado(
                        "Ollama conectado. Modelo disponible: " +
                        _modeloActual
                    );

                    // Muestra una confirmación.
                    MessageBox.Show(
                        "La conexión con Ollama funciona correctamente.",
                        "Conexión correcta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    // Informa que el modelo seleccionado no está instalado.
                    ActualizarEstado("No se encontró el modelo seleccionado.");

                    // Indica cómo descargar el modelo.
                    MessageBox.Show(
                        "No se encontró el modelo " + _modeloActual +
                        ". Ejecuta en PowerShell: ollama pull " + _modeloActual,
                        "Modelo no encontrado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                // Indica que no se pudo comprobar la conexión.
                _ollamaConectado = false;

                // Actualiza el estado de la aplicación.
                ActualizarEstado("No se pudo conectar con Ollama.");

                // Informa el posible motivo del error.
                MessageBox.Show(
                    "No se pudo conectar con Ollama.\n\n" +
                    "Comprueba que Ollama esté instalado y funcionando.\n\n" +
                    "Detalle: " + ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                // Reactiva el botón cuando termina la comprobación.
                btnConectar.Enabled = true;
            }
        }

        /// <summary>
        /// Configura el agente usando el problema y las instrucciones.
        /// No realiza llamadas a Internet ni necesita una clave API.
        /// </summary>
        private void btnCrearAgente_Click(object? sender, EventArgs e)
        {
            // Comprueba que el usuario haya descrito el problema.
            if (string.IsNullOrWhiteSpace(txtProblema.Text))
            {
                // Actualiza el estado.
                ActualizarEstado("Falta describir el problema.");

                // Informa qué campo debe completarse.
                MessageBox.Show(
                    "Escribe el problema que debe resolver el agente.",
                    "Falta el problema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Devuelve el foco al campo del problema.
                txtProblema.Focus();

                // Detiene el proceso de creación.
                return;
            }

            // Comprueba que existan instrucciones para el agente.
            if (string.IsNullOrWhiteSpace(txtInstrucciones.Text))
            {
                // Actualiza el estado.
                ActualizarEstado("Faltan las instrucciones.");

                // Informa al usuario qué debe completar.
                MessageBox.Show(
                    "Escribe las instrucciones que debe seguir el agente.",
                    "Faltan instrucciones",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Devuelve el foco al campo de instrucciones.
                txtInstrucciones.Focus();

                // Detiene el proceso de creación.
                return;
            }

            // Obtiene el modelo seleccionado.
            _modeloActual = cmbModelo.SelectedItem?.ToString()
                ?? ModeloPredeterminado;

            // Construye las instrucciones completas del agente.
            _instruccionesSistema =
                txtInstrucciones.Text.Trim() +
                Environment.NewLine + Environment.NewLine +
                "Problema que debes resolver:" +
                Environment.NewLine +
                txtProblema.Text.Trim() +
                Environment.NewLine + Environment.NewLine +
                "Responde siempre en español. " +
                "Explica tus respuestas de forma clara. " +
                "Si falta información, pregunta antes de asumir. " +
                "No inventes datos.";

            // Marca que el agente ya está configurado.
            _agenteCreado = true;

            // Borra el historial de una configuración anterior.
            _historial.Clear();

            // Limpia el historial visual del chat.
            rtbConversacion.Clear();

            // Muestra el resultado de la configuración.
            AgregarMensaje(
                "Sistema",
                "Agente configurado correctamente.\n" +
                "Modelo: " + _modeloActual + "\n" +
                "Proveedor: Ollama local.\n" +
                "No se necesita una clave API."
            );

            // Informa que el agente está listo para conversar.
            ActualizarEstado(
                "Agente configurado. Puedes enviar consultas."
            );

            // Habilita el botón Enviar.
            btnEnviar.Enabled = true;

            // Informa al usuario que la configuración terminó.
            MessageBox.Show(
                "El agente se configuró correctamente.\n" +
                "Ahora puedes escribir una consulta.",
                "Agente listo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Envía la consulta a Ollama y muestra la respuesta en el chat.
        /// Conserva los mensajes anteriores para mantener el contexto.
        /// </summary>
        private async void btnEnviar_Click(object? sender, EventArgs e)
        {
            // Obtiene el texto escrito por el usuario.
            string consulta = txtConsulta.Text.Trim();

            // Evita enviar mensajes vacíos.
            if (string.IsNullOrWhiteSpace(consulta))
            {
                // No hace nada si no hay texto.
                return;
            }

            // Comprueba que el agente haya sido configurado.
            if (!_agenteCreado)
            {
                // Informa que primero debe crearse el agente.
                ActualizarEstado("Primero debes crear el agente.");

                // Muestra una indicación al usuario.
                MessageBox.Show(
                    "Completa el problema y las instrucciones, " +
                    "y pulsa Crear agente.",
                    "Agente no creado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Finaliza el método.
                return;
            }

            // Evita enviar consultas simultáneas.
            btnEnviar.Enabled = false;

            try
            {
                // Informa que la inteligencia artificial está trabajando.
                ActualizarEstado("Qwen está pensando...");

                // Muestra el mensaje del usuario en el historial visual.
                AgregarMensaje("Tú", consulta);

                // Prepara la lista de mensajes que recibirá Ollama.
                List<MensajeOllama> mensajesSolicitud =
                    new List<MensajeOllama>();

                // Agrega las instrucciones como mensaje del sistema.
                mensajesSolicitud.Add(
                    new MensajeOllama(
                        "system",
                        _instruccionesSistema
                    )
                );

                // Agrega los mensajes anteriores de la conversación.
                mensajesSolicitud.AddRange(_historial);

                // Agrega la consulta actual al final de la solicitud.
                mensajesSolicitud.Add(
                    new MensajeOllama("user", consulta)
                );

                // Prepara el cuerpo de la petición para Ollama.
                var cuerpoSolicitud = new
                {
                    // Indica qué modelo procesará la consulta.
                    model = _modeloActual,

                    // Envía las instrucciones y el historial de conversación.
                    messages = mensajesSolicitud,

                    // Solicita la respuesta completa en una sola petición.
                    stream = false
                };

                // Convierte la solicitud a formato JSON.
                string json = JsonSerializer.Serialize(
                    cuerpoSolicitud,
                    OpcionesJson
                );

                // Prepara el contenido HTTP en formato JSON.
                using StringContent contenido = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                // Envía la petición a la API local de Ollama.
                using HttpResponseMessage respuesta = await ClienteHttp.PostAsync(
                    UrlOllama,
                    contenido
                );

                // Comprueba si la respuesta HTTP fue satisfactoria.
                respuesta.EnsureSuccessStatusCode();

                // Lee la respuesta JSON.
                string respuestaJson =
                    await respuesta.Content.ReadAsStringAsync();

                // Convierte la respuesta JSON en un documento.
                using JsonDocument documento =
                    JsonDocument.Parse(respuestaJson);

                // Obtiene el objeto message de la respuesta.
                JsonElement mensajeJson =
                    documento.RootElement.GetProperty("message");

                // Extrae el contenido de la respuesta de Qwen.
                string textoRespuesta =
                    mensajeJson.GetProperty("content").GetString()
                    ?? "El modelo no devolvió texto.";

                // Guarda la consulta del usuario en la memoria de la sesión.
                _historial.Add(
                    new MensajeOllama("user", consulta)
                );

                // Guarda la respuesta del modelo en la memoria de la sesión.
                _historial.Add(
                    new MensajeOllama("assistant", textoRespuesta)
                );

                // Muestra la respuesta en el historial visual.
                AgregarMensaje("Qwen", textoRespuesta);

                // Limpia el campo para la siguiente consulta.
                txtConsulta.Clear();

                // Actualiza el estado después de recibir la respuesta.
                ActualizarEstado("Respuesta recibida correctamente.");
            }
            catch (TaskCanceledException)
            {
                // Informa si la solicitud superó el tiempo de espera.
                AgregarMensaje(
                    "Error",
                    "La consulta tardó demasiado. Intenta nuevamente."
                );

                // Actualiza el estado.
                ActualizarEstado("Tiempo de espera agotado.");
            }
            catch (HttpRequestException ex)
            {
                // Informa los problemas de comunicación con Ollama.
                AgregarMensaje(
                    "Error",
                    "No se pudo comunicar con Ollama.\n" + ex.Message
                );

                // Actualiza el estado.
                ActualizarEstado("Error de conexión con Ollama.");
            }
            catch (Exception ex)
            {
                // Muestra los demás errores sin cerrar la aplicación.
                AgregarMensaje(
                    "Error",
                    "Ocurrió un error al procesar la consulta.\n" +
                    ex.Message
                );

                // Actualiza el estado.
                ActualizarEstado("Error al consultar el modelo.");
            }
            finally
            {
                // Reactiva el botón Enviar al terminar la operación.
                btnEnviar.Enabled = true;

                // Devuelve el foco al campo de consulta.
                txtConsulta.Focus();
            }
        }

        /// <summary>
        /// Inicia una conversación nueva sin conservar los mensajes anteriores.
        /// </summary>
        private void btnNuevaSesion_Click(object? sender, EventArgs e)
        {
            // Comprueba que el agente esté configurado.
            if (!_agenteCreado)
            {
                // Informa que primero debe configurarse el agente.
                ActualizarEstado("Crea el agente antes de iniciar una sesión.");

                // Muestra una indicación al usuario.
                MessageBox.Show(
                    "Primero pulsa Crear agente.",
                    "Agente no creado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                // Finaliza el método.
                return;
            }

            // Elimina los mensajes anteriores de la memoria de la sesión.
            _historial.Clear();

            // Limpia el historial que se muestra en pantalla.
            rtbConversacion.Clear();

            // Limpia cualquier consulta que estuviera escrita.
            txtConsulta.Clear();

            // Informa que se inició una sesión nueva.
            AgregarMensaje(
                "Sistema",
                "Nueva sesión iniciada. El historial anterior se ha borrado."
            );

            // Actualiza el estado de la aplicación.
            ActualizarEstado("Nueva sesión lista.");
        }

        /// <summary>
        /// Devuelve la fecha y hora local del equipo.
        /// Es una herramienta local segura que no modifica archivos.
        /// </summary>
        /// <returns>Fecha y hora actual del equipo.</returns>
        private static string ObtenerFechaHoraLocal()
        {
            // Devuelve la fecha y hora en un formato legible en español.
            return DateTime.Now.ToString(
                "dddd, dd 'de' MMMM 'de' yyyy, HH:mm:ss"
            );
        }

        /// <summary>
        /// Agrega un mensaje al historial visual del chat.
        /// </summary>
        /// <param name="emisor">Nombre de quien envía el mensaje.</param>
        /// <param name="mensaje">Texto del mensaje.</param>
        private void AgregarMensaje(string emisor, string mensaje)
        {
            // Evita procesar mensajes nulos.
            if (mensaje is null)
            {
                // Finaliza el método si no hay texto.
                return;
            }

            // Agrega el nombre del emisor y el mensaje al historial.
            rtbConversacion.AppendText(
                emisor + ":" + Environment.NewLine +
                mensaje + Environment.NewLine + Environment.NewLine
            );

            // Coloca el cursor al final del texto.
            rtbConversacion.SelectionStart =
                rtbConversacion.TextLength;

            // Desplaza el historial para mostrar el mensaje más reciente.
            rtbConversacion.ScrollToCaret();
        }

        /// <summary>
        /// Actualiza el estado que aparece en la interfaz.
        /// </summary>
        /// <param name="estado">Descripción del estado actual.</param>
        private void ActualizarEstado(string estado)
        {
            // Muestra el estado en la etiqueta existente.
            lblEstado.Text = "Estado: " + estado;
        }

        /// <summary>
        /// Mantiene ocultos los caracteres del antiguo campo de clave API.
        /// El campo ya no se utiliza para conectarse con Ollama.
        /// </summary>
        private void txtApikey_TextChanged(object sender, EventArgs e)
        {
            // Conserva el comportamiento original del control.
            txtApikey.PasswordChar = '*';
        }

        /// <summary>
        /// Permite escribir consultas en varias líneas.
        /// </summary>
        private void txtMensaje_TextChanged(object sender, EventArgs e)
        {
            // Configura el campo de consulta existente.
            txtMensaje.Multiline = true;
        }

        /// <summary>
        /// Actualiza el modelo seleccionado.
        /// </summary>
        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Comprueba que exista una selección válida.
            if (cmbModelo.SelectedItem is string modeloSeleccionado)
            {
                // Guarda el nombre del modelo seleccionado.
                _modeloActual = modeloSeleccionado;
            }
        }

        /// <summary>
        /// Representa un mensaje que se envía a la API de Ollama.
        /// </summary>
        private sealed class MensajeOllama
        {
            /// <summary>
            /// Identifica al emisor: system, user o assistant.
            /// </summary>
            public string Role { get; set; }

            /// <summary>
            /// Contiene el texto del mensaje.
            /// </summary>
            public string Content { get; set; }

            /// <summary>
            /// Construye un mensaje con su emisor y contenido.
            /// </summary>
            /// <param name="role">Tipo de emisor del mensaje.</param>
            /// <param name="content">Texto que contiene el mensaje.</param>
            public MensajeOllama(string role, string content)
            {
                // Guarda el tipo de emisor.
                Role = role;

                // Guarda el texto del mensaje.
                Content = content;
            }
        }
    }
}